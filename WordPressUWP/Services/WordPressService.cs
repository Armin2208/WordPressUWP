﻿using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using WordPressPCL;
using WordPressPCL.Models;
using WordPressPCL.Utility;
using WordPressUWP.Helpers;
using WordPressUWP.Interfaces;

namespace WordPressUWP.Services
{
    public class WordPressService : ViewModelBase, IWordPressService 
    {
        private WordPressClient _client;
        private ApplicationDataContainer _localSettings;

        private bool _isLoggedIn;
        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set { Set(ref _isLoggedIn, value); }
        }

        private User _currentUser;
        public User CurrentUser
        {
            get { return _currentUser; }
            set { Set(ref _currentUser, value); }
        }

        public WordPressService()
        {
            _client = new WordPressClient(ApiCredentials.WordPressUri);
            _localSettings = ApplicationData.Current.LocalSettings;
            Init();
        }

        public async void Init()
        {

            IsLoggedIn = false;
            var username = _localSettings.ReadString("Username");
            if(username != null)
            {
                // get password
                var jwt = SettingsStorageExtensions.GetCredentialFromLocker(username);
                if(jwt != null && !string.IsNullOrEmpty(jwt.Password))
                {
                    // set jwt
                    _client.SetJWToken(jwt.Password);
                    IsLoggedIn = await _client.IsValidJWToken();
                    if (IsLoggedIn)
                    {
                        CurrentUser = await _client.Users.GetCurrentUser();
                    }
                }
            }
        }

        public async Task<bool> AuthenticateUser(string username, string password)
        {
            _client.AuthMethod = AuthMethod.JWT;
            await _client.RequestJWToken(username, password);
            var isAuthenticated = await IsUserAuthenticated();

            if (isAuthenticated)
            {
                // Store username & JWT token for logging in on next app launch
                SettingsStorageExtensions.SaveString(_localSettings, "Username", username);
                SettingsStorageExtensions.SaveCredentialsToLocker(username, _client.GetToken());
            }

            return isAuthenticated;
        }

        public async Task<List<CommentThreaded>> GetCommentsForPost(int postid)
        {
            var comments = await _client.Comments.Query(new CommentsQueryBuilder()
            {
                Posts = new int[] { postid },
                Page = 1,
                PerPage = 100
            });

            return ThreadedCommentsHelper.GetThreadedComments(comments);
        }

        public async Task<IEnumerable<Post>> GetLatestPosts(int page = 0, int perPage = 20)
        {
            page++;
            return await _client.Posts.Query(new PostsQueryBuilder()
            {
                Page = page,
                PerPage = perPage,
                Embed = true
            });
        }

        public User GetUser()
        {
            return CurrentUser;
        }

        public async Task<bool> IsUserAuthenticated()
        {
            IsLoggedIn = await _client.IsValidJWToken();
            return IsLoggedIn;
        }

        public async Task<Comment> PostComment(int postId, string text)
        {
            return await _client.Comments.Create(new Comment(postId, text));
        }

        public async Task<bool> Logout()
        {
            _client.Logout();
            IsLoggedIn = await _client.IsValidJWToken();
            SettingsStorageExtensions.RemoveCredentialsFromLocker();
            return IsLoggedIn;
        }
    }
}

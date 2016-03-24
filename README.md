# WordPressUWP
This is a portable library for consuimg the WordPress REST-API in a Universal Windows Plattform App.

#Quickstart

## WordPress Plugins
As the WP REST API (Version 2) Plugin is currently being integrated into WordPress core you'll still need to install the plugin on your site for this library to work. Also, there are two additional plugins for authentication.

* [WordPress REST API (Version 2)](https://wordpress.org/plugins/rest-api/)
* [Basic Authentication handler](https://github.com/WP-API/Basic-Auth)
* [WP REST API - OAuth 1.0a Server](https://github.com/WP-API/OAuth1)

## Including WordPressUWP
We plan on making WordPressUWP available through Nuget as soon as we reach a Beta-Status. Until then you can just download the ZIP-file and include the projekt in your Visual Studio solution.

## Using the API Wrapper

```c#
// Initialize
var client = new WordPressClient("http://demo.wp-api.org/wp-json/wp/v2/");

// Posts
var posts = await client.ListPosts();
var postbyid = await client.GetPost(id);

// Comments
var comments = await client.ListComments();
var commentbyid = await client.GetComment(id);

// Users
// Basic authentication - not recommended for production use
client.Username = "TheUserName";
client.Password = "TheUserPassword";
var currentuser = await client.GetCurrentUser();
```
    

The full documentation will be made available here (but isn't yet...) 
http://thomaspe.github.io/WordPressUWP

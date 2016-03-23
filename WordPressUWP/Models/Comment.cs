﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordPressUWP.Models
{
	public class Comment
	{
		public int Id { get; set; }

		public int Post { get; set; }

		public int Parent { get; set; }

		public int Author { get; set; }

		[JsonProperty("author_name")]
		public string AuthorName { get; set; }

		[JsonProperty("author_url")]
		public string AuthorUrl { get; set; }

		[JsonProperty("author_avatar_urls")]
		public AuthorAvatarUrls AuthorAvatarUrls { get; set; }

		public string Date { get; set; }

		[JsonProperty("date_gmt")]
		public string DateGmt { get; set; }

		public Content Content { get; set; }

		public string Link { get; set; }

		public string Status { get; set; }

		public string Type { get; set; }
		//public Links _links { get; set; }
	}

	public class AuthorAvatarUrls
	{
		[JsonProperty("24")]
		public string Size24 { get; set; }

		[JsonProperty("48")]
		public string Size48 { get; set; }

		[JsonProperty("96")]
		public string Size96 { get; set; }
	}
}
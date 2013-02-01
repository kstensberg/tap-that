using System;
using LitJson;

namespace EightBitIdeas.WebApi.Json
{
	public class LoginResponse
	{
		public string authToken;
		public int userId;
		public string name;
		
		public LoginResponse(JsonData json)
		{
			this.authToken = (string)json["authToken"];
			this.userId = (int)json["userId"];
			this.name = (string)json["name"];
		}
	}
}


using System;
using LitJson;

namespace EightBitIdeas.WebApi.Json
{
	public class LoginResponse
	{
		public string authToken;
		public int userId;
		public int totalTaps;
		
		public LoginResponse(JsonData json)
		{
			this.authToken = (string)json["authToken"];
			this.userId = (int)json["userId"];
			this.totalTaps = (int)json["totalTaps"];
		}
	}
}


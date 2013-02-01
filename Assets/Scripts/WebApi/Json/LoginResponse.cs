using System;

namespace EightBitIdeas.WebApi.Json
{
	public struct LoginResponse
	{
		public string authToken;
		public int userId;
		public int totalTaps;
	}
}


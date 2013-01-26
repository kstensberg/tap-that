using System;

namespace EightBitIdeas.WebApi
{
	public struct LoginResponse
	{
		public string authToken;
		public int userId;
		public int totalTaps;
	}
}


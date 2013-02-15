using System;
using System.Collections.Generic;
using LitJson;
using EightBitIdeas.Lib8bit.Net.Http.WebApi.ApiResponse;

namespace EightBitIdeas.WebApi.Json
{
	public class TapsResponse : IWebApiResponse
	{
		public int rank;
		public int totalTaps;
		public List<LeaderboardResponse> leaderboard;
		
		public void FromJsonData(JsonData json)
		{
			this.rank = (int)json["rank"];
			this.totalTaps = (int)json["totalTaps"];
			this.leaderboard = new List<LeaderboardResponse>();
			
			for (int c = 0; c < json["leaderboard"].Count; c++)
			{
				LeaderboardResponse leaderboardResponse = new LeaderboardResponse();
				leaderboardResponse.FromJsonData(json["leaderboard"][c]);
				
				this.leaderboard.Add(leaderboardResponse);
			}
		}
	}
}


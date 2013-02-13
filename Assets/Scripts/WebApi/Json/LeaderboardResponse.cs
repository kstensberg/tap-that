using System;
using LitJson;
using System.Collections.Generic;
using EightBitIdeas.Lib8bit.Net.Http.WebApi.ApiResponse;

namespace EightBitIdeas.WebApi.Json
{
	public class LeaderboardListResponse : List<LeaderboardResponse>, IWebApiResponse
	{
		public void FromJsonData(JsonData json)
		{
			for (int c = 0; c < json.Count; c++) {
				LeaderboardResponse leaderboardResponse = new LeaderboardResponse();
				
				leaderboardResponse.FromJsonData(json[c]);
				Add(leaderboardResponse);
			}
		}
	}
	
	public class LeaderboardResponse : IWebApiResponse
	{
		public int rank;
		public string name;
		public int totalTaps;
		public int delta;
		
		public void FromJsonData(JsonData json)
		{
			this.rank = (int)json["rank"];
			this.name = (string)json["name"];
			this.totalTaps = (int)json["totalTaps"];
			this.delta = (int)json["delta"];
		}
	}
}


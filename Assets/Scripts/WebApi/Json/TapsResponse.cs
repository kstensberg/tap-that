using System;
using System.Collections.Generic;
using LitJson;

namespace EightBitIdeas.WebApi.Json
{
	public class TapsResponse
	{
		public int rank;
		public int totalTaps;
		public List<LeaderboardResponse> leaderboard;
		
		public TapsResponse(JsonData json)
		{
			this.rank = (int)json["rank"];
			this.totalTaps = (int)json["totalTaps"];
			this.leaderboard = new List<LeaderboardResponse>();
			
			for (int c = 0; c < json["leaderboard"].Count; c++)
			{
				this.leaderboard.Add(new LeaderboardResponse(json["leaderboard"][c]));
			}
		}
	}
	
	public class LeaderboardResponse
	{
		public int rank;
		public string name;
		public int totalTaps;
		public int delta;
		
		public LeaderboardResponse(JsonData json)
		{
			this.rank = (int)json["rank"];
			this.name = (string)json["name"];
			this.totalTaps = (int)json["totalTaps"];
			this.delta = (int)json["delta"];
		}
	}
}


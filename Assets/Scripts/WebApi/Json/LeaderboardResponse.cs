using System;
using System.Collections.Generic;
using LitJson;

namespace EightBitIdeas.WebApi.Json
{
	public class LeaderboardResponse
	{
		public int rank;
		public int totalTaps;
		public List<NearRankLeaderboardEntry> nearRank;
		
		public LeaderboardResponse(JsonData json)
		{
			this.rank = (int)json["rank"];
			this.totalTaps = (int)json["totalTaps"];
			this.nearRank = new List<NearRankLeaderboardEntry>();
			
			foreach (JsonData nearRank in json["nearRank"])
			{
				this.nearRank.Add(new NearRankLeaderboardEntry(nearRank));
			}
		}
	}
	
	public class NearRankLeaderboardEntry
	{
		public int rank;
		public string name;
		public int totalTaps;
		public int delta;
		
		public NearRankLeaderboardEntry(JsonData json)
		{
			this.rank = (int)json["rank"];
			this.name = (string)json["name"];
			this.totalTaps = (int)json["totalTaps"];
			this.delta = (int)json["delta"];
		}
	}
}


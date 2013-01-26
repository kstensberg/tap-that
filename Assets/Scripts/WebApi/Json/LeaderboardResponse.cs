using System;
using System.Collections.Generic;

namespace EightBitIdeas.WebApi.Json
{
	public class LeaderboardResponse
	{
		public int rank;
		public int totalTaps;
		public List<NearRankLeaderboardEntry> nearRank;
	}
	
	public class NearRankLeaderboardEntry
	{
		public int rank;
		public string name;
		public int totalTaps;
		public int delta;
	}
}


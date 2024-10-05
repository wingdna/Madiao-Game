using System;

namespace GameServer_FightTheLandlord
{
	[Serializable]
	public class ResultData
	{
		public int Id;
		public bool IsDeleted;
		public int UserId;
		public int TotalCount;
		public int WinCount;
	}
}


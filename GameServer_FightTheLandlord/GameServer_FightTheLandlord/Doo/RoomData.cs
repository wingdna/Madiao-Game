using System;

namespace GameServer_FightTheLandlord
{
	[Serializable]
	public class RoomData
	{
		public int Id;
		public bool IsDeleted;
		public int RoomId;
		public int UserId;
		public int MaxCount;
	}
}


using System;

namespace TeamZ.GameSaving.States
{
	public class EntityState : MonoBehaviourState
	{
		public Guid Id { get; set; }

		public string AssetGuid { get; set; }

		public UnityEngine.Vector3 Scale { get; set; }

		public UnityEngine.Quaternion Rotation { get; set; }

		public UnityEngine.Vector3 Position { get; set; }

		public Guid LevelId
		{
			get;
			set;
		}
	}
}
namespace TeamZ.GameSaving.States
{
	public class LevelObjectState : MonoBehaviourState
	{
		public int Strength { get; set; }

		public bool IsDestructible { get; set; }

        public bool IsOnlyMovable { get; set; }

		public UnityEngine.Vector3 HighlightingColor { get; set; }

		public bool IsAlreadyExploded { get; set; }
	}
}

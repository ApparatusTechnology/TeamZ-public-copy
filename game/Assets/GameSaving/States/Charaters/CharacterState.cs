
namespace TeamZ.GameSaving.States.Charaters
{
	public abstract class CharacterState : MonoBehaviourState
    {
        public int Health
        {
            get;
            set;
        }

        public int Armor
        {
            get;
            set;
        }

        public int PunchDamage
		{
            get;
            set;
        }

		public int KickDamage
		{
			get;
			set;
		}

        public string Name
        {
            get;
            set;
        }

        public float RunSpeed
        {
            get;
            set;
        }

        public float CreepSpeed
        {
            get;
            set;
        }

        public float StrikeSpeed
        {
            get;
            set;
        }

        public float JumpSpeed
        {
            get;
            set;
        }

        public float JumpForce
        {
            get;
            set;
        }
    }
}

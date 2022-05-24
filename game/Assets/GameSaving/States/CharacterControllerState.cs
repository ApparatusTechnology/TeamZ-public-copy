using System;
using TeamZ.Code.Game.Characters;

namespace TeamZ.GameSaving.States
{
	public class CharacterControllerState : MonoBehaviourState
	{
		[Obsolete]
		public CharacterControllerScript.Direction CurrentDirection { get; set; }

		public bool IsClimbed { get; set; }

		[Obsolete]
        public bool IsKeyUpWasPressed { get; set; }
    }
}

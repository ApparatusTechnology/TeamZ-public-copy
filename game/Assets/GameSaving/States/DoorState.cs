namespace TeamZ.GameSaving.States
{
    public class DoorState : MonoBehaviourState
    {
        public string Name { get; set; }

        public Code.Game.Levels.DoorScript.DoorType Type { get; set; }

        public Code.Game.Levels.DoorScript.DoorState State { get; set; }
    }
}
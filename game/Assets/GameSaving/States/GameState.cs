using System;
using System.Collections.Generic;
using TeamZ.Code.Game.Players;

namespace TeamZ.GameSaving.States
{
    public class GameState
    {
        public GameState()
        {
        }

        public Guid LevelId
        {
            get;
            set;
        }

        public List<GameObjectState> GameObjectsStates
        {
            get;
            set;
        }

        public HashSet<Guid> VisitedLevels
        {
            get;
            set;
        } = new HashSet<Guid>();


        public PlayerServiceState PlayerServiceState
        {
            get;
            set;
        }
    }
}
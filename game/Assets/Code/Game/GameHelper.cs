using UnityEngine;

namespace TeamZ.Code.Game
{
    public static class GameHelper
    {
        public static bool IsPaused => Time.timeScale < 0.1;
        public static bool IsActive => !IsPaused;
    }
}

using System;
using UnityEngine;

namespace TeamZ.Code.Game.Cutscenes
{
    [CreateAssetMenu(fileName = "CutsceneName", menuName = "TeamZ/Cutscene", order = 1)]
    public class CutsceneConfig : ScriptableObject
    {
        [Serializable]
        public class CutscenePage
        {
            public Sprite Sprite;
            public float Duration = 2;
        }
        
        public CutscenePage[] Pages;
    }
}
using System;
using UnityEditor;

namespace TeamZ.Code.Game.Highlighting
{
    [Serializable]
    public class StringContainer<TValue>
    {
        public TValue Value;
        public string TextValue;
    }
}
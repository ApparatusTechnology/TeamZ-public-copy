using UnityEngine;

namespace TeamZ.Code.Game.Grab
{
    public class Grabbable : MonoBehaviour
    {
        public Vector3 Offset;
        public bool LockRotation;

        public Color HighlightingColor = Color.green;
    }
}
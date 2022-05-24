using System;
using UnityEngine;

namespace TeamZ.GameSaving.States
{
    public class CameraState : MonoBehaviourState
    {
        public Guid PlayerId { get; set; }

        public Vector3 Position { get; set; }
    }
}

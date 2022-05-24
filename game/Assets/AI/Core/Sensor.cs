using System;
using UniRx;
using UnityEngine;

namespace TeamZ.AI.Core
{
    public abstract class Sensor : MonoBehaviour
    {
        public ReactiveCollection<GameObject> DetectedObjects { get;  }
            = new ReactiveCollection<GameObject>();

    }
}
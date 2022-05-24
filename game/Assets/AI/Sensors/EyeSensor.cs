using System;
using System.Runtime;
using TeamZ.AI.Core;
using UnityEngine;

namespace TeamZ.AI.Sensors
{
    [RequireComponent(typeof(Collider2D))]
    public class EyeSensor : Sensor
    {
        public LayerMask VisibleLayers;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (this.VisibleLayers.Contains(other.gameObject.layer))
            {
                this.DetectedObjects.Add(other.gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (this.VisibleLayers.Contains(other.gameObject.layer))
            {
                this.DetectedObjects.Remove(other.gameObject);
            }
        }
    }
}
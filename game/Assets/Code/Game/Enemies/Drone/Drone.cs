using System;
using System.Linq;
using TeamZ.AI.Core;
using TeamZ.AI.States;
using UniRx;
using UniRx.Async.Triggers;
using UnityEditor;
using UnityEngine;

namespace TeamZ.Assets.Code.Game.Enemies.Drone
{
    public class Drone : MonoBehaviour
    {
        public Transform[] PatrolPath;

        public AIAgent AIAgent { get; private set; }

        private void Start()
        {
            Observable.Timer(TimeSpan.FromSeconds(1)).Subscribe(_ =>
            {
                this.AIAgent = this.GetComponent<AIAgent>();
                this.AIAgent.SetMindState(new DronePatrol(this.PatrolPath.Select(o => o.position).ToArray()));
            });
        }
    }

    [CustomEditor(typeof(Drone))]
    public class ExampleEditor : Editor
    {
        public void OnSceneGUI()
        {
            var drone = target as Drone;
            GUI.color = Handles.color = new Color(1, 0.8f, 0.4f, 1);

            for (int i = 0; i < drone.PatrolPath.Length; i++)
            {
                var point = drone.PatrolPath[i];
                Handles.DrawWireDisc(point.position, point.forward, 2.0f);
                Handles.Label(point.position, $"P{i}");
            }
        }
    }
}
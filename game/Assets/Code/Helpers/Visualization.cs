using System;
using UniRx;
using UnityEngine;

namespace Code.Helpers
{
    public static class Visualization
    {
        public static void DrawLine(Vector3 position, Vector3 direction, int length = 2, int durationInSeconds = 5)
        {
            var material = GameObject.FindObjectOfType<MaterialStorage>().Standart;
            var gameObject = new GameObject("~Line");
            var line = gameObject.AddComponent<LineRenderer>();
            line.startColor = Color.red;
            line.endColor = new Color(1, 0, 0, 0);
            line.sortingLayerName = "UI";
            line.widthMultiplier = 0.5f;
            line.material = material;
            
            line.SetPositions(new[]
            {
                position,
                position + direction * length
            });

            Observable
                .Timer(TimeSpan.FromSeconds(durationInSeconds))
                .Subscribe(_ => GameObject.Destroy(gameObject));
        }
    }
}
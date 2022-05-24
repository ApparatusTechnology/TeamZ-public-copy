using System;
using System.Threading;
using UniRx.Async;
using UnityEngine;

namespace TeamZ.Code.Helpers.Extentions
{
    public static class UniTaskFixed
    {
        public static async UniTask Delay(TimeSpan time, CancellationToken token = default)
        {
            var startTime = Time.unscaledTime;
            while (Time.unscaledTime - startTime < 5)
            {
                await UniTask.DelayFrame(10);
                if (token.IsCancellationRequested)
                {
                    break;
                }
            }
        }
    }
}
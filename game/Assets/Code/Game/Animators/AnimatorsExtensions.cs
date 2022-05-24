using System.Collections;
using UniRx;
using UniRx.Async;
using UnityEngine;

namespace TeamZ.Assets.Code.Game.Animators
{
    public static class AnimatorsExtensions 
    {
        public static async UniTask Scale(this GameObject gameObject, Vector3 from, Vector3 to, float time)
        {
            await Observable.FromCoroutine(() => ScaleCorutine());

            IEnumerator ScaleCorutine()
            {
                var initialTime = Time.time;
                var scaleDelta = to - from;

                while (gameObject && Time.time - initialTime < time)
                {
                    gameObject.transform.localScale = from + scaleDelta * (Time.time - initialTime) / time;
                    yield return null;
                }

                if (gameObject)
                {
                    gameObject.transform.localScale = to;
                }
            }
        }

        public static async UniTask Move(this GameObject gameObject, Vector3 to, float time)
        {
            Debug.Log("Move anim start");
            await Observable.FromCoroutine(() => MoveCorutine());
            Debug.Log("Move anim stop");

            IEnumerator MoveCorutine()
            {
                var initialTime = Time.time;
                var initialPosition = gameObject.transform.position;
                var delta = to - initialPosition;

                while (gameObject && Time.time - initialTime < time)
                {
                    gameObject.transform.position = initialPosition + delta * (Time.time - initialTime) / time;
                    yield return null;
                }

                if (gameObject)
                {
                    gameObject.transform.position = to;
                }
            }
        }
    }
}

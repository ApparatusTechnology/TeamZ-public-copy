using System.Runtime.CompilerServices;
using TeamZ.Code.Game.Players;
using UnityEngine;

namespace TeamZ
{
    public static class GameObjectExtentions
    {
        public static void Activate(this MonoBehaviour monoBeahviour)
        {
            monoBeahviour.gameObject.SetActive(true);
        }

        public static void Deactivate(this MonoBehaviour monoBeahviour)
        {
            monoBeahviour.gameObject.SetActive(false);
        }

        public static Player GetPlayer(this GameObject gameObject)
            => gameObject.GetComponentInParent<Player>() ?? gameObject.GetComponent<Player>();

        public static void Destroy(this Component monoBehaviour)
        {
            if (!monoBehaviour)
            {
                return;
            }

            GameObject.Destroy(monoBehaviour);
        }

        public static void DestroyGameObject(this Component monoBehaviour)
        {
            if (!monoBehaviour)
            {
                return;
            }

            monoBehaviour.gameObject.Destroy();
        }

        public static void Destroy(this GameObject gameObject)
        {
            if (gameObject)
            {
                GameObject.Destroy(gameObject);
            }
        }

        public static void LookAt2D(this Transform transform, Vector3 target)
        {
            var postion = transform.position;
            var direction = (target - postion).normalized;

            transform.right = -direction;
        }
    }
}
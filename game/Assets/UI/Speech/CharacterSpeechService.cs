using System.Linq;
using TeamZ.Assets.Code.Game.Animators;
using TeamZ.Assets.Code.Helpers;
using TeamZ.Code.DependencyInjection;
using TeamZ.Code.Game.Characters;
using TeamZ.Code.Game.Messages.GameSaving;
using TeamZ.Code.Game.Players;
using TeamZ.Code.Helpers;
using TeamZ.GameSaving;
using TMPro;
using UniRx;
using UniRx.Async;
using UnityEngine;

namespace TeamZ.Assets.UI.Speech
{
    public class CharacterSpeechService : MonoBehaviour
    {
        public GameObject[] SpeechBubblePrefabs;

        private GameObject previousSpeechBox = null;

        Dependency<EntitiesStorage> entityStorage;


        public async UniTask Speech(Player player, string[] messages, float duration = 1)
        {
            var prefab = this.SpeechBubblePrefabs.SelectRandom();
            await this.Speech(player, prefab, messages, duration);
        }

        public async UniTask Speech(Player player, GameObject speechBoxPrefab, string[] messages, float duration = 1)
        {
            if (this.previousSpeechBox)
            {
                await this.previousSpeechBox.gameObject.Scale(Vector3.one, Vector3.zero, 0.1f);

                if (this.previousSpeechBox)
                {
                    GameObject.DestroyImmediate(this.previousSpeechBox.gameObject);
                    this.previousSpeechBox = null;
                }
            }

            var speechAttachPoint = player.GetComponentInChildren<SpeechAttachPoint>().transform;
            this.previousSpeechBox = GameObject.Instantiate(speechBoxPrefab, speechAttachPoint.transform.position, Quaternion.identity, this.entityStorage.Value.Root.transform);
            var speechBoxText = this.previousSpeechBox.GetComponentInChildren<TextMeshPro>();
            speechBoxText.text = messages.First();

            var follower = this.previousSpeechBox.AddComponent<Follower>();
            follower.Transform = this.previousSpeechBox.transform;
            follower.TargetToFollow = speechAttachPoint;

            if (!this.previousSpeechBox)
            {
                return;
            }

            var millisecondsDelay = (int)(duration * 1000);
            await this.previousSpeechBox.gameObject.Scale(Vector3.zero, Vector3.one, 0.1f);
            await UniTask.Delay(millisecondsDelay);

            foreach (var message in messages.Skip(1))
            {
                speechBoxText.text = message;
                await UniTask.Delay(millisecondsDelay);
            }

            if (!this.previousSpeechBox)
            {
                return;
            }
            await this.previousSpeechBox.gameObject.Scale(Vector3.one, Vector3.zero, 0.1f);

            this.previousSpeechBox.Destroy();
            this.previousSpeechBox = null;
        }

        public void Destroy()
        {
            this.previousSpeechBox.Destroy();
        }
    }
}
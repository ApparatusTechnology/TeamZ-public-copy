using System.Collections;
using TeamZ.Code.Game.Characters;
using TeamZ.GameSaving.MonoBehaviours;
using TeamZ.GameSaving.States;
using UniRx;
using UnityEngine;

namespace TeamZ.Code.Game.Levels
{
    public class DoorScript : MonoBehaviourWithState<DoorState>
    {
        public enum DoorType
        {
            Horizontal,
            Vertical,
            Double
        }

        public enum DoorState
        {
            Open,
            Close,
            Deactivate
        }

        public DoorState State = DoorState.Close;
        public DoorType Type = DoorType.Horizontal;

        public GameObject DoorImage;
        public float AnimationSpeed = 15;

        private float doorPosition;
        private RectTransform doorSize;

        // Start is called before the first frame update
        private async void Start()
        {
            if (this.Type == DoorType.Vertical)
                this.doorPosition = this.DoorImage.transform.position.y;
            else
                this.doorPosition = this.DoorImage.transform.position.x;

            this.doorSize = this.DoorImage.GetComponent<RectTransform>();

            switch (this.State)
            {
                case DoorState.Open:
                    await this.StartCoroutine(this.Open());
                    break;
                case DoorState.Close:
                    await this.StartCoroutine(this.Close());
                    break;
            }
        }

        private IEnumerator Open()
        {
            if (this.Type == DoorType.Vertical)
            {
                while (this.DoorImage.transform.position.y < this.doorPosition + 3.8 * this.doorSize.rect.height)
                {
                    var position = this.DoorImage.transform.position;

                    position.y += Time.deltaTime * this.AnimationSpeed;

                    this.DoorImage.transform.position = position;

                    yield return null;
                }
            }
            else if (this.Type == DoorType.Horizontal)
            {
                while (this.DoorImage.transform.position.x > this.doorPosition - this.doorSize.rect.width)
                {
                    var position = this.DoorImage.transform.position;

                    position.x -= Time.deltaTime * this.AnimationSpeed;

                    this.DoorImage.transform.position = position;

                    yield return null;
                }
            }

            this.State = DoorState.Open;
        }

        private IEnumerator Close()
        {
            if (this.Type == DoorType.Vertical)
            {
                while (this.DoorImage.transform.position.y > this.doorPosition)
                {
                    var position = this.DoorImage.transform.position;

                    position.y -= Time.deltaTime * this.AnimationSpeed;

                    this.DoorImage.transform.position = position;

                    yield return null;
                }
            }
            else if (this.Type == DoorType.Horizontal)
            {
                while (this.DoorImage.transform.position.x < this.doorPosition)
                {
                    var position = this.DoorImage.transform.position;

                    position.x += Time.deltaTime * this.AnimationSpeed;

                    this.DoorImage.transform.position = position;

                    yield return null;
                }
            }

            this.State = DoorState.Close;
        }

        private async void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponentInParent<ICharacter>() is ICharacter && this.State != DoorState.Deactivate)
            {
                this.StopCoroutine(this.Open());
                this.StopCoroutine(this.Close());
                await this.StartCoroutine(this.Open());
            }
        }

        private async void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponentInParent<ICharacter>() is ICharacter && this.State != DoorState.Deactivate)
            {
                this.StopCoroutine(this.Open());
                this.StopCoroutine(this.Close());
                await this.StartCoroutine(this.Close());
            }
        }

        public override GameSaving.States.DoorState GetState()
        {
            return new GameSaving.States.DoorState
            {
                Name = this.name,
                Type = this.Type,
                State = this.State
            };
        }

        public override void SetState(GameSaving.States.DoorState state)
        {
            this.name = state.Name;
            this.Type = state.Type;
            this.State = state.State;
        }
    }
}
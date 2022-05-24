using TeamZ.Assets.UI.Terminal;
using TeamZ.Code.Game.Activation.Core;
using TeamZ.Code.Helpers;
using TeamZ.GameSaving.MonoBehaviours;
using TeamZ.GameSaving.States;
using UniRx.Async;
using UnityEngine;

namespace TeamZ.Code.Game.Levels
{
    public class Generator : MonoBehaviourWithState<GeneratorState>, IActivable
    {
        public bool IsActive = true;

        public EntityReference Door;

        public EntityReference Portal;

        private UnityDependency<Terminal> Terminal;

        private bool isActivated = false;

        public void Activate()
        {
            if (!this.IsActive)
            {
                this.Terminal.Value.PrintAndHideAsync("Generator is broken!", 500, 2000, true).Forget();
                return;
            }

            if (this.Door.Entity != null)
            {
                if (this.Door.Entity.GetComponent<DoorScript>().State == DoorScript.DoorState.Deactivate)
                {
                    this.Door.Entity.GetComponent<DoorScript>().State = DoorScript.DoorState.Close;
                }
                else
                {
                    this.Door.Entity.GetComponent<DoorScript>().State = DoorScript.DoorState.Deactivate;
                }
            }
            else
            {
                return;
            }

            if (this.Portal.Entity != null)
            {
                var activated = this.Portal.Entity.GetComponent<Portal>().IsActivated;
                this.Portal.Entity.GetComponent<Portal>().IsActivated = !activated;
            }
            else
            {
                return;
            }

            var tip = GameObject.Find("Lift");

            if (tip != null)
            {
                Destroy(tip);
            }

            this.isActivated = !this.isActivated;

            this.Terminal.Value.PrintAndHideAsync("Generator activated successfully!", 500, 2000, true).Forget();
        }

        public override GeneratorState GetState()
        {
            return new GeneratorState
            {
                Name = this.name,
                IsActive = this.IsActive,
                IsActivated = this.isActivated,
                DoorId = this.Door.Entity.Id,
                PortalId = this.Portal.Entity.Id
            };
        }

        public override void SetState(GeneratorState state)
        {
            this.name = state.Name;
            this.IsActive = state.IsActive;
            this.isActivated = state.IsActivated;
            this.Door = new EntityReference(state.DoorId);
            this.Portal = new EntityReference(state.PortalId);
        }
    }
}
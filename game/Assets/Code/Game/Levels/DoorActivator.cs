using System;
using TeamZ.Assets.UI.Terminal;
using TeamZ.Code.Game.Activation.Core;
using TeamZ.Code.Game.Inventory;
using TeamZ.Code.Helpers;
using TeamZ.GameSaving.MonoBehaviours;
using TeamZ.GameSaving.States;
using UniRx.Async;
using UnityEngine;

namespace TeamZ.Code.Game.Levels
{
    public class DoorActivator : MonoBehaviourWithState<GeneratorState>, IActivable
    {
        public bool IsActive = true;

        public EntityReference Door;

        private UnityDependency<Terminal> Terminal;

        private bool isActivated = false;

        public void Activate()
        {
            if (!this.IsActive)
            {
                this.Terminal.Value.PrintAndHideAsync("Door activator is broken!", 500, 2000, true).Forget();
                return;
            }

            if (this.Door.Entity.GetComponent<DoorScript>().State != DoorScript.DoorState.Deactivate)
            {
                this.Terminal.Value.PrintAndHideAsync("The gate is already unlocked.", 500, 2000, true).Forget();
                return;
            }

            if (!InventoryManager.HasItem<AccessCard>())
            {
                this.Terminal.Value.PrintAndHideAsync("You need an Access Card to open this gate.", 500, 2000, true).Forget();
                return;
            }

            int accessCardIdx = InventoryManager.FindIndexOfItem<AccessCard>();
            InventoryManager.UseAndRemove(accessCardIdx);

            if (this.Door.Entity.GetComponent<DoorScript>().State == DoorScript.DoorState.Deactivate)
            {
                this.Door.Entity.GetComponent<DoorScript>().State = DoorScript.DoorState.Close;
            }
            else
            {
                this.Door.Entity.GetComponent<DoorScript>().State = DoorScript.DoorState.Deactivate;
            }

            this.isActivated = !this.isActivated;

            this.Terminal.Value.PrintAndHideAsync("The door activated successfully!", 500, 2000, true).Forget();
        }

        public override GeneratorState GetState()
        {
            if (this.Door.Entity is null)
            {
                throw new InvalidOperationException($"The door activator '{this.gameObject.name}' doesn't have door to activate.");
            }

            return new GeneratorState
            {
                Name = this.name,
                IsActive = this.IsActive,
                IsActivated = this.isActivated,
                DoorId = this.Door.Entity.Id
            };
        }

        public override void SetState(GeneratorState state)
        {
            this.name = state.Name;
            this.IsActive = state.IsActive;
            this.isActivated = state.IsActivated;
            this.Door = new EntityReference(state.DoorId);
        }
    }
}
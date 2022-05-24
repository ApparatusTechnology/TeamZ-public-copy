using TeamZ.Code.Helpers;
using TeamZ.GameSaving.MonoBehaviours;
using TeamZ.GameSaving.States;
using UniRx.Async;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TeamZ.Code.Game.Cutscenes
{
    public class CutsceneActivatorState : MonoBehaviourState
    {
        public string AssetGuid { get; set; }
    }
    
    public class CutsceneActivator : MonoBehaviourWithState<CutsceneActivatorState>
    {
        public bool NeedDestroyAfterShow = true;

        public UnityDependency<CutsceneService> CutsceneService;

        [AssetReferenceUILabelRestriction("Cutscene")]
        public AssetReference Config;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.GetPlayer())
            {
                this.CutsceneService.Value.ShowCutscene(this).Forget();
            }
        }

        public override CutsceneActivatorState GetState()
            => new CutsceneActivatorState
            {
                AssetGuid = this.Config.AssetGUID
            };

        public override void SetState(CutsceneActivatorState state)
        {
            this.Config = new AssetReference(state.AssetGuid);
        }
    }
}
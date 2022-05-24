using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamZ.Assets.GameSaving.States;
using TeamZ.GameSaving.Interfaces;
using TeamZ.GameSaving.MonoBehaviours;
using UnityEngine;

namespace TeamZ.Assets.GameSaving.MonoBehaviours
{
    public class RendererStateProvider : MonoBehaviourWithState<RenderState>
    {
        public Renderer Renderer;

        public override RenderState GetState()
            => new RenderState
            {
                SortingLayerName = this.Renderer.sortingLayerName,
                SortingLayerOrder = this.Renderer.sortingOrder
            };

        public override void SetState(RenderState state)
        {
            this.Renderer.sortingLayerName = state.SortingLayerName;
            this.Renderer.sortingOrder = state.SortingLayerOrder;
        }
    }
}

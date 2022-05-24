using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamZ.GameSaving.States;
using UnityEngine;

namespace TeamZ.Assets.GameSaving.States
{
    public class RenderState : MonoBehaviourState
    {
        public string SortingLayerName { get; set; }
        public int SortingLayerOrder { get; set; }
    }
}

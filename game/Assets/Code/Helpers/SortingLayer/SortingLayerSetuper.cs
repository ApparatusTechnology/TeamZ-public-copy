using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamZ.Code.Game.Levels;
using UnityEngine;

namespace TeamZ.Assets.Code.Helpers
{
    public class SortingLayerSetuper : MonoBehaviour
    {
        public string SortingLayerName;
        public int SortingLayerOrder;

        private void Start()
        {
            foreach (var item in this.GetComponentsInChildren<Renderer>())
            {
                item.sortingLayerName = this.SortingLayerName;
                item.sortingOrder = this.SortingLayerOrder;
            }
        }

    }
}

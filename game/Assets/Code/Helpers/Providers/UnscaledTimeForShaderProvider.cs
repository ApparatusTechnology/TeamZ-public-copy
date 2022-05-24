using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TeamZ.Assets.Code.Helpers
{
    public class UnscaledTimeForShaderProvider : MonoBehaviour
    {
        private void Update()
        {
            Shader.SetGlobalFloat("_UnscaledTime", Time.unscaledTime);
        }
    }
}

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace t1az2z.Tools.Tools.Common
{
    public class CutoutMask : Image
    {
        public override Material materialForRendering
        {
            get
            {
                var material = new Material(base.materialForRendering);
                material.SetInt("_StencilComp", (int)CompareFunction.NotEqual);
                return material;
            }
        }
    }
}
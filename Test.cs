using TimersSystemUnity.Extension;
using UnityEngine;

namespace TimersSystemUnity
{
    internal class Test
    {
        public void Method()
        {
            var gg = new SpriteRenderer();
            gg.SetAplhaDynamic(3f,5f,3f);

            gg.Ext().SetAlpha(5);

            
        }
    }
}

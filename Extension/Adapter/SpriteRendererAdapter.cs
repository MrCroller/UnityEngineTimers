using TimersSystemUnity.Interfaces;
using UnityEngine;

namespace TimersSystemUnity.Extension.Adapter
{
    public class SpriteRendererAdapter : Component, IColor
    {
        public Color Color { get => _sr.color; set => _sr.color = value; }

        private readonly SpriteRenderer _sr;

        public SpriteRendererAdapter(SpriteRenderer sr) => _sr = sr;
    }
}

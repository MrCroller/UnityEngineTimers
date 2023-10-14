using TimersSystemUnity.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TimersSystemUnity.Extension.Adapter
{
    public class TextAdapter : Component, IColor
    {
        public Color Color { get => _text.color; set => _text.color = value; }

        private readonly TMP_Text _text;

        public TextAdapter(TMP_Text text) => _text = text;
    }
}

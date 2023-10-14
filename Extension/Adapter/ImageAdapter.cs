using TimersSystemUnity.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace TimersSystemUnity.Extension.Adapter
{
    public class ImageAdapter : Component, IColor
    {
        public Color Color { get => _image.color; set => _image.color = value; }

        private Image _image;

        public ImageAdapter(Image image) => _image = image;
    }
}

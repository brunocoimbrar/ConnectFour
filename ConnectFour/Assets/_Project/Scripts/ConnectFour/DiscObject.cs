using UnityEngine;
using UnityEngine.UI;

namespace ConnectFour
{
    public sealed class DiscObject : MonoBehaviour
    {
        [SerializeField]
        private Image _image;

        private Color _defaultColor;

        public int? ControllerIndex { get; set; }

        public Color? Color
        {
            get => _image != null && _image.color != _defaultColor ? _image.color : (Color?)null;
            set
            {
                if (_image != null)
                {
                    _image.color = value.GetValueOrDefault(_defaultColor);
                }
            }
        }

        public Transform Parent => transform.parent;

        public void Initialize()
        {
            if (_image != null)
            {
                _defaultColor = _image.color;
            }
        }
    }
}

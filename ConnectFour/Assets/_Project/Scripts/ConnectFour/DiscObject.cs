using UnityEngine;
using UnityEngine.UI;

namespace ConnectFour
{
    public sealed class DiscObject : MonoBehaviour
    {
        [SerializeField]
        private Image _image;

        public int? ControllerIndex { get; private set; }

        public Transform Parent => transform.parent;

        public void SetControllerIndex(int index, Color? color = null)
        {
            ControllerIndex = index;

            if (color.HasValue && _image != null)
            {
                _image.color = color.Value;
            }
        }
    }
}

using System;
using UnityEngine;
using UnityEngine.UI;

namespace ConnectFour
{
    /// <summary>
    /// A simplified <see cref="AspectRatioFitter"/> that works inside a <see cref="LayoutGroup"/>.
    /// </summary>
    [ExecuteAlways]
    public sealed class AspectRatioFitterLayoutElement : LayoutElement
    {
        public enum AspectMode
        {
            WidthControlsHeight,
            HeightControlsWidth,
        }

        [SerializeField]
        private AspectMode _aspectMode;
        [SerializeField] [Min(0.001f)]
        private float _aspectRatio = 1.0f;

        private RectTransform _rectTransform;

        public float AspectRatio
        {
            get => _aspectRatio;
            set => _aspectRatio = value;
        }

        public AspectMode CurrentAspectMode
        {
            get => _aspectMode;
            set => _aspectMode = value;
        }

        private void Update()
        {
            if (_rectTransform == null)
            {
                _rectTransform = GetComponent<RectTransform>();
            }

            switch (_aspectMode)
            {
                case AspectMode.WidthControlsHeight:
                {
                    float height = _rectTransform.rect.width / _aspectRatio;

                    if (Mathf.Approximately(minHeight, height))
                    {
                        return;
                    }

                    minHeight = height;
                    preferredHeight = height;
                    _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);

                    break;
                }

                case AspectMode.HeightControlsWidth:
                {
                    float width = _rectTransform.rect.height * _aspectRatio;

                    if (Mathf.Approximately(minWidth, width))
                    {
                        return;
                    }

                    minWidth = width;
                    preferredWidth = width;
                    _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);

                    break;
                }

                default:
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}

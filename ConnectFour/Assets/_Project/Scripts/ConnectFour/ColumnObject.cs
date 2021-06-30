using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ConnectFour
{
    public sealed class ColumnObject : MonoBehaviour, IPointerClickHandler
    {
        public delegate void EventHandler(ColumnObject sender);

        public event EventHandler OnClicked;

        [SerializeField]
        private DiscObject _discTemplate;

        private readonly List<DiscObject> _discs = new List<DiscObject>();

        public bool IsFilled
        {
            get
            {
                foreach (DiscObject disc in _discs)
                {
                    if (disc.ControllerIndex == null)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public DiscObject DiscTemplate
        {
            get => _discTemplate;
            set => _discTemplate = value;
        }

        public Transform Parent => transform.parent;

        public void Initialize(int capacity)
        {
            _discs.Capacity = capacity;
            _discs.Clear();
            _discTemplate.gameObject.SetActive(false);

            for (int i = 0; i < capacity; i++)
            {
                _discs.Add(Instantiate(_discTemplate, _discTemplate.Parent, false));
                _discs[i].gameObject.SetActive(true);
            }
        }

        public int? AddControllerIndex(int index, Color? color = null)
        {
            for (int i = 0; i < _discs.Count; i++)
            {
                if (_discs[i].ControllerIndex == null)
                {
                    _discs[i].SetControllerIndex(index, color);

                    return i;
                }
            }

            return null;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClicked?.Invoke(this);
        }
    }
}

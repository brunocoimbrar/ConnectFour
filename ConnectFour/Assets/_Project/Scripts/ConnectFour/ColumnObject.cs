using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ConnectFour
{
    public interface IColumnData
    {
        delegate void EventHandler(IColumnData sender);

        event EventHandler OnClicked;

        int DiscCount { get; }

        int? GetControllerIndex(int discIndex);
    }

    public sealed class ColumnObject : MonoBehaviour, IColumnData, IPointerClickHandler
    {
        public event IColumnData.EventHandler OnClicked;

        [SerializeField]
        private DiscObject _discTemplate;

        private readonly List<DiscObject> _discs = new List<DiscObject>();

        public int DiscCount { get; private set; }

        public DiscObject DiscTemplate
        {
            get => _discTemplate;
            set => _discTemplate = value;
        }

        public Transform Parent => transform.parent;

        public void Initialize(int capacity)
        {
            DiscCount = 0;
            _discs.Capacity = capacity;
            _discs.Clear();
            _discTemplate.gameObject.SetActive(false);

            for (int i = 0; i < capacity; i++)
            {
                _discs.Add(Instantiate(_discTemplate, _discTemplate.Parent, false));
                _discs[i].gameObject.SetActive(true);
            }
        }

        public int? AddControllerIndex(int controllerIndex, Color? color = null)
        {
            if (DiscCount < _discs.Count)
            {
                DiscCount++;

                for (int i = 0; i < _discs.Count; i++)
                {
                    if (_discs[i].ControllerIndex == null)
                    {
                        _discs[i].SetControllerIndex(controllerIndex, color);

                        return i;
                    }
                }
            }

            return null;
        }

        public int? GetControllerIndex(int discIndex)
        {
            return _discs[discIndex].ControllerIndex;
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            OnClicked?.Invoke(this);
        }
    }
}

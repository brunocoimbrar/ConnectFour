using UnityEngine;
using UnityEngine.EventSystems;

namespace ConnectFour
{
    public sealed class ColumnObject : MonoBehaviour, IPointerClickHandler
    {
        public delegate void EventHandler(ColumnObject sender);

        public event EventHandler OnClicked;

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClicked?.Invoke(this);
        }
    }
}

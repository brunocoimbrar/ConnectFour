using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ConnectFour
{
    public interface IColumnEventHandler
    {
        delegate void ColumnEventHandler(int columnIndex);

        event ColumnEventHandler OnColumnClicked;
    }

    [Serializable]
    public sealed class BoardSystem : IDisposable, IColumnEventHandler
    {
        public enum MoveState
        {
            Valid,
            Invalid,
            Win,
            Draw,
        }

        public event IColumnEventHandler.ColumnEventHandler OnColumnClicked;

        [SerializeField] [Min(MinColumnCapacity)]
        private int _columnCapacity = 6;
        [SerializeField]
        private ColumnObject[] _columns = Array.Empty<ColumnObject>();
        [SerializeField]
        private GameObject[] _discPrefabs = Array.Empty<GameObject>();

        private const int MinColumnCapacity = 1;

        public int ColumnCapacity
        {
            get => _columnCapacity;
            set => _columnCapacity = Mathf.Max(MinColumnCapacity, value);
        }

        public ColumnObject[] Columns
        {
            get => _columns;
            set => _columns = value;
        }

        public GameObject[] DiscPrefabs
        {
            get => _discPrefabs;
            set => _discPrefabs = value;
        }

        public MoveState TryMove(int controllerIndex, int columnIndex)
        {
            Transform columnTransform = _columns[columnIndex].transform;

            if (columnTransform.childCount >= _columnCapacity)
            {
                return MoveState.Invalid;
            }

            Object.Instantiate(_discPrefabs[controllerIndex], columnTransform, false);

            // TODO: implement MoveState.Win condition

            foreach (ColumnObject columnObject in _columns)
            {
                if (columnObject.transform.childCount < _columnCapacity)
                {
                    return MoveState.Valid;
                }
            }

            return MoveState.Draw;
        }

        public void Dispose()
        {
            DestroyDiscs();

            foreach (ColumnObject column in _columns)
            {
                column.OnClicked -= HandleColumnClicked;
            }

            OnColumnClicked = null;
        }

        public void Initialize()
        {
            foreach (ColumnObject column in _columns)
            {
                column.OnClicked += HandleColumnClicked;
            }
        }

        public void Restart()
        {
            DestroyDiscs();
        }

        private void DestroyDiscs()
        {
            foreach (ColumnObject column in _columns)
            {
                Transform transform = column.transform;
                int childCount = transform.childCount;

                for (int i = 0; i < childCount; i++)
                {
                    Object.Destroy(transform.GetChild(i).gameObject);
                }
            }
        }

        private void HandleColumnClicked(ColumnObject sender)
        {
            OnColumnClicked?.Invoke(Array.IndexOf(_columns, sender));
        }
    }
}

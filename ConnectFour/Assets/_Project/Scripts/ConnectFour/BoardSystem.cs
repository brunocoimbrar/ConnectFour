using System;
using System.Collections.Generic;
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

        [SerializeField] [Min(MinColumnCount)]
        private int _columnCount = 7;
        [SerializeField] [Min(MinColumnCapacity)]
        private int _columnCapacity = 6;
        [SerializeField]
        private ColumnObject _columnTemplate;
        [SerializeField]
        private Color[] _discColors = Array.Empty<Color>();

        private const int MinColumnCapacity = 1;
        private const int MinColumnCount = 1;
        private readonly List<ColumnObject> _columns = new List<ColumnObject>();

        public int ColumnCapacity
        {
            get => _columnCapacity;
            set => _columnCapacity = Mathf.Max(MinColumnCapacity, value);
        }

        public int ColumnCount
        {
            get => _columnCount;
            set => _columnCount = Mathf.Max(MinColumnCount, value);
        }

        public ColumnObject ColumnTemplate
        {
            get => _columnTemplate;
            set => _columnTemplate = value;
        }

        public Color[] DiscColors
        {
            get => _discColors;
            set => _discColors = value;
        }

        public IReadOnlyList<ColumnObject> Columns => _columns;

        public MoveState TryMove(int controllerIndex, int columnIndex)
        {
            Color? discColor = controllerIndex < _discColors.Length ? _discColors[controllerIndex] : (Color?)null;
            int? discIndex = _columns[columnIndex].AddControllerIndex(controllerIndex, discColor);

            if (discIndex == null)
            {
                return MoveState.Invalid;
            }

            // TODO: implement MoveState.Win condition

            foreach (ColumnObject column in _columns)
            {
                if (!column.IsFilled)
                {
                    return MoveState.Valid;
                }
            }

            return MoveState.Draw;
        }

        public void Dispose()
        {
            foreach (ColumnObject column in _columns)
            {
                Object.Destroy(column.gameObject);
            }
        }

        public void Initialize()
        {
            _columns.Capacity = _columnCount;
            _columns.Clear();
            _columnTemplate.gameObject.SetActive(false);

            for (int i = 0; i < _columnCount; i++)
            {
                _columns.Add(Object.Instantiate(_columnTemplate, _columnTemplate.Parent, false));

                _columns[i].OnClicked += HandleColumnClicked;
                _columns[i].Initialize(_columnCapacity);
                _columns[i].gameObject.SetActive(true);
            }
        }

        private void HandleColumnClicked(ColumnObject sender)
        {
            OnColumnClicked?.Invoke(_columns.IndexOf(sender));
        }
    }
}

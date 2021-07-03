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
        [SerializeField] [Min(MinWinSequenceSize)]
        private int _winSequenceSize = 4;
        [SerializeField]
        private ColumnObject _columnTemplate;
        [SerializeField]
        private Color[] _discColors = Array.Empty<Color>();

        private const int MinColumnCapacity = 1;
        private const int MinColumnCount = 1;
        private const int MinWinSequenceSize = 1;
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

        public int WinSequenceSize
        {
            get => _winSequenceSize;
            set => _winSequenceSize = Mathf.Max(MinWinSequenceSize, value);
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

            if (IsWinMove(controllerIndex, columnIndex))
            {
                return MoveState.Win;
            }

            foreach (ColumnObject column in _columns)
            {
                if (column.DiscCount < ColumnCapacity)
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

        private bool HasDiagonalBackwardSequence(int controllerIndex, int columnIndex)
        {
            int count = 1;
            int discIndex = _columns[columnIndex].DiscCount - 1;

            for (int i = 1; i < _winSequenceSize; i++)
            {
                int targetDiscIndex = discIndex - i;
                int targetColumnIndex = columnIndex + i;

                if (targetDiscIndex >= 0 && targetColumnIndex < _columns.Count && targetDiscIndex < _columns[targetColumnIndex].DiscCount && controllerIndex == _columns[targetColumnIndex].GetControllerIndex(targetDiscIndex))
                {
                    count++;

                    continue;
                }

                break;
            }

            if (count >= WinSequenceSize)
            {
                return true;
            }

            for (int i = 1; i < _winSequenceSize; i++)
            {
                int targetDiscIndex = discIndex + i;
                int targetColumnIndex = columnIndex - i;

                if (targetColumnIndex >= 0 && targetDiscIndex < _columns[targetColumnIndex].DiscCount && controllerIndex == _columns[targetColumnIndex].GetControllerIndex(targetDiscIndex))
                {
                    count++;

                    continue;
                }

                break;
            }

            return count >= WinSequenceSize;
        }

        private bool HasDiagonalForwardSequence(int controllerIndex, int columnIndex)
        {
            int count = 1;
            int discIndex = _columns[columnIndex].DiscCount - 1;

            for (int i = 1; i < _winSequenceSize; i++)
            {
                int targetDiscIndex = discIndex + i;
                int targetColumnIndex = columnIndex + i;

                if (targetColumnIndex < _columns.Count && targetDiscIndex < _columns[targetColumnIndex].DiscCount && controllerIndex == _columns[targetColumnIndex].GetControllerIndex(targetDiscIndex))
                {
                    count++;

                    continue;
                }

                break;
            }

            if (count >= WinSequenceSize)
            {
                return true;
            }

            for (int i = 1; i < _winSequenceSize; i++)
            {
                int targetDiscIndex = discIndex - i;
                int targetColumnIndex = columnIndex - i;

                if (targetColumnIndex >= 0 && targetDiscIndex >= 0 && targetColumnIndex < _columns.Count && targetDiscIndex < _columns[targetColumnIndex].DiscCount && controllerIndex == _columns[targetColumnIndex].GetControllerIndex(targetDiscIndex))
                {
                    count++;

                    continue;
                }

                break;
            }

            return count >= WinSequenceSize;
        }

        private bool HasHorizontalSequence(int controllerIndex, int columnIndex)
        {
            int count = 1;
            int discIndex = _columns[columnIndex].DiscCount - 1;

            for (int i = columnIndex + 1; i < _columnCount; i++)
            {
                if (discIndex < _columns[i].DiscCount && controllerIndex == _columns[i].GetControllerIndex(discIndex))
                {
                    count++;

                    continue;
                }

                break;
            }

            if (count >= WinSequenceSize)
            {
                return true;
            }

            for (int i = columnIndex - 1; i >= 0; i--)
            {
                if (discIndex < _columns[i].DiscCount && controllerIndex == _columns[i].GetControllerIndex(discIndex))
                {
                    count++;

                    continue;
                }

                break;
            }

            return count >= WinSequenceSize;
        }

        private bool HasVerticalSequence(int controllerIndex, int columnIndex)
        {
            if (_columns[columnIndex].DiscCount < _winSequenceSize)
            {
                return false;
            }

            for (int i = _columns[columnIndex].DiscCount - 2; i >= _columns[columnIndex].DiscCount - _winSequenceSize; i--)
            {
                if (controllerIndex != _columns[columnIndex].GetControllerIndex(i))
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsWinMove(int controllerIndex, int columnIndex)
        {
            return HasVerticalSequence(controllerIndex, columnIndex)
                || HasHorizontalSequence(controllerIndex, columnIndex)
                || HasDiagonalForwardSequence(controllerIndex, columnIndex)
                || HasDiagonalBackwardSequence(controllerIndex, columnIndex);
        }
    }
}

using System;
using System.Collections.Generic;

namespace ConnectFour.Tests
{
    public sealed class BoardData : IBoardData
    {
        public event IBoardData.ColumnEventHandler OnColumnClicked;

        public event IBoardData.ColumnEventHandler OnColumnPointerEnter;

        public event IBoardData.ColumnEventHandler OnColumnPointerExit;

        public int ColumnCapacity => 6;

        public int ColumnCount => 7;

        public int WinSequenceSize => 4;

        public IReadOnlyList<IColumnData> Columns => Array.Empty<IColumnData>();

        public void AddPreview(int controllerIndex, int columnIndex) { }

        public void RemovePreview(int columnIndex) { }

        public void Invoke(int columnIndex)
        {
            OnColumnClicked?.Invoke(this, columnIndex);
        }
    }
}

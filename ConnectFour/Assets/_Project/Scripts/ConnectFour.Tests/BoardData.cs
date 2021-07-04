using System;
using System.Collections.Generic;

namespace ConnectFour.Tests
{
    public sealed class BoardData : IBoardData
    {
        public event IBoardData.ColumnEventHandler OnColumnClicked;

        public int ColumnCapacity => 6;

        public int ColumnCount => 7;

        public int WinSequenceSize => 4;

        public IReadOnlyList<IColumnData> Columns => Array.Empty<IColumnData>();

        public void Invoke(int columnIndex)
        {
            OnColumnClicked?.Invoke(this, columnIndex);
        }
    }
}

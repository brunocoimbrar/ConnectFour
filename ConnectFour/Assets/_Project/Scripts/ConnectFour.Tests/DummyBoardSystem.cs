using System;
using System.Collections.Generic;

namespace ConnectFour.Tests
{
    public sealed class DummyBoardSystem : IBoardSystem
    {
        public event IBoardSystem.ColumnEventHandler OnColumnClicked;

        public event IBoardSystem.ColumnEventHandler OnColumnPointerEnter;

        public event IBoardSystem.ColumnEventHandler OnColumnPointerExit;

        public int ColumnCapacity => 6;

        public int ColumnCount => 7;

        public int WinSequenceSize => 4;

        public IReadOnlyList<IColumn> Columns => Array.Empty<IColumn>();

        public void AddPreview(int controllerIndex, int columnIndex) { }

        public void RemovePreview(int columnIndex) { }

        public void Invoke(int columnIndex)
        {
            OnColumnClicked?.Invoke(this, columnIndex);
        }
    }
}

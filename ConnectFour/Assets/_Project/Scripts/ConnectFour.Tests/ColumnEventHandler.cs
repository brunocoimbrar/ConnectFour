namespace ConnectFour.Tests
{
    public sealed class ColumnEventHandler : IColumnEventHandler
    {
        public event IColumnEventHandler.ColumnEventHandler OnColumnClicked;

        public void Invoke(int columnIndex)
        {
            OnColumnClicked?.Invoke(columnIndex);
        }
    }
}

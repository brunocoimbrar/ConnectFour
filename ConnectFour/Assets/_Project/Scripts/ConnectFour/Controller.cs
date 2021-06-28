using UnityEngine;

namespace ConnectFour
{
    public abstract class Controller : ScriptableObject
    {
        public delegate void EndTurnEventHandler(Controller sender, int columnIndex);

        public event EndTurnEventHandler OnTurnEnded;

        protected IColumnEventHandler ColumnEventHandler { get; private set; }

        public abstract void BeginTurn();

        public virtual void Initialize(IColumnEventHandler columnEventHandler)
        {
            ColumnEventHandler = columnEventHandler;
        }

        protected void EndTurn(int columnIndex)
        {
            OnTurnEnded?.Invoke(this, columnIndex);
        }
    }
}

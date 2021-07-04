using UnityEngine;

namespace ConnectFour
{
    public interface IControllerData
    {
        string DisplayName { get; }
    }

    public abstract class Controller : ScriptableObject, IControllerData
    {
        public delegate void EndTurnEventHandler(Controller sender, int columnIndex);

        public event EndTurnEventHandler OnTurnEnded;

        [SerializeField]
        private string _displayName;

        public string DisplayName => _displayName;

        protected IBoardData BoardData { get; private set; }

        protected ITurnData TurnData { get; private set; }

        protected IWorldContext WorldContext { get; private set; }

        public abstract void BeginTurn();

        public virtual void Initialize(IWorldContext worldContext, IBoardData boardData, ITurnData turnData)
        {
            BoardData = boardData;
            TurnData = turnData;
            WorldContext = worldContext;
        }

        protected void EndTurn(int columnIndex)
        {
            OnTurnEnded?.Invoke(this, columnIndex);
        }
    }
}

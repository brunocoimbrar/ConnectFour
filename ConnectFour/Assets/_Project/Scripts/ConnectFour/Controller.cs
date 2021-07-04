using UnityEngine;

namespace ConnectFour
{
    public interface IController
    {
        string DisplayName { get; }
    }

    public abstract class Controller : ScriptableObject, IController
    {
        public delegate void EndTurnEventHandler(Controller controller, int columnIndex);

        public event EndTurnEventHandler OnTurnEnded;

        [SerializeField]
        private string _displayName;

        public string DisplayName => _displayName;

        protected IBoardSystem BoardSystem { get; private set; }

        protected ITurnSystem TurnSystem { get; private set; }

        protected IWorld World { get; private set; }

        public abstract void BeginTurn();

        public virtual void Initialize(IWorld world, IBoardSystem boardSystem, ITurnSystem turnSystem)
        {
            BoardSystem = boardSystem;
            TurnSystem = turnSystem;
            World = world;
        }

        protected void EndTurn(int columnIndex)
        {
            OnTurnEnded?.Invoke(this, columnIndex);
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ConnectFour
{
    public interface ITurnSystem
    {
        IReadOnlyList<IController> Controllers { get; }
    }

    [Serializable]
    public sealed class TurnSystem : IDisposable, ITurnSystem
    {
        public delegate void BeginTurnEventHandler(IController controller);

        public delegate void EndTurnEventHandler(IController controller, int columnIndex);

        public event BeginTurnEventHandler OnTurnBegan;

        public event EndTurnEventHandler OnTurnEnded;

        [SerializeField]
        private Controller[] _controllersAssets;

        private Controller[] _controllers;

        public Controller[] ControllersAssets
        {
            get => _controllersAssets;
            set => _controllersAssets = value;
        }

        public IReadOnlyList<IController> Controllers => _controllers;

        public void Initialize(IWorld world, IBoardSystem boardSystem, ITurnSystem turnSystem)
        {
            _controllers = new Controller[_controllersAssets.Length];

            for (int i = 0; i < _controllers.Length; i++)
            {
                _controllers[i] = Object.Instantiate(_controllersAssets[i]);
                _controllers[i].Initialize(world, boardSystem, turnSystem);
                _controllers[i].OnTurnEnded += HandleControllerTurnEnded;
            }
        }

        public void Dispose()
        {
            foreach (Controller controller in _controllers)
            {
                Object.Destroy(controller);
            }

            OnTurnBegan = null;
            OnTurnEnded = null;
        }

        public void BeginTurn(int turnId)
        {
            Controller controller = _controllers[turnId % _controllers.Length];
            controller.BeginTurn();
            OnTurnBegan?.Invoke(controller);
        }

        private void HandleControllerTurnEnded(Controller controller, int columnIndex)
        {
            OnTurnEnded?.Invoke(controller, columnIndex);
        }
    }
}

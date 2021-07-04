using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ConnectFour
{
    [Serializable]
    public sealed class TurnSystem : IDisposable
    {
        public delegate void BeginTurnEventHandler(IControllerData controllerData);

        public delegate void EndTurnEventHandler(IControllerData controllerData, int columnIndex);

        public event BeginTurnEventHandler OnTurnBegan;

        public event EndTurnEventHandler OnTurnEnded;

        [SerializeField]
        private Controller[] _controllersAssets;

        public Controller[] ControllersAssets
        {
            get => _controllersAssets;
            set => _controllersAssets = value;
        }

        private Controller[] _controllers;

        public IReadOnlyList<IControllerData> Controllers => _controllers;

        public void BeginTurn(int turnId)
        {
            Controller controller = _controllers[turnId % _controllers.Length];
            controller.BeginTurn();
            OnTurnBegan?.Invoke(controller);
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

        public void Initialize(IWorldContext worldContext, IBoardData boardData)
        {
            _controllers = new Controller[_controllersAssets.Length];

            for (int i = 0; i < _controllers.Length; i++)
            {
                _controllers[i] = Object.Instantiate(_controllersAssets[i]);
                _controllers[i].Initialize(worldContext, boardData);
                _controllers[i].OnTurnEnded += HandleControllerTurnEnded;
            }
        }

        private void HandleControllerTurnEnded(Controller sender, int columnIndex)
        {
            OnTurnEnded?.Invoke(sender, columnIndex);
        }
    }
}

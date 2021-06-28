using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ConnectFour
{
    [Serializable]
    public sealed class TurnSystem : IDisposable
    {
        public delegate void EndTurnEventHandler(int controllerIndex, int columnIndex);

        public event EndTurnEventHandler OnTurnEnded;

        [SerializeField]
        private Controller[] _controllersAssets;

        public Controller[] ControllersAssets
        {
            get => _controllersAssets;
            set => _controllersAssets = value;
        }

        private Controller[] _controllers;

        public void BeginTurn(int turnId)
        {
            _controllers[turnId % _controllers.Length].BeginTurn();
        }

        public void Dispose()
        {
            foreach (Controller controller in _controllers)
            {
                Object.Destroy(controller);
            }

            OnTurnEnded = null;
        }

        public void Initialize(IColumnEventHandler columnEventHandler)
        {
            _controllers = new Controller[_controllersAssets.Length];

            for (int i = 0; i < _controllers.Length; i++)
            {
                _controllers[i] = Object.Instantiate(_controllersAssets[i]);
                _controllers[i].Initialize(columnEventHandler);
                _controllers[i].OnTurnEnded += HandleControllerTurnEnded;
            }
        }

        private void HandleControllerTurnEnded(Controller sender, int columnIndex)
        {
            int current = Array.IndexOf(_controllers, sender);
            OnTurnEnded?.Invoke(current, columnIndex);
        }
    }
}

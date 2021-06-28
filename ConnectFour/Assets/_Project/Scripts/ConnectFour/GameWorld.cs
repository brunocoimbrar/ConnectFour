using System;
using UnityEngine;

namespace ConnectFour
{
    public sealed class GameWorld : MonoBehaviour
    {
        [SerializeField]
        private BoardSystem _boardSystem;
        [SerializeField]
        private TurnSystem _turnSystem;

        public BoardSystem BoardSystem
        {
            get => _boardSystem;
            set => _boardSystem = value;
        }

        public TurnSystem TurnSystem
        {
            get => _turnSystem;
            set => _turnSystem = value;
        }

        private void Awake()
        {
            _boardSystem.Initialize();

            _turnSystem.OnTurnEnded += HandlePlayerSystemTurnEnded;
            _turnSystem.Initialize((IColumnEventHandler)_boardSystem);
        }

        private void Start()
        {
            _turnSystem.BeginTurn(0);
        }

        private void OnDestroy()
        {
            _turnSystem.OnTurnEnded -= HandlePlayerSystemTurnEnded;
            _turnSystem.Dispose();
            _boardSystem.Dispose();
        }

        private void HandlePlayerSystemTurnEnded(int controllerIndex, int columnIndex)
        {
            BoardSystem.MoveState state = _boardSystem.TryMove(controllerIndex, columnIndex);

            switch (state)
            {
                case BoardSystem.MoveState.Draw:
                {
                    Debug.Log("Draw");

                    break;
                }

                case BoardSystem.MoveState.Valid:
                {
                    _turnSystem.BeginTurn(controllerIndex + 1);

                    break;
                }

                case BoardSystem.MoveState.Invalid:
                {
                    Debug.Log("Invalid");

                    break;
                }

                case BoardSystem.MoveState.Win:
                {
                    Debug.Log("Win");

                    break;
                }

                default:
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}

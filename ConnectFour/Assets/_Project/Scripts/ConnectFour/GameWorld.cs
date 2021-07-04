using System;
using System.Collections;
using UnityEngine;

namespace ConnectFour
{
    public interface IWorld
    {
        Coroutine StartCoroutine(IEnumerator coroutine);

        void StopCoroutine(Coroutine coroutine);
    }

    public sealed class GameWorld : MonoBehaviour, IWorld
    {
        [SerializeField]
        private BoardSystem _boardSystem;
        [SerializeField]
        private TurnSystem _turnSystem;
        [SerializeField]
        private UISystem _uiSystem;

        public BoardSystem.MoveState LastMoveState { get; private set; }

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

        public UISystem UISystem
        {
            get => _uiSystem;
            set => _uiSystem = value;
        }

        private void Awake()
        {
            _boardSystem.Initialize();

            _turnSystem.OnTurnBegan += HandleTurnBegan;
            _turnSystem.OnTurnEnded += HandleTurnEnded;
            _turnSystem.Initialize(this, _boardSystem, _turnSystem);

            if (_uiSystem != null)
            {
                _uiSystem.OnRestartButtonClick += HandleRestartButtonClick;
                _uiSystem.Initialize(this);
            }
        }

        private void Start()
        {
            _turnSystem.BeginTurn(0);
        }

        private void OnDestroy()
        {
            _uiSystem?.Dispose();
            _turnSystem.Dispose();
            _boardSystem.Dispose();
        }

        private void HandleTurnBegan(IController controller)
        {
            int controllerIndex = _turnSystem.Controllers.IndexOf(controller);
            _uiSystem?.SetController(controllerIndex + 1, controller.DisplayName, _boardSystem.DiscColors[controllerIndex]);
        }

        private void HandleTurnEnded(IController controller, int columnIndex)
        {
            int controllerIndex = _turnSystem.Controllers.IndexOf(controller);
            LastMoveState = _boardSystem.TryMove(controllerIndex, columnIndex);

            switch (LastMoveState)
            {
                case BoardSystem.MoveState.Draw:
                {
                    _uiSystem?.SetDrawMoveFeedbackActive();

                    break;
                }

                case BoardSystem.MoveState.Valid:
                {
                    controllerIndex++;
                    _turnSystem.BeginTurn(controllerIndex);

                    break;
                }

                case BoardSystem.MoveState.Invalid:
                {
                    _turnSystem.BeginTurn(controllerIndex);
                    _uiSystem?.SetInvalidMoveFeedbackActive();

                    break;
                }

                case BoardSystem.MoveState.Win:
                {
                    _uiSystem?.SetWinMoveFeedbackActive();

                    break;
                }

                default:
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void HandleRestartButtonClick()
        {
            StopAllCoroutines();
            OnDestroy();
            Awake();
            Start();
        }
    }
}

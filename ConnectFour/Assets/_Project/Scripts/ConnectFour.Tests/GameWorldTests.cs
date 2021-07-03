using NUnit.Framework;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace ConnectFour.Tests
{
    public sealed class GameWorldTests
    {
        private ColumnObject _columnTemplate;
        private DiscObject _discTemplate;
        private GameWorld _gameWorld;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            GameObject gameWorldGameObject = new GameObject(nameof(GameWorld));
            gameWorldGameObject.SetActive(false);
            _gameWorld = gameWorldGameObject.AddComponent<GameWorld>();
            _columnTemplate = new GameObject(nameof(ColumnObject)).AddComponent<ColumnObject>();
            _columnTemplate.transform.parent = _gameWorld.transform;
            _discTemplate = new GameObject(nameof(DiscObject)).AddComponent<DiscObject>();
            _discTemplate.transform.parent = _columnTemplate.transform;
            _columnTemplate.DiscTemplate = _discTemplate;

            _gameWorld.BoardSystem = new BoardSystem
            {
                ColumnCount = 7,
                ColumnCapacity = 6,
                WinSequenceSize = 4,
                ColumnTemplate = _columnTemplate,
                DiscColors = Array.Empty<Color>(),
            };

            _gameWorld.TurnSystem = new TurnSystem
            {
                ControllersAssets = new Controller[]
                {
                    ScriptableObject.CreateInstance<PlayerController>(),
                    ScriptableObject.CreateInstance<PlayerController>(),
                },
            };

            _gameWorld.gameObject.SetActive(true);

            // skip 2 frames to ensure that both Awake and Start gets called
            yield return null;
            yield return null;
        }

        [TearDown]
        public void TearDown()
        {
            foreach (Controller controllerAsset in _gameWorld.TurnSystem.ControllersAssets)
            {
                Object.Destroy(controllerAsset);
            }

            Object.Destroy(_gameWorld);

            _gameWorld = null;
            _columnTemplate = null;
            _discTemplate = null;
        }

        [Test]
        public void DrawFlow()
        {
            Click(3);
            Assert.AreEqual(BoardSystem.MoveState.Valid, _gameWorld.LastMoveState);

            for (int i = 0; i < 3; i++)
            {
                do
                {
                    Click(i);
                }
                while (_gameWorld.LastMoveState == BoardSystem.MoveState.Valid);
            }

            for (int i = 4; i < _gameWorld.BoardSystem.ColumnCount; i++)
            {
                do
                {
                    Click(i);
                }
                while (_gameWorld.LastMoveState == BoardSystem.MoveState.Valid);
            }

            do
            {
                Click(3);
            }
            while (_gameWorld.LastMoveState == BoardSystem.MoveState.Valid);

            Assert.AreEqual(BoardSystem.MoveState.Draw, _gameWorld.LastMoveState);
        }

        [Test]
        public void WinFlow_DiagonalBackward()
        {
            Click(3);
            Assert.AreEqual(BoardSystem.MoveState.Valid, _gameWorld.LastMoveState);
            Click(2);
            Assert.AreEqual(BoardSystem.MoveState.Valid, _gameWorld.LastMoveState);
            Click(1);
            Assert.AreEqual(BoardSystem.MoveState.Valid, _gameWorld.LastMoveState);
            Click(0);
            Assert.AreEqual(BoardSystem.MoveState.Valid, _gameWorld.LastMoveState);
            Click(2);
            Assert.AreEqual(BoardSystem.MoveState.Valid, _gameWorld.LastMoveState);
            Click(1);
            Assert.AreEqual(BoardSystem.MoveState.Valid, _gameWorld.LastMoveState);
            Click(0);
            Assert.AreEqual(BoardSystem.MoveState.Valid, _gameWorld.LastMoveState);
            Click(0);
            Assert.AreEqual(BoardSystem.MoveState.Valid, _gameWorld.LastMoveState);
            Click(1);
            Assert.AreEqual(BoardSystem.MoveState.Valid, _gameWorld.LastMoveState);
            Click(1);
            Assert.AreEqual(BoardSystem.MoveState.Valid, _gameWorld.LastMoveState);
            Click(0);
            Assert.AreEqual(BoardSystem.MoveState.Win, _gameWorld.LastMoveState);
        }

        [Test]
        public void WinFlow_DiagonalForward()
        {
            Click(0);
            Assert.AreEqual(BoardSystem.MoveState.Valid, _gameWorld.LastMoveState);
            Click(1);
            Assert.AreEqual(BoardSystem.MoveState.Valid, _gameWorld.LastMoveState);
            Click(2);
            Assert.AreEqual(BoardSystem.MoveState.Valid, _gameWorld.LastMoveState);
            Click(3);
            Assert.AreEqual(BoardSystem.MoveState.Valid, _gameWorld.LastMoveState);
            Click(1);
            Assert.AreEqual(BoardSystem.MoveState.Valid, _gameWorld.LastMoveState);
            Click(2);
            Assert.AreEqual(BoardSystem.MoveState.Valid, _gameWorld.LastMoveState);
            Click(3);
            Assert.AreEqual(BoardSystem.MoveState.Valid, _gameWorld.LastMoveState);
            Click(3);
            Assert.AreEqual(BoardSystem.MoveState.Valid, _gameWorld.LastMoveState);
            Click(2);
            Assert.AreEqual(BoardSystem.MoveState.Valid, _gameWorld.LastMoveState);
            Click(2);
            Assert.AreEqual(BoardSystem.MoveState.Valid, _gameWorld.LastMoveState);
            Click(3);
            Assert.AreEqual(BoardSystem.MoveState.Win, _gameWorld.LastMoveState);
        }

        [Test]
        public void WinFlow_Horizontal()
        {
            for (int i = 0; i < _gameWorld.BoardSystem.ColumnCapacity; i++)
            {
                Click(0);
                Assert.AreEqual(BoardSystem.MoveState.Valid, _gameWorld.LastMoveState);
            }

            Click(0);
            Assert.AreEqual(BoardSystem.MoveState.Invalid, _gameWorld.LastMoveState);

            for (int i = 1; i <= 2; i++)
            {
                Click(i);
                Assert.AreEqual(BoardSystem.MoveState.Valid, _gameWorld.LastMoveState);
                Click(i);
                Assert.AreEqual(BoardSystem.MoveState.Valid, _gameWorld.LastMoveState);
            }

            Click(3);
            Assert.AreEqual(BoardSystem.MoveState.Win, _gameWorld.LastMoveState);
        }

        [Test]
        public void WinFlow_Vertical()
        {
            for (int i = 0; i < _gameWorld.BoardSystem.WinSequenceSize - 1; i++)
            {
                Click(0);
                Assert.AreEqual(BoardSystem.MoveState.Valid, _gameWorld.LastMoveState);
                Click(1);
                Assert.AreEqual(BoardSystem.MoveState.Valid, _gameWorld.LastMoveState);
            }

            Click(0);
            Assert.AreEqual(BoardSystem.MoveState.Win, _gameWorld.LastMoveState);
        }

        private void Click(int columnIndex)
        {
            ExecuteEvents.Execute(_gameWorld.BoardSystem.Columns[columnIndex].gameObject, null, delegate(IPointerClickHandler handler, BaseEventData data)
            {
                handler.OnPointerClick(null);
            });
        }
    }
}

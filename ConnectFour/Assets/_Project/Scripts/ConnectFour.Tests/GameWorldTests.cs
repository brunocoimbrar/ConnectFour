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

        [SetUp]
        public void SetUp()
        {
            GameObject gameWorldGameObject = new GameObject(nameof(GameWorld));
            gameWorldGameObject.SetActive(false);
            _gameWorld = gameWorldGameObject.AddComponent<GameWorld>();
            _columnTemplate = new GameObject(nameof(ColumnObject)).AddComponent<ColumnObject>();
            _columnTemplate.transform.parent = _gameWorld.transform;
            _discTemplate = new GameObject(nameof(DiscObject)).AddComponent<DiscObject>();
            _discTemplate.transform.parent = _columnTemplate.transform;
            _columnTemplate.DiscTemplate = _discTemplate;
        }

        [TearDown]
        public void TearDown()
        {
            Object.Destroy(_gameWorld);

            _gameWorld = null;
            _columnTemplate = null;
            _discTemplate = null;
        }

        [UnityTest]
        public IEnumerator SampleTest()
        {
            _gameWorld.BoardSystem = new BoardSystem
            {
                ColumnCount = 7,
                ColumnCapacity = 6,
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

            // skip 2 turns to ensure that both Awake and Start gets called
            yield return null;
            yield return null;

            Assert.Fail();

            foreach (Controller controllerAsset in _gameWorld.TurnSystem.ControllersAssets)
            {
                Object.Destroy(controllerAsset);
            }
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

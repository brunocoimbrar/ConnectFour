using NUnit.Framework;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ConnectFour.Tests
{
    public class BoardSystemTests
    {
        private ColumnObject _columnTemplate;
        private DiscObject _discTemplate;

        [SetUp]
        public void SetUp()
        {
            _columnTemplate = new GameObject(nameof(ColumnObject)).AddComponent<ColumnObject>();
            _discTemplate = new GameObject(nameof(DiscObject)).AddComponent<DiscObject>();
            _discTemplate.transform.parent = _columnTemplate.transform;
            _columnTemplate.DiscTemplate = _discTemplate;
        }

        [TearDown]
        public void TearDown()
        {
            UnityEngine.Object.Destroy(_columnTemplate);

            _columnTemplate = null;
            _discTemplate = null;
        }

        [Test]
        public void CanMoveUntilColumnCapacityIsReached()
        {
            using BoardSystem boardSystem = new BoardSystem
            {
                ColumnCount = 2,
                ColumnCapacity = 2,
                ColumnTemplate = _columnTemplate,
                DiscColors = Array.Empty<Color>(),
            };

            boardSystem.Initialize();
            Assert.AreEqual(BoardSystem.MoveState.Valid, boardSystem.TryMove(0, 0));
            Assert.AreEqual(BoardSystem.MoveState.Valid, boardSystem.TryMove(0, 0));
            Assert.AreEqual(BoardSystem.MoveState.Invalid, boardSystem.TryMove(0, 0));
        }

        [Test]
        public void CanMoveUntilAllColumnCapacityAreReached()
        {
            using BoardSystem boardSystem = new BoardSystem
            {
                ColumnCount = 2,
                ColumnCapacity = 2,
                ColumnTemplate = _columnTemplate,
                DiscColors = Array.Empty<Color>(),
            };

            boardSystem.Initialize();
            Assert.AreEqual(BoardSystem.MoveState.Valid, boardSystem.TryMove(0, 0));
            Assert.AreEqual(BoardSystem.MoveState.Valid, boardSystem.TryMove(0, 0));
            Assert.AreEqual(BoardSystem.MoveState.Valid, boardSystem.TryMove(0, 1));
            Assert.AreEqual(BoardSystem.MoveState.Draw, boardSystem.TryMove(0, 1));
        }

        [Test]
        public void ColumnClickedReturnsCorrectColumnIndex()
        {
            using BoardSystem boardSystem = new BoardSystem
            {
                ColumnCount = 6,
                ColumnCapacity = 2,
                ColumnTemplate = _columnTemplate,
                DiscColors = Array.Empty<Color>(),
            };

            boardSystem.Initialize();

            int expected = -1;

            void handleColumnClicked(IBoardSystem boardData, int columnIndex)
            {
                Assert.AreEqual(expected, columnIndex);
                Assert.AreEqual(expected, columnIndex);
            }

            boardSystem.OnColumnClicked += handleColumnClicked;

            for (int i = 0; i < boardSystem.ColumnCount; i++)
            {
                int index = i;
                expected = index;
                ExecuteEvents.Execute(((ColumnObject)boardSystem.Columns[index]).gameObject, null, delegate (IPointerClickHandler handler, BaseEventData data)
                {
                    handler.OnPointerClick(null);
                });
            }

            Assert.AreNotEqual(expected, -1);
        }
    }
}

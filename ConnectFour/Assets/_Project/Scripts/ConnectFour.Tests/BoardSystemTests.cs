using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ConnectFour.Tests
{
    public class BoardSystemTests
    {
        [Test]
        public void CanMoveUntilColumnCapacityIsReached()
        {
            ColumnObject[] columnObjects =
            {
                new GameObject(nameof(ColumnObject)).AddComponent<ColumnObject>(),
                new GameObject(nameof(ColumnObject)).AddComponent<ColumnObject>(),
            };

            GameObject discPrefab = new GameObject("DiscPrefab");

            using (BoardSystem boardSystem = new BoardSystem())
            {
                boardSystem.Columns = columnObjects;
                boardSystem.DiscPrefabs = new[]
                {
                    discPrefab,
                };

                boardSystem.ColumnCapacity = 2;
                boardSystem.Initialize();
                Assert.AreEqual(BoardSystem.MoveState.Valid, boardSystem.TryMove(0, 0));
                Assert.AreEqual(BoardSystem.MoveState.Valid, boardSystem.TryMove(0, 0));
                Assert.AreEqual(BoardSystem.MoveState.Invalid, boardSystem.TryMove(0, 0));
            }

            Object.Destroy(discPrefab);

            foreach (ColumnObject columnObject in columnObjects)
            {
                Object.Destroy(columnObject);
            }
        }
        [Test]
        public void CanMoveUntilAllColumnCapacityAreReached()
        {
            ColumnObject[] columnObjects =
            {
                new GameObject(nameof(ColumnObject)).AddComponent<ColumnObject>(),
                new GameObject(nameof(ColumnObject)).AddComponent<ColumnObject>(),
            };

            GameObject discPrefab = new GameObject("DiscPrefab");

            using (BoardSystem boardSystem = new BoardSystem())
            {
                boardSystem.Columns = columnObjects;
                boardSystem.DiscPrefabs = new[]
                {
                    discPrefab,
                };

                boardSystem.ColumnCapacity = 2;
                boardSystem.Initialize();
                Assert.AreEqual(BoardSystem.MoveState.Valid, boardSystem.TryMove(0, 0));
                Assert.AreEqual(BoardSystem.MoveState.Valid, boardSystem.TryMove(0, 0));
                Assert.AreEqual(BoardSystem.MoveState.Valid, boardSystem.TryMove(0, 1));
                Assert.AreEqual(BoardSystem.MoveState.Draw, boardSystem.TryMove(0, 1));
            }

            Object.Destroy(discPrefab.gameObject);

            foreach (ColumnObject columnObject in columnObjects)
            {
                Object.Destroy(columnObject.gameObject);
            }
        }

        [Test]
        public void ColumnClickedReturnsCorrectColumnIndex()
        {
            ColumnObject[] columnObjects =
            {
                new GameObject(nameof(ColumnObject)).AddComponent<ColumnObject>(),
                new GameObject(nameof(ColumnObject)).AddComponent<ColumnObject>(),
                new GameObject(nameof(ColumnObject)).AddComponent<ColumnObject>(),
                new GameObject(nameof(ColumnObject)).AddComponent<ColumnObject>(),
                new GameObject(nameof(ColumnObject)).AddComponent<ColumnObject>(),
                new GameObject(nameof(ColumnObject)).AddComponent<ColumnObject>(),
            };

            GameObject discPrefab = new GameObject("DiscPrefab");

            using (BoardSystem boardSystem = new BoardSystem())
            {
                boardSystem.Columns = columnObjects;
                boardSystem.DiscPrefabs = new[]
                {
                    discPrefab,
                };

                boardSystem.ColumnCapacity = 1;
                boardSystem.Initialize();

                int expected = -1;

                void handleColumnClicked(int columnIndex)
                {
                    Assert.AreEqual(expected, columnIndex);
                }

                boardSystem.OnColumnClicked += handleColumnClicked;

                for (int i = 0; i < columnObjects.Length; i++)
                {
                    int index = i;
                    expected = index;
                    ExecuteEvents.Execute<IPointerClickHandler>(columnObjects[index].gameObject, null, delegate
                    {
                        columnObjects[index].OnPointerClick(null);
                    });
                }

                Assert.AreNotEqual(expected, -1);
            }

            Object.Destroy(discPrefab.gameObject);

            foreach (ColumnObject columnObject in columnObjects)
            {
                Object.Destroy(columnObject.gameObject);
            }
        }
    }
}

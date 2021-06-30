using NUnit.Framework;
using UnityEngine;

namespace ConnectFour.Tests
{
    public class TurnSystemTests
    {
        [Test]
        public void HumanVsHuman3TurnsFlow()
        {
            ColumnEventHandler columnEventHandler = new ColumnEventHandler();
            TurnSystem turnSystem = new TurnSystem
            {
                ControllersAssets = new Controller[]
                {
                    ScriptableObject.CreateInstance<PlayerController>(),
                    ScriptableObject.CreateInstance<PlayerController>(),
                },
            };

            turnSystem.Initialize(columnEventHandler);

            bool isTriggered = false;
            int currentTurnIndex = 0;
            int randomColumnIndex = Random.Range(0, 6);

            void handleTurnEnded(int controllerIndex, int columnIndex)
            {
                isTriggered = true;
                Assert.AreEqual(currentTurnIndex % turnSystem.ControllersAssets.Length, controllerIndex);
                Assert.AreEqual(randomColumnIndex, columnIndex);
            }

            turnSystem.OnTurnEnded += handleTurnEnded;

            void endTurn()
            {
                turnSystem.BeginTurn(currentTurnIndex);
                columnEventHandler.Invoke(randomColumnIndex);
                Assert.IsTrue(isTriggered);

                isTriggered = false;
                currentTurnIndex++;
                randomColumnIndex = Random.Range(0, 6);
            }

            // first turn
            endTurn();

            // second turn
            endTurn();

            // third turn
            endTurn();

            turnSystem.OnTurnEnded -= handleTurnEnded;
            turnSystem.Dispose();

            foreach (Controller controllerAsset in turnSystem.ControllersAssets)
            {
                Object.Destroy(controllerAsset);
            }
        }
    }
}

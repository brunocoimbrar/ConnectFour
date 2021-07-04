using NUnit.Framework;
using UnityEngine;

namespace ConnectFour.Tests
{
    public class TurnSystemTests
    {
        [Test]
        public void HumanVsHuman3TurnsFlow()
        {
            BoardData boardData = new BoardData();
            TurnSystem turnSystem = new TurnSystem
            {
                ControllersAssets = new Controller[]
                {
                    ScriptableObject.CreateInstance<PlayerController>(),
                    ScriptableObject.CreateInstance<PlayerController>(),
                },
            };

            turnSystem.Initialize(null, boardData);

            bool isTriggered = false;
            int currentTurnIndex = 0;
            int randomColumnIndex = Random.Range(0, 6);

            void handleTurnEnded(IControllerData controllerData, int columnIndex)
            {
                isTriggered = true;
                Assert.AreEqual(turnSystem.Controllers[currentTurnIndex % turnSystem.Controllers.Count], controllerData);
                Assert.AreEqual(randomColumnIndex, columnIndex);
            }

            turnSystem.OnTurnEnded += handleTurnEnded;

            void endTurn()
            {
                turnSystem.BeginTurn(currentTurnIndex);
                boardData.Invoke(randomColumnIndex);
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

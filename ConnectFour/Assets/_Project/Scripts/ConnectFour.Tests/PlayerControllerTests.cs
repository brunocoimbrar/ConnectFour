using NUnit.Framework;
using UnityEngine;

namespace ConnectFour.Tests
{
    public class PlayerControllerTests
    {
        [Test]
        public void EndTurnOnColumnClicked()
        {
            ColumnEventHandler columnEventHandler = new ColumnEventHandler();
            PlayerController playerController = ScriptableObject.CreateInstance<PlayerController>();
            playerController.Initialize(columnEventHandler);

            bool isTriggered = false;
            int randomColumnIndex = Random.Range(0, 6);

            void handleTurnEnded(Controller sender, int columnIndex)
            {
                isTriggered = true;
                Assert.AreEqual(randomColumnIndex, columnIndex);
            }

            playerController.OnTurnEnded += handleTurnEnded;
            playerController.BeginTurn();
            columnEventHandler.Invoke(randomColumnIndex);
            Assert.IsTrue(isTriggered);

            playerController.OnTurnEnded -= handleTurnEnded;
            Object.Destroy(playerController);
        }
    }
}

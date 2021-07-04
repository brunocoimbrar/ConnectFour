using NUnit.Framework;
using UnityEngine;

namespace ConnectFour.Tests
{
    public class PlayerControllerTests
    {
        [Test]
        public void EndTurnOnColumnClicked()
        {
            BoardData boardData = new BoardData();
            PlayerController playerController = ScriptableObject.CreateInstance<PlayerController>();
            playerController.Initialize(null, boardData);

            bool isTriggered = false;
            int randomColumnIndex = Random.Range(0, 6);

            void handleTurnEnded(Controller sender, int columnIndex)
            {
                isTriggered = true;
                Assert.AreEqual(randomColumnIndex, columnIndex);
            }

            playerController.OnTurnEnded += handleTurnEnded;
            playerController.BeginTurn();
            boardData.Invoke(randomColumnIndex);
            Assert.IsTrue(isTriggered);

            playerController.OnTurnEnded -= handleTurnEnded;
            Object.Destroy(playerController);
        }
    }
}

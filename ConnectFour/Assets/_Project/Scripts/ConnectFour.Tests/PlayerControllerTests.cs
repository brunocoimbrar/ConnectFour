using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectFour.Tests
{
    public class PlayerControllerTests
    {
        public sealed class TurnData : ITurnData
        {
            public IReadOnlyList<IControllerData> Controllers { get; private set; }

            public TurnData(params IControllerData[] controllerDatas)
            {
                Controllers = controllerDatas;
            }
        }

        [Test]
        public void EndTurnOnColumnClicked()
        {
            BoardData boardData = new BoardData();
            PlayerController playerController = ScriptableObject.CreateInstance<PlayerController>();
            playerController.Initialize(null, boardData, new TurnData(playerController));

            bool isTriggered = false;
            int randomColumnIndex = UnityEngine.Random.Range(0, 6);

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
            UnityEngine.Object.Destroy(playerController);
        }
    }
}

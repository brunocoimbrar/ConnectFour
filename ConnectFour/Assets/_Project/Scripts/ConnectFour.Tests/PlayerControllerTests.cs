using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectFour.Tests
{
    public class PlayerControllerTests
    {
        public sealed class TurnSystem : ITurnSystem
        {
            public IReadOnlyList<IController> Controllers { get; }

            public TurnSystem(params IController[] controllerDatas)
            {
                Controllers = controllerDatas;
            }
        }

        [Test]
        public void EndTurnOnColumnClicked()
        {
            DummyBoardSystem dummyBoardSystem = new DummyBoardSystem();
            PlayerController playerController = ScriptableObject.CreateInstance<PlayerController>();
            playerController.Initialize(null, dummyBoardSystem, new TurnSystem(playerController));

            bool isTriggered = false;
            int randomColumnIndex = UnityEngine.Random.Range(0, 6);

            void handleTurnEnded(Controller sender, int columnIndex)
            {
                isTriggered = true;
                Assert.AreEqual(randomColumnIndex, columnIndex);
            }

            playerController.OnTurnEnded += handleTurnEnded;
            playerController.BeginTurn();
            dummyBoardSystem.Invoke(randomColumnIndex);
            Assert.IsTrue(isTriggered);

            playerController.OnTurnEnded -= handleTurnEnded;
            UnityEngine.Object.Destroy(playerController);
        }
    }
}

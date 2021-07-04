using UnityEngine;

namespace ConnectFour
{
    [CreateAssetMenu(menuName = "Create PlayerController", fileName = "PlayerController", order = 0)]
    public sealed class PlayerController : Controller
    {
        public override void BeginTurn()
        {
            BoardData.OnColumnClicked += HandleBoardSystemColumnClicked;
        }

        private void HandleBoardSystemColumnClicked(IBoardData boardData, int columnIndex)
        {
            BoardData.OnColumnClicked -= HandleBoardSystemColumnClicked;
            EndTurn(columnIndex);
        }
    }
}

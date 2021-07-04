using UnityEngine;

namespace ConnectFour
{
    [CreateAssetMenu(menuName = "Create PlayerController", fileName = "PlayerController", order = 0)]
    public sealed class PlayerController : Controller
    {
        public override void BeginTurn()
        {
            BoardData.OnColumnClicked += HandleBoardDataColumnClicked;
            BoardData.OnColumnPointerEnter += HandleBoardDataColumnPointerEnter;
            BoardData.OnColumnPointerExit += HandleBoardDataColumnPointerExit;
        }

        private void HandleBoardDataColumnClicked(IBoardData boardData, int columnIndex)
        {
            BoardData.OnColumnClicked -= HandleBoardDataColumnClicked;
            BoardData.OnColumnPointerEnter -= HandleBoardDataColumnPointerEnter;
            BoardData.OnColumnPointerExit -= HandleBoardDataColumnPointerExit;
            BoardData.RemovePreview(columnIndex);
            EndTurn(columnIndex);
        }

        private void HandleBoardDataColumnPointerEnter(IBoardData boardData, int columnIndex)
        {
            BoardData.AddPreview(TurnData.Controllers.IndexOf(this), columnIndex);
        }

        private void HandleBoardDataColumnPointerExit(IBoardData boardData, int columnIndex)
        {
            BoardData.RemovePreview(columnIndex);
        }
    }
}

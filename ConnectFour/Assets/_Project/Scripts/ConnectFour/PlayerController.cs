using UnityEngine;

namespace ConnectFour
{
    [CreateAssetMenu(menuName = "Create PlayerController", fileName = "PlayerController", order = 0)]
    public sealed class PlayerController : Controller
    {
        public override void BeginTurn()
        {
            ColumnEventHandler.OnColumnClicked += HandleBoardSystemColumnClicked;
        }

        private void HandleBoardSystemColumnClicked(int columnIndex)
        {
            ColumnEventHandler.OnColumnClicked -= HandleBoardSystemColumnClicked;
            EndTurn(columnIndex);
        }
    }
}

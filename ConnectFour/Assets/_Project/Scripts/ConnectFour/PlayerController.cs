using UnityEngine;

namespace ConnectFour
{
    [CreateAssetMenu(menuName = "Create PlayerController", fileName = "PlayerController", order = 0)]
    public sealed class PlayerController : Controller
    {
        public override void BeginTurn()
        {
            BoardSystem.OnColumnClicked += HandleColumnClicked;
            BoardSystem.OnColumnPointerEnter += HandleColumnPointerEnter;
            BoardSystem.OnColumnPointerExit += HandleColumnPointerExit;
        }

        private void HandleColumnClicked(IBoardSystem boardSystem, int columnIndex)
        {
            BoardSystem.OnColumnClicked -= HandleColumnClicked;
            BoardSystem.OnColumnPointerEnter -= HandleColumnPointerEnter;
            BoardSystem.OnColumnPointerExit -= HandleColumnPointerExit;
            BoardSystem.RemovePreview(columnIndex);
            EndTurn(columnIndex);
        }

        private void HandleColumnPointerEnter(IBoardSystem boardSystem, int columnIndex)
        {
            BoardSystem.AddPreview(TurnSystem.Controllers.IndexOf(this), columnIndex);
        }

        private void HandleColumnPointerExit(IBoardSystem boardSystem, int columnIndex)
        {
            BoardSystem.RemovePreview(columnIndex);
        }
    }
}

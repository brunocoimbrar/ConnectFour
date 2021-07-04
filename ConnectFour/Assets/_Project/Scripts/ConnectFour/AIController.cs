using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectFour
{
    [CreateAssetMenu(menuName = "Create AIController", fileName = "AIController", order = 0)]
    public sealed class AIController : Controller
    {
        [SerializeField]
        private float _previewDelay = 0.25f;
        [SerializeField]
        private float _moveDelay = 0.25f;

        private static readonly List<int> CachedChoices = new List<int>();

        public override void BeginTurn()
        {
            IEnumerator coroutine()
            {
                IReadOnlyList<IColumn> columns = BoardSystem.Columns;
                CachedChoices.Clear();

                for (int i = 0; i < columns.Count; i++)
                {
                    if (columns[i].DiscCount < BoardSystem.ColumnCapacity)
                    {
                        CachedChoices.Add(i);
                    }
                }

                int choice = CachedChoices[Random.Range(0, CachedChoices.Count)];

                yield return new WaitForSeconds(_previewDelay);

                BoardSystem.AddPreview(TurnSystem.Controllers.IndexOf(this), choice);

                yield return new WaitForSeconds(_moveDelay);

                BoardSystem.RemovePreview(choice);
                EndTurn(choice);
            }

            World.StartCoroutine(coroutine());
        }
    }
}

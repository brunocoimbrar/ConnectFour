using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectFour
{
    [CreateAssetMenu(menuName = "Create AIController", fileName = "AIController", order = 0)]
    public sealed class AIController : Controller
    {
        [SerializeField]
        private float _moveDelay = 1;

        private static readonly List<int> CachedChoices = new List<int>();

        public override void BeginTurn()
        {
            IEnumerator coroutine()
            {
                yield return new WaitForSeconds(_moveDelay);

                IReadOnlyList<IColumn> columns = BoardSystem.Columns;
                CachedChoices.Clear();

                for (int i = 0; i < columns.Count; i++)
                {
                    if (columns[i].DiscCount < BoardSystem.ColumnCapacity)
                    {
                        CachedChoices.Add(i);
                    }
                }

                EndTurn(CachedChoices[Random.Range(0, CachedChoices.Count)]);
            }

            World.StartCoroutine(coroutine());
        }
    }
}

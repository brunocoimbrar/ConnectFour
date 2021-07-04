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
        [SerializeField]
        private bool _useWeights = true;
        [SerializeField] [Min(0)]
        private int _attackWeight = 1;
        [SerializeField] [Min(0)]
        private int _defenseWeight = 1;

        private static readonly List<int> CachedChoices = new List<int>();

        public override void BeginTurn()
        {
            IEnumerator coroutine()
            {
                int controllerIndex = TurnSystem.Controllers.IndexOf(this);
                IReadOnlyList<IColumn> columns = BoardSystem.Columns;
                CachedChoices.Clear();

                if (_useWeights)
                {
                    int otherControllerIndex = (controllerIndex + 1) % TurnSystem.Controllers.Count;
                    bool hasWinMove = false;
                    bool hasLoseMove = false;

                    for (int i = 0; i < columns.Count; i++)
                    {
                        if (columns[i].DiscCount < BoardSystem.ColumnCapacity)
                        {
                            if (hasWinMove || !hasLoseMove)
                            {
                                if (BoardSystem.IsWinMove(controllerIndex, i, out IBoardSystem.MoveDetails moveDetails))
                                {
                                    if (!hasWinMove)
                                    {
                                        hasWinMove = true;
                                        CachedChoices.Clear();
                                    }

                                    CachedChoices.Add(i);
                                }

                                if (hasWinMove)
                                {
                                    continue;
                                }

                                int count = (moveDetails.Sum + moveDetails.Max) * _attackWeight;

                                for (int j = 0; j < count; j++)
                                {
                                    CachedChoices.Add(i);
                                }
                            }

                            if (!hasWinMove)
                            {
                                if (BoardSystem.IsWinMove(otherControllerIndex, i, out IBoardSystem.MoveDetails moveDetails))
                                {
                                    if (!hasLoseMove)
                                    {
                                        hasLoseMove = true;
                                        CachedChoices.Clear();
                                    }

                                    CachedChoices.Add(i);
                                }

                                if (hasLoseMove)
                                {
                                    continue;
                                }

                                int count = (moveDetails.Sum - 4) * (moveDetails.Max - 1) * _defenseWeight;

                                for (int j = 0; j < count; j++)
                                {
                                    CachedChoices.Add(i);
                                }
                            }
                        }
                    }
                }

                if (CachedChoices.Count == 0)
                {
                    for (int i = 0; i < columns.Count; i++)
                    {
                        if (columns[i].DiscCount < BoardSystem.ColumnCapacity)
                        {
                            CachedChoices.Add(i);
                        }
                    }
                }

                int choice = CachedChoices[Random.Range(0, CachedChoices.Count)];

                yield return new WaitForSeconds(_previewDelay);

                BoardSystem.AddPreview(controllerIndex, choice);

                yield return new WaitForSeconds(_moveDelay);

                BoardSystem.RemovePreview(choice);
                EndTurn(choice);
            }

            World.StartCoroutine(coroutine());
        }
    }
}

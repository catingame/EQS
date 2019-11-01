using EQS.Classes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EQS
{
    internal class QueryInstace
    {
        private QueryTemplate _queryTemplate;
        private IQuerier _querier;
        private List<QueryItem> _items;

        internal IQuerier Querier => _querier;

        internal List<QueryItem> GetItemDetails()
        {
            return _items;
        }

        internal QueryInstace(QueryTemplate queryTemplate, IQuerier querier)
        {
            this._queryTemplate = queryTemplate;
            this._querier = querier;
        }

        internal QueryResult Execute()
        {
            _queryTemplate.GenerateItems(this, out _items);

            _queryTemplate.RunTest(this, (test) => test.Purpose == TestPurpose.Filter ? 0 : 1);

            return FinalizeQuery(ref _items, ref _querier);
        }

        private QueryResult FinalizeQuery(ref List<QueryItem> items, ref IQuerier querier)
        {
            items.OrderBy(item => item.IsDiscarded ? 1 : -item.Score);

            NormalizeScores(ref _items);

            return new QueryResult(items, querier);
        }

        private void NormalizeScores(ref List<QueryItem> items)
        {
            var minScore = Single.MaxValue;
            var maxScore = Single.MinValue;

            foreach(var item in items)
            {
                minScore = Math.Min(item.Score, minScore);
                maxScore = Math.Max(item.Score, maxScore);
            }

            if (minScore == maxScore)
            {
                var score = minScore == 0 ? 0 : 1;
                foreach (var item in items)
                {
                    item.Score = score;
                }
            }
            else
            {
                var scoreRange = maxScore - minScore;
                foreach (var item in items)
                {
                    item.Score = item.Score = (item.Score - minScore) / scoreRange;
                }
            }
        }
    }
}
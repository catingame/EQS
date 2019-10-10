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

        internal IQuerier querier { get { return _querier; } }
        
        internal List<QueryItem> GetItemDetails() 
        { 
            return _items.ConvertAll(item => item.Clone() as QueryItem); 
        }

        internal void UpdateItem(QueryItem item, Action<QueryItem> update)
        {
            var _item = _items.Find(_item => _item.Idx == item.Idx);
            if (_item != null)
            {
                update.Invoke(_item);
            }
        }

        internal QueryInstace(QueryTemplate queryTemplate, IQuerier querier)
        {
            this._queryTemplate = queryTemplate;
            this._querier = querier;
        }

        internal QueryResult Execute()
        {
            _queryTemplate.GenerateItems(this, out _items);

            NormalizeScores(ref _items);

            _queryTemplate.RunTest(this, (test) => test.Purpose == TestPurpose.Filter ? 0 : 1);

            NormalizeScores(ref _items);

            return FinalizeQuery(ref _items, ref _querier);
        }

        private QueryResult FinalizeQuery(ref List<QueryItem> items, ref IQuerier querier)
        {
            items.OrderBy(item => item.IsDiscarded ? 1 : -item.Score);

            return new QueryResult(items, querier);
        }

        private void NormalizeScores(ref List<QueryItem> items)
        {
            var minScore = Int32.MaxValue;
            var maxScore = Int32.MinValue;

            foreach(var item in items)
            {
                minScore = Math.Min(item.Score, minScore);
                maxScore = Math.Max(item.Score, maxScore);
            }

            if (minScore == maxScore)
            {
                var score = 1 / items.Count;

                foreach (var item in items)
                {
                    item.Score = score;
                }
            }
            else
            {
                var sum = 0;
                var scoreRange = maxScore - minScore;

                foreach (var item in items)
                {
                    sum += item.Score = (item.Score - minScore) / scoreRange;
                }

                sum = 1 / sum;

                foreach (var item in items)
                {
                    item.Score *= sum;
                }
            }
        }
    }
}
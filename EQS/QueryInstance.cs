using EQS.Classes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EQS
{
    internal class QueryInstance
    {
        private readonly QueryTemplate _queryTemplate;
        private readonly List<QueryItem> _items = new List<QueryItem>();
        private readonly List<QueryItemDetails> _itemDetails = new List<QueryItemDetails>();
        
        private IQuerier _querier;
        
        internal IQuerier Querier => _querier;

        internal List<QueryItem> GetItemDetails()
        {
            return _items;
        }

        internal QueryInstance(QueryTemplate queryTemplate, IQuerier querier)
        {
            this._queryTemplate = queryTemplate;
            this._querier = querier;
        }

        internal QueryResult Execute()
        {
            _queryTemplate.GenerateItems(this);

            _queryTemplate.RunTest(this, (test) => test.Purpose == TestPurpose.Filter ? 0 : 1);

            return FinalizeQuery(in _items, ref _querier);
        }

        internal void AddItemData(Func<List<QueryItem>> getItemData)
        {
            var numOfTest = _queryTemplate.NumOfTest;
            var idx = _items.Count;
            
            foreach (var item in getItemData())
            {
                // TODO: validate item
                //var isValidate = true;
                //if (isValidate)
                {
                    item.Idx = idx++;

                    _items.Add(item);
                    _itemDetails.Add(new QueryItemDetails()
                    {
                        Idx = item.Idx,
                        _testResults = new List<Single>(numOfTest),
                        _testWeightedScores = new List<Single>(numOfTest)
                    });
                }
            }
        }

        private QueryResult FinalizeQuery(in List<QueryItem> items, ref IQuerier querier)
        {
            items.Sort();

            NormalizeScores(in _items);

            return new QueryResult(items, querier);
        }

        private void NormalizeScores(in List<QueryItem> items)
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
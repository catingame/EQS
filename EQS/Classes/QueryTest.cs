using System;
using System.Collections.Generic;
using System.Numerics;

namespace EQS.Classes
{
    public abstract class QueryTest : IPrepareContext
    {
        private IQuerier _querier;
        private Func<List<QueryItem>> _getItemDetails;
        
        private TestScoringEquation ScoringEquation => TestScoringEquation.Linear;
        private TestFilterType FilterType => TestFilterType.Range;
        private int FilterMin => 0;
        private int FilterMax => 0;
        private CachedScoreOp MultipleContextScoreOp => CachedScoreOp.AverageScore;
        private int ClampMin => 0;
        private int ClampMax => 0;
        private int ScoringFactorValue => 1;

        internal QueryItem CurrentIterator { get; set; }
        internal IQueryContext ClampContext { get; }
        internal TestPurpose Purpose { get; }
        internal IQuerier Querier { get; }

        internal void RunTest(in QueryInstace queryInstace)
        {
            _querier = queryInstace.querier;
            _getItemDetails = queryInstace.GetItemDetails;
        }

        internal void LoopOverItems()
        {
            var items = _getItemDetails();

            var IsUsingClamp = ClampMin != ClampMax;

            if (IsUsingClamp)
            {
                var qurierLocations = (this as IPrepareContext).PrepareContext_Location(ClampContext, _querier);
                foreach (var item in items)
                {
                    CurrentIterator = item;

                    foreach (var qurierLocation in qurierLocations)
                    {
                        var distance = System.Numerics.Vector3.Distance(CurrentIterator.Location, qurierLocation.To);
                        if (ClampMin <= distance && distance <= ClampMax)
                        {
                            OnQuery();
                        }
                    }
                }
            }
            else
            {
                foreach (var item in items)
                {
                    CurrentIterator = item;
                    OnQuery();
                }
            }
        }

        internal void SetScoreSingle(Single score)
        {
            CurrentIterator.Score = 0;
            CurrentIterator.Operation = MultipleContextScoreOp;
            CurrentIterator.SetScore(Purpose, FilterType, score, FilterMin, FilterMax);
        }

        internal void NormalizeItemScores(in QueryInstace queryInstace)
        {
            var items = _getItemDetails();
            var minScore = int.MaxValue;
            var maxScore = int.MinValue;

            foreach (var item in items)
            {
                minScore = Math.Min(item.Score, minScore);
                maxScore = Math.Max(item.Score, maxScore);
                if (item.IsDiscarded)
                {
                    queryInstace.UpdateItem(item, _item => _item.IsDiscarded = item.IsDiscarded);
                }
            }

            if (minScore == maxScore) return;

            var localReferenceValue = minScore;
            var valueSpan = Math.Max(Math.Abs(localReferenceValue - minScore), Math.Abs(localReferenceValue - maxScore));

            foreach (var item in items)
            {
                var testValue = item.Score;
                var weightedScore = 0;

                if (!item.IsDiscarded)
                {
                    var clampedScore = Math.Clamp(testValue, minScore, maxScore);
                    var normalizedScore = Math.Abs(localReferenceValue - clampedScore) / valueSpan;

                    switch (ScoringEquation)
                    {
                        case TestScoringEquation.Linear:
                            weightedScore = ScoringFactorValue * normalizedScore;
                            break;
                        case TestScoringEquation.InverseLinear:
                            var inverseNormalizedScore = 1 - normalizedScore;
                            weightedScore = ScoringFactorValue * inverseNormalizedScore;
                            break;
                        case TestScoringEquation.Constant:
                            weightedScore = normalizedScore > 0 ? ScoringFactorValue : weightedScore;
                            break;
                    }
                }

                item.Score = weightedScore;
            }

            // * normalize
            minScore = int.MaxValue;
            maxScore = int.MinValue;

            foreach (var item in items)
            {
                minScore = Math.Min(item.Score, minScore);
                maxScore = Math.Max(item.Score, maxScore);
            }

            if (minScore == maxScore) return;

            var sum = 0;
            var scoreRange = maxScore - minScore;

            foreach (var item in items)
            {
                if (!item.IsDiscarded)
                {
                    var normalizedScore = (item.Score - minScore) / scoreRange;
                    sum += item.Score = normalizedScore != 0 ? normalizedScore : int.MinValue;
                }
            }

            sum = 1 / sum;

            foreach (var item in items)
            {
                var weight = item.Score * sum;
                queryInstace.UpdateItem(item, _item => _item.Score *= weight);
            }
        }

        abstract internal void OnRunTest();

        abstract internal void OnQuery();
    }

    internal enum TestPurpose
    {
        Score,
        Filter
    }

    internal enum TestScoringEquation
    {
        Linear,
        InverseLinear,
        Constant
    }

    internal enum TestFilterType
    {
        Maximum,
        Minimum,
        Range
    }
}
using System;
using System.Collections.Generic;
using System.Numerics;

namespace EQS.Classes
{
    public abstract class QueryTest : IPrepareContext
    {
        private IQuerier _querier;
        private QueryInstace _queryInstace;


        private TestScoringEquation ScoringEquation => TestScoringEquation.Linear;
        
        private TestFilterType FilterType => TestFilterType.Range;
        private Int32 FilterMin => 0;
        private Int32 FilterMax => 0;

        private CachedScoreOp MultipleContextScoreOp => CachedScoreOp.AverageScore;
        private Int32 ClampMin => 0;
        private Int32 ClampMax => 0;

        private Single ScoringFactorValue => 1;

        internal QueryItem CurrentIterator { get; set; }
        internal IQueryContext ClampContext { get; }
        internal TestPurpose Purpose { get; }
        internal IQuerier Querier => _querier;

        internal void RunTest(in QueryInstace queryInstace)
        {
            _queryInstace = queryInstace;
            _querier = queryInstace.Querier;
            OnRunTest();
        }

        internal void LoopOverItems()
        {
            var items = _queryInstace.GetItemDetails();

            var IsUsingClamp = ClampMin != ClampMax;

            if (IsUsingClamp)
            {
                var qurierLocations = (this as IPrepareContext).PrepareContext_Location(ClampContext, _querier);
                foreach (var item in items)
                {
                    CurrentIterator = item;

                    foreach (var qurierLocation in qurierLocations)
                    {
                        var distance = System.Numerics.Vector3.Distance(CurrentIterator.Location, qurierLocation);
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
            var items = _queryInstace.GetItemDetails();
            var minScore = Single.MaxValue;
            var maxScore = Single.MinValue;

            foreach (var item in items)
            {
                minScore = Math.Min(item.Score, minScore);
                maxScore = Math.Max(item.Score, maxScore);
            }

            if (minScore == maxScore) return;

            var localReferenceValue = minScore;
            var valueSpan = Math.Max(Math.Abs(localReferenceValue - minScore), Math.Abs(localReferenceValue - maxScore));

            foreach (var item in items)
            {
                var testValue = item.Score;
                Single weightedScore = 0;

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
        }

        internal abstract void OnRunTest();

        internal abstract void OnQuery();
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
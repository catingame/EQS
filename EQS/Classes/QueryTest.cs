using System;
using System.Collections.Generic;
using System.Numerics;

namespace EQS.Classes
{
    public abstract class QueryTest : IPrepareContext
    {
        private QueryInstance _queryInstance;

        internal IQuerier Querier { get; private set; }
        internal QueryItem CurrentIterator { get; set; }

        public TestScoringEquation ScoringEquation = TestScoringEquation.Linear;
        public TestScoreOperator MultipleContextScoreOp = TestScoreOperator.AverageScore;
        public TestPurpose Purpose = TestPurpose.Score;
        public TestNormalizationType NormalizationType = TestNormalizationType.Absolute;

        public TestFilterType FilterType = TestFilterType.Range;
        public Single ScoreFilterMin = 0;
        public Single ScoreFilterMax = 0;

        public TestClampingType ClampMinType = TestClampingType.None;
        public Single ScoreClampMin = 0;

        public TestClampingType ClampMaxType = TestClampingType.None;
        public Single ScoreClampMax = 0;

        public Single ScoringFactorValue = 1;

        internal void RunTest(in QueryInstance queryInstance)
        {
            _queryInstance = queryInstance;
            Querier = queryInstance.Querier;
            OnRunTest();
        }

        internal void LoopOverItems()
        {
            var items = _queryInstance.GetItemDetails();
            foreach (var item in items)
            {
                CurrentIterator = item;
                OnQuery();
            }
        }

        internal void SetScore(Single score)
        {
            CurrentIterator.Score = 0;
            CurrentIterator.Operation = MultipleContextScoreOp;
            CurrentIterator.SetScore(Purpose, FilterType, score, ScoreFilterMin, ScoreFilterMax);
        }

        internal void NormalizeItemScores(in QueryInstance queryInstance)
        {
            var items = _queryInstance.GetItemDetails();
            var minScore = (NormalizationType == TestNormalizationType.Absolute) ? 0 : Single.MaxValue;
            var maxScore = Single.MinValue;

            minScore = ClampMinType switch
            {
                TestClampingType.FilterThreshold => ScoreFilterMin,
                TestClampingType.SpecifiedValue => ScoreClampMin,
                _ => minScore
            };

            maxScore = ClampMaxType switch
            {
                TestClampingType.FilterThreshold => ScoreFilterMax,
                TestClampingType.SpecifiedValue => ScoreClampMax,
                _ => maxScore
            };

            if ((ClampMinType == TestClampingType.None) ||
                (ClampMaxType == TestClampingType.None)
            )
            {
                foreach (var item in items)
                {
                    if (ClampMinType == TestClampingType.None)
                    {
                        minScore = Math.Min(item.Score, minScore);
                    }

                    if(ClampMaxType == TestClampingType.None)
                    {
                        maxScore = Math.Max(item.Score, maxScore);
                    }
                }
            }

            if (minScore == maxScore) return;

            var localReferenceValue = minScore; // TODO: 
            var valueSpan = Math.Max(Math.Abs(localReferenceValue - minScore), Math.Abs(localReferenceValue - maxScore));

            foreach (var item in items)
            {
                Single weightedScore = 0f;
                
                var clampedScore = Math.Clamp(item.Score, minScore, maxScore);
                var normalizedScore = ScoringFactorValue >= 0 
                    ? Math.Abs(localReferenceValue - clampedScore) / valueSpan
                    : 1f - Math.Abs(localReferenceValue - clampedScore) / valueSpan;
                var  absoluteWeight = (ScoringFactorValue >= 0) ? ScoringFactorValue : -ScoringFactorValue;

                weightedScore = ScoringEquation switch
                {
                    TestScoringEquation.Linear => (absoluteWeight * normalizedScore),
                    TestScoringEquation.InverseLinear => (absoluteWeight * (1 - normalizedScore)),
                    TestScoringEquation.Square => (absoluteWeight * (normalizedScore * normalizedScore)),
                    TestScoringEquation.SquareRoot => (absoluteWeight * (Single) Math.Sqrt(normalizedScore)),
                    TestScoringEquation.Constant => (normalizedScore > 0 ? absoluteWeight : weightedScore),
                    _ => weightedScore
                };

                item.Score += weightedScore;
            }
        }

        internal abstract void OnRunTest();

        internal abstract void OnQuery();
    }

    public enum TestPurpose
    {
        Score,
        Filter,
        FilterAndScore
    }

    public enum TestScoringEquation
    {
        Linear,
        Square,
        InverseLinear,
        SquareRoot,
        Constant
    }

    public enum TestFilterType
    {
        Maximum,
        Minimum,
        Range,
        Match
    }

    public enum TestNormalizationType
    {
        Absolute,
        RelativeToScores
    }

    public enum TestClampingType
    {
        None,
        SpecifiedValue,
        FilterThreshold
    };
    public enum TestScoreOperator
    {
        AverageScore,
        MinScore,
        MaxScore
    }
}
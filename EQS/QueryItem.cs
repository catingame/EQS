using EQS.Classes;
using System;
using System.Numerics;

namespace EQS
{
    internal class QueryItem : ICloneable, IComparable<QueryItem>
    {
        internal Int32 Idx;
        internal TestScoreOperator Operation = TestScoreOperator.AverageScore;
        internal Single  Score = 0f;

        internal object RawData { get; } = null;
        internal Type RawDataType { get; } = null;

        // * down-casting
        internal QueryItem(in object data, in Type dataType)
        {
            RawData = data;
            RawDataType = dataType;
        }

        // TODO:
        internal Vector3 Location =>  (RawData is Vector3 vec) ? vec : new Vector3();

        internal void SetScore(TestPurpose testPurpose, TestFilterType filterType, Single score, Single filterMin, Single filterMax)
        {
            var passedTest = true;

            if (testPurpose != TestPurpose.Score)
            {
                passedTest = filterType switch
                {
                    TestFilterType.Maximum => (score <= filterMax),
                    TestFilterType.Minimum => (score >= filterMin),
                    TestFilterType.Range => (filterMin <= score) && (score <= filterMax),
                    TestFilterType.Match => false,
                    _ => false,
                };
            }
            
            if(passedTest)
            {
                SetScoreInternal(score);
            }
        }

        internal void SetScoreInternal(Single score)
        {
            switch (Operation)
            {
                case TestScoreOperator.AverageScore:
                    Score += score;
                    break;
                case TestScoreOperator.MinScore:
                    Score = Math.Min(score, Score);
                    break;
                case TestScoreOperator.MaxScore:
                    Score = Math.Max(score, Score);
                    break;
            }
        }

        public object Clone()
        {
            return new QueryItem(RawData, RawDataType)
            {
                Idx = Idx,
                Operation = Operation,
                Score = Score
            };
        }

        public int CompareTo(QueryItem obj)
        {
            return obj == null ? 1 : Score.CompareTo(obj.Score);
        }
    }
}
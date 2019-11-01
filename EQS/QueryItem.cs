using EQS.Classes;
using System;
using System.Numerics;

namespace EQS
{
    internal class QueryItem : ICloneable
    {
        internal Int32 Idx;
        internal CachedScoreOp Operation = CachedScoreOp.None;
        internal Single  Score = 0;
        internal Boolean IsDiscarded = false;

        internal object RawData { get; } = null;
        internal Type RawDataType { get; } = null;

        // * down-casting
        internal QueryItem(in object data, in Type dataType)
        {
            RawData = data;
            RawDataType = dataType;
        }

        internal Vector3 Location =>  (RawData is Vector3 vec) ? vec : new Vector3();

        internal void SetScore(TestPurpose testPurpose, TestFilterType filterType, Single score, Int32 filterMin, Int32 filterMax)
        {
            if (testPurpose != TestPurpose.Score)
            {
                switch (filterType)
                {
                    case TestFilterType.Maximum:
                        IsDiscarded = !(score <= filterMax);
                        break;
                    case TestFilterType.Minimum:
                        IsDiscarded = !(score >= filterMin);
                        break;
                    case TestFilterType.Range:
                        IsDiscarded = !((filterMin <= score) && (score <= filterMax));
                        break;
                }
            }
            else if (!IsDiscarded)
            {
                // TODO: Cast Single To Int32
                SetScoreInternal((Int32)score);
            }
        }

        internal void SetScoreInternal(Int32 score)
        {
            switch (Operation)
            {
                case CachedScoreOp.AverageScore:
                    Score += score;
                    break;
                case CachedScoreOp.MinScore:
                    Score = Math.Min(score, Score);
                    break;
                case CachedScoreOp.MaxScore:
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
                Score = Score,
                IsDiscarded = IsDiscarded
            };
        }
    }

    internal enum CachedScoreOp
    {
        None,
        AverageScore,
        MinScore,
        MaxScore
    }
}
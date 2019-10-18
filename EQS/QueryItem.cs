using EQS.Classes;
using System;
using System.Numerics;

namespace EQS
{
    internal class QueryItem : ICloneable
    {
        internal Int32 Idx;
        internal CachedScoreOp Operation = CachedScoreOp.None;
        internal Int32  Score = 0;
        internal Boolean IsDiscarded = false;

        internal object RawData { get; } = null;

        internal QueryItem(object rawData)
        {
            RawData = rawData;
        }

        internal System.Numerics.Vector3 Location =>  (RawData is Location vec) ? vec.To : new System.Numerics.Vector3();

        internal void SetScore(TestPurpose testPurpose, TestFilterType filterType, Single score, Int32 filterMin, Int32 filterMax)
        {
            if (testPurpose != TestPurpose.Score)
            {
                switch (filterType)
                {
                    case TestFilterType.Maximum:
                        IsDiscarded = !(Score <= filterMax);
                        break;
                    case TestFilterType.Minimum:
                        IsDiscarded = !(Score >= filterMin);
                        break;
                    case TestFilterType.Range:
                        IsDiscarded = !((filterMin <= Score) && (Score <= filterMax));
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
                    Score += Score;
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
            return new QueryItem(RawData)
            {
                IsDiscarded = IsDiscarded,
                Operation = Operation,
                Score = Score
            };
        }
    }

    public interface IData
    {
        Location GetLocation();
        Rotation GetRotation();
    }

    internal enum CachedScoreOp
    {
        None,
        AverageScore,
        MinScore,
        MaxScore
    }
}
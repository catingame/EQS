using System;
using System.Collections.Generic;
using System.Text;
using EQS.Classes;

namespace EQS.Tests
{
    public class Test_Random : QueryTest
    {
        private readonly Random _random = new Random();

        internal override void OnRunTest()
        {
            LoopOverItems();
        }

        internal override void OnQuery()
        {
            SetScore((Single)_random.NextDouble());
        }
    }
}

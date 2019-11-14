using EQS.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic.CompilerServices;

namespace EQS
{
    public class QueryTemplate
    {
        private readonly List<QueryGenerator> _generator;
        private readonly List<QueryTest> _tests;
        private readonly IQueryCondition _condition;

        internal Int32 NumOfTest => _tests.Count;

        public QueryTemplate(in List<QueryGenerator> generator, in List<QueryTest> tests) : this(in generator, in tests, null)
        {   
        }

        public QueryTemplate(in List<QueryGenerator> generator, in List<QueryTest> tests, IQueryCondition condition)
        {
            _generator = generator;
            _tests = tests;
            _condition = condition;
        }

        internal void GenerateItems(in QueryInstance queryInstance)
        {
            foreach (var generator in _generator)
            {
                generator.GenerateItems(in queryInstance);
            }
        }

        internal void RunTest(in QueryInstance queryInstance, Func<QueryTest, Int32> p)
        {
            _tests.OrderBy(p);

            foreach (var test in _tests)
            {
                test.RunTest(in queryInstance);
                test.NormalizeItemScores(in queryInstance);
            }
        }
    }
}
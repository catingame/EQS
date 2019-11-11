using EQS.Classes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EQS
{
    public class QueryTemplate
    {
        private readonly QueryGenerator _generator;
        private readonly List<QueryTest> _tests;
        private readonly IQueryCondition _condition;

        public QueryTemplate(in QueryGenerator generator, in List<QueryTest> tests) : this(in generator, in tests, null)
        {   
        }

        public QueryTemplate(in QueryGenerator generator, in List<QueryTest> tests, IQueryCondition condition)
        {
            _generator = generator;
            _tests = tests;
            _condition = condition;
        }

        internal void GenerateItems(in QueryInstance queryInstance, out List<QueryItem> items)
        {
            _generator.GenerateItems(in queryInstance, out items);
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
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

        internal void GenerateItems(in QueryInstace queryInstace, out List<QueryItem> items)
        {
            _generator.GenerateItems(in queryInstace, out items);
        }

        internal void RunTest(in QueryInstace queryInstace, Func<QueryTest, Int32> p)
        {
            _tests.OrderBy(p);

            foreach (var test in _tests)
            {
                test.RunTest(in queryInstace);
                test.NormalizeItemScores(in queryInstace);
            }
        }
    }
}
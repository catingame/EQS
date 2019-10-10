using EQS.Classes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EQS
{
    internal class QueryTemplate
    {
        readonly private QueryGenerator _generator;
        readonly private List<QueryTest> _tests;
        readonly private IQueryCondition _condition;

        internal QueryTemplate(ref QueryGenerator generator, ref List<QueryTest> tests) : this(ref generator, ref tests, null)
        {   
        }

        internal QueryTemplate(ref QueryGenerator generator, ref List<QueryTest> tests, IQueryCondition condition)
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EQS.Classes;

namespace EQS.Generators
{
    public class Generator_Composite : QueryGenerator
    {
        public List<QueryGenerator> Generators;

        public Generator_Composite(IQueryContext context) : base(context)
        {
        }

        internal override List<QueryItem> DoItemGeneration()
        {
            var items = new List<QueryItem>();
            foreach (var generator in Generators)
            {
                items.AddRange(generator.DoItemGeneration());
            }
            return items;
        }
    }
}

using EQS.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace EQS.Generators
{
    internal class GeneratorActor : QueryGenerator
    {
        internal GeneratorActor(IQueryContext context) : base(context)
        {
        }

        internal override void DoItemGeneration()
        {
            var actors = (this as IPrepareContext).PrepareContext_RawData(Context, querier);
            foreach (var actor in actors)
            {
                AddGeneratedItem(new QueryItem(actor));
            }
        }
    }
}

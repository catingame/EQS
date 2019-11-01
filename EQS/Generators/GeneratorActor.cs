using EQS.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace EQS.Generators
{
    public class GeneratorActor : QueryGenerator
    {
        public GeneratorActor(IQueryContext context) : base(context)
        {
        }

        internal override void DoItemGeneration()
        {
            var actors = (this as IPrepareContext).PrepareContext_Querier(Context, querier);
            foreach (var actor in actors)
            {
                AddGeneratedItem(new QueryItem(actor, actor.GetType()));
            }
        }
    }
}

using EQS.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace EQS.Generators
{
    public class Generator_Actor : QueryGenerator
    {
        public Generator_Actor(IQueryContext context) : base(context)
        {
        }

        internal override List<QueryItem> DoItemGeneration()
        {
            var actors = (this as IPrepareContext).PrepareContext_Querier(Context, Querier);
            
            var items = new List<QueryItem>();
            foreach (var actor in actors)
            {
                items.Add(new QueryItem(actor, actor.GetType()));
            }
            return items;
        }
    }
}

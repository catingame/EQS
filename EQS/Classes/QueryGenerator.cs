using System;
using System.Collections.Generic;

namespace EQS.Classes
{
    public abstract class QueryGenerator : IPrepareContext
    {
        internal IQuerier Querier;
        internal IQueryContext Context { get; }

        protected QueryGenerator(IQueryContext context) 
        {
            Context = context;
        }

        internal void GenerateItems(in QueryInstance queryInstance)
        {
            Querier = queryInstance.Querier;
            queryInstance.AddItemData(DoItemGeneration);
        }

        internal abstract List<QueryItem> DoItemGeneration();
    }
}
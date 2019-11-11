using System;
using System.Collections.Generic;

namespace EQS.Classes
{
    public abstract class QueryGenerator : IPrepareContext
    {
        private int _uid = 0;
        private List<QueryItem> _items;

        internal IQuerier Querier;
        internal IQueryContext Context { get; }

        protected QueryGenerator(IQueryContext context) 
        {
            Context = context;
        }

        internal void GenerateItems(in QueryInstace queryInstance, out List<QueryItem> items)
        {
            Querier = queryInstance.Querier;

            _items = items = new List<QueryItem>();

            DoItemGeneration();
        }

        internal void AddGeneratedItem(in QueryItem item)
        {
            item.Idx = _uid++;
            _items.Add(item);
        }

        internal abstract void DoItemGeneration();
    }
}
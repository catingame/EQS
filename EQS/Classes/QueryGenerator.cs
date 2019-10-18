using System;
using System.Collections.Generic;

namespace EQS.Classes
{
    public abstract class QueryGenerator : IPrepareContext
    {
        private int _uid = 0;
        private List<QueryItem> _items { get; set; }

        internal IQuerier querier;

        internal IQueryContext Context { get; }

        protected QueryGenerator(IQueryContext context) 
        {
            Context = context;
        }

        internal void GenerateItems(in QueryInstace queryInstace, out List<QueryItem> items)
        {
            querier = queryInstace.querier;

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
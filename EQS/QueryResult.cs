using System;
using System.Collections.Generic;

namespace EQS
{
    internal class QueryResult
    {
        private List<QueryItem> items;
        private IQuerier querier;

        internal QueryResult(in List<QueryItem> items, in IQuerier querier)
        {
            this.items = items;
            this.querier = querier;
        }

        internal List<T> GetAllItem<T>() where T : class
        {
            var actors = new List<T>();

            foreach (var item in items)
            {
                actors.Add(item.RawData as T);
            }

            return actors;
        }
    }
}
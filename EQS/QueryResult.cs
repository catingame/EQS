using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace EQS
{
    public class QueryResult
    {
        private List<QueryItem> items;
        private IQuerier querier;

        internal QueryResult(in List<QueryItem> items, in IQuerier querier)
        {
            this.items = items;
            this.querier = querier;
        }

        public T GetItem<T>(Int32 idx)
        {
            Contract.Assert(items[idx].RawDataType == typeof(T));
            return (T)items[idx].RawData;
        }

        public IEnumerable<T> GetAllItems<T>()
        {
            Contract.Assert(items.Count > 0);
            Contract.Assert(items[0].RawDataType == typeof(T));
            return items.ConvertAll(item => (T)item.RawData).ToArray();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace EQS
{
    public partial class QueryResult
    {
        private readonly List<QueryItem> _items;
        private IQuerier _querier;

        public Int32 Length => _items.Count - 1;

        internal QueryResult(in List<QueryItem> items, in IQuerier querier)
        {
            this._items = items;
            this._querier = querier;
        }

        public T GetItem<T>(Int32 idx)
        {
            Contract.Assert(_items.Count > idx);
            Contract.Assert(_items[idx].RawDataType == typeof(T));
            return (T)_items[idx].RawData;
        }

        public IEnumerable<T> GetAllItems<T>()
        {
            Contract.Assert(_items.Count > 0);
            Contract.Assert(_items[0].RawDataType == typeof(T));
            return _items.ConvertAll(item => (T)item.RawData).ToArray();
        }

        public T GetBestScoreResult<T>()
        {
            Contract.Assert(_items.Count > 0);
            Contract.Assert(_items[0].RawDataType == typeof(T));

            var sortedList = _items;
            sortedList.Sort();
            return (T)sortedList.Last().RawData;
        }
    }
}
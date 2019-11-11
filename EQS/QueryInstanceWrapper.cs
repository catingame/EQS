using System;
using System.Collections.Generic;
namespace EQS
{
    public class QueryInstanceWrapper
    {
        private readonly QueryTemplate _queryTemplate;
        private readonly IQuerier _querier;

        public QueryResult QueryResult { get; private set; }

        public QueryInstanceWrapper(QueryTemplate queryTemplate, IQuerier querier)
        {
            this._queryTemplate = queryTemplate;
            this._querier = querier;
        }

        public void Run()
        {
            var envQueryInstance = new QueryInstance(_queryTemplate, _querier);
            QueryResult = envQueryInstance.Execute();
        }
    }
}
﻿using System;
using System.Collections.Generic;
namespace EQS
{
    public class QueryInstanceWrapper
    {
        private readonly QueryTemplate queryTemplate;
        private readonly IQuerier querier;
        private QueryResult queryResult;

        public QueryResult QueryResult
        {
            get { return queryResult; }
        }

        public QueryInstanceWrapper(QueryTemplate queryTemplate, IQuerier querier)
        {
            this.queryTemplate = queryTemplate;
            this.querier = querier;
        }

        public void Run()
        {
            QueryInstace envQueryInstance = new QueryInstace(queryTemplate, querier);
            queryResult = envQueryInstance.Execute();
        }
    }
}
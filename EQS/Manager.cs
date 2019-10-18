using System;

namespace EQS
{
    public class Manager
    {
        public static QueryInstanceWrapper RunEQSQuery(QueryTemplate queryTemplate, IQuerier querier) => new QueryInstanceWrapper(queryTemplate, querier);
    }
}

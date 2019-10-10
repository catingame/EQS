using System;

namespace EQS
{
    public class Manager
    {
        static QueryInstanceWrapper RunEQSQuery(QueryTemplate queryTemplate, IQuerier querier) => new QueryInstanceWrapper(queryTemplate, querier);
    }
}

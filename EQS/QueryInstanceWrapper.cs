using System.Collections.Generic;
namespace EQS
{
    internal class QueryInstanceWrapper
    {
        readonly private QueryTemplate queryTemplate;
        readonly private IQuerier querier;
        private QueryResult queryResult;

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

        public List<T> GetResults<T>() where T : class
        {
            return queryResult.GetAllItem<T>();
        }
    }
}
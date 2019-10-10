using EQS.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace EQS.Contexts
{
    class ContextQuerier : IQueryContext
    {
        public List<IData> ProvideActorsSet(in IQuerier querier)
        {
            return new List<IData>() { querier };
        }
    }
}
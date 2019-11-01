using EQS.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace EQS.Contexts
{
    public class ContextQuerier : IQueryContext
    {
        public List<IQuerier> ProvideActorsSet(in IQuerier querier)
        {
            return new List<IQuerier>() { querier };
        }
    }
}
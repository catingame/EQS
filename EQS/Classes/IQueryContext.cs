using System;
using System.Collections.Generic;

namespace EQS.Classes
{
    public interface IQueryContext
    {
        List<IQuerier> ProvideActorsSet(in IQuerier querier);
    }
}
using System;
using System.Collections.Generic;

namespace EQS.Classes
{
    public interface IQueryContext
    {
        List<IData> ProvideActorsSet(in IQuerier querier);
    }
}
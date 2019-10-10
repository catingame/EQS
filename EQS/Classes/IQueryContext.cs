using System;
using System.Collections.Generic;

namespace EQS.Classes
{
    internal interface IQueryContext
    {
        List<IData> ProvideActorsSet(in IQuerier querier);
    }
}
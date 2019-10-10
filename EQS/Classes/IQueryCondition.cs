using System;

namespace EQS.Classes
{
    internal interface IQueryCondition
    {
        internal bool CheckCondtion(in IQuerier querier);
    }
}
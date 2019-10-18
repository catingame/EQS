using System;

namespace EQS.Classes
{
    public interface IQueryCondition
    {
        bool CheckCondtion(in IQuerier querier);
    }
}
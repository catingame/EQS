using System;

namespace EQS.Classes
{
    public interface IQueryCondition
    {
        bool CheckCondition(in IQuerier querier);
    }
}
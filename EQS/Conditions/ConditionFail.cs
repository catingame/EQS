using System;
using System.Collections.Generic;
using System.Text;
using EQS.Classes;

namespace EQS.Conditions
{
    internal class ConditionFail : IQueryCondition
    {
        Boolean IQueryCondition.CheckCondition(in IQuerier querier)
        {
            return false;
        }
    }
}

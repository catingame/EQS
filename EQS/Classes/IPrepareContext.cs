using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace EQS.Classes
{
    interface IPrepareContext
    {
        List<IQuerier> PrepareContext_Querier(in IQueryContext context, in IQuerier querier)
        {
            return context.ProvideActorsSet(querier);
        }
        List<Vector3> PrepareContext_Location(in IQueryContext context, in IQuerier querier)
        {
            return context.ProvideActorsSet(querier).ConvertAll(data => data.GetLocation());
        }
        List<Vector3> PrepareContext_Rotation(in IQueryContext context, in IQuerier querier)
        {
            return context.ProvideActorsSet(querier).ConvertAll(data => data.GetRotation());
        }
    }
}
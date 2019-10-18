using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace EQS.Classes
{
    interface IPrepareContext
    {
        List<IData> PrepareContext_RawData(in IQueryContext context, in IQuerier querier)
        {
            return context.ProvideActorsSet(querier);
        }
        List<Location> PrepareContext_Location(in IQueryContext context, in IQuerier querier)
        {
            return context.ProvideActorsSet(querier).ConvertAll(data => data.GetLocation());
        }
        List<Rotation> PrepareContext_Rotation(in IQueryContext context, in IQuerier querier)
        {
            return context.ProvideActorsSet(querier).ConvertAll(data => data.GetRotation());
        }
    }
}
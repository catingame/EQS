using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using EQS.Classes;

namespace EQS.Generators
{
    class Generator_SimpleGrid : QueryGenerator
    {
        public Single GridSize = 500f;
        public Single SpaceBetween = 100f;

        public Generator_SimpleGrid(IQueryContext context) : base(context)
        {
        }

        internal override void DoItemGeneration()
        {
            var radiusValue = GridSize;
            var densityValue = SpaceBetween;

            Int32 itemCount = (Int32) ((radiusValue * 2.0f / densityValue) + 1);
            Int32 itemCountHalf = itemCount / 2;

            var contextLocations = (this as IPrepareContext).PrepareContext_Location(Context, querier);

            foreach (var contextLocation in contextLocations)
            {
                for (var indexX = 0; indexX < itemCount; ++indexX)
                {
                    for (var indexY = 0; indexY < itemCount; ++indexY)
                    {
                        var v = contextLocation - new Vector3(
                                    densityValue * (indexX - itemCountHalf),
                                    densityValue * (indexY - itemCountHalf), 
                                    0f
                                );

                        AddGeneratedItem(new QueryItem(v, v.GetType()));
                    }
                }
            }
        }
    }
}

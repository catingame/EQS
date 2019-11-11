using EQS.Debug;

namespace EQS
{
    public partial class QueryResult
    {
        public void DrawDebugItem(DebugShape shape)
        {
            var factory = DebugDrawFactory.Get();
            foreach (var item in _items)
            {
                factory.DrawDebugItem(shape, DebugItem.Converter(item));
            }   
        }
    }
}

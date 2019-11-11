using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace EQS.Debug
{
    public enum DebugShape
    {
        Box,
        Sphere,
        Capsule,

    }
    public class DebugDrawFactory
    {
        private static DebugDrawFactory _instance;
        public static DebugDrawFactory Get()
        {
            return _instance ??= new DebugDrawFactory();
        }

        private readonly Dictionary<DebugShape, Action<DebugItem>> _painters = new Dictionary<DebugShape, Action<DebugItem>>();
        public Boolean RegisterFactory(DebugShape shape, Action<DebugItem> painter)
        {
            return _painters.TryAdd(shape, painter);
        }

        public void DrawDebugItem(DebugShape shape, in DebugItem item)
        {
            if (_painters.TryGetValue(shape, out var painter))
            {
                painter.Invoke(item);   
            }
            Console.WriteLine($"This {shape} is not registered.");
        }
    }
}

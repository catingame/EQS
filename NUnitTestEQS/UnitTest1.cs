using System;
using NUnit.Framework;

using System.Collections.Generic;
using System.Numerics;
using EQS;
using EQS.Generators;
using EQS.Tests;
using EQS.Contexts;
using EQS.Classes;
using EQS.Debug;

namespace NUnitTestEQS
{
    public class DummyActor : IQuerier
    {
        public Vector3 GetLocation()
        {
            return new Vector3();
        }

        public Vector3 GetRotation()
        {
            return new Vector3();
        }
    }

    public class QueryResult
    {
        private DummyActor _querier;

        [SetUp]
        public void Setup()
        {
            _querier = new DummyActor();
        }

        [Test]
        public void GetBestScoreResult()
        {
            var generator = new Generator_Spiral(new ContextQuerier());
            var tests = new List<QueryTest>()
            {
                new Test_Distance(new ContextQuerier())
            };
            var template = new QueryTemplate(generator, tests);
            var wrapper = Manager.RunEQSQuery(template, _querier);

            wrapper.Run();

            var result = wrapper.QueryResult.GetBestScoreResult<Vector3>();

            Assert.AreEqual(wrapper.QueryResult.GetItem<Vector3>(wrapper.QueryResult.Length), result);
        }

        [Test]
        public void DrawDebugItem()
        {
            var generator = new Generator_Spiral(new ContextQuerier());
            var template = new QueryTemplate(generator, new List<QueryTest>());
            var wrapper = Manager.RunEQSQuery(template, _querier);

            wrapper.Run();

            DebugDrawFactory.Get().RegisterFactory(DebugShape.Sphere, item =>
            {
                // * Write draw API in here.
                Console.Write($"{item.Point} => {item.Score}");
            });

            wrapper.QueryResult.DrawDebugItem(DebugShape.Sphere);
        }
    }
}
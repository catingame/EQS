using NUnit.Framework;

using System.Collections.Generic;
using System.Numerics;
using EQS;
using EQS.Generators;
using EQS.Tests;
using EQS.Contexts;
using EQS.Classes;

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
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetBestScoreResult()
        {
            var qurier = new DummyActor();

            var generator = new GeneratorSpiral(new ContextQuerier());
            var tests = new List<QueryTest>()
            {
                new TestDelayDistance(new ContextQuerier())
            };

            var template = new QueryTemplate(generator, tests);

            var wrapper = Manager.RunEQSQuery(template, qurier);

            wrapper.Run();

            var result = wrapper.QueryResult.GetBestScoreResult<Vector3>();

            Assert.AreEqual(wrapper.QueryResult.GetItem<Vector3>(wrapper.QueryResult.Length), result);
        }
    }
}
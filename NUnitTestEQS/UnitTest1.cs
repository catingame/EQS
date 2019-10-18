using NUnit.Framework;

using System.Collections.Generic;

using EQS;
using EQS.Generators;
using EQS.Tests;
using EQS.Contexts;
using EQS.Classes;

namespace NUnitTestEQS
{
    public class DummyActor : IQuerier
    {
        public Location GetLocation()
        {
            return new Location();
        }

        public Rotation GetRotation()
        {
            return new Rotation();
        }
    }

    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var qurier = new DummyActor();

            var generator = new GeneratorSpiral(new ContextQuerier());

            TestDelayDot testDelayDot = new TestDelayDot(new ContextQuerier(), new ContextQuerier(), 0);
            var tests = new List<QueryTest>() { testDelayDot };

            
            var template = new QueryTemplate(generator, tests);

            var wrapper = Manager.RunEQSQuery(template, qurier);

            wrapper.Run();

            
            var results = wrapper.GetResults<DummyActor>();

            
            Assert.Pass();
        }
    }
}
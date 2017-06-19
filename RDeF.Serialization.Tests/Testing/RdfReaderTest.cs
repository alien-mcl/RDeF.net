using NUnit.Framework;
using RDeF.Serialization;

namespace RDeF.Testing
{
    public class RdfReaderTest<T> : RdfTest where T : IRdfReader, new()
    {
        protected IRdfReader Reader { get; private set; }

        public virtual void TheTest()
        {
        }

        [SetUp]
        public void Setup()
        {
            Reader = new T();
            ScenarioSetup();
            TheTest();
        }

        protected virtual void ScenarioSetup()
        {
        }
    }
}

using NUnit.Framework;
using RDeF.Serialization;

namespace RDeF.Testing
{
    public class RdfWriterTest<T> where T : IRdfWriter, new()
    {
        protected IRdfWriter Writer { get; private set; }

        public virtual void TheTest()
        {
        }

        [SetUp]
        public void Setup()
        {
            Writer = new T();
            ScenarioSetup();
            TheTest();
        }

        protected virtual void ScenarioSetup()
        {
        }
    }
}

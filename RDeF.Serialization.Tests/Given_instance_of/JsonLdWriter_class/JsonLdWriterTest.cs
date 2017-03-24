using NUnit.Framework;
using RDeF.Serialization;

namespace Given_instance_of.JsonLdWriter_class
{
    public class JsonLdWriterTest
    {
        protected IRdfWriter Writer { get; private set; }

        public virtual void TheTest()
        {
        }

        [SetUp]
        public void Setup()
        {
            Writer = new JsonLdWriter();
            ScenarioSetup();
            TheTest();
        }

        protected virtual void ScenarioSetup()
        {
        }
    }
}

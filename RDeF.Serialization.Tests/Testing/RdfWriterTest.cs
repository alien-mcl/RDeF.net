using System.Threading.Tasks;
using NUnit.Framework;
using RDeF.Serialization;

namespace RDeF.Testing
{
    public class RdfWriterTest<T> : RdfTest where T : IRdfWriter, new()
    {
        protected IRdfWriter Writer { get; private set; }

        public virtual Task TheTest()
        {
            return Task.CompletedTask;
        }

        [SetUp]
        public async Task Setup()
        {
            Writer = new T();
            ScenarioSetup();
            await TheTest();
        }

        protected virtual void ScenarioSetup()
        {
        }
    }
}

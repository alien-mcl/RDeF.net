using System.Threading.Tasks;
using NUnit.Framework;
using RDeF.Serialization;

namespace RDeF.Testing
{
    public class RdfReaderTest<T> : RdfTest where T : IRdfReader, new()
    {
        protected IRdfReader Reader { get; private set; }

        public virtual Task TheTest()
        {
            return Task.CompletedTask;
        }

        [SetUp]
        public async Task Setup()
        {
            Reader = new T();
            ScenarioSetup();
            await TheTest();
        }

        protected virtual void ScenarioSetup()
        {
        }
    }
}

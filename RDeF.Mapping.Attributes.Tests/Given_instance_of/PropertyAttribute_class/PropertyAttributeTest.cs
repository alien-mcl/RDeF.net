using NUnit.Framework;
using PropertyAttribute = RDeF.Mapping.Attributes.PropertyAttribute;

namespace Given_instance_of.PropertyAttribute_class
{
    public abstract class PropertyAttributeTest
    {
        protected const string ExpectedPrefix = "prefix";

        protected const string ExpectedTerm = "term";

        protected PropertyAttribute Attribute { get; set; }

        public virtual void TheTest()
        {
        }

        [SetUp]
        public void Setup()
        {
            Attribute = new PropertyAttribute(ExpectedPrefix, ExpectedTerm);
            ScenarioSetup();
            TheTest();
        }

        protected virtual void ScenarioSetup()
        {
        }
    }
}
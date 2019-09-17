﻿using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using RDeF.Entities;
using RDeF.Mapping;

namespace Given_instance_of.DefaultEntityContext_class
{
    public abstract class DefaultEntityContextTest
    {
        internal Mock<IChangeDetector> ChangeDetector { get; private set; }

        protected Mock<IEntitySource> EntitySource { get; set; }

        protected Mock<IMappingsRepository> MappingsRepository { get; private set; }

        protected Mock<ILiteralConverter> LiteralConverter { get; private set; }

        protected DefaultEntityContext Context { get; set; }

        public virtual Task TheTest()
        {
            return Task.CompletedTask;
        }

        [SetUp]
        public async Task Setup()
        {
            EntitySource = new Mock<IEntitySource>(MockBehavior.Strict);
            MappingsRepository = new Mock<IMappingsRepository>(MockBehavior.Strict);
            ChangeDetector = new Mock<IChangeDetector>(MockBehavior.Strict);
            LiteralConverter = new Mock<ILiteralConverter>(MockBehavior.Strict);
            Context = new DefaultEntityContext(
                EntitySource.Object,
                MappingsRepository.Object,
                ChangeDetector.Object,
                new[] { LiteralConverter.Object });
            ScenarioSetup();
            await TheTest();
        }

        protected virtual void ScenarioSetup()
        {
        }
    }
}

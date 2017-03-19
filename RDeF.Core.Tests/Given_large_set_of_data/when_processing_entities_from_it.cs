using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using RDeF.Data;

namespace Given_large_set_of_data
{
    [TestFixture]
    public class when_processing_entities_from_it : PerformanceTest
    {
        private DateTime StartedAt { get; set; }

        private DateTime FinishedAt { get; set; }

        public override void TheTest()
        {
            StartedAt = DateTime.Now;
            foreach (var entity in Context.AsQueryable<IProduct>())
            {
                var name = entity.Name;
                var description = entity.Description;
                var ordinal = entity.Ordinal;
                var price = entity.Price;
                var categories = entity.Categories.ToList();
                var comments = entity.Comments.ToList();
            }

            FinishedAt = DateTime.Now;
        }

        ////[TestCase(100)]
        public void Should_process_those_entities_in_timely_fashion(int expectedMilliseconds)
        {
            (FinishedAt - StartedAt).TotalMilliseconds.Should().BeLessThan(expectedMilliseconds);
            Console.Write((FinishedAt - StartedAt).TotalMilliseconds);
        }
    }
}

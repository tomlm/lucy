using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Lucy.PatternMatchers;
using Lucy.PatternMatchers.Matchers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lucy.Tests
{
    [TestClass]
    public class MergeTests
    {
        [TestMethod]
        public void MergeNestedChildren()
        {
            var engine = new LucyEngine(new LucyDocument()
            {
                Entities = new List<EntityDefinition>()
                {
                    new EntityDefinition() { Name = "@PizzaOrder", Patterns = new List<Pattern>(){ "(@Topping)* (@AddToppings)* " } },
                    new EntityDefinition() { Name = "@AddToppings", Patterns = new List<Pattern>(){ "(@ToppingQuantifier)* (@Topping)+" } },
                    new EntityDefinition() { Name = "@ToppingQuantifier", Patterns = new List<Pattern>(){ "extra" } },
                    new EntityDefinition() { Name = "@Topping", Patterns = new List<Pattern>(){ "Cheese" } },
                }
            });

            string text = "extra cheese";
            var results = engine.MatchEntities(text, null);
            Trace.TraceInformation("\n" + LucyEngine.VisualEntities(text, results));
            Assert.AreEqual(1, results[0].Children.Count());
        }
    }
}

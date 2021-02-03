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
    public class BuiltinTests
    {
        [TestMethod]
        public void TestAllBuiltIns()
        {
            var engine = new LucyEngine(new LucyDocument()
            {
                Entities = new List<EntityDefinition>()
                {
                    //                    new EntityDefinition() { Name = "@test", Patterns = new List<Pattern>(){ "@url" } },
                }
            }, useAllBuiltIns: true);

            string text = "this is a http://foo.com?x=13&y=132123 cheese";
            var results = engine.MatchEntities(text, null);
            Trace.TraceInformation("\n" + LucyEngine.VisualEntities(text, results));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("http://foo.com?x=13&y=132123", results[0].Text);
        }

        [TestMethod]
        public void TestBuiltinsReferenced()
        {
            var engine = new LucyEngine(new LucyDocument()
            {
                Entities = new List<EntityDefinition>()
                {
                    new EntityDefinition() { Name = "@test", Patterns = new List<Pattern>(){ "@url" } },
                }
            }, useAllBuiltIns: false);

            string text = "this is a http://foo.com?x=13&y=132123 cheese";
            var results = engine.MatchEntities(text, null);
            Trace.TraceInformation("\n" + LucyEngine.VisualEntities(text, results));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("http://foo.com?x=13&y=132123", results[0].Text);
        }

        [TestMethod]
        public void TestNoBuiltinsReferenced()
        {
            var engine = new LucyEngine(new LucyDocument()
            {
                Entities = new List<EntityDefinition>()
                {
                }
            }, useAllBuiltIns: false);

            string text = "this is a http://foo.com?x=13&y=132123 cheese";
            var results = engine.MatchEntities(text, null);
            Trace.TraceInformation("\n" + LucyEngine.VisualEntities(text, results));
            Assert.AreEqual(0, results.Count());
        }

        [TestMethod]
        public void TestExternalEntities()
        {
            var engine = new LucyEngine(new LucyDocument()
            {
                ExternalEntities = new List<string>() { "url" },
                Entities = new List<EntityDefinition>()
                {
                }
            }, useAllBuiltIns: false);

            string text = "this is a http://foo.com?x=13&y=132123 cheese";
            var results = engine.MatchEntities(text, null);
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("http://foo.com?x=13&y=132123", results[0].Text);
        }
    }
}

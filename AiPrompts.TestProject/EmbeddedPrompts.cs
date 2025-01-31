using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace AiPrompts
{
    [TestClass]
    public sealed class GetAllEmbeddedPrompts
    {
        [TestMethod]
        public void GetAllPathsUnitTests()
        {

            IEnumerable<string> embeddedPrompts
                = EmbeddedPrompts.GetAllPaths();

            foreach (string embeddedPrompt in embeddedPrompts)
            {
                Console.WriteLine(embeddedPrompt);
                Assert.IsTrue(embeddedPrompt.EndsWith(".txt"));
            }
        }
    }
}

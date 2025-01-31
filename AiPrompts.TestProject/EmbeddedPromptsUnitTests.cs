using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace AiPrompts
{
    [TestClass]
    public sealed class GetAllEmbeddedPrompts
    {
        [TestMethod]
        public void GetAllSolutionPathsUnitTest()
        {

            IEnumerable<string> embeddedPrompts
                = EmbeddedPrompts.GetAllPaths("Solution.txt");

            foreach (string embeddedPrompt in embeddedPrompts)
            {
                Console.WriteLine(embeddedPrompt);
                Assert.IsTrue(embeddedPrompt.EndsWith(".txt"));
            }
        }

        [TestMethod]
        public void GetAllProjectPathsUnitTest()
        {

            IEnumerable<string> embeddedPrompts
                = EmbeddedPrompts.GetAllPaths("Project.txt");

            foreach (string embeddedPrompt in embeddedPrompts)
            {
                Console.WriteLine(embeddedPrompt);
                Assert.IsTrue(embeddedPrompt.EndsWith(".txt"));
            }
        }

        [TestMethod]
        public void GetAllSolutionPromptContentUnitTest()
        {
            IEnumerable<string> embeddedPrompts
                = EmbeddedPrompts.GetAllPaths("Solution.txt");

            foreach (string embeddedPrompt in embeddedPrompts)
            {
                Console.WriteLine(embeddedPrompt);
                string prompt = EmbeddedPrompts.GetPrompt(embeddedPrompt);
                Console.WriteLine(prompt);
                Assert.IsTrue(prompt.Length > 0);
            }
        }


        [TestMethod]
        public void GetAllProjectPromptContentUnitTest()
        {
            IEnumerable<string> embeddedPrompts
                = EmbeddedPrompts.GetAllPaths("Project.txt");

            foreach (string embeddedPrompt in embeddedPrompts)
            {
                Console.WriteLine(embeddedPrompt);
                string prompt = EmbeddedPrompts.GetPrompt(embeddedPrompt);
                Console.WriteLine(prompt);
                Assert.IsTrue(prompt.Length > 0);
            }
        }
    }
}

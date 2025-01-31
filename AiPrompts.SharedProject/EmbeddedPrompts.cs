using System.Collections.Generic;
using System.Reflection;

namespace AiPrompts
{
    public static class EmbeddedPrompts
    {
        public static IEnumerable<string> GetAllPaths(string endsWith = ".txt")
        {
            string[] embeddedResources
                = Assembly
                    .GetExecutingAssembly()
                        .GetManifestResourceNames();

            foreach (string embeddedResource in embeddedResources)
                if (embeddedResource.EndsWith(endsWith))
                    yield return embeddedResource;
        }

        public static string GetPrompt(string path)
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path))
            using (var reader = new System.IO.StreamReader(stream))
                return reader.ReadToEnd();
        }
    }
}

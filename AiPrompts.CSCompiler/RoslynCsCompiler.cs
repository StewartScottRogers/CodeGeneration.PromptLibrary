using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Text;

namespace AiPrompts
{
    public static class RoslynCsCompiler
    {
        public static SyntaxTree CompileSyntaxTree(string csCode)
        {
            SyntaxTree syntaxTree
                = CSharpSyntaxTree.ParseText(csCode);

            return syntaxTree;
        }

        public static string PrintSyntaxTree(this SyntaxNode node, string indent = "", StringBuilder stringBuilder = null)
        {
            if (stringBuilder is null)
                stringBuilder = new StringBuilder();

            stringBuilder
                .AppendLine($"{indent}{node.Kind()} - {node}");

            foreach (SyntaxNodeOrToken syntaxNodeOrToken in node.ChildNodesAndTokens())
                if (syntaxNodeOrToken.IsNode)

                    syntaxNodeOrToken
                        .AsNode()
                            .PrintSyntaxTree(
                                indent + "  ",
                                stringBuilder
                             );
                else
                    stringBuilder.AppendLine($"{indent}  {syntaxNodeOrToken.Kind()} - {syntaxNodeOrToken}");

            return stringBuilder.ToString();
        }
    }
}

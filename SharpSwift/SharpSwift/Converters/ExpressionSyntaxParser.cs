﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SharpSwift.Converters
{
    partial class ConvertToSwift
    {
        [ParsesType(typeof(MemberAccessExpressionSyntax))]
        public static string MemberAccessExpression(MemberAccessExpressionSyntax node)
        {
            return SyntaxNode(node.Expression) + node.OperatorToken.Text + SyntaxNode(node.Name);
        }

        //something.Method("arg")
        [ParsesType(typeof(InvocationExpressionSyntax))]
        public static string InvocationExpression(InvocationExpressionSyntax node)
        {
            return SyntaxNode(node.Expression) + SyntaxNode(node.ArgumentList);
        }

        //new Something()
        [ParsesType(typeof(ObjectCreationExpressionSyntax))]
        public static string ObjectCreationExpression(ObjectCreationExpressionSyntax node)
        {
            var output = Type(((IdentifierNameSyntax)node.Type).Identifier.Text) + "(";

            foreach (var arg in node.ArgumentList.Arguments)
            {
                output += SyntaxNode(arg.Expression).Trim() + ", ";
            }

            output = output.Trim(' ', ',') + ")";
            return output;
        }

        //var something = something_else;
        [ParsesType(typeof(LocalDeclarationStatementSyntax))]
        public static string LocalDeclarationStatement(LocalDeclarationStatementSyntax node)
        {
            return SyntaxNode(node.Declaration) + Semicolon(node.SemicolonToken);
        }

        //something (+/-/+=/etc) something_else
        [ParsesType(typeof(BinaryExpressionSyntax))]
        public static string BinaryExpression(BinaryExpressionSyntax node)
        {
            var output = SyntaxNode(node.Left).Trim();
            output += " " + node.OperatorToken.Text + " ";
            output += SyntaxNode(node.Right).Trim();
            return output;
        }

        [ParsesType(typeof(ExpressionStatementSyntax))]
        public static string ExpressionStatement(ExpressionStatementSyntax node)
        {
            return SyntaxNode(node.Expression) + Semicolon(node.SemicolonToken);
        }

        [ParsesType(typeof(ExpressionSyntax))]
        public static string Expression(ExpressionSyntax node)
        {
            return node.ToString();
        }

        [ParsesType(typeof(LiteralExpressionSyntax))]
        public static string LiteralExpression(LiteralExpressionSyntax node)
        {
            if (node.IsKind(SyntaxKind.CharacterLiteralExpression))
            {
                //this is sketch, probably shouldn't use char literals o.o
                return '"' + node.Token.ValueText.Replace("\\'", "'").Replace("\"", "\\\"") + '"';
            }
            return node.ToString();
        }
    }
}

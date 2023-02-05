using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace Roslintor
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class RoslintorAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "Roslintor";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Localizing%20Analyzers.md for more on localization
        //private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        //private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        //private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Title = "Rename your method";
        private const string MessageFormat = "Method '{0}' is not written in camelCase. Consider changing your method name to camelCase.";
        private const string Description = "Change your method name to camelCase.";
        private const string Category = "Naming";

        //private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
        //    DiagnosticId,
        //    Title,
        //    MessageFormat,
        //    Category,
        //    DiagnosticSeverity.Warning,
        //    isEnabledByDefault: true,
        //    description: Description);

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            Title,
            MessageFormat,
            Category,
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            // TODO: Consider registering other actions that act on syntax instead of or in addition to symbols
            // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Method);
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            // TODO: Replace the following code with your own analysis, generating Diagnostic objects for any issues you find
            var namedTypeSymbol = context.Symbol;

            // Find just those named type symbols with names containing lowercase letters.
            //if (namedTypeSymbol.Name.ToCharArray().Any(char.IsLower))
            //{
            //    // For all such symbols, produce a diagnostic.
            //    var diagnostic = Diagnostic.Create(Rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);

            //    context.ReportDiagnostic(diagnostic);
            //}
            if (!IsCamelCase(namedTypeSymbol.Name))
            {
                var diagnostic = Diagnostic.Create(Rule, namedTypeSymbol.Locations.First(), namedTypeSymbol.Name);

                context.ReportDiagnostic(diagnostic);
            }
        }


        private static bool IsCamelCase(string name)
        {
            if (string.IsNullOrEmpty(name) || char.IsUpper(name[0]))
            {
                return false;
            }

            for (int i = 1; i < name.Length; i++)
            {
                if (char.IsUpper(name[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}

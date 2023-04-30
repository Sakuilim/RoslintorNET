; Unshipped analyzer release
; https://github.com/dotnet/roslyn-analyzers/blob/main/src/Microsoft.CodeAnalysis.Analyzers/ReleaseTrackingAnalyzers.Help.md

### New Rules

Rule ID | Category | Severity | Notes
--------|----------|----------|-------
AC001 | Naming | Warning | ArgumentCountAnalyzer
CA001 | Performance | Warning | MethodCyclomaticComplexityAnalyzer, [Documentation](https://learn.microsoft.com/dotnet/fundamentals/code-analysis/quality-rules/ca001)
CA002 | Performance | Warning | MethodCognitiveComplexityAnalyzer, [Documentation](https://learn.microsoft.com/dotnet/fundamentals/code-analysis/quality-rules/ca002)
CA003 | Performance | Warning | ClassCognitiveComplexityAnalyzer, [Documentation](https://learn.microsoft.com/dotnet/fundamentals/code-analysis/quality-rules/ca003)
CA004 | Performance | Warning | ClassCyclomaticComplexityAnalyzer, [Documentation](https://learn.microsoft.com/dotnet/fundamentals/code-analysis/quality-rules/ca004)
CA01 | Performance | Warning | CyclomaticComplexityAnalyzer, [Documentation](https://learn.microsoft.com/dotnet/fundamentals/code-analysis/quality-rules/ca01)
CA04 | Performance | Warning | ClassCyclomaticComplexityAnalyzer, [Documentation](https://learn.microsoft.com/dotnet/fundamentals/code-analysis/quality-rules/ca04)
CD001 | Performance | Warning | CodeDuplicationAnalyzer
FA001 | Format | Warning | NestingLevelAnalyzer
MA001 | Maintainability | Warning | MethodMaintainabilityAnalyzer
MA002 | Maintainability | Warning | ClassMaintainabilityAnalyzer
MSA002 | Style | Warning | MethodSizeAnalyzer
MSA003 | Style | Warning | MethodSize80Analyzer
PPA001 | Performance | Warning | PerformancePracticeAnalyzer
Roslintor | Naming | Warning | RoslintorAnalyzer
SSA001 | Security | Warning | SecureStringAnalyzer
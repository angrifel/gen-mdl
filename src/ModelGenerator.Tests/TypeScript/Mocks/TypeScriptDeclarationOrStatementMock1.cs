namespace ModelGenerator.Tests.TypeScript.Mocks
{
  using ModelGenerator.TypeScript;
  using System.IO;
  using System;

  public class TypeScriptDeclarationOrStatementMock1 : TypeScriptDeclarationOrStatement, IEquatable<TypeScriptDeclarationOrStatementMock1>
  {
    private readonly string _desiredOutput;

    public TypeScriptDeclarationOrStatementMock1(string desiredOutput)
    {
      _desiredOutput = desiredOutput;
    }

    public bool Equals(TypeScriptDeclarationOrStatementMock1 other) =>
      other != null &&
      _desiredOutput == other._desiredOutput;

    public override bool Equals(TypeScriptDeclarationOrStatement other) => Equals(other as TypeScriptDeclarationOrStatementMock1);

    public override bool Equals(object obj) => Equals(obj as TypeScriptDeclarationOrStatementMock1);

    public override void Generate(TextWriter output)
    {
      output.WriteLine(_desiredOutput);
    }

    public override int GetHashCode() => unchecked(_desiredOutput?.GetHashCode() ?? 0);
  }
}

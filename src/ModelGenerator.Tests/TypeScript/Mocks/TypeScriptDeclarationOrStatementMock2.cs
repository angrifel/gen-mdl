namespace ModelGenerator.Tests.TypeScript.Mocks
{
  using ModelGenerator.TypeScript;
  using System;
  using System.IO;

  public class TypeScriptDeclarationOrStatementMock2 : TypeScriptDeclarationOrStatement, IEquatable<TypeScriptDeclarationOrStatementMock2>
  {
    private readonly string _desiredOutput;

    public TypeScriptDeclarationOrStatementMock2(string desiredOutput)
    {
      _desiredOutput = desiredOutput;
    }

    public override void Generate(TextWriter output)
    {
      output.WriteLine(_desiredOutput);
    }

    public bool Equals(TypeScriptDeclarationOrStatementMock2 other) =>
      other != null &&
      _desiredOutput == other._desiredOutput;

    public override bool Equals(TypeScriptDeclarationOrStatement other) => Equals(other as TypeScriptDeclarationOrStatementMock2);

    public override bool Equals(object obj) => Equals(obj as TypeScriptDeclarationOrStatementMock2);

    public override int GetHashCode() => _desiredOutput?.GetHashCode() ?? 0;
  }
}

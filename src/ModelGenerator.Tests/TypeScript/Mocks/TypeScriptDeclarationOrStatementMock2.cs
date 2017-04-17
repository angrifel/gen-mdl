namespace ModelGenerator.Tests.TypeScript.Mocks
{
  using ModelGenerator.TypeScript;
  using System.IO;

  public class TypeScriptDeclarationOrStatementMock2 : TypeScriptDeclarationOrStatement
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
  }
}

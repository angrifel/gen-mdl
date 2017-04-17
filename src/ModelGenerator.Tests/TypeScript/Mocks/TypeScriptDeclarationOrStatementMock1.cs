namespace ModelGenerator.Tests.TypeScript.Mocks
{
  using ModelGenerator.TypeScript;
  using System.IO;

  public class TypeScriptDeclarationOrStatementMock1 : TypeScriptDeclarationOrStatement
  {
    private readonly string _desiredOutput;

    public TypeScriptDeclarationOrStatementMock1(string desiredOutput)
    {
      _desiredOutput = desiredOutput;
    }

    public override void Generate(TextWriter output)
    {
      output.WriteLine(_desiredOutput);
    }
  }
}

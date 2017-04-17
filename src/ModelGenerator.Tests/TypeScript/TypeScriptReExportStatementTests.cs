namespace ModelGenerator.Tests.TypeScript
{
  using ModelGenerator.TypeScript;
  using System;
  using System.IO;
  using Xunit;

  public class TypeScriptReExportStatementTests
  {
    [Fact]
    public void TestDefaultReExport()
    {
      // arrange
      var reExportStatement = new TypeScriptReExportStatement { FileName = "some-component" };
      var actualOutput = new StringWriter();
      var expectedOutput = "export * from './some-component';" + Environment.NewLine;

      // act
      reExportStatement.Generate(actualOutput);

      // assert
      Assert.Equal(expectedOutput, actualOutput.ToString());
    }

    [Fact]
    public void TestObjectReExport()
    {
      // arrange
      var reExportStatement = new TypeScriptReExportStatement { Object = "someComponent", FileName = "some-component" };
      var actualOutput = new StringWriter();
      var expectedOutput = "export { someComponent } from './some-component';" + Environment.NewLine;

      // act
      reExportStatement.Generate(actualOutput);

      // assert
      Assert.Equal(expectedOutput, actualOutput.ToString());
    }

    [Fact]
    public void TestObjectReExportWithAlias()
    {
      // arrange
      var reExportStatement = new TypeScriptReExportStatement { Object = "someComponent", Alias = "myComponent", FileName = "some-component" };
      var actualOutput = new StringWriter();
      var expectedOutput = "export { someComponent as myComponent } from './some-component';" + Environment.NewLine;

      // act
      reExportStatement.Generate(actualOutput);

      // assert
      Assert.Equal(expectedOutput, actualOutput.ToString());
    }
  }
}

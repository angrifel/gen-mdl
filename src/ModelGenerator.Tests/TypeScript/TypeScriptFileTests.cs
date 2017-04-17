namespace ModelGenerator.Tests.TypeScript
{
  using ModelGenerator.TypeScript;
  using System;
  using System.Collections.Generic;
  using System.IO;
  using Xunit;

  public class TypeScriptFileTests
  {
    [Fact]
    public void TestEmptyFile()
    {
      // arrange
      var typescriptFile = new TypeScriptFile();
      var expectedOutput = "";
      var actualOutput = new StringWriter();

      // act
      typescriptFile.Generate(actualOutput);

      // assert
      Assert.Equal(expectedOutput, actualOutput.ToString());
    }

    [Fact]
    public void TestFileWithOneComponent()
    {
      // arrange
      var typescriptFile = 
        new TypeScriptFile
        {
          Contents = new List<TypeScriptDeclarationOrStatement>
          {
            new Mocks.TypeScriptDeclarationOrStatementMock1("component1;")
          }
        };
      var expectedOutput = "component1;" + Environment.NewLine;
      var actualOutput = new StringWriter();

      // act
      typescriptFile.Generate(actualOutput);

      // assert
      Assert.Equal(expectedOutput, actualOutput.ToString());
    }

    [Fact]
    public void TestFileWithTwocompoenentOfSameType()
    {
      // arrange
      var typescriptFile =
        new TypeScriptFile
        {
          Contents = new List<TypeScriptDeclarationOrStatement>
          {
            new Mocks.TypeScriptDeclarationOrStatementMock1("component1;"),
            new Mocks.TypeScriptDeclarationOrStatementMock1("component2;")
          }
        };
      var expectedOutput =
        "component1;" + Environment.NewLine +
        "component2;" + Environment.NewLine;
      var actualOutput = new StringWriter();

      // act
      typescriptFile.Generate(actualOutput);

      // assert
      Assert.Equal(expectedOutput, actualOutput.ToString());
    }

    [Fact]
    public void TestFileWithTwoComponentsOfDiferentTypes()
    {
      // arrange
      var typescriptFile =
        new TypeScriptFile
        {
          Contents = new List<TypeScriptDeclarationOrStatement>
          {
            new Mocks.TypeScriptDeclarationOrStatementMock1("componentT1;"),
            new Mocks.TypeScriptDeclarationOrStatementMock2("componentT2;")
          }
        };
      var expectedOutput =
        "componentT1;" + Environment.NewLine +
        Environment.NewLine +
        "componentT2;" + Environment.NewLine;
      var actualOutput = new StringWriter();

      // act
      typescriptFile.Generate(actualOutput);

      // assert
      Assert.Equal(expectedOutput, actualOutput.ToString());
    }

    [Fact]
    public void TestFileWithManyComponentOfDifferentTypesInterleaved()
    {
      // arrange
      var typescriptFile =
        new TypeScriptFile
        {
          Contents = new List<TypeScriptDeclarationOrStatement>
          {
            new Mocks.TypeScriptDeclarationOrStatementMock1("componentT1A;"),
            new Mocks.TypeScriptDeclarationOrStatementMock2("componentT2A;"),
            new Mocks.TypeScriptDeclarationOrStatementMock1("componentT1B;")
          }
        };
      var expectedOutput =
        "componentT1A;" + Environment.NewLine +
        Environment.NewLine +
        "componentT2A;" + Environment.NewLine +
        Environment.NewLine +
        "componentT1B;" + Environment.NewLine;
      var actualOutput = new StringWriter();

      // act
      typescriptFile.Generate(actualOutput);

      // assert
      Assert.Equal(expectedOutput, actualOutput.ToString());
    }
  }
}

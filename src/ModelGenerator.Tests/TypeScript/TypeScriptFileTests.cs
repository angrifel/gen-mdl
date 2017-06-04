//  This file is part of gen-mdl - A Source code generator for model definitions.
//  Copyright (c) angrifel

//  Permission is hereby granted, free of charge, to any person obtaining a copy of
//  this software and associated documentation files (the "Software"), to deal in
//  the Software without restriction, including without limitation the rights to
//  use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
//  of the Software, and to permit persons to whom the Software is furnished to do
//  so, subject to the following conditions:

//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.

//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.

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

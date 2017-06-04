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
  using System.IO;
  using Xunit;

  public class TypeScriptExportStatementTests
  {
    [Fact]
    public void TestExportObject()
    {
      // arrange
      var exportStatement = new TypeScriptExportStatement { Object = "Invoice" };
      var outputWriter = new StringWriter();
      var expectedOutput = @"export Invoice;" + Environment.NewLine;

      // act
      exportStatement.Generate(outputWriter);

      // assert
      Assert.Equal(expectedOutput, outputWriter.ToString());
    }

    [Fact]
    public void TestExportTypeDeclaration()
    {
      // arrange
      var typeDeclaration = new TypeScriptClass { Name = "Invoice" };
      var exportStatement = new TypeScriptExportStatement { TypeDeclaration = typeDeclaration };
      var outputWriter = new StringWriter();
      var expectedOutputWriter = new StringWriter();
      expectedOutputWriter.Write("export ");
      typeDeclaration.Generate(expectedOutputWriter);

      // act
      exportStatement.Generate(outputWriter);

      // assert
      Assert.Equal(expectedOutputWriter.ToString(), outputWriter.ToString());
    }

    [Fact]
    public void TestExportDefaultObject()
    {
      // arrange
      var exportStatement = new TypeScriptExportStatement { Object = "Invoice", IsDefault = true };
      var outputWriter = new StringWriter();
      var expectedOutput = @"export default Invoice;" + Environment.NewLine;

      // act
      exportStatement.Generate(outputWriter);

      // assert
      Assert.Equal(expectedOutput, outputWriter.ToString());
    }

    [Fact]
    public void TestExportDefaultTypeDeclaration()
    {
      // arrange
      var typeDeclaration = new TypeScriptClass { Name = "Invoice" };
      var exportStatement = new TypeScriptExportStatement { TypeDeclaration = typeDeclaration, IsDefault = true };
      var outputWriter = new StringWriter();
      var expectedOutputWriter = new StringWriter();
      expectedOutputWriter.Write("export default ");
      typeDeclaration.Generate(expectedOutputWriter);

      // act
      exportStatement.Generate(outputWriter);

      // assert
      Assert.Equal(expectedOutputWriter.ToString(), outputWriter.ToString());
    }
  }
}

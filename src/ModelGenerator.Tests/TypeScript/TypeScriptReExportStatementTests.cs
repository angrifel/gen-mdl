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

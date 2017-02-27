//  This file is part of mdlgen - A Source code generator for model definitions.
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
  using System.Collections.Generic;
  using System.IO;
  using Xunit;

  public class TypeScriptEnumTests
  {
    [Fact]
    public void TestEnumGeneration()
    {
      // arrange
      var draftEnumMember = new TypeScriptEnumMember { Name = "Draft" };
      var finalEnumMember = new TypeScriptEnumMember { Name = "Final" };
      var typeScriptEnum = new TypeScriptEnum
      {
        Name = "BillStatus",
        Members = new List<TypeScriptEnumMember> { draftEnumMember, finalEnumMember }
      };
      var output = new StringWriter();
      var expectOutputWriter = new StringWriter();
      expectOutputWriter.WriteLine("enum BillStatus {");
      draftEnumMember.Generate(expectOutputWriter, false);
      finalEnumMember.Generate(expectOutputWriter, true);
      expectOutputWriter.WriteLine("}");

      // act
      typeScriptEnum.Generate(output);

      // assert
      Assert.Equal(expectOutputWriter.GetStringBuilder().ToString(), output.GetStringBuilder().ToString());
    }
  }
}

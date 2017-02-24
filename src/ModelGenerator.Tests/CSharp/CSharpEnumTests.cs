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

namespace ModelGenerator.Tests.CSharp
{
  using ModelGenerator.CSharp;
  using System.Collections.Generic;
  using System.IO;
  using Xunit;

  public class CSharpEnumTests
  {
    [Fact]
    public void TestEnumGeneration()
    {
      // arrange
      var draftEnumMember = new CSharpEnumMember { Name = "Draft" };
      var finalEnumMember = new CSharpEnumMember { Name = "Final" };
      var csharpEnum = new CSharpEnum {
        Name = "BillStatus",
        Members = new List<CSharpEnumMember> { draftEnumMember, finalEnumMember }
      };
      var output = new StringWriter();
      var expectOutputWriter = new StringWriter();
      expectOutputWriter.WriteLine("  public enum BillStatus");
      expectOutputWriter.WriteLine("  {");
      draftEnumMember.Generate(expectOutputWriter, false);
      finalEnumMember.Generate(expectOutputWriter, true);
      expectOutputWriter.WriteLine("  }");

      // act
      csharpEnum.Generate(output);

      // assert
      Assert.Equal(expectOutputWriter.GetStringBuilder().ToString(), output.GetStringBuilder().ToString());
    }
  }
}

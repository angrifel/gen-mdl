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

namespace ModelGenerator.Tests.TypeScript.Utilities
{
  using ModelGenerator.TypeScript.Utilities;
  using System.Linq;
  using Xunit;

  public class TypeScriptFactsTests
  {
    [Fact]
    public void TestAmmendmentFillAllNativeTypes()
    {
      // arrange
      // nothing to do

      // act
      var nativeTypes = TypeScriptFacts.GetNativeTypes().ToList();

      // assert
      Assert.True(nativeTypes.Remove("boolean"));
      Assert.True(nativeTypes.Remove("number"));
      Assert.True(nativeTypes.Remove("string"));
      Assert.True(nativeTypes.Remove("Date"));
      Assert.True(nativeTypes.Remove("any"));
      Assert.True(nativeTypes.Count == 0, "Additional Native types present than those that were tested. Adjust your unit test.");
    }
  }
}

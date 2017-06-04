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

namespace ModelGenerator.Tests.CSharp.Utilities
{
  using ModelGenerator.CSharp.Utilities;
  using System.Linq;
  using Xunit;

  public class CSharpFactsTests
  {
    [Fact]
    public void TestAmmendmentFillAllNativeTypes()
    {
      // arrange
      // nothing here

      // act
      var nativeTypes = CSharpFacts.GetNativeTypes().ToList();

      // assert
      Assert.True(nativeTypes.Remove("bool"));
      Assert.True(nativeTypes.Remove("char"));
      Assert.True(nativeTypes.Remove("byte"));
      Assert.True(nativeTypes.Remove("short"));
      Assert.True(nativeTypes.Remove("int"));
      Assert.True(nativeTypes.Remove("long"));
      Assert.True(nativeTypes.Remove("float"));
      Assert.True(nativeTypes.Remove("double"));
      Assert.True(nativeTypes.Remove("decimal"));
      Assert.True(nativeTypes.Remove("string"));
      Assert.True(nativeTypes.Remove("object"));
      Assert.True(nativeTypes.Remove("TimeSpan"));
      Assert.True(nativeTypes.Remove("DateTime"));
      Assert.True(nativeTypes.Remove("DateTimeOffset"));
      Assert.True(nativeTypes.Remove("Guid"));
      Assert.True(nativeTypes.Count == 0, "Additional Native types present than those that were tested. Adjust your unit test.");
    }
  }
}

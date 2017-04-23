//  This file is part of genmdl - A Source code generator for model definitions.
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
  using ModelGenerator.Model;
  using ModelGenerator.TypeScript.Services;
  using System.Collections.Generic;
  using System.Linq;
  using Xunit;

  public class TypeScriptGeneratorTests
  {
    [Fact]
    public void TestGenerateOutputOnEmptySpec()
    {
      // arrange
      var spec =
        new Spec
        {
          Targets = new Dictionary<string, TargetInfo>
          {
            { Constants.TypeScriptTarget, new TargetInfo { Path = "some_path", Namespace = "Blogged"} }
          }
        };
      var generator = new TypeScriptGenerator();

      // act
      var outputs = generator.GenerateOutputs(spec);

      // assert
      Assert.Equal(0, outputs.Count());
    }
  }
}

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
  using ModelGenerator.Model;
  using ModelGenerator.TypeScript;
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Xunit;

  public class TypeScriptAmmendmentTests
  {
    [Fact]
    public void TestAmmendSpecificationMapsAllAliasesOnEmptySpec()
    {
      // arrange
      var spec = new Spec { Targets = new Dictionary<string, TargetInfo> { { Constants.TypeScriptTarget, new TargetInfo() } } };
      var ammendment = new TypeScriptAmmendment();

      // act
      ammendment.AmmedSpecification(spec);

      // assert
      var typeAliases = spec.Targets[Constants.TypeScriptTarget].TypeAliases;
      Assert.True(typeAliases.Remove("bool"));
      Assert.True(typeAliases.Remove("char"));
      Assert.True(typeAliases.Remove("byte"));
      Assert.True(typeAliases.Remove("short"));
      Assert.True(typeAliases.Remove("int"));
      Assert.True(typeAliases.Remove("long"));
      Assert.True(typeAliases.Remove("float"));
      Assert.True(typeAliases.Remove("double"));
      Assert.True(typeAliases.Remove("decimal"));
      Assert.True(typeAliases.Remove("string"));
      Assert.True(typeAliases.Remove("object"));
      Assert.True(typeAliases.Remove("time"));
      Assert.True(typeAliases.Remove("date"));
      Assert.True(typeAliases.Remove("datetime"));
      Assert.True(typeAliases.Remove("guid"));
      Assert.True(typeAliases.Count == 0, "Additional TypeAliases present than those that were tested. Adjust your code and/or unit test.");
    }

    [Fact]
    public void TestAmmendSpecificationMapsAllAliasesToExpectedNativeTypesOnEmptySpec()
    {
      // arrange
      var spec = new Spec { Targets = new Dictionary<string, TargetInfo> { { Constants.TypeScriptTarget, new TargetInfo() } } };
      var ammendment = new TypeScriptAmmendment();

      // act
      ammendment.AmmedSpecification(spec);

      // assert
      var typeAliases = spec.Targets[Constants.TypeScriptTarget].TypeAliases.ToList();
      Assert.True(typeAliases.Remove(new KeyValuePair<string, string>("bool", "boolean")));
      Assert.True(typeAliases.Remove(new KeyValuePair<string, string>("char", "string")));
      Assert.True(typeAliases.Remove(new KeyValuePair<string, string>("byte", "number")));
      Assert.True(typeAliases.Remove(new KeyValuePair<string, string>("short", "number")));
      Assert.True(typeAliases.Remove(new KeyValuePair<string, string>("int", "number")));
      Assert.True(typeAliases.Remove(new KeyValuePair<string, string>("long", "string")));
      Assert.True(typeAliases.Remove(new KeyValuePair<string, string>("float", "number")));
      Assert.True(typeAliases.Remove(new KeyValuePair<string, string>("double", "number")));
      Assert.True(typeAliases.Remove(new KeyValuePair<string, string>("decimal", "string")));
      Assert.True(typeAliases.Remove(new KeyValuePair<string, string>("string", "string")));
      Assert.True(typeAliases.Remove(new KeyValuePair<string, string>("object", "any")));
      Assert.True(typeAliases.Remove(new KeyValuePair<string, string>("time", "Date")));
      Assert.True(typeAliases.Remove(new KeyValuePair<string, string>("date", "Date")));
      Assert.True(typeAliases.Remove(new KeyValuePair<string, string>("datetime", "Date")));
      Assert.True(typeAliases.Remove(new KeyValuePair<string, string>("guid", "string")));
      Assert.True(typeAliases.Count == 0, "Additional TypeAliases present than those that were tested. Adjust your code and/or unit test.");
    }

    [Fact]
    public void TestAmmendSpecificationFailsToReplaceBuiltinTypes()
    {
      // arrange
      var spec =
        new Spec
        {
          Targets = new Dictionary<string, TargetInfo>
          {
            { Constants.TypeScriptTarget,
              new TargetInfo
              {
                TypeAliases = new Dictionary<string, string>
                {
                  { "datetime", "MyDate"}
                }
              }
            }
          }
        };
      var ammendment = new TypeScriptAmmendment();
      var exception = (Exception)null;

      // act
      try { ammendment.AmmedSpecification(spec); } catch (Exception ex) { exception = ex; }

      // assert
      Assert.NotNull(exception);
    }
  }
}

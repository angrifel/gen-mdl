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
  using ModelGenerator.Model;
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Xunit;

  public class CSharpAmmendmentTests
  {
    [Fact]
    public void TestAmmendmentFillAllNativeTypes()
    {
      // arrange
      var spec = new Spec { Targets = new Dictionary<string, TargetInfo> { { Constants.CSharpTarget, new TargetInfo() } } };
      var ammendment = new CSharpAmmendment();

      // act
      ammendment.AmmedSpecification(spec);

      // assert
      var nativeTypes = spec.Targets[Constants.CSharpTarget].NativeTypes;
      
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

    [Fact]
    public void TestAmmendSpecificationMapsAllAliasesOnEmptySpec()
    {
      // arrange
      var spec = new Spec { Targets = new Dictionary<string, TargetInfo> { { Constants.CSharpTarget, new TargetInfo() } } };
      var ammendment = new CSharpAmmendment();

      // act
      ammendment.AmmedSpecification(spec);

      // assert
      var typeAliases = spec.Targets[Constants.CSharpTarget].TypeAliases;
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
      Assert.True(typeAliases.Count == 0, "Additional TypeAliases present than those that were tested. Adjust your unit test.");
    }

    [Fact]
    public void TestAmmendSpecificationMapsAllAliasesToExpectedNativeTypesOnEmptySpec()
    {
      // arrange
      var spec = new Spec { Targets = new Dictionary<string, TargetInfo> { { Constants.CSharpTarget, new TargetInfo() } } };
      var ammendment = new CSharpAmmendment();

      // act
      ammendment.AmmedSpecification(spec);

      // assert
      var typeAliases = spec.Targets[Constants.CSharpTarget].TypeAliases.ToList();
      Assert.True(typeAliases.Remove(new KeyValuePair<string, string>("bool", "bool")));
      Assert.True(typeAliases.Remove(new KeyValuePair<string, string>("char", "char")));
      Assert.True(typeAliases.Remove(new KeyValuePair<string, string>("byte", "byte")));
      Assert.True(typeAliases.Remove(new KeyValuePair<string, string>("short", "short")));
      Assert.True(typeAliases.Remove(new KeyValuePair<string, string>("int", "int")));
      Assert.True(typeAliases.Remove(new KeyValuePair<string, string>("long", "long")));
      Assert.True(typeAliases.Remove(new KeyValuePair<string, string>("float", "float")));
      Assert.True(typeAliases.Remove(new KeyValuePair<string, string>("double", "double")));
      Assert.True(typeAliases.Remove(new KeyValuePair<string, string>("decimal", "decimal")));
      Assert.True(typeAliases.Remove(new KeyValuePair<string, string>("string", "string")));
      Assert.True(typeAliases.Remove(new KeyValuePair<string, string>("object", "object")));
      Assert.True(typeAliases.Remove(new KeyValuePair<string, string>("time", "TimeSpan")));
      Assert.True(typeAliases.Remove(new KeyValuePair<string, string>("date", "DateTime")));
      Assert.True(typeAliases.Remove(new KeyValuePair<string, string>("datetime", "DateTimeOffset")));
      Assert.True(typeAliases.Remove(new KeyValuePair<string, string>("guid", "Guid")));
      Assert.True(typeAliases.Count == 0, "Additional TypeAliases present than those that were tested. Adjust your unit test.");
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
            { Constants.CSharpTarget,
              new TargetInfo
              {
                TypeAliases = new Dictionary<string, string>
                {
                  { "datetime", "DateTime"}
                }
              }
            }
          }
        };

      var ammendment = new CSharpAmmendment();
      var exception = (Exception)null;

      // act
      try { ammendment.AmmedSpecification(spec); } catch (Exception ex) { exception = ex; }

      // assert
      Assert.NotNull(exception);
    }
  }
}

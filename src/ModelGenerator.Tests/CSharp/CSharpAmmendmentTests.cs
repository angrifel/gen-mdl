﻿namespace ModelGenerator.Tests.CSharp
{
  using ModelGenerator.CSharp;
  using ModelGenerator.Model;
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;
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
      Assert.True(nativeTypes.Remove("System.TimeSpan"));
      Assert.True(nativeTypes.Remove("System.DateTime"));
      Assert.True(nativeTypes.Remove("System.DateTimeOffset"));
      Assert.True(nativeTypes.Remove("System.Guid"));
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
      Assert.True(typeAliases.Remove(new KeyValuePair<string, string>("time", "System.TimeSpan")));
      Assert.True(typeAliases.Remove(new KeyValuePair<string, string>("date", "System.DateTime")));
      Assert.True(typeAliases.Remove(new KeyValuePair<string, string>("datetime", "System.DateTimeOffset")));
      Assert.True(typeAliases.Remove(new KeyValuePair<string, string>("guid", "System.Guid")));
      Assert.True(typeAliases.Count == 0, "Additional TypeAliases present than those that were tested. Adjust your unit test.");
    }

    [Fact]
    public void TestAmmendSpecificationDoesNotReplacePreexistingTypeAliases()
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
                  { "datetime", "System.DateTime"}
                }
              }
            }
          }
        };
      var ammendment = new CSharpAmmendment();

      // act
      ammendment.AmmedSpecification(spec);

      // assert
      Assert.True(spec.Targets[Constants.CSharpTarget].TypeAliases.Contains(new KeyValuePair<string, string>("datetime", "System.DateTime")));
    }
  }
}

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

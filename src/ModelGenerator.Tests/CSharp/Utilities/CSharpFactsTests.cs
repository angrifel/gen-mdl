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

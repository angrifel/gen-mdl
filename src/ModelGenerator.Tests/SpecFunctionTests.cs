namespace ModelGenerator.Tests
{
  using System.Collections.Generic;
  using Xunit;

  public class SpecFunctionTests
  {
    public static IEnumerable<object[]> ToPascalCaseTestSuite
    {
      get
      {
        return new object[][]
        {
          new object[] { "id",  "Id" },
          new object[] { "db_connection",  "DbConnection" },
          new object[] { "another_sql_component", "AnotherSqlComponent" }
        };
      }
    }

    public static IEnumerable<object[]> ToCamelCaseTestSuite
    {
      get
      {
        return new object[][]
        {
          new object[] { "id",  "id" },
          new object[] { "db_connection",  "dbConnection" },
          new object[] { "another_sql_component", "anotherSqlComponent" }
        };
      }
    }

    public static IEnumerable<object[]> TestToHyphenatedCaseTestSuite
    {
      get
      {
        return new object[][]
        {
          new object[] { "id",  "id" },
          new object[] { "db_connection",  "db-connection" },
          new object[] { "another_sql_component", "another-sql-component" }
        };
      }
    }

    public static IEnumerable<object[]> TestIsSnakeCaseLowerCaseTestSuite
    {
      get
      {
        return new object[][]
        {
          new object[] { "id",  true },
          new object[] { "db_connection",  true },
          new object[] { "another_sql_component", true },
          new object[] { "iD",  false },
          new object[] { "db-connection",  false },
          new object[] { "another_Sql_component", false }
        };
      }
    }

    [Theory]
    [MemberData(nameof(ToPascalCaseTestSuite))]
    public void TestToPascalCase(string input, string output)
    {
      // arrange
      // act
      var actualOutput = SpecFunctions.ToPascalCase(input);
      
      // assert
      Assert.Equal(output, actualOutput);
    }

    [Theory]
    [MemberData(nameof(ToCamelCaseTestSuite))]
    public void TestToCamelCase(string input, string output)
    {
      // arrange
      // act
      var actualOutput = SpecFunctions.ToCamelCase(input);

      // assert
      Assert.Equal(output, actualOutput);
    }

    [Theory]
    [MemberData(nameof(TestToHyphenatedCaseTestSuite))]
    public void TestToHyphenatedCase(string input, string output)
    {
      // arrange
      // act
      var actualOutput = SpecFunctions.ToHyphenatedCase(input);

      // assert
      Assert.Equal(output, actualOutput);
    }

    [Theory]
    [MemberData(nameof(TestIsSnakeCaseLowerCaseTestSuite))]
    public void TestIsSnakeCaseLowerCase(string input, bool output)
    {
      // arrange
      // act
      var actualOutput = SpecFunctions.IsSnakeCaseLowerCase(input);

      // assert
      Assert.Equal(output, actualOutput);
    }
  }
}

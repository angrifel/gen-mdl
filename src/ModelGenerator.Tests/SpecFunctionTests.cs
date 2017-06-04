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

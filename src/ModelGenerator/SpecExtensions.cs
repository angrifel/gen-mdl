using ModelGenerator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelGenerator
{
  public static class SpecExtensions
  {
    public static string GetMemberType(this Alternative<string, EntityMemberInfo> valueOrEntityMemberInfoAlternative) =>
      valueOrEntityMemberInfoAlternative.Value as string ?? ((EntityMemberInfo)valueOrEntityMemberInfoAlternative.Value).Type;

    public static bool GetIsNullable(this Alternative<string, EntityMemberInfo> valueOrEntityMemberInfoAlternative) =>
      valueOrEntityMemberInfoAlternative.Value is string ? false : ((EntityMemberInfo)valueOrEntityMemberInfoAlternative.Value).IsNullable;
  }
}

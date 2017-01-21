namespace ModelGenerator.Model
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;

  public class Spec
  {
    public Spec()
    {
      this.Entities = new Dictionary<string, OrderedDictionary<string, Alternative<string, EntityMemberInfo>>>();
      this.Enums = new Dictionary<string, List<Alternative<string, QualifiedEnumMember>>>();
    }

    public Dictionary<string, TargetInfo> Targets { get; set; }

    public Dictionary<string, OrderedDictionary<string, Alternative<string, EntityMemberInfo>>> Entities { get; set; }

    public Dictionary<string, List<Alternative<string, QualifiedEnumMember>>> Enums { get; set; }
  }
}

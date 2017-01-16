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
      this.Entities = new OrderedDictionary<string, OrderedDictionary<string, string>>();
      this.Enums = new OrderedDictionary<string, List<Alternative<string, QualifiedEnumMember>>>();
    }

    public Dictionary<string, TargetInfo> Targets { get; set; }

    public OrderedDictionary<string, OrderedDictionary<string, string>> Entities { get; set; }

    public OrderedDictionary<string, List<Alternative<string, QualifiedEnumMember>>> Enums { get; set; }
  }
}

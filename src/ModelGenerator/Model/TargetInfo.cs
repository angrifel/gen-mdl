namespace ModelGenerator.Model
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;
  using YamlDotNet.Serialization;

  public class TargetInfo
  {
    public TargetInfo()
    {
      this.TypeAliases = new Dictionary<string, string>();
    }

    public string Path { get; set; }

    public IDictionary<string, string> TypeAliases { get; set; }

    [YamlIgnore]
    public ISet<string> NativeTypes { get; set; }

    public string Namespace { get; set; }

  }
}

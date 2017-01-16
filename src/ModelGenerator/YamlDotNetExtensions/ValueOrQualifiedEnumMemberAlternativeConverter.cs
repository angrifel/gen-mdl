namespace ModelGenerator.YamlDotNetExtensions
{
  using Model;
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using YamlDotNet.Core;
  using YamlDotNet.Core.Events;
  using YamlDotNet.Serialization;

  public class ValueOrQualifiedEnumMemberAlternativeConverter : IYamlTypeConverter
  {
    public bool Accepts(Type type)
    {
      return type == typeof(Alternative<string, QualifiedEnumMember>);
    }

    public object ReadYaml(IParser parser, Type type)
    {
      if (parser.Accept<Scalar>())
      {
        var scalar = parser.Expect<Scalar>();
        return new Alternative<string, QualifiedEnumMember>(scalar.Value, null);
      }
      else
      {
        parser.Expect<MappingStart>();
        var nameScalar = parser.Expect<Scalar>();
        var valueScalar = parser.Expect<Scalar>();
        parser.Expect<MappingEnd>();
        var qem = new QualifiedEnumMember { Name = nameScalar.Value, Value = long.Parse(valueScalar.Value) };
        return new Alternative<string, QualifiedEnumMember>(null, qem);
      }
    }

    public void WriteYaml(IEmitter emitter, object value, Type type)
    {
      throw new NotSupportedException();
    }
  }
}

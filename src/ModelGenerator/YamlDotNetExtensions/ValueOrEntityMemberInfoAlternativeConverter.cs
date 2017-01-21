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

  public class ValueOrEntityMemberInfoAlternativeConverter : IYamlTypeConverter
  {
    private readonly Func<Deserializer> _deserializerFactory;

    public ValueOrEntityMemberInfoAlternativeConverter(Func<Deserializer> deserializerFactory)
    {
      if (deserializerFactory == null) throw new ArgumentNullException(nameof(deserializerFactory));
      _deserializerFactory = deserializerFactory;
    }

    public bool Accepts(Type type)
    {
      return type == typeof(Alternative<string, EntityMemberInfo>);
    }

    public object ReadYaml(IParser parser, Type type)
    {
      if (parser.Accept<Scalar>())
      {
        var scalar = parser.Expect<Scalar>();
        return new Alternative<string, EntityMemberInfo>(scalar.Value, null);
      }
      else
      {
        var emi = _deserializerFactory().Deserialize<EntityMemberInfo>(parser);
        return new Alternative<string, EntityMemberInfo>(null, emi);
      }
    }

    public void WriteYaml(IEmitter emitter, object value, Type type)
    {
      throw new NotSupportedException();
    }
  }
}

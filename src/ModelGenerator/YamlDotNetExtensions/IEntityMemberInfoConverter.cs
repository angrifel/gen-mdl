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

namespace ModelGenerator.YamlDotNetExtensions
{
  using Model;
  using System;
  using YamlDotNet.Core;
  using YamlDotNet.Core.Events;
  using YamlDotNet.Serialization;

  public class IEntityMemberInfoConverter : IYamlTypeConverter
  {
    private readonly Func<Deserializer> _deserializerFactory;

    public IEntityMemberInfoConverter(Func<Deserializer> deserializerFactory)
    {
      _deserializerFactory = deserializerFactory ?? throw new ArgumentNullException(nameof(deserializerFactory));
    }

    public bool Accepts(Type type)
    {
      return type == typeof(IEntityMemberInfo);
    }

    public object ReadYaml(IParser parser, Type type)
    {
      if (parser.Accept<Scalar>())
      {
        var scalar = parser.Expect<Scalar>();
        return new EntityMemberInfo { Type = scalar.Value };
      }
      else
      {
        return _deserializerFactory().Deserialize<EntityMemberInfo>(parser);
      }
    }

    public void WriteYaml(IEmitter emitter, object value, Type type)
    {
      throw new NotSupportedException();
    }
  }
}

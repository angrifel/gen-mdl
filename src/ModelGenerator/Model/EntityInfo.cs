namespace ModelGenerator.Model
{
  public class EntityInfo
  {
    public OrderedDictionary<string, IEntityMemberInfo> Members { get; set; } = new OrderedDictionary<string, IEntityMemberInfo>();
  }
}
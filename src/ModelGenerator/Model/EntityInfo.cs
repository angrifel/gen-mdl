namespace ModelGenerator.Model
{
  public class EntityInfo
  {
    public EntityInfo()
    {
      this.Members = new OrderedDictionary<string, IEntityMemberInfo>();
    }

    public OrderedDictionary<string, IEntityMemberInfo> Members { get; set; }
  }
}
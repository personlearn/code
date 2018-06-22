using Spider.Core.Selector;
using Spider.Extension.Model.Attribute;

namespace Spider.Extension.Model
{
    public class BaseEntity
    {
        [Field(DataType = DataType.Long, IsPrimary = true, Expression = "Id", Type = SelectorType.Enviroment)]
        public long Id { get; set; }
    }
}

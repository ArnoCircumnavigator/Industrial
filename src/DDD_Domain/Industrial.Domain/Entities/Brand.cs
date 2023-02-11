using Industrial.Infra.DDDesign.CoreFramework;

namespace Industrial.Domain.Entities
{
    /// <summary>
    /// 品牌/品规
    /// </summary>
    public class Brand : AggregateRoot<UniqueId>
    {
        public Brand(string itemNum, string name, string unit)
            : this(new UniqueId(), itemNum, name, unit)
        {

        }

        public Brand(UniqueId id, string itemNum, string name, string unit)
            : base(id)
        {
            this.ItemNum = itemNum;
            this.Name = name;
            this.Unit = unit;
        }

        /// <summary>
        /// 编号
        /// </summary>
        public string ItemNum { get; private set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 统计单位
        /// </summary>
        public string Unit { get; private set; }
    }
}

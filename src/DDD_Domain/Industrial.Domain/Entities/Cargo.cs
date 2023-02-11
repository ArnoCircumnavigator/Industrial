using Industrial.Infra.DDDesign.CoreFramework;

namespace Industrial.Domain.Entities
{
    public class Cargo : Entity<UniqueId>
    {
        public Cargo(UniqueId id)
            : base(id)
        {

        }

        /// <summary>
        /// 货物的品规信息
        /// </summary>
        public Brand Brand { get; private set; }
        /// <summary>
        /// 位置
        /// </summary>
        public UniqueId LocationId { get; private set; }
    }
}

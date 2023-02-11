using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Industrial.Infra.DDDesign.CoreFramework
{
    public abstract class Entity<TEntityId> : IEntity<TEntityId>
    {
        protected Entity(TEntityId id)
        {
            Id = id;
        }

        public TEntityId Id { get; private set; }
    }
}

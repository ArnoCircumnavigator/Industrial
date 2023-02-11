namespace Industrial.Infra.DDDesign.CoreFramework
{
    public class AggregateRoot<TAggregateRootId> :
        Entity<TAggregateRootId>,
        IAggregateRoot<TAggregateRootId>
    {
        public AggregateRoot(TAggregateRootId id)
            : base(id)
        {

        }

        public TRole ActAs<TRole>() where TRole : class
        {
            object? role = null;
            Type? roleTypeDefinition = typeof(TRole);
            Type? aggregateRootType = this.GetType();

            if (!roleTypeDefinition.IsInterface)
            {
                throw new NotSupportedException("Role must be an interface.");
            }

            if (roleTypeDefinition.IsAssignableFrom(aggregateRootType))
            {
                return this as TRole;
            }

            var roleType = DomainInitializer.Current.GetActorRoleType(this.GetType(), roleTypeDefinition);
            if (roleType == null)
            {
                throw new NotSupportedException($"AggregateRoot of type {aggregateRootType.FullName} cannot act role {roleTypeDefinition.FullName}");
            }

            System.Reflection.ConstructorInfo? constructor = roleType.GetConstructors().Where(c => c.GetParameters()[0].ParameterType == aggregateRootType).SingleOrDefault();
            if (constructor == null)
            {
                throw new NotSupportedException($"No available constructor found on type {roleType.FullName}");
            }

            try
            {
                role = constructor.Invoke(new object[] { this }) as TRole;
            }
            catch (Exception ex)
            {
                string error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                throw new Exception(error);
            }

            return role as TRole;
        }
    }
}

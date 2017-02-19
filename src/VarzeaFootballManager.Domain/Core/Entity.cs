namespace VarzeaFootballManager.Domain.Core
{
    public abstract class Entity : IEntity
    {
        public string Id { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public System.DateTime? ModifiedAt { get; set; }

        public System.Collections.Generic.IDictionary<string, object> CatchAll = new System.Collections.Generic.Dictionary<string, object>();

        protected Entity()
        {
            CreatedAt = System.DateTime.Now;
        }

        public static bool operator ==(Entity lhs, Entity rhs)
        {
            if (ReferenceEquals(lhs, null) && ReferenceEquals(rhs, null))
                return true;

            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
                return false;

            return lhs.Equals(rhs);
        }

        public static bool operator !=(Entity lhs, Entity rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object obj)
        {
            var other = obj as Entity;

            if (ReferenceEquals(other, null))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (GetRealType() != other.GetRealType())
                return false;

            if (string.IsNullOrWhiteSpace(Id) || string.IsNullOrWhiteSpace(other.Id))
                return false;

            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return (GetRealType() + Id).GetHashCode();
        }

        private System.Type GetRealType()
        {
            return this.GetType();
            //return NHibernateProxyHelper.GetClassWithoutInitializingProxy(this);
        }
    }
}

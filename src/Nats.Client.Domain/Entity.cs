using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nats.Client.Domain
{
    [Serializable]
    public abstract class Entity
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity), Key()]
        public int Id { get; protected set; }
        public Guid Guid { get; set; } = Guid.NewGuid();
        public bool IsDeleted { get; protected set; } = false;

        public void Delete()
        {
            IsDeleted = true;
        }
        
        public override bool Equals(object obj)
        {
            if (!(obj is Entity other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            if (GetType() != other.GetType())
                return false;
            if (Id == default || other.Id == default)
                return false;
            return Id == other.Id;
        }

        public static bool operator ==(Entity a, Entity b)
        {
            if (a is null && b is null)
                return true;
            if (a is null || b is null)
                return false;
            return a.Equals(b);
        }

        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (GetType().ToString() + Guid).GetHashCode();
        }
    }
}
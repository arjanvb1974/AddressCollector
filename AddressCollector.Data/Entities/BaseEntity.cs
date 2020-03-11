using System;
using AddressCollector.Data.Auth;

namespace AddressCollector.Data.Entities
{
    public class BaseEntity : IAuditableEntity
    {
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
    }
}

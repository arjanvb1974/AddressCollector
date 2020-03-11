using System;

namespace AddressCollector.Data.Entities
{
    interface IAuditableEntity
    {
        DateTime CreateDate { get; set; }
        DateTime LastUpdateDate { get; set; }
    }
}

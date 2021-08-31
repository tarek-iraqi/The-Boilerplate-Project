using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public interface IBaseEntity
    {
        string CreatedBy { get; set; }
        DateTime CreatedOn { get; set; }
        string LastModifiedBy { get; set; }
        DateTime? LastModifiedOn { get; set; }
        bool IsDeleted { get; set; }
    }
}

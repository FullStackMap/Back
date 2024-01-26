using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map.Domain.Entities;

public class MapUser : IdentityUser<Guid>
{
    public virtual IList<Trip>? Trips{ get; set; }
}

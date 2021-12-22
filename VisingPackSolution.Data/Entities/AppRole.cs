using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

#nullable disable

namespace VisingPackSolution.Data
{
    public partial class AppRole: IdentityRole<Guid>
    {
        public string Description { get; set; }
    }
}

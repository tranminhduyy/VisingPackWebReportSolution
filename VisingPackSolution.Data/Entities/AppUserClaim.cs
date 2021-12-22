using System;
using System.Collections.Generic;

#nullable disable

namespace VisingPackSolution.Data
{
    public partial class AppUserClaim
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}

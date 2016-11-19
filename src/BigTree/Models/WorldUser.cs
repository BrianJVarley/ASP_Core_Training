using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;

namespace BigTree.Models
{
    public class WorldUser : IdentityUser
    {
        public DateTime FirstTrip { get; set; }
    }
}
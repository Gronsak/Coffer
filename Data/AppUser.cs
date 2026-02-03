using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coffer.Models;
using Microsoft.AspNetCore.Identity;

namespace Coffer.Data;

// Add profile data for application users by adding properties to the AppUser class
public class AppUser : IdentityUser
{
    public List<Ledger> LedgersOwned { get; } = [];
    public List<Ledger> MemberOf { get; } = [];
}


using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class Role : IdentityRole<int>
{
   public Role() : base()
   {
   }
   
   public Role(string roleName) : base(roleName)
   {
   }
}
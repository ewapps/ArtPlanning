using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ArtPlanning.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Column("last_connection_date")]
        public DateTime? LastConnectionDate { get; set; }
        [Column("added_date")]
        public DateTime? AddedDate { get; set; }
        [Column("modification_date")]
        public DateTime? ModificationDate { get; set; }
        [Column("added_user")]
        public string AddedUser { get; set; }
        [Column("modification_user")]
        public string ModificationUser { get; set; }
        [Column("last_name")]
        public string LastName { get; set; }
        [Column("first_name")]
        public string FirstName { get; set; }
        [Column("active")]
        public bool? Active { get; set; }
        [Column("language_id")]
        public int Language { get; set; }
        [Column("birth_date")]
        public DateTime? BirthDate { get; set; }
        [Column("color")]
        public string Color { get; set; }
        [Column("initials")]
        public string Initials { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {            
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);         
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("ARTSHIPPERSDatabaseAuthorization", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
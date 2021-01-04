using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using InterviewSystem.Models;

namespace InterviewSystem.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UniqueID { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("InterviewConnection", throwIfV1Schema: false)
        {
        }
        public IDbSet<InterviewDetail> InterviewDetail { get; set; }
        public IDbSet<InterviewerDetails> InterviewerDetails { get; set; }
        public IDbSet<CandidateDetails> CandidateDetails { get; set; }
        public IDbSet<InterviewRounds> InterviewRounds { get; set; }
        public IDbSet<Departments> Departments { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        //public IDbSet<Apps> Apps { get; set; }
        //public IDbSet<UserAppMaps> UserAppMaps { get; set; }
        //public IDbSet<MainAppCredentials> MainAppCredentials { get; set; }

        //public IDbSet<Client1Credentials> Client1Credentials { get; set; }
        //public IDbSet<Client1Map> Client1Map { get; set; }
        //public System.Data.Entity.DbSet<InterviewSystem.Models.CandidateResult> CandidateResults { get; set; }

        //public System.Data.Entity.DbSet<InterviewSystem.Models.GiveRating> GiveRatings { get; set; }

    }
}
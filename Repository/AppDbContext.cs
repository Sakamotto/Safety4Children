using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Safety4Children.Entities;
using Safety4Children.Repository.IdentityEntities;

namespace Safety4Children.Repository
{
    public class AppDbContext
        : IdentityDbContext<AppUser, Role, int, IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            builder.Entity<UsuarioPai>()
                .HasData(
                    new UsuarioPai
                    {
                        Id = 1,
                        Nome = "Fulano de Tal",
                        Cpf = "71985694719",
                        Email = "fulano.tal@teste.com",
                        NormalizedEmail = "FULANO.TAL@TESTE.COM",
                        PasswordHash = "AQAAAAEAACcQAAAAEEVjXvqjVsNgg//Kp2nmmIc8cVqwehn9NayYOAl6iqthSU3yClvT5iQDdDc4J5lKHg==",
                        SecurityStamp = "KRV4CMQKAQCZGZYKSMRW3L7NIJ7CTS6C",
                        ConcurrencyStamp = "d7d50895-1e1c-4582-8bd1-6badd9daea7e",
                        LockoutEnabled = true,
                        UserName = "fulano.tal@teste.com",
                        NormalizedUserName = "FULANO.TAL@TESTE.COM",
                    }
                );
        }
    }
}

using CodeWithMe.Core.Models;
using CodeWithMe.Core.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMe.Core.DataExtensions;

/// <summary>
/// Seed des comptes de login mock définis par le contrat EDU Platform v1 (section 8.3).
/// Idempotent — exécutable plusieurs fois sans doublon.
/// Appelé depuis <see cref="StartupExtension.MigrateDb"/>.
/// </summary>
public static class Seeder
{
    public static async Task SeedContractAccountsAsync(IServiceProvider services)
    {
        await using var scope = services.CreateAsyncScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var ctx = scope.ServiceProvider.GetRequiredService<ApiContext>();

        // S'assurer que le tenant par défaut existe (via HasData lors de la migration).
        var hasTenant = await ctx.Tenants.IgnoreQueryFilters()
            .AnyAsync(t => t.Id == Tenant.DefaultTenantId);
        if (!hasTenant)
        {
            ctx.Tenants.Add(new Tenant
            {
                Id = Tenant.DefaultTenantId,
                Code = Tenant.DefaultTenantCode,
                Name = "Default Tenant",
                Type = TenantType.School,
                Status = TenantStatus.Active,
                Locale = "fr-FR",
                Timezone = "Europe/Paris"
            });
            await ctx.SaveChangesAsync();
        }

        // Section 8.3 — comptes mock, mot de passe universel "password".
        var seeds = new (string Email, Role Role)[]
        {
            ("super_admin@test.com", Role.SuperAdmin),
            ("directeur@test.com", Role.Directeur),
            ("dp@test.com", Role.DirecteurPedagogique),
            ("enseignant@test.com", Role.Enseignant),
            ("secretaire@test.com", Role.Secretaire),
            ("comptable@test.com", Role.Comptable),
            ("apprenant@test.com", Role.Apprenant),
            ("parent@test.com", Role.Parent)
        };

        foreach (var (email, role) in seeds)
        {
            if (await userManager.FindByEmailAsync(email) is not null) continue;

            var user = new User
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                Role = role,
                TenantId = role == Role.SuperAdmin ? null : Tenant.DefaultTenantId,
                FirstName = role.ToString(),
                LastName = "Demo"
            };
            var result = await userManager.CreateAsync(user, "password");
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    "Seed user " + email + " failed: " +
                    string.Join("; ", result.Errors.Select(e => e.Description)));
            }
        }
    }
}

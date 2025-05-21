using Microsoft.Extensions.DependencyInjection;

namespace IIdentifii.Blog.Repository
{
    public static class SeedHelpers
    {
        public static async Task SeedRolesAndUsersAsync(
            IServiceProvider serviceProvider)
        {
            using IServiceScope scope = serviceProvider.CreateScope();

            UserManager<IIdentifiiUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<IIdentifiiUser>>();

            await AddRoles(scope);

            await AddModerator(userManager, moderatorEmail: "moderator@example.com");

            await AddUser(userManager, userEmail: "user@example.com");
        }

        private static async Task AddModerator(
            UserManager<IIdentifiiUser> userManager, 
            string moderatorEmail)
        {
            IIdentifiiUser? moderatorUser = await userManager.FindByEmailAsync(moderatorEmail);

            if (moderatorUser == null)
            {
                moderatorUser = new IIdentifiiUser
                {
                    UserName = "moderator",
                    Email = moderatorEmail,
                    DisplayName = "Content Moderator",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(moderatorUser, "Moderator123!");
            }

            if (!await userManager.IsInRoleAsync(moderatorUser, RoleConstants.Moderator))
            {
                await userManager.AddToRoleAsync(moderatorUser, RoleConstants.Moderator);
            }
        }

        private static async Task AddUser(
            UserManager<IIdentifiiUser> userManager,
            string userEmail)
        {
            IIdentifiiUser? moderatorUser = await userManager.FindByEmailAsync(userEmail);

            if (moderatorUser == null)
            {
                moderatorUser = new IIdentifiiUser
                {
                    UserName = "user",
                    Email = userEmail,
                    DisplayName = "Regular User",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(moderatorUser, "User123!");
            }

            if (!await userManager.IsInRoleAsync(moderatorUser, RoleConstants.User))
            {
                await userManager.AddToRoleAsync(moderatorUser, RoleConstants.User);
            }
        }

        private static async Task AddRoles(
            IServiceScope scope)
        {
            RoleManager<IdentityRole<Guid>> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            foreach (string role in RoleConstants.AllRoles)
            {
                if (await roleManager.RoleExistsAsync(role))
                {
                    continue;
                }

                await roleManager.CreateAsync(new IdentityRole<Guid>(role));
            }
        }
    }
}

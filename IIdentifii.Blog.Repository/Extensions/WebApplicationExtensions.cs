using Microsoft.Extensions.Hosting;

namespace Microsoft.AspNetCore.Builder
{
    public static class WebApplicationExtensions
    {
        public static async Task RunMigrationsAsync(
            this WebApplication app, 
            CancellationToken token = default)
        {
            if (app.Environment.IsEnvironment("Testing"))
            {
                return;
            }

            await ApplyMigrationsAsync<AppDbContext>(app, token);
        }

        public static async Task SeedRolesAndUsersAsync(
            this WebApplication app,
            CancellationToken token = default)
        {
            using IServiceScope scope = app.Services.CreateScope();

            await SeedRolesAndUsersAsync(scope.ServiceProvider);
        }

        public static async Task SeedRolesAndUsersAsync(
            IServiceProvider services)
        {
            UserManager<IIdentifiiUser> userManager = services.GetRequiredService<UserManager<IIdentifiiUser>>();

            await AddRoles(services);

            await AddModerator(userManager, SeedDataConstants.ModeratorId, moderatorEmail: SeedDataConstants.ModeratorEmail, SeedDataConstants.ModeratorPassword);

            await AddUser(userManager, SeedDataConstants.UserId, userEmail: SeedDataConstants.UserEmail, SeedDataConstants.UserPassword);
        }

        public static async Task SeedBlogsAsync(
            this WebApplication app,
            CancellationToken token = default)
        {
            using (IServiceScope scope = app.Services.CreateScope())
            {
                IServiceProvider provider = scope.ServiceProvider;

                AppDbContext context = provider.GetRequiredService<AppDbContext>();

                if(context.BlogPosts.Any())
                {
                    return;
                }

                CommentModel commentModel = CommentModel.CreateSeedComment(SeedDataConstants.CommentId, SeedDataConstants.UserId, SeedDataConstants.BlogPostId);

                List<ReactionModel> reactionModels = new List<ReactionModel>() { ReactionModel.CreateSeedReaction(SeedDataConstants.ReactionId, SeedDataConstants.BlogPostId, SeedDataConstants.UserId) };
                List<ReactionAggregateModel> reactionAggregateModels = new List<ReactionAggregateModel>();
                List<TagModel> tagModels = new List<TagModel>();

                foreach (ReactionType type in Enum.GetValues<ReactionType>())
                {
                    reactionAggregateModels.Add(ReactionAggregateModel.CreateSeedReaction(Guid.CreateVersion7(), SeedDataConstants.BlogPostId, type, (type == ReactionType.Like)? 1 : 0));
                }

                foreach (TagType type in Enum.GetValues<TagType>())
                {
                    tagModels.Add(TagModel.CreateSeedTag(Guid.CreateVersion7(), SeedDataConstants.BlogPostId, SeedDataConstants.ModeratorId, type));
                }

                BlogPostModel blogPostModel = BlogPostModel.CreateSeedPost(
                    SeedDataConstants.BlogPostId, 
                    SeedDataConstants.UserId, 
                    title: "Sample blog post",
                    content: "Sample blog post content",
                    reactionModels: reactionModels, 
                    reactionAggregateModels: reactionAggregateModels,
                    commentModels: new List<CommentModel>() { commentModel }, 
                    tagModels: tagModels);

                await context.BlogPosts.AddAsync(blogPostModel, token);

                await context.SaveChangesAsync(token);
            }
        }

        private static async Task ApplyMigrationsAsync<T>(
            this WebApplication app, 
            CancellationToken token = default)
            where T : DbContext
        {
            using (IServiceScope scope = app.Services.CreateScope())
            {
                IServiceProvider provider = scope.ServiceProvider;

                T context = provider.GetRequiredService<T>();

                await context.Database.EnsureCreatedAsync(token);

                await context.Database.MigrateAsync(token);
            }
        }

        private static async Task AddModerator(
            UserManager<IIdentifiiUser> userManager,
            Guid moderatorId,
            string moderatorEmail,
            string password)
        {
            IIdentifiiUser? moderatorUser = await userManager.FindByEmailAsync(moderatorEmail);

            if (moderatorUser == null)
            {
                moderatorUser = new IIdentifiiUser
                {
                    Id = moderatorId,
                    UserName = moderatorEmail,
                    Email = moderatorEmail,
                    DisplayName = moderatorEmail,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(moderatorUser, password);
            }

            if (!await userManager.IsInRoleAsync(moderatorUser, RoleConstants.Moderator))
            {
                await userManager.AddToRoleAsync(moderatorUser, RoleConstants.Moderator);
            }
        }

        private static async Task AddUser(
            UserManager<IIdentifiiUser> userManager,
            Guid userId,
            string userEmail,
            string password)
        {
            IIdentifiiUser? moderatorUser = await userManager.FindByEmailAsync(userEmail);

            if (moderatorUser == null)
            {
                moderatorUser = new IIdentifiiUser
                {
                    Id = userId,
                    UserName = userEmail,
                    Email = userEmail,
                    DisplayName = userEmail,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(moderatorUser, password);
            }

            if (!await userManager.IsInRoleAsync(moderatorUser, RoleConstants.User))
            {
                await userManager.AddToRoleAsync(moderatorUser, RoleConstants.User);
            }
        }

        private static async Task AddRoles(
            IServiceProvider services)
        {
            RoleManager<IdentityRole<Guid>> roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

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

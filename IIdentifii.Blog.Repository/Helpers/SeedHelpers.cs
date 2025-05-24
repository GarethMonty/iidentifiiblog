namespace IIdentifii.Blog.Repository
{
    public static class SeedHelpers
    {
        internal static async Task GenerateTestDataEnvironment_Normal(
            IServiceProvider services,
            int userCount = 10,
            int moderatorCount = 2,
            int postCount = 100,
            int reactionsCount = 1000,
            int commentCount = 1000,
            int tagCount = 5)
        {
            AppDbContext db = services.GetRequiredService<AppDbContext>();
            UserManager<IIdentifiiUser> userManager = services.GetRequiredService<UserManager<IIdentifiiUser>>();
            RoleManager<IdentityRole<Guid>> roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            List<IIdentifiiUser> users = IIDentifiiUserData.GetUsers(userCount);
            List<IIdentifiiUser> moderators = IIDentifiiUserData.GetUsers(moderatorCount);

            List<Guid> userIdList = users.Select(a => a.Id).Append(SeedDataConstants.UserId).ToList();
            List<Guid> moderatorIdList = users.Select(a => a.Id).Append(SeedDataConstants.ModeratorId).ToList();

            List<BlogPostModel> posts = BlogPostData.GetPosts(postCount, userIdList);
            posts.Add(BlogPostModel.CreateSeedPost(SeedDataConstants.BlogPostId, SeedDataConstants.UserId, "Sample Blog Post", "This is a sample blog post content."));

            List<Guid> postIdList = posts.Select(a => a.Id).ToList();

            List<ReactionModel> reactions = ReactionData.GetReactions(reactionsCount, userIdList, postIdList);
            reactions.AddRange(ReactionModel.CreateSeedReaction(SeedDataConstants.ReactionId, SeedDataConstants.BlogPostId, SeedDataConstants.UserId));

            List<CommentModel> comments = CommentData.GetComments(commentCount, userIdList, postIdList);
            comments.Add(CommentModel.CreateSeedComment(SeedDataConstants.CommentId, SeedDataConstants.UserId, SeedDataConstants.BlogPostId, "This is a sample comment content."));

            List<TagModel> tags = TagData.GetTags(tagCount, moderatorIdList, postIdList);
            tags.Add(TagModel.CreateSeedTag(SeedDataConstants.TagId, SeedDataConstants.BlogPostId, SeedDataConstants.ModeratorId, TagType.FalseInformation));

            //Add constant roles
            await AddRoles(roleManager);

            //Add constant users
            await AddUser(userManager, SeedDataConstants.ModeratorId, userEmail: SeedDataConstants.ModeratorEmail, SeedDataConstants.ModeratorPassword, RoleConstants.Moderator);
            await AddUser(userManager, SeedDataConstants.UserId, userEmail: SeedDataConstants.UserEmail, SeedDataConstants.UserPassword, RoleConstants.User);

            foreach (IIdentifiiUser user in users)
            {
                await AddUser(userManager, user.Id, user.Email!, SeedDataConstants.UserPassword, RoleConstants.User);
            }

            foreach (IIdentifiiUser moderator in moderators)
            {
                await AddUser(userManager, moderator.Id, moderator.Email!, SeedDataConstants.ModeratorPassword, RoleConstants.Moderator);
            }

            foreach (BlogPostModel blogPost in posts)
            {
                foreach (ReactionAggregateModel agg in blogPost.ReactionAggregates)
                {
                    agg.BlogPostId = blogPost.Id;
                    agg.Count = reactions.Count(r => r.BlogPostId == blogPost.Id && r.Type == agg.Type);
                }
            }

            db.BlogPosts.AddRange(posts);
            db.Reactions.AddRange(reactions);
            db.Comments.AddRange(comments);
            db.Tags.AddRange(tags);

            await db.SaveChangesAsync();
        }

        private static async Task AddUser(
            UserManager<IIdentifiiUser> userManager,
            Guid userId,
            string userEmail,
            string password,
            string role)
        {
            IIdentifiiUser? user = await userManager.FindByEmailAsync(userEmail);

            if (user == null)
            {
                user = new IIdentifiiUser
                {
                    Id = userId,
                    UserName = userEmail,
                    Email = userEmail,
                    DisplayName = userEmail,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(user, password);
            }

            if (!await userManager.IsInRoleAsync(user, role))
            {
                await userManager.AddToRoleAsync(user, role);
            }
        }

        private static async Task AddRoles(
            RoleManager<IdentityRole<Guid>> roleManager)
        {
            foreach (string role in RoleConstants.AllRoles)
            {
                if (await roleManager.RoleExistsAsync(role))
                {
                    continue;
                }

                await roleManager.CreateAsync(new IdentityRole<Guid>(role));
            }
        }

        internal static class IIDentifiiUserData
        {
            internal static List<IIdentifiiUser> GetUsers(int count)
            {
                Faker faker = new Faker();
                Faker<IIdentifiiUser> userFaker = new Faker<IIdentifiiUser>()
                    .RuleFor(u => u.Id, f => Guid.NewGuid())
                    .RuleFor(u => u.DisplayName, f => f.Person.UserName)
                    .RuleFor(u => u.UserName, f => f.Internet.Email())
                    .RuleFor(u => u.Email, (f, u) => f.Internet.Email());

                List<IIdentifiiUser> users = userFaker.Generate(count);

                return users;
            }
        }
        internal static class BlogPostData
        {
            internal static List<BlogPostModel> GetPosts(int count, List<Guid> authorIds)
            {
                Faker faker = new Faker();

                Faker<BlogPostModel> postFaker = new Faker<BlogPostModel>()
                    .RuleFor(p => p.Id, f => Guid.NewGuid())
                    .RuleFor(p => p.Title, f => f.Lorem.Sentence(5))
                    .RuleFor(p => p.Content, f => f.Lorem.Paragraphs(2))
                    .RuleFor(p => p.PostedAt, f => f.Date.Past(1))
                    .RuleFor(p => p.AuthorId, f => f.PickRandom(authorIds))
                    .RuleFor(p => p.Reactions, f => new List<ReactionModel>())
                    .RuleFor(p => p.Comments, f => new List<CommentModel>())
                    .RuleFor(p => p.Tags, f => new List<TagModel>())
                    .RuleFor(p => p.ReactionAggregates, f => Enum.GetValues<ReactionType>().Select(rt => new ReactionAggregateModel
                    {
                        Id = Guid.NewGuid(),
                        BlogPostId = Guid.Empty,
                        Type = rt,
                        Count = 0
                    }).ToList());

                List<BlogPostModel> posts = postFaker.Generate(count);

                return posts;
            }

        }

        internal static class ReactionData
        {
            internal static List<ReactionModel> GetReactions(int count, List<Guid> userIds, List<Guid> postIds)
            {
                Faker faker = new Faker();
                Faker<ReactionModel> reactionFaker = new Faker<ReactionModel>()
                    .RuleFor(r => r.Id, f => Guid.NewGuid())
                    .RuleFor(r => r.BlogPostId, f => f.PickRandom(postIds))
                    .RuleFor(r => r.UserId, f => f.PickRandom(userIds))
                    .RuleFor(r => r.Type, f => f.PickRandom(Enum.GetValues<ReactionType>()))
                    .RuleFor(r => r.ReactedAt, f => f.Date.Recent(30));
                List<ReactionModel> reactions = reactionFaker.Generate(count);

                reactions = reactions.GroupBy(r => new { r.BlogPostId, r.UserId, r.Type })
                    .Select(g => g.First())
                    .ToList();

                return reactions;
            }
        }

        internal static class CommentData
        {
            internal static List<CommentModel> GetComments(int count, List<Guid> userIds, List<Guid> postIds)
            {
                Faker faker = new Faker();
                Faker<CommentModel> commentFaker = new Faker<CommentModel>()
                    .RuleFor(c => c.Id, f => Guid.NewGuid())
                    .RuleFor(c => c.BlogPostId, f => f.PickRandom(postIds))
                    .RuleFor(c => c.UserId, f => f.PickRandom(userIds))
                    .RuleFor(c => c.Content, f => f.Lorem.Sentence(10))
                    .RuleFor(c => c.CreatedAt, f => f.Date.Recent(30));
                List<CommentModel> comments = commentFaker.Generate(count);
                return comments;
            }
        }

        internal static class TagData
        {
            internal static List<TagModel> GetTags(int count, List<Guid> moderatorIds, List<Guid> postIds)
            {
                Faker faker = new Faker();
                Faker<TagModel> tagFaker = new Faker<TagModel>()
                    .RuleFor(t => t.Id, f => Guid.NewGuid())
                    .RuleFor(t => t.Type, f => f.PickRandom(Enum.GetValues<TagType>()))
                    .RuleFor(t => t.ModeratorId, f => f.PickRandom(moderatorIds))
                    .RuleFor(t => t.BlogPostId, f => f.PickRandom(postIds));
                List<TagModel> tags = tagFaker.Generate(count);
                return tags;
            }
        }

    }
}

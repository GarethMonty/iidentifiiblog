namespace IIdentifii.Blog
{
    public class ReactionProcessingBackgroundService : BackgroundService
    {
        #region Fields

        private readonly ILogger<ReactionProcessingBackgroundService> _logger;

        private readonly IMemoryCache _memoryCache;

        private readonly IServiceProvider _serviceProvider;

        private readonly IReactionHandler _reactionHandler;

        #endregion

        public ReactionProcessingBackgroundService(
            ILogger<ReactionProcessingBackgroundService> logger,
            IMemoryCache memoryCache,
            IServiceProvider serviceProvider,
            IReactionHandler reactionHandler)
        {
            _logger = logger;

            _memoryCache = memoryCache;

            _serviceProvider = serviceProvider;

            _reactionHandler = reactionHandler;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    ReactionOperation? reactionOperation = await _reactionHandler.DequeueReactionAsync(stoppingToken);

                    if (reactionOperation == null)
                    {
                        await Task.Delay(1000, stoppingToken);
                        continue;
                    }

                    using IServiceScope scope = _serviceProvider.CreateScope();
                    IReactionAggregateRepository reactionAggregateRepository = scope.ServiceProvider.GetRequiredService<IReactionAggregateRepository>();

                    ReactionAggregateModel reactionAggregateModel = await GetReactionAggregateModelAsync(
                        scope,
                        reactionAggregateRepository,
                        reactionOperation.BlogPostId,
                        reactionOperation.Type,
                        stoppingToken);

                    int? trackedCount = 0;
                    switch (reactionOperation.OperationType)
                    {
                        case ReactionOperationType.Remove:
                            if(reactionAggregateModel.Count <= 0)
                            {
                                break;
                            }

                            --reactionAggregateModel.Count;
                            trackedCount = reactionAggregateRepository.AddReactionUpdate(reactionAggregateModel);

                            break;
                        case ReactionOperationType.Update:

                            ++reactionAggregateModel.Count;
                            trackedCount = reactionAggregateRepository.AddReactionUpdate(reactionAggregateModel);

                            if(reactionOperation.PreviousType != null)
                            {
                                ReactionAggregateModel previousReactionAggregateModel = await GetReactionAggregateModelAsync(
                                    scope,
                                    reactionAggregateRepository,
                                    reactionOperation.BlogPostId,
                                    reactionOperation.PreviousType.Value,
                                    stoppingToken);

                                if (previousReactionAggregateModel.Count <= 0)
                                {
                                    break;
                                }

                                --previousReactionAggregateModel.Count;

                                trackedCount = reactionAggregateRepository.AddReactionUpdate(previousReactionAggregateModel);
                            }
                            break;
                        case ReactionOperationType.Add:
                        default:
                            ++reactionAggregateModel.Count;
                            trackedCount = reactionAggregateRepository.AddReactionUpdate(reactionAggregateModel);
                            break;

                    }

                    if (trackedCount == null || trackedCount >= 100 || stopwatch.Elapsed > TimeSpan.FromSeconds(3))
                    {
                        await reactionAggregateRepository.SaveReactionChangesAsync(stoppingToken);

                        stopwatch.Restart();
                    }
                }
                catch (OperationCanceledException)
                { }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failure Processing Reaction Queue");
                }
            }
        }

        private async Task<ReactionAggregateModel> GetReactionAggregateModelAsync(
            IServiceScope scope,
            IReactionAggregateRepository reactionAggregateRepository,
            Guid blogPostId,
            ReactionType type,
            CancellationToken token)
        {
            if (_memoryCache.TryGetValue(GetKey(blogPostId, type), out object? reactionAggregateObj) 
                && reactionAggregateObj is ReactionAggregateModel reactionAggregateModel 
                && reactionAggregateModel != null)
            {
                return reactionAggregateModel;
            }

            reactionAggregateModel = await reactionAggregateRepository.GetReactionAggregateByIdAsync(blogPostId, type, token);

            if (reactionAggregateModel == null)
            {
                reactionAggregateModel = new ReactionAggregateModel()
                {
                    Id = Guid.CreateVersion7(),
                    BlogPostId = blogPostId,
                    Type = type,
                };

                IReactionRepository reactionRepository = scope.ServiceProvider.GetRequiredService<IReactionRepository>();

                reactionAggregateModel.Count = await reactionRepository.GetReactionCountAsync(blogPostId, type, token);

                await reactionAggregateRepository.AddReactionAggregateAsync(reactionAggregateModel, token);
            }

            _memoryCache.Set(GetKey(blogPostId, type), reactionAggregateModel, TimeSpan.FromHours(12));

            return reactionAggregateModel;
        }

        private static string GetKey(
            Guid blogPostId,
            ReactionType type)
        {
            return $"reaction|{blogPostId}|{type}";
        }
    }
}

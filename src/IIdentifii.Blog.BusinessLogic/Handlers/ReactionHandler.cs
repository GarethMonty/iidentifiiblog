namespace IIdentifii.Blog.BusinessLogic
{
    public class ReactionHandler : IReactionHandler
    {
        #region Fields

        private readonly static Channel<ReactionOperation> _reactionChannel = Channel.CreateUnbounded<ReactionOperation>(new UnboundedChannelOptions()
        {
            SingleReader = true,
            SingleWriter = false,
            AllowSynchronousContinuations = false,
        });

        #endregion

        #region Constructor Methods

        public ReactionHandler()
        {

        }

        #endregion

        #region Methods

        public async Task QueueReactionAsync(
            ReactionOperation reactionModel,
            CancellationToken token)
        {
            if (reactionModel == null)
            {
                throw new ArgumentNullException(nameof(reactionModel));
            }

            try
            {
                await _reactionChannel.Writer.WriteAsync(reactionModel, token);
            }
            catch (OperationCanceledException)
            {
                // Handle cancellation if needed
            }
        }

        public async Task<ReactionOperation?> DequeueReactionAsync(
            CancellationToken token)
        {
            try
            {
                return await _reactionChannel.Reader.ReadAsync(token);

            }
            catch (OperationCanceledException)
            {
                // Handle cancellation if needed
                return null;
            }
        }
        #endregion
    }
}

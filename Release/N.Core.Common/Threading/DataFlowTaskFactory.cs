using System;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace N.Core.Common.Threading
{
    public class DataFlowTaskFactory
    {
        private CancellationTokenSource _token;
        private ActionBlock<object> _task;
        public delegate void Callback(object state);

        public void StartWork(Callback callback, object state, double delayInSeconds)
        {
            // Create & start the task.
            _token = new CancellationTokenSource();
            _task = CreateContinuousTask(callback, delayInSeconds, _token.Token) as ActionBlock<object>;
            _task.Post(state);
        }

        public void StopWork()
        {
            // CancellationTokenSource implements IDisposable.
            using (_token)
            {
                _token.Cancel();
            }

            _token = null;
            _task = null;
        }

        private ITargetBlock<object> CreateContinuousTask(Callback callback, double delayInSeconds, CancellationToken cancelToken)
        {
            // Validate parameters.
            if (callback == null) throw new ArgumentNullException("callback");

            /// Create the block.
            /// The declaration and assignment are separated because it calls itself.
            /// Async so you can wait easily when the delay comes.
            ActionBlock<object> block = null;

            block = new ActionBlock<object>(async state =>
            {
                // Perform the action.
                callback(state);

                // Wait
                await Task.Delay(TimeSpan.FromSeconds(delayInSeconds), cancelToken).ConfigureAwait(false);

                // Post the action back to the block.
                block.Post(state);
            }, new ExecutionDataflowBlockOptions { CancellationToken = cancelToken });

            // Return the block.
            return block;
        }
    }
}

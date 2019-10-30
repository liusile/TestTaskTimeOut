using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestTaskTimeOut
{
    static class TaskTimeout
    {
        public static async Task WaitAsync(Func<CancellationToken, Task> action, TimeSpan timeout, CancellationToken cancellationToken)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            using (var timeoutCts = new CancellationTokenSource(timeout))
            using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(timeoutCts.Token, cancellationToken))
            {
                try
                {
                    await action(linkedCts.Token).ConfigureAwait(false);
                }
                catch (OperationCanceledException exception)
                {
                    var timeoutReached = timeoutCts.IsCancellationRequested && !cancellationToken.IsCancellationRequested;
                    if (timeoutReached)
                    {
                        throw new TimeoutException("任务超时",exception);
                    }

                    throw;
                }
            }
        }

        public static async Task<TResult> WaitAsync<TResult>(Func<CancellationToken, Task<TResult>> action, TimeSpan timeout, CancellationToken cancellationToken)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            using (var timeoutCts = new CancellationTokenSource(timeout))
            using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(timeoutCts.Token, cancellationToken))
            {
                try
                {
                    return await action(linkedCts.Token).ConfigureAwait(false);
                }
                catch (OperationCanceledException exception)
                {
                    var timeoutReached = timeoutCts.IsCancellationRequested && !cancellationToken.IsCancellationRequested;
                    if (timeoutReached)
                    {
                        throw new TimeoutException("任务超时",exception);
                    }

                    throw;
                }
            }
        }
    }
}

using System.Threading;
using System.Threading.Tasks;

namespace AmazingUWPToolkit.Extensions
{
    public static class TaskExtensions
    {
        #region Public Methods

        public static async Task CompleteNotEarlierThan(this Task taskToComplete, int millisecondsDelay, CancellationToken delayCancellationToken = default)
        {
            await Task.WhenAll(taskToComplete, Task.Delay(millisecondsDelay, delayCancellationToken));
        }

        public static async Task<T> CompleteNotEarlierThan<T>(this Task<T> taskToComplete, int millisecondsDelay, CancellationToken delayCancellationToken = default)
        {
            await Task.WhenAll(taskToComplete, Task.Delay(millisecondsDelay, delayCancellationToken));

            return await taskToComplete;
        }

        #endregion
    }
}
using System.Runtime.CompilerServices;

namespace TrackAndFieldResults.Utils
{
    /// <summary>
    /// Helper for paralell tasks
    /// </summary>
    /// <remarks>Taken from: https://github.com/SergKorol/AwaitHandleSample and 
    /// https://dev.to/serhii_korol_ab7776c50dba/the-elegant-way-to-await-multiple-tasks-in-net-11pl</remarks>
    public static class TaskHelper
    {
        public static TaskAwaiter<(T, T)> GetAwaiter<T>(this (Task<T>, Task<T>) tasks)
        {
            async Task<(T, T)> MergeTasks()
            {
                var (task1, task2) = tasks;
                await TaskExt.WhenAll(task1, task2);

                return (task1.Result, task2.Result);
            }

            return MergeTasks().GetAwaiter();
        }
    }
}

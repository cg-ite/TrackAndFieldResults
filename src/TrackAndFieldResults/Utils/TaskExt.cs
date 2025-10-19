namespace TrackAndFieldResults.Utils
{
    /// <summary>
    /// Helper for paralell tasks
    /// </summary>
    /// <remarks>Taken from: https://github.com/SergKorol/AwaitHandleSample and 
    /// https://dev.to/serhii_korol_ab7776c50dba/the-elegant-way-to-await-multiple-tasks-in-net-11pl</remarks>
    /// <example>
    /// <code>
    /// var horo = new HoroService();
    /// var taurus = horo.MakeDailyHoroscopeBySign(HoroService.ZodiacSign.Taurus);
    /// var scorpio = horo.MakeDailyHoroscopeBySign(HoroService.ZodiacSign.Scorpio);
    /// var(forecast1, forecast2) = await(taurus, scorpio);
    /// Console.WriteLine(forecast1);
    /// Console.WriteLine(forecast2);
    /// </code>
    /// </example>
    public static class TaskExt
    {
        public static async Task<Task<T>[]> WhenAll<T>(params Task<T>[] tasks)
        {
            try
            {
                await Task.WhenAll(tasks);
            }
            catch (InvalidOperationException e)
            {
                var faultedTasks = tasks.Where(task => task.Exception != null).ToArray();
                return faultedTasks;
            }

            return tasks.Where(task => task.Status == TaskStatus.RanToCompletion).ToArray();
        }
    }

    public static class TaskEx
    {
        public static async Task<(T1 result1, T2 result2)> WhenAll<T1, T2>(Task<T1> task1, Task<T2> task2) => (await task1, await task2);
        public static async Task<(T1 result1, T2 result2, T3 result3)> WhenAll<T1, T2, T3>(Task<T1> task1, Task<T2> task2, Task<T3> task3) => (await task1, await task2, await task3);
        // etc etc
    }

}

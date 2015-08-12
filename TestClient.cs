using System;
using System.Diagnostics;
using System.Collections;

namespace SplunkClient
{
	public class TestClient
	{
		public static async Task<long> SendMultipleTestEventsAsync(int howMany, SplunkLogger logger)
		{
			Stopwatch timer = new Stopwatch ();
			timer.Start ();

			for (int i = 1; i <= howMany; i++) {
				string time = timer.ElapsedMilliseconds.ToString();
				await logger.LogAsync ("This is iPhone test event " + i + " out of " + howMany + 
                    ".  It has been " + time + " millis since requests started.");
			}
			timer.Stop ();
			return timer.ElapsedMilliseconds;
		}

        public static async Task<long> SendTestBatch(int batchSize, SplunkLogger logger)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();

            Queue<string> batch = new Queue<string>();
            for (int i = 1; i <= batchSize; i++)
            {
                string time = timer.ElapsedMilliseconds.ToString();
                batch.Enqueue("This is test event " + i + " out of " + batchSize + 
                    " in a batch.  It has been " + time + " millis since request began.");
            }

            await logger.SendBatchAsync(batch);
            timer.Stop();
            return timer.ElapsedMilliseconds;
        }
	}
}

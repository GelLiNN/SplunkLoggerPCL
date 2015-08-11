using System;
using System.Diagnostics;

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
				await logger.LogAsync ("This is iPhone test event " + i + " out of " + howMany + ".  It has been " + 
					time + " millis since requests started.");
			}
			timer.Stop ();
			return timer.ElapsedMilliseconds;
		}
	}
}

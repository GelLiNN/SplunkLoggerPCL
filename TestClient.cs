using System;
using System.Diagnostics;

namespace SplunkClient
{
	public class TestClient
	{
		public static void main(string[] args)
		{
			SplunkLogger logger = new SplunkLogger ("https://10.80.8.76:8088/services/collector", "81EBB9BA-BEAC-43AF-B482-F683CFBBE68C", true);
			logger.Log ("This is a test Event");
			logger.setLevel ("error");
			logger.Log ("This is a test Error Event");

			Action log = async () =>
			{
				await logger.LogAsync("This is a test Async requested event, it should appear twice in index");
			};
			log();
			log();
		}
	}
}

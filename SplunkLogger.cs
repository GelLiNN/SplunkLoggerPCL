using System;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SplunkClient
{
	/*
	* Logger is a portable class that easily sends data
	* to Splunk over HTTP/HTTPS providing great abstraction
	* for developers logging mobile data to Splunk */
	public class SplunkLogger
	{
		private string level;
		private string uri;
		private HttpClient client; 
		private bool sslEnabled;
		private List<Exception> exceptions;

		// Keeps track of severity levels
		private List<string> levels;

		/*
		* Constructs a SplunkLogger with (most importantly), URI and Http Event Collector token */
		public SplunkLogger (string newUri, string token, bool ssl)
		{
			client = new HttpClient ();
			client.DefaultRequestHeaders.Authorization = 
				new System.Net.Http.Headers.AuthenticationHeaderValue("Splunk", token);
			this.uri = newUri;

			// severity levels for SplunkLogger
			levels = new List<string> ();
			levels.Add ("ERROR");
			levels.Add ("INFO");
			levels.Add ("OFF");
			levels.Add ("VERBOSE");
			levels.Add ("WARNING");
			this.level = "INFO";

			sslEnabled = ssl;
			/*
			if (sslEnabled) {
				System.Net.ServicePointManager.ServerCertificateValidationCallback += 
					(sender, cert, chain, sslPolicyErrors) => true;
			}
			*/
		}

		/*
		* Tells SplunkLogger whether or not to enable SSL */
		public void enableSSL ()
		{
			/* Should enable SSL in DIFFERENT WAY if Splunk deployment isn't self signed
			 * otherwise, this code will automatically validate server cert
			if (!sslEnabled) {
				System.Net.ServicePointManager.ServerCertificateValidationCallback += 
					(sender, cert, chain, sslPolicyErrors) => true;
			}
			*/
			sslEnabled = true;
		}

		/*
		* Changes the severity level of this SplunkLogger */
		public void setLevel (string newLevel)
		{
			if (levels != null && levels.Contains(newLevel.ToUpper())) {
				level = newLevel;
			}
		}

		/*
		* Logs a string message/event to Splunk's Http Event Collector */
		public void Log (string message)
		{
			string JSONstr = "{\"event\":{\"message\":\"" + message + "\", \"severity\":\"" + level + "\", \"sourcetype\":\"Mobile Application\"}}";
			HttpContent content = new StringContent(JSONstr);
			var resp = (HttpResponseMessage) client.PostAsync(
				this.uri, content).Result;
		}

		/*
		* Logs a string message/event to Splunk's Http Event Collector Async */
		async public Task LogAsync (string message)
		{
			string JSONstr = "{\"event\":{\"message\":\"" + message + "\", \"severity\":\"" + level + "\"}}";
			HttpContent content = new StringContent(JSONstr);

			var resp = await client.PostAsync (this.uri, content);
			resp.EnsureSuccessStatusCode ();
		}
	}
}
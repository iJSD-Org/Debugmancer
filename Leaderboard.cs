using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Debugmancer.Models;
using Godot;
using Mono.Web;
using Newtonsoft.Json.Linq;

namespace Debugmancer
{
	class Leaderboard : Node
	{
		public List<ScoreEntry> ScoreEntries;

		public override void _Ready()
		{
			GetLeaderboard();
		}

		public void AddScoreEntry(string name, int score)
		{
			
			string url = $"http://dreamlo.com/lb/i_4PXzwaHUKL5JkjB4_RMw1VmyiI6leUCyInbaAIlpzg/add/{HttpUtility.UrlEncode(name)}/{score}";
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
			using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
			{
				using (Stream stream = response.GetResponseStream())
				{
					using (StreamReader reader = new StreamReader(stream))
					{
						//Console.WriteLine(reader.ReadToEnd());
					}
				}
			}
		}

		public void GetLeaderboard()
		{
			string uri = "http://dreamlo.com/lb/5f536936eb371809c4113b75/json";

			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
			using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
			{
				using (Stream stream = response.GetResponseStream())
				{
					using (StreamReader reader = new StreamReader(stream ?? throw new InvalidOperationException()))
					{
						//Deserialize Result
						JObject leaderboard = JObject.Parse(reader.ReadToEnd());
						List<JToken> results = leaderboard["dreamlo"]["leaderboard"]["entry"].Children().ToList();
						ScoreEntries = results.Select(result => result.ToObject<ScoreEntry>()).ToList();
					}
				}
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Debugmancer.Models;
using Godot;
using Newtonsoft.Json.Linq;

namespace Debugmancer.Levels
{
	public class Leaderboard : Control
	{
		public List<ScoreEntry> ScoreEntries;
		private readonly PackedScene _leaderboardItem = ResourceLoader.Load<PackedScene>("res://Objects/LeaderboardItem.tscn");

		public override void _EnterTree()
		{
			GetLeaderboard();
			LoadLeaderboard();
		}

		public override void _Input(InputEvent @event)
		{
			if(Input.IsActionPressed("E")) 
			{
				GetTree().ChangeScene("res://Levels/Main Menu.tscn");
			}
		}

		public void GetLeaderboard()
		{
			string uri = "http://dreamlo.com/lb/5f63459ceb371809c4315ad6/json";

			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
			using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
			{
				using (Stream stream = response.GetResponseStream())
				{
					using (StreamReader reader = new StreamReader(stream ?? throw new InvalidOperationException()))
					{
						//Deserialize Result
						JObject leaderboard = JObject.Parse(reader.ReadToEnd());
						try
						{
							List<JToken> results = leaderboard["dreamlo"]?["leaderboard"]?["entry"]?.Children().ToList();
							ScoreEntries = (results ?? throw new InvalidOperationException()).Select(result => result.ToObject<ScoreEntry>()).ToList();
						}
						catch
						{
							// None if there is no result
						}
					}
				}
			}
		}

		private void LoadLeaderboard()
		{
			foreach (ScoreEntry scoreEntry in ScoreEntries)
			{
				Node instance =  _leaderboardItem.Instance();
				instance.GetNode<Label>("HBoxContainer/Name").Text = scoreEntry.Name;
				instance.GetNode<Label>("HBoxContainer/Score").Text = scoreEntry.Score.ToString();
				GetNode<VBoxContainer>("Panel/ScrollContainer/VBoxContainer").AddChild(instance);
			}
		}
	}
}

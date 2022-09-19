using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using Debugmancer.Models;
using Godot;
using Newtonsoft.Json.Linq;

namespace Debugmancer.Levels
{
	public partial class Leaderboard : Control
	{
		public List<ScoreEntry> ScoreEntries;
		private readonly PackedScene _leaderboardItem = ResourceLoader.Load<PackedScene>("res://Objects/LeaderboardItem.tscn");
		private static readonly HttpClient Client = new();

		public override void _EnterTree()
		{
			GetLeaderboard();
			LoadLeaderboard();
		}

		public override void _Input(InputEvent @event)
		{
			if (Input.IsActionPressed("E"))
			{
				GetTree().ChangeSceneToFile("res://Levels/Main Menu.tscn");
			}
		}

		public async void GetLeaderboard()
		{
			string uri = "http://dreamlo.com/lb/5f63459ceb371809c4315ad6/json";

			HttpResponseMessage response = await Client.GetAsync(uri);
			response.EnsureSuccessStatusCode();
			Stream stream = await response.Content.ReadAsStreamAsync();

			try
			{
				using StreamReader reader = new StreamReader(stream);
				//Deserialize Result
				JObject leaderboard = JObject.Parse(await reader.ReadToEndAsync());

				List<JToken> results = leaderboard["dreamlo"]?["leaderboard"]?["entry"]?.Children()
					.ToList();
				ScoreEntries = (results ?? throw new InvalidOperationException())
					.Select(result => result.ToObject<ScoreEntry>()).ToList();
			}
			catch
			{
				// Nothing will happen if response is empty or null
			}
		}

		private void LoadLeaderboard()
		{
			foreach (ScoreEntry scoreEntry in ScoreEntries)
			{
				Node instance = _leaderboardItem.Instantiate();
				instance.GetNode<Label>("HBoxContainer/Name").Text = scoreEntry.Name;
				instance.GetNode<Label>("HBoxContainer/Score").Text = scoreEntry.Score.ToString();
				GetNode<VBoxContainer>("Panel/ScrollContainer/VBoxContainer").AddChild(instance);
			}
		}
	}
}

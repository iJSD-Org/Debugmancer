using System;
using System.IO;
using System.Net;
using System.Net.Http;
using Debugmancer.Objects.Player;
using Godot;

namespace Debugmancer.Levels
{
	public partial class Death : Control
	{

		private static readonly HttpClient Client = new();

		public override void _EnterTree()
		{
			AddScoreEntry(((RichPresence)GetNode("/root/RichPresence")).Client.CurrentUser.ToString(), Globals.Score);
		}

		public override void _Ready()
		{
			GetNode<AnimationPlayer>("AnimationPlayer").Play("FadeIn");
			GetNode<Label>("Score").Text = $"You scored : {Globals.Score}";
			Globals.ResetValues();
		}

		public override void _Process(double delta)
		{
			base._Process(delta);

			if (Input.IsActionPressed("E") && GetNode<Label>("Prompt").Visible)
			{
				GetNode<AnimationPlayer>("AnimationPlayer").Play("FadeOut");
			}
		}

		public async void AddScoreEntry(string name, int score)
		{

			string url = $"http://dreamlo.com/lb/Pv6PwoSKi0e2o9TOfiZb-QuaU0x_d4VE2kmz0kXoVsqg/add/{Uri.EscapeDataString(name)}/{score}";

			HttpResponseMessage response = await Client.GetAsync(url);
			response.EnsureSuccessStatusCode();
			Stream stream = await response.Content.ReadAsStreamAsync();
			using StreamReader reader = new StreamReader(stream);
			if (await reader.ReadToEndAsync() == "OK")
			{
				Label scoreStatus = GetNode<Label>("ScoreStatus");
				scoreStatus.Text = "Score upload Success!";
				scoreStatus.AddThemeColorOverride("font_color", new Color("Green"));
			}
			else
			{
				Label scoreStatus = GetNode<Label>("ScoreStatus");
				scoreStatus.Text = "Score upload fail!";
				scoreStatus.AddThemeColorOverride("font_color", new Color("Red"));
			}
			GetNode<Label>("Prompt").Visible = true;
		}

		public void _on_AnimationPlayer_finished(string animName)
		{
			if (animName == "FadeOut")
			{
				GetNode<AudioStreamPlayer>("/root/BackgroundMusic/MenuMusic").Play(12.47f);
				GetTree().ChangeSceneToFile("res://Levels/Main Menu.tscn");
			}
		}
	}
}

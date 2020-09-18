using System.IO;
using System.Net;
using Debugmancer.Objects.Player;
using Godot;

namespace Debugmancer.Levels
{
	public class Death : Control
	{

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

		public override void _Process(float delta)
		{
			base._Process(delta);

			if (Input.IsActionPressed("E") && GetNode<Label>("Prompt").Visible)
			{
				GetNode<AnimationPlayer>("AnimationPlayer").Play("FadeOut");
			}
		}

		public void AddScoreEntry(string name, int score)
		{

			string url = $"http://dreamlo.com/lb/Pv6PwoSKi0e2o9TOfiZb-QuaU0x_d4VE2kmz0kXoVsqg/add/{WebUtility.UrlEncode(name)}/{score}";
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
			using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
			{
				using (Stream stream = response.GetResponseStream())
				{
					using (StreamReader reader = new StreamReader(stream ?? new MemoryStream()))
					{
						if (reader.ReadToEnd() == "OK")
						{
							Label scoreStatus = GetNode<Label>("ScoreStatus");
							scoreStatus.Text = "Score upload Success!";
							scoreStatus.AddColorOverride("font_color",Color.ColorN("Green"));
						}
						else
						{
							Label scoreStatus = GetNode<Label>("ScoreStatus");
							scoreStatus.Text = "Score upload fail!";
							scoreStatus.AddColorOverride("font_color", Color.ColorN("Red"));
						}
						GetNode<Label>("Prompt").Visible = true;
					}
				}
			}
		}

		public void _on_AnimationPlayer_finished(string animName)
		{
			if (animName == "FadeOut")
			{
				GetNode<AudioStreamPlayer>("/root/BackgroundMusic/MenuMusic").Play(12.47f);
				GetTree().ChangeScene("res://Levels/Main Menu.tscn");
			}
		}
	}
}

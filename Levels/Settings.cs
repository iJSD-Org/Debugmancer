using System.Collections.Generic;
using System.Linq;
using DiscordRPC;
using DiscordRPC.Message;
using Godot;

namespace Debugmancer.Levels
{
	public partial class Settings : Window
	{

		public override void _Ready()
		{
			DiscordRpcClient client = ((RichPresence)GetNode("/root/RichPresence")).Client;
			GetNode<Label>("TabContainer/Discord/VBoxContainer/Label").Text = client.CurrentUser != null ? $"Connected as {client.CurrentUser}" : "Not connected";
			OptionButton options = GetNode<OptionButton>("TabContainer/Display/VBoxContainer/HBoxContainer/OptionButton");
			List<Vector2i> resolutions = new List<Vector2i> { new Vector2i(800, 600), new Vector2i(960, 540), new Vector2i(1280, 720), new Vector2i(1920, 1080) };
			

			if (!resolutions.Contains(DisplayServer.WindowGetSize()))
			{
				resolutions.Add(DisplayServer.WindowGetSize());
				resolutions.Sort((v1, v2) =>
				{
					if (v1.x > v2.x)
					{
						return 1;
					}

					if (v1.x == v2.x)
					{
						if (v1.y > v2.y)
						{
							return 1;
						}

						if (v1.y == v2.y)
						{
							return 0;
						}
						return -1;
					}
					return -1;
				});
			}

			foreach (string resolution in resolutions.Select(r => $"{r.x}x{r.y}"))
			{
				options.AddItem(resolution);
			}

			GetNode<Godot.Button>("TabContainer/Discord/VBoxContainer/HBoxContainer/Button").Disabled = client.CurrentUser != null;

			// Select current resolution

			options.Selected = resolutions.Select((value, index) => new { value, index })
				.SkipWhile(pair => DisplayServer.WindowGetSize() != pair.value).First().index;
			client.OnReady += client_OnReady;
			client.OnConnectionFailed += client_OnConnectionFailed;
		}

		private void _on_SettingsDialog_about_to_show()
		{
			DiscordRpcClient client = ((RichPresence)GetNode("/root/RichPresence")).Client;
			GetNode<Godot.Button>("TabContainer/Discord/VBoxContainer/HBoxContainer/Button").Disabled = client.CurrentUser != null;
			GetNode<Label>("TabContainer/Discord/VBoxContainer/Label").Text = client.CurrentUser != null ? $"Connected as {client.CurrentUser}" : "Not connected";
		}

		private void _on_CheckButton_toggled(bool buttonPressed)
		{
			ProjectSettings.SetSetting("display/window/size/fullscreen", true);

			GetNode<OptionButton>("TabContainer/Display/VBoxContainer/HBoxContainer/OptionButton").Disabled =
				buttonPressed;
		}

		private void _on_Button_pressed()
		{
			DiscordRpcClient client = ((RichPresence)GetNode("/root/RichPresence")).Client;
			GetNode<Godot.Button>("TabContainer/Discord/VBoxContainer/HBoxContainer/Button").Disabled = true;
			GetNode<Label>("TabContainer/Discord/VBoxContainer/Label").Text = "Connecting...";
		}

		private void client_OnReady(object sender, ReadyMessage e)
		{
			GetNode<Godot.Button>("TabContainer/Discord/VBoxContainer/HBoxContainer/Button").Disabled = true;
			GetNode<Label>("TabContainer/Discord/VBoxContainer/Label").Text = $"Connected as {e.User}";
		}

		private void client_OnConnectionFailed(object sender, ConnectionFailedMessage e)
		{
			GetNode<Godot.Button>("TabContainer/Discord/VBoxContainer/HBoxContainer/Button").Disabled = false;
			GetNode<Label>("TabContainer/Discord/VBoxContainer/Label").Text = "Connection failed";
		}

		private void _on_OptionButton_item_selected(int index)
		{
			int[] res = GetNode<OptionButton>("TabContainer/Display/VBoxContainer/HBoxContainer/OptionButton")
				.GetItemText(index).Split('x').Select(int.Parse).ToArray();
			DisplayServer.WindowSetSize(new Vector2i(res[0], res[1]));
		}

	}
}

using System;
using DiscordRPC;
using Godot;
using Button = DiscordRPC.Button;

namespace Debugmancer
{
	public partial class RichPresence : Node
	{
		public DiscordRpcClient Client;

		public override void _Ready()
		{
			Client = new DiscordRpcClient("752376874339926187")
			{
				ShutdownOnly = true
			};

			Client.OnReady += (o, e) =>
			{
				GD.Print($"Received Ready from user {Client.CurrentUser}");
			};

			Client.Initialize();

			DiscordRPC.RichPresence rp = new DiscordRPC.RichPresence
			{
				Buttons = new []{ new Button { Label = "Source code", Url = "https://github.com/iJSD-Org/Debugmancer"} },
				Assets = new Assets
				{
					SmallImageKey = "godot-3",
					SmallImageText = "Made with Godot Engine",
					LargeImageKey = "bugmancer",
				},
				Timestamps = new Timestamps(DateTime.UtcNow)
			};
			Client.SetPresence(rp);
		}

		public override void _ExitTree()
		{
			Client.Dispose();
		}
	}
}

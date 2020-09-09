using System;
using Debugmancer.NamedPipeClient;
using DiscordRPC;
using Godot;

namespace Debugmancer
{
	class RichPresence : Node
	{
		public DiscordRpcClient Client;

		public override void _Ready()
		{
			Client = new DiscordRpcClient("752376874339926187", -1, null, true, new UnityNamedPipe()) { ShutdownOnly = true };

			Client.Initialize();

			DiscordRPC.RichPresence rp = new DiscordRPC.RichPresence
			{
				Details = "https://github.com/sitiom/Debugmancer",
				Assets = new Assets()
				{
					SmallImageKey = "godot",
					SmallImageText = "Made with Godot Engine",
					LargeImageKey = "godot",
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

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
			InitRpc();
		}

		private void InitRpc()
		{
			RichPresence rpc = (RichPresence)GetNode("/root/RichPresence");

			rpc.Client = new DiscordRpcClient("752376874339926187", -1, null, true, new UnityNamedPipe()) { ShutdownOnly = true };

			rpc.Client.Initialize();

			DiscordRPC.RichPresence rp = new DiscordRPC.RichPresence
			{
				Assets = new Assets()
				{
					SmallImageKey = "godot",
					SmallImageText = "Made with Godot Engine",
					LargeImageKey = "godot",
					LargeImageText = "Debugmancer",
				},
				Timestamps = new Timestamps(DateTime.UtcNow)
			};
			rpc.Client.SetPresence(rp);
		}

		public override void _ExitTree()
		{
			Client.Dispose();
		}
	}
}

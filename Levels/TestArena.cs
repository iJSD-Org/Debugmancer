using System;
using System.Threading;
using System.Threading.Tasks;
using Debugmancer.NamedPipeClient;
using DiscordRPC;
using Godot;
using Thread = Godot.Thread;

namespace Debugmancer.Levels
{
	public class TestArena : Node2D
	{

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
				Details = "",
				State = "In TestArena",
				Assets = new Assets()
				{
					SmallImageKey = "godot",
					SmallImageText = "Made in Godot Engine",
					LargeImageKey = "godot",
					LargeImageText = "Debugmancer",
				},
				Timestamps = new Timestamps(DateTime.UtcNow)
			};
			rpc.Client.SetPresence(rp);
			GD.Print("Yep Im here");
		}
	}
}

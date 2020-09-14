using Godot;
using System;
using Debugmancer.Objects.Player;

namespace Debugmancer.Objects
{
	public class ModuleDropper : CanvasLayer
	{
		public int ModulesDropped;
		public Control Module;
		public override void _Input(InputEvent @event)
		{
			if(Input.IsActionPressed("E")) 
			{
				GetNode<ColorRect>("ModuleDropper/FadeAnim/Overlay").Visible = false;
				GetNode<TextureRect>("ModuleDropper/FadeAnim/Book").Visible = false;
				GetNode<Label>("ModuleDropper/BlinkAnim/Label").Visible = false;
				GetNode<AnimationPlayer>("ModuleDropper/BlinkAnim").Stop();
				Module.Visible = false;
				GetTree().Paused = false;
			}
		}
		public override void _Process(float delta)
		{
			if(Globals.Score > 2000 && ModulesDropped == 5)
			{
				ShowModule(GetNode<Control>("ModuleDropper/FadeAnim/Book/GitModule"));
				GetParent().GetNode("TestArena/Player").GetNode<TextureProgress>("HUD/VBoxContainer/Health").MaxValue = 25;
				GetParent().GetNode("TestArena/Player").GetNode<TextureProgress>("HUD/VBoxContainer/Health").Value = 25;
				GetParent().GetNode("TestArena/Player").GetNode<Health>("Health").MaxHealth += 10;
				GetParent().GetNode("TestArena/Player").GetNode<Health>("Health").Recover(10);
			}
			else if(Globals.Score > 1500 && ModulesDropped == 4) 
			{
				ShowModule( GetNode<Control>("ModuleDropper/FadeAnim/Book/OOPModule"));
				Globals.CritChance *= 2;
			}
			else if(Globals.Score > 1000 && ModulesDropped == 3) 
			{
				ShowModule(GetNode<Control>("ModuleDropper/FadeAnim/Book/DataStructuresModule"));
				Globals.PlayerDamage *= 2;
			}
			else if(Globals.Score > 500 && ModulesDropped == 2)
			{
				ShowModule(GetNode<Control>("ModuleDropper/FadeAnim/Book/BasicProgrammingModule"));
				GetParent().GetNode("TestArena/Player").GetNode<Timer>("RecoverTimer").Start();
			}
			else if(Globals.Score > 250 && ModulesDropped == 1) 
			{
				ShowModule(GetNode<Control>("ModuleDropper/FadeAnim/Book/SelfStudyModule"));
				Globals.CanDash = true;
			}
			else if(Globals.Score >= 0 && ModulesDropped == 0) ShowModule(GetNode<Control>("ModuleDropper/FadeAnim/Book/DebugModule"));

		}
		public void ShowModule(Control module)
		{
			GetTree().Paused = true;
			ModulesDropped++;
			Module = module;
			GetNode<ColorRect>("ModuleDropper/FadeAnim/Overlay").Visible = true;
			GetNode<TextureRect>("ModuleDropper/FadeAnim/Book").Visible = true;
			GetNode<Label>("ModuleDropper/BlinkAnim/Label").Visible = true;
			module.Visible = true;
			GetNode<AnimationPlayer>("ModuleDropper/FadeAnim").Play("FadeIn");
			GetNode<AnimationPlayer>("ModuleDropper/BlinkAnim").Play("Blink");
		}
	}
}

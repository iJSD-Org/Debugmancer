using Godot;
using System;

namespace Debugmancer.Objects
{
	public class ModuleDropper : CanvasLayer
	{
		public int _modulesDropped = 0;
		public Control _module;
		public override void _Input(InputEvent @event)
		{
			if(Input.IsActionPressed("E")) 
			{
				GetNode<ColorRect>("ModuleDropper/FadeAnim/Overlay").Visible = false;
				GetNode<TextureRect>("ModuleDropper/FadeAnim/Book").Visible = false;
				GetNode<Label>("ModuleDropper/BlinkAnim/Label").Visible = false;
				GetNode<AnimationPlayer>("ModuleDropper/BlinkAnim").Stop(true);
				_module.Visible = false;
				GetTree().Paused = false;
			}
		}
		public override void _Process(float delta)
		{
			if(Globals.score > 2000 && _modulesDropped == 5)
			{
				ShowModule(GetNode<Control>("ModuleDropper/FadeAnim/Book/GitModule"));
				GetParent().GetNode("TestArena/Player").GetNode<TextureProgress>("HUD/VBoxContainer/Health").MaxValue = 25;
				GetParent().GetNode("TestArena/Player").GetNode<TextureProgress>("HUD/VBoxContainer/Health").Value = 25;
				GetParent().GetNode("TestArena/Player").GetNode<Health>("Health").MaxHealth += 10;
				GetParent().GetNode("TestArena/Player").GetNode<Health>("Health").Recover(10);

			}
			else if(Globals.score > 1500 && _modulesDropped == 4) 
			{
				ShowModule( GetNode<Control>("ModuleDropper/FadeAnim/Book/OOPModule"));
				Globals.critChance *= 2;
			}
			else if(Globals.score > 1000 && _modulesDropped == 3) 
			{
				ShowModule(GetNode<Control>("ModuleDropper/FadeAnim/Book/DataStructuresModule"));
				Globals.playerDamage *= 2;
			}
			else if(Globals.score > 500 && _modulesDropped == 2)
			{
				ShowModule(GetNode<Control>("ModuleDropper/FadeAnim/Book/BasicProgrammingModule"));
				GetParent().GetNode("TestArena/Player").GetNode<Timer>("RecoverTimer").Start();
			}
			else if(Globals.score > 250 && _modulesDropped == 1) 
			{
				ShowModule(GetNode<Control>("ModuleDropper/FadeAnim/Book/SelfStudyModule"));
				Globals.canDash = true;
			}
			else if(Globals.score >= 0 && _modulesDropped == 0) ShowModule(GetNode<Control>("ModuleDropper/FadeAnim/Book/DebugModule"));

		}
		public void ShowModule(Control module)
		{
			GetTree().Paused = true;
			_modulesDropped++;
			_module = module;
			GetNode<ColorRect>("ModuleDropper/FadeAnim/Overlay").Visible = true;
			GetNode<TextureRect>("ModuleDropper/FadeAnim/Book").Visible = true;
			GetNode<Label>("ModuleDropper/BlinkAnim/Label").Visible = true;
			module.Visible = true;
			GetNode<AnimationPlayer>("ModuleDropper/FadeAnim").Play("FadeIn");
			GetNode<AnimationPlayer>("ModuleDropper/BlinkAnim").Play("Blink");
		}
	}
}

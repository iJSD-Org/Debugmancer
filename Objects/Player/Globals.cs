namespace Debugmancer.Objects.Player
{
	public static class Globals
	{
		public static int Score;
		public static double ScoreMultiplier = 1;
		public static bool CanDash;
		public static int PlayerDamage = 1;
		public static int CritChance = 1;
		public static int Energy = 100;
		public static bool IsDying;
		public static bool IsRecover;

		public static void ResetValues()
		{
			Score = 0;
			ScoreMultiplier = 1;
			CanDash = false;
			PlayerDamage = 1;
			CritChance = 1;
			Energy = 100;
			IsDying = false;
			IsRecover = false;
		}
	}
}

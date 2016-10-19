namespace LuminousVector
{
	public enum GameEvent
	{
		GAME_PAUSED,
		GAME_UNPAUSED,
		GAME_READY,
		DUMMY_EVENT,
		PLAYER_CRASH,
		PLAYER_DIE,
		GENERATOR_LOOP
	}

	public enum TurnDir
	{
		Left = 0,
		Right = 1,
		Up = 2,
		Down = 3
	}

	public enum PlaneAxis
	{
		XZ, ZX, XY, YX, ZY, YZ
	}

	public enum RotationAxis
	{
		X, Y, Z
	}
}

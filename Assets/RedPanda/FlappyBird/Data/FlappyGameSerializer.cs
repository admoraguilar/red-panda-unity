using WaterToolkit.Serialization;

namespace FlappyBird
{
	public class FlappyGameSerializer
	{
		private DataSerializer _gameData = new DataSerializer("flappy-data");

		public DataSerializer gameData => _gameData;
	}

	public static class SFlappyGameSerialzer
	{
		public static readonly FlappyGameSerializer instance = new FlappyGameSerializer();

		public static DataSerializer gameData => instance.gameData;
	}
}

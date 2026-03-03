using _Project.Develop.Runtime.Meta;
using UnityEngine;


namespace _Project.Develop.Runtime.Gameplay.Configs
{
	[CreateAssetMenu(menuName = "Gameplay Config", fileName = "GameplayConfig")]
	public class GameplayConfig : ScriptableObject
	{
		[SerializeField] private string _numbers;
		[SerializeField] private string _letters;
		[SerializeField] private int    _sequenceLength;

		private GameMode _gameMode;

		public GameMode  GameMode => _gameMode;

		public char[] Numbers        => _numbers.ToCharArray();
		public char[] Letters        => _letters.ToCharArray();
		public int    SequenceLength => _sequenceLength;
	}
}

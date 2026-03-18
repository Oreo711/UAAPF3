using UnityEngine;


namespace _Project.Develop.Runtime.Configs.Meta
{
	[CreateAssetMenu(menuName = "Reset Cost Config", fileName = "ResetCostConfig")]
	public class StatsResetCostConfig : ScriptableObject
	{
		[SerializeField] private int _resetCost;

		public int ResetCost => _resetCost;
	}
}

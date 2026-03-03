using UnityEngine;


namespace _Project.Develop.Runtime.Utilities.CoroutineManagement
{
    public class CoroutinePerformer : MonoBehaviour, ICoroutinePerformer
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}

using UnityEngine;

namespace SO
{
    public enum EnvironmentType
    {
        Local = 1,
        Remote = 2,
    }

    [CreateAssetMenu(fileName = "Environment", menuName = "SO/Environment")]
    public class EnvironmentSO : ScriptableObject
    {
        public EnvironmentType environmentType = EnvironmentType.Remote;
        public string RemoteUrl = "http://localhost:8000/";
    }
}
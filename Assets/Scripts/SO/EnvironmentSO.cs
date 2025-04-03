using UnityEngine;

namespace SO
{
    public enum EnvironmentType
    {
        Local = 1,
        Remote = 2,
    }

    public enum EnvironmentLevel
    {
        VRImage360 = 1, //
        VRModel3d = 2,
    }

    [CreateAssetMenu(fileName = "Environment", menuName = "SO/Environment")]
    public class EnvironmentSO : ScriptableObject
    {
        public EnvironmentType environmentType = EnvironmentType.Remote;
        public string RemoteUrl = "http://localhost:8000/";
        public EnvironmentLevel environmentLevel = EnvironmentLevel.VRModel3d;
    }
}
using UnityEngine;

namespace _3dProjectionSelection.Scripts.Physics
{
    public class DebugBounds : MonoBehaviour
    {
        public static DebugBounds Singleton;

        public GameObject debugSphere;

        private void Awake()
        {
            Singleton = this;
        }
        
        
    }
}
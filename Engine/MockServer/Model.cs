using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    public class Model : MonoBehaviour
    {
        private static Model singleton;

        [SerializeField] private float _adjacent;
        public static float adjacent { get { return singleton._adjacent; } }


        private void Awake()
        {
            if (singleton != null)
            {
                Debug.LogError("Multiple Model singletons");
            }
            singleton = this;
        }
    }
}


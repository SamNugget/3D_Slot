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
        [SerializeField] private int _rows;
        public static int rows { get { return singleton._rows; } }


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


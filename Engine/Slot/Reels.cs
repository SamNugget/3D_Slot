using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Server;

namespace Slot
{
    public class Reels : MonoBehaviour, Buildable
    {
        private static Reels singleton;

        [SerializeField] private Reel[] reels;

        private static bool isSpinning = false;


        public static void spin()
        {
            if (isSpinning)
                return;
            if (!Money.sufficientFunds())
                return;

            // todo: get result from server
        }


        public void build()
        {
            foreach (Reel reel in reels)
                reel.build();
        }


        private void Start()
        {
            Symbols.singleton.build();
            ReelsData.singleton.build();
            Money.singleton.build();
            Reels.singleton.build();
        }

        private void Awake()
        {
            if (singleton != null)
            {
                Debug.LogError("Multiple Reels singletons");
            }
            singleton = this;
        }
    }
}


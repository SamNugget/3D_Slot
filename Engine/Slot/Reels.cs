using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Server;

namespace Slot
{
    public class Reels : MonoBehaviour, Buildable, ISlotClient
    {
        private static Reels singleton;

        private IServer server;

        [SerializeField] private Reel[] reels;
        [SerializeField] private float _spinDelay;
        public static float spinDelay { get { return singleton._spinDelay; } }
        [SerializeField] private float spinSpeed;

        private static bool isSpinning = false;
        public static Result spinResult;


        public void recieveResult(Result result)
        {
            spinResult = result;
        }

        public static void spin()
        {
            if (isSpinning)
                return;
            if (!Money.sufficientFunds())
                return;

            singleton.StartCoroutine(_spin());
        }

        private static IEnumerator _spin()
        {
            isSpinning = true;

            // place bet
            Money.updateBalance(-Money.bet);
            Money.pushToUI();

            // request result from server
            spinResult = null;
            singleton.server.requestResult(singleton, Money.bet);


            // start spinning the reels
            Reel[] reels = singleton.reels;
            int[] stagger = ReelsData.stagger;
            for (int i = 0; i < reels.Length; i++)
            {
                Reel reel = reels[i];
                reel.StartCoroutine(reel.spin(spinDelay, stagger[i], singleton.spinSpeed * Symbols.size.y));
            }


            // wait for result from server
            while (spinResult == null)
            {
                yield return null;
            }
            Money.updateBalance(spinResult.winnings);


            // wait for spinning to stop
            bool spinning = true;
            while (spinning)
            {
                spinning = false;
                foreach (Reel reel in singleton.reels)
                {
                    if (reel.isSpinning)
                    {
                        spinning = true;
                        yield return null;
                        break;
                    }
                }
            }


            // when all spinning is done
            Money.pushToUI();

            isSpinning = false;
        }


        public void build()
        {
            foreach (Reel reel in reels)
                reel.build();
            server = transform.parent.Find("Server").GetComponent<IServer>();
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


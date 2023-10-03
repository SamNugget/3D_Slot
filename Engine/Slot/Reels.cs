using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Server;

namespace Slot
{
    public class Reels : BuildableSingleton, ISlotClient
    {
        private static Reels singleton;
        protected override Buildable _singleton
        {
            set
            {
                if (singleton != null)
                {
                    Debug.LogError("Multiple Reels singletons");
                }
                singleton = (Reels)value;
            }
        }

        private IServer server;

        [SerializeField] private Reel[] reels;
        [SerializeField] private float _spinDelay;
        public static float spinDelay { get { return singleton._spinDelay; } }
        [SerializeField] private float spinSpeed;

        private static bool isSpinning = false;
        private static bool isBlocked = false;
        public static Result spinResult;


        public void recieveResult(Result result)
        {
            spinResult = result;
        }

        public static bool spin()
        {
            if (isSpinning || isBlocked || !Money.sufficientFunds())
                return false;

            singleton.StartCoroutine(_spin());
            return true;
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
            int cumStagger = 0;
            float speed = singleton.spinSpeed * Symbols.size.y;
            for (int i = 0; i < reels.Length; i++)
            {
                Reel reel = reels[i];
                cumStagger += stagger[i];
                reel.StartCoroutine(reel.spin(spinDelay, cumStagger, speed));
            }


            // wait for result from server
            while (spinResult == null)
            {
                yield return null;
            }
            Money.updateBalance(spinResult.winnings);


            // wait for spinning to stop
            yield return _waitForReels();


            yield return Win.play();


            // when all spinning is done
            Money.pushToUI();

            isSpinning = false;
        }

        private static IEnumerator _waitForReels()
        {
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
        }


        public override void build()
        {
            server = transform.parent.Find("Server").GetComponent<IServer>();
        }


        private void Start()
        {
            // objects to be configured, this alternative to the
            // Unity Start() function imposes an order
            List<Buildable> toBuild = new List<Buildable>()
            {
                Symbols.singleton,
                ReelsData.singleton,
                Money.singleton,
                Reels.singleton
            };
            toBuild.AddRange(reels);

            foreach (Buildable b in toBuild)
            {
                if (b == null)
                    Debug.LogError("Trying to build a null object, make sure singletons are set in Awake().");
                else
                    b.build();
            }


            Transform root = transform.parent;
            Buildable[] buildables = root.GetComponentsInChildren<Buildable>();
            int remaining = buildables.Length - toBuild.Count;
            if (remaining != 0)
                Debug.LogWarning(remaining + " buildable(s) is/are not being built.");
        }
    }
}


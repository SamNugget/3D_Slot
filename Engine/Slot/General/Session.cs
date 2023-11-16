using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Server;

namespace Slot
{
    public class Session : BuildableSingleton, ISlotClient
    {
        private static Session singleton;
        protected override Buildable _singleton
        {
            set
            {
                if (singleton != null)
                {
                    Debug.LogError("Multiple Session singletons");
                }
                singleton = (Session)value;
            }
        }

        private IServer server;

        public static bool isBlocked = true;
        public static Result spinResult;


        public void recieveResult(Result result)
        {
            spinResult = result;
        }

        public static bool spin()
        {
            string err;

            if (isBlocked)
            {
                err = "Spins are blocked.";
            }
            else if (Reels.isSpinning)
            {
                err = "Reels are spinning.";
            }
            else if (!Money.sufficientFunds())
            {
                err = "Insufficient funds.";
            }
            else
            {
                isBlocked = true;
                singleton.StartCoroutine(_spin());
                return true;
            }

            Debug.Log(err);
            return false;
        }

        private static IEnumerator _spin()
        {
            // place bet
            Money.updateBalance(-Money.bet);
            Money.pushToUI();

            // request result from server
            spinResult = null;
            singleton.server.requestResult(singleton, Money.bet);

            // start spinning the reels
            Reels.startSpin();

            // wait for server
            yield return _waitForResult();

            // update (back-end) balance
            Money.updateBalance(spinResult.winnings);

            // wait for spinning to stop
            yield return Reels.waitForLand();

            // play win animation, if applicable
            yield return Win.play();

            // update user's balance
            Money.pushToUI();

            isBlocked = false;
        }

        private static IEnumerator _waitForResult()
        {
            while (spinResult == null)
            {
                yield return null;
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
                Session.singleton,
                Money.singleton,
                Reels.singleton
            };

            foreach (Buildable b in toBuild)
            {
                if (b == null)
                    Debug.LogError("Trying to build a null object, make sure singletons are set in Awake().");
                else
                    b.build();
            }


            Transform root = transform.parent;
            Buildable[] buildables = root.GetComponentsInChildren<BuildableSingleton>();
            int remaining = buildables.Length - toBuild.Count;
            if (remaining != 0)
                Debug.LogWarning(remaining + " buildable(s) is/are not being built.");
        }
    }
}


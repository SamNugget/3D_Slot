using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI;

namespace Slot
{
    public class Money : MonoBehaviour, Buildable
    {
        public static Money singleton;


        [SerializeField] private string _currency = "fun";
        public static string currency { get { return singleton._currency; } }
        [SerializeField] private float balance = 1000f;
        [SerializeField] private float bet = 1f;


        public static bool sufficientFunds()
        {
            if (singleton.balance > singleton.bet)
            {
                singleton.balance -= singleton.bet;
                return true;
            }
            return false;
        }


        public void build()
        {
            UIManager.setBalance(balance);
            UIManager.setBet(bet);
        }


        private void Awake()
        {
            if (singleton != null)
            {
                Debug.LogError("Multiple Money singletons");
            }
            singleton = this;
        }
    }
}


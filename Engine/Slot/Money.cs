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
        [SerializeField] private float _balance = 1000f;
        [SerializeField] private float _bet = 1f;
        public static float bet { get { return singleton._bet; } }


        public static bool sufficientFunds()
        {
            return (singleton._balance > singleton._bet);
        }

        public static void updateBalance(float change)
        {
            singleton._balance += change;

            // push via banking API
        }

        public static void pushToUI()
        {
            UIManager.setBalance(singleton._balance);
            UIManager.setBet(singleton._bet);
        }


        public void build()
        {
            UIManager.setBalance(_balance);
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


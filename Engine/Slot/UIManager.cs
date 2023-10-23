using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Slot;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager singleton;

        [SerializeField] private TextMeshProUGUI balance;
        [SerializeField] private TextMeshProUGUI bet;


        public static void setBalance(float balance)
        {
            singleton.balance.text = "Balance: " + balance + ' ' + Money.currency;
        }

        public static void setBet(float bet)
        {
            singleton.bet.text = "Total Bet: " + bet + ' ' + Money.currency;
        }

        public void spinPressed()
        {
            Session.spin();
        }


        private void Awake()
        {
            if (singleton != null)
            {
                Debug.LogError("Multiple UIManager singletons");
            }
            singleton = this;
        }
    }
}


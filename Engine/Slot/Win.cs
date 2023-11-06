using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Server;

namespace Slot
{
    public class Win : BuildableSingleton
    {
        public static Win singleton;
        protected override Buildable _singleton
        {
            set
            {
                if (singleton != null)
                {
                    Debug.LogError("Multiple Win singletons");
                }
                singleton = (Win)value;
            }
        }

        [SerializeField] private float highlightTime;
        [SerializeField] private WinStage[] winStages;


        public static IEnumerator play()
        {
            Result spinResult = Session.spinResult;
            float winnings = spinResult.winnings;


            if (winnings == 0)
            {
                yield break;
            }


            // highlight win symbols
            Reels.highlightSymbols(spinResult.lineWins);
            yield return new WaitForSeconds(singleton.highlightTime);


            WinStage winStage = singleton._getStage(winnings);
            // play anim for win
            yield return winStage.animate();


            // reset win symbols
            Reels.highlightSymbols();
        }

        private WinStage _getStage(float winnings)
        {
            WinStage last = winStages[0];
            for (int i = 1; i < winStages.Length; i++)
            {
                if (winnings < winStages[i].minWinnings)
                    return last;
                last = winStages[i];
            }
            return last;
        }


        public override void build()
        {
        
        }
    }
}


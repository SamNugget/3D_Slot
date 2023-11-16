using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Server;
using UnityEngine.UI;

namespace Slot
{
    public class Reels : BuildableSingleton
    {
        public static Reels singleton;
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

        [SerializeField] private Reel[] reels;
        [SerializeField] private float _spinDelay;
        public static float spinDelay { get { return singleton._spinDelay; } }
        [SerializeField] private float spinSpeed;
        [SerializeField] private Color dimmedColour;

        public static bool isSpinning { get { return _isSpinning; } }
        private static bool _isSpinning = false;


        public static void startSpin()
        {
            _isSpinning = true;

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
        }

        public static IEnumerator waitForLand()
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

            _isSpinning = false;
        }

        public void setSymbolVisibility(bool visible)
        {
            foreach (Reel r in reels)
            {
                r.setSymbolVisibility(visible);
            }
        }

        public static void highlightSymbols(LineWin[] lineWins = null)
        {
            Reel[] reels = singleton.reels;
            Color baseColour = lineWins == null ? Color.white : singleton.dimmedColour;

            for (int x = 0; x < reels.Length; x++)
            {
                Reel reel = reels[x];

                for (int y = 0; y < reel.height; y++)
                {
                    reel.symbols[y].image.color = baseColour;
                }
            }

            if (lineWins != null)
            {
                for (int i = 0; i < lineWins.Length; i++)
                {
                    int[][] symbolPositions = lineWins[i].symbolPositions;

                    for (int j = 0; j < symbolPositions.Length; j++)
                    {
                        int[] pos = symbolPositions[j];
                        reels[pos[0]].symbols[pos[1]].image.color = Color.white;
                    }
                }
            }
        }


        public override void build()
        {
            StartCoroutine(_build());
        }

        private IEnumerator _build()
        {
            foreach (Reel r in reels)
                r.build();

            //setSymbolVisibility(true);
            Session.isBlocked = false;
            yield break;
        }
    }
}


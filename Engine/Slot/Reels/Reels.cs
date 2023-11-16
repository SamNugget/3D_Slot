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

        public enum Direction : int
        {
            LEFT = 0,
            RIGHT = 1,
            UP = 2,
            DOWN = 3
        }

        [SerializeField] private Reel[] reels;
        [SerializeField] private float _spinDelay;
        public Direction direction = Direction.DOWN;
        public static Vector2 directionVector
        {
            get
            {
                Vector2 size = Symbols.size;
                switch (singleton.direction)
                {
                    case Direction.LEFT: return new Vector2(-1 * size.x, 0);
                    case Direction.RIGHT: return new Vector2(1 * size.x, 0);
                    case Direction.UP: return new Vector2(0, 1 * size.y);
                    default: return new Vector2(0, -1 * size.y);
                }
            }
        }
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
            for (int i = 0; i < reels.Length; i++)
            {
                Reel reel = reels[i];
                cumStagger += stagger[i];
                reel.StartCoroutine(reel.spin(spinDelay, cumStagger, singleton.spinSpeed));
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

        public static void setSymbolVisibility(bool visible)
        {
            foreach (Reel r in singleton.reels)
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
                    Image image = reel.symbols[y].GetComponent<Image>();
                    image.color = baseColour;
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
                        Image image = reels[pos[0]].symbols[pos[1]].GetComponent<Image>();
                        image.color = Color.white;
                    }
                }
            }
        }


        public override void build()
        {
            foreach (Reel r in reels)
                r.build();
            Session.isBlocked = false;
        }
    }
}


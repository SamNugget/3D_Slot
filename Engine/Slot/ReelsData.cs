using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Slot
{
    public class ReelsData : BuildableSingleton
    {
        public static ReelsData singleton;
        protected override Buildable _singleton
        {
            set
            {
                if (singleton != null)
                {
                    Debug.LogError("Multiple ReelsData singletons");
                }
                singleton = (ReelsData)value;
            }
        }


        [SerializeField] private string[] _strips;
        public static List<SymbolType[]> strips;

        [SerializeField] private int[] _stagger;
        public static int[] stagger { get { return singleton._stagger; } }


        public static SymbolType[] getSection(int stripIndex, int reelPosition, int height)
        {
            SymbolType[] strip = strips[stripIndex];
            SymbolType[] section = new SymbolType[height];

            for (int i = 0; i < height; i++)
            {
                int reelIndex = reelPosition + i;
                if (reelIndex >= strip.Length)
                {
                    reelIndex -= strip.Length;
                }

                section[i] = strip[reelIndex];
            }

            return section;
        }


        public override void build()
        {
            strips = new List<SymbolType[]>();

            for (int x = 0; x < _strips.Length; x++)
            {
                string s = _strips[x];
                string[] symbolIDs = s.Split(", ");
                SymbolType[] symbols = new SymbolType[symbolIDs.Length];

                for (int i = 0; i < symbolIDs.Length; i++)
                {
                    string symbolID = symbolIDs[i];
                    symbols[i] = Symbols.getSymbolType(symbolID);
                }

                strips.Add(symbols);
            }
        }


        private void Awake()
        {
            if (singleton != null)
            {
                Debug.LogError("Multiple ReelsData singletons");
            }
            singleton = this;
        }
    }
}

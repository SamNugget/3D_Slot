using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Slot
{
    public class ReelsData : MonoBehaviour, Buildable
    {
        public static ReelsData singleton;


        [SerializeField] private string[] _strips;
        public static List<SymbolType[]> strips;


        public void build()
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

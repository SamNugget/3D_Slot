using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Slot
{
    public class Symbols : MonoBehaviour, Buildable
    {
        public static Symbols singleton;

        [SerializeField] private Vector2 _size;
        public static Vector2 size { get { return singleton._size; } }

        [SerializeField] private GameObject _fab;
        public static GameObject fab { get { return singleton._fab; } }

        [SerializeField] private SymbolType[] _symbolTypes;
        private static Dictionary<string, SymbolType> symbolTypes;


        public static SymbolType getSymbolType(string iD)
        {
            if (!symbolTypes.ContainsKey(iD))
            {
                Debug.LogError("No SymbolType with ID " + iD);
                return singleton._symbolTypes[0];
            }

            return symbolTypes[iD];
        }

        public static SymbolType[] getSymbolTypes()
        {
            return singleton._symbolTypes;
        }

        public static SymbolType getWeightedRandom()
        {
            // todo: weighted according to totals in strip
            int i = Random.Range(0, singleton._symbolTypes.Length);
            return singleton._symbolTypes[i];
        }


        public void build()
        {
            symbolTypes = new Dictionary<string, SymbolType>();

            foreach (SymbolType symbolType in _symbolTypes)
            {
                symbolTypes.Add(symbolType.iD, symbolType);
            }
        }


        private void Awake()
        {
            if (singleton != null)
            {
                Debug.LogError("Multiple Symbols singletons");
            }
            singleton = this;
        }
    }
}


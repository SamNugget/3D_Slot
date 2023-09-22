using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Slot
{
    public class Reel : MonoBehaviour, Buildable
    {
        [SerializeField] private int height;
        [SerializeField] private int strip;

        [HideInInspector] public Symbol[] symbols;


        public void build()
        {
            Vector2 size = Symbols.size;

            symbols = new Symbol[height + 2];
            for (int i = 0; i < symbols.Length; i++)
            {
                symbols[i] = createSymbol(Symbols.getRandom());

                Vector3 pos = new Vector3(0f, (i - 1) * -size.y, 0f);
                symbols[i].transform.localPosition = pos;
            }
        }

        private Symbol createSymbol(SymbolType type)
        {
            GameObject g = Instantiate(Symbols.fab, transform);
            Symbol s = g.GetComponent<Symbol>();
            s.type = type;
            Image i = g.GetComponent<Image>();
            i.sprite = type.sprite;
            return s;
        }
    }
}


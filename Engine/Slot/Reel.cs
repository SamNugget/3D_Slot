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

        public bool isSpinning = false;


        public void build()
        {
            Vector2 size = Symbols.size;

            symbols = new Symbol[height + 2];
            for (int i = 0; i < symbols.Length; i++)
            {
                symbols[i] = createSymbol(Symbols.getWeightedRandom());

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

        // todo: probably won't work after extending Symbol, need to replace component, bad at runtime
        private void resetSymbol(Symbol symbol, SymbolType newType)
        {
            symbol.type = newType;

            Image i = symbol.GetComponent<Image>();
            i.sprite = newType.sprite;

            symbol.transform.localPosition += new Vector3(0f, (height + 2) * Symbols.size.y, 0f);
        }

        public IEnumerator spin(float delay, int stagger, float speed)
        {
            isSpinning = true;


            int toDisplay = height + 1; // includes the one above
            float yMin = toDisplay * -Symbols.size.y;

            while (isSpinning)
            {
                float change = speed * Time.deltaTime;

                for (int i = 0; i < symbols.Length; i++)
                {
                    // move each symbol
                    Symbol symbol = symbols[i];
                    symbol.transform.localPosition -= new Vector3(0f, change, 0f);

                    // check if it has gone too far
                    if (symbol.transform.localPosition.y < yMin)
                    {
                        if (delay > 0f || Reels.spinResult == null)
                        {
                            resetSymbol(symbol, Symbols.getWeightedRandom());
                        }
                        else if (stagger > 0)
                        {
                            resetSymbol(symbol, Symbols.getWeightedRandom());
                            stagger--;
                        }
                        else
                        {
                            int reelPosition = Reels.spinResult.reelPositions[strip];
                            SymbolType[] stripSection = ReelsData.getSection(strip, reelPosition, toDisplay);

                            if (toDisplay > 0)
                            {
                                resetSymbol(symbol, stripSection[toDisplay - 1]);
                                toDisplay--;
                            }
                            else
                            {
                                isSpinning = false;
                            }
                        }
                    }
                }


                // todo: centre the positions of all the symbols


                yield return null;
                delay -= Time.deltaTime;
            }
        }
    }
}


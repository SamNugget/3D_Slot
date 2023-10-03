using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Slot
{
    public class Reel : Buildable
    {
        [SerializeField] private int height;
        [SerializeField] private int strip;

        [HideInInspector] public List<Symbol> symbols;
        private Symbol bottom { get { return symbols[symbols.Count - 1]; } }
        List<Symbol> toRemove = new List<Symbol>();

        public bool isSpinning = false;


        public override void build()
        {
            Vector2 size = Symbols.size;

            symbols = new List<Symbol>();
            for (int i = 0; i < height + 2; i++)
            {
                Symbol s = createSymbol(Symbols.getWeightedRandom());
                symbols.Insert(i, s);

                Vector3 pos = new Vector3(0f, (i - 1) * -size.y, 0f);
                s.transform.localPosition = pos;
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

            symbol.transform.localPosition += new Vector3(0f, (height + 2) * Symbols.size.y);

            toRemove.Add(symbol);
        }

        public IEnumerator spin(float delay, int stagger, float speed)
        {
            isSpinning = true;


            int toDisplay = height + 1; // includes the one above
            float yMin = toDisplay * -Symbols.size.y;

            while (isSpinning)
            {
                float change = speed * Time.deltaTime;

                for (int i = 0; i < symbols.Count; i++)
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


                foreach (Symbol r in toRemove)
                {
                    symbols.Remove(r);
                    symbols.Insert(0, r);
                }


                yield return null;
                delay -= Time.deltaTime;
            }


            yield return _snap();
        }

        private IEnumerator _snap()
        {
            float offset = symbols[0].transform.localPosition.y;
            foreach (Symbol s in symbols)
            {
                s.transform.localPosition -= new Vector3(0, offset);
            }

            // todo: gradual return

            yield return null;
        }
    }
}


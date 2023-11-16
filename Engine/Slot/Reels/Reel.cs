using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Slot
{
    public class Reel : Buildable
    {
        public int height;
        [SerializeField] private int strip;

        [HideInInspector] public List<Symbol> symbols;
        private Symbol bottom { get { return symbols[symbols.Count - 1]; } }
        List<Symbol> toRemove = new List<Symbol>();

        public bool isSpinning = false;


        public override void build()
        {
            symbols = new List<Symbol>();
            for (int i = 0; i < height + 2; i++)
            {
                Symbol s = createSymbol(Symbols.getWeightedRandom());
                symbols.Insert(i, s);

                Vector3 pos = Reels.directionVector * (-1.5f + i);
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

        public void setSymbolVisibility(bool visible)
        {
            foreach (Symbol s in symbols)
            {
                s.visible = visible;
            }
        }

        // todo: probably won't work after extending Symbol, need to replace component, bad at runtime
        private void resetSymbol(Symbol symbol, SymbolType newType)
        {
            symbol.reset(newType, height);
            toRemove.Add(symbol);
        }

        public IEnumerator spin(float delay, int stagger, float speed)
        {
            isSpinning = true;


            Vector2 dirVector = Reels.directionVector;
            bool positive = dirVector.x > 0 || dirVector.y > 0;

            int displayed = (height + 1);
            Vector2 limit = (displayed - 0.5f) * dirVector;

            while (isSpinning)
            {
                float change = speed * Time.deltaTime;
                Vector2 changeVector = (Vector3)dirVector * change;

                for (int i = 0; i < symbols.Count; i++)
                {
                    // move each symbol
                    Symbol symbol = symbols[i];
                    symbol.transform.localPosition += (Vector3)changeVector;

                    // check if it has gone too far
                    if (exceedsLimit(symbol, limit, positive))
                    {
                        if (delay > 0f || Session.spinResult == null)
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
                            int reelPosition = Session.spinResult.reelPositions[strip];
                            SymbolType[] stripSection = ReelsData.getSectionAsTypes(strip, reelPosition, displayed);

                            if (displayed > 0)
                            {
                                resetSymbol(symbol, stripSection[displayed - 1]);
                                displayed--;
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


            Reels.Direction direction = Reels.singleton.direction;
            bool vertical = direction == Reels.Direction.UP || direction == Reels.Direction.DOWN;

            yield return _snap(vertical);
        }

        private static bool exceedsLimit(Symbol symbol, Vector2 limit, bool positive)
        {
            Vector2 position = symbol.transform.localPosition;
            return positive ? (position.x > limit.x || position.y > limit.y) :
                (position.x < -limit.x || position.y < -limit.y);
        }

        private IEnumerator _snap(bool vertical)
        {
            Vector2 dirVector = Reels.directionVector;

            //Vector2 position = symbols[0].transform.localPosition;
            //Vector2 offset = vertical ? new Vector2(0, -position.y) : new Vector2(-position.x, 0);

            for (int i = 0; i < height + 2; i++)
            {
                Vector3 pos = dirVector * (0.5f + i);
                symbols[i].transform.localPosition = pos;
            }

            // todo: gradual return

            yield return null;
        }
    }
}


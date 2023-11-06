using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Slot; // would not be in real server
using Server;

namespace MockServer
{
    public class Server : MonoBehaviour, IServer
    {
        // todo: interface to be implemented in connection class
        public void requestResult(ISlotClient client, float bet)
        {
            List<SymbolType[]> strips = ReelsData.strips;
            int columns = strips.Count;


            // generate reel positions
            int[] reelPositions = new int[columns];
            string[][] symbols = new string[columns][];
            for (int i = 0; i < strips.Count; i++)
            {
                int reelPos = UnityEngine.Random.Range(0, strips[i].Length);
                reelPositions[i] = reelPos;
                symbols[i] = ReelsData.getSection(i, reelPos, Model.rows);
            }


            // pattern match to get winnings
            LineWin[] lineWins = _findLineWins(symbols);


            Result result = new Result() {
                reelPositions = reelPositions,
                winnings = _calculateWinnings(lineWins),
                symbols = symbols,
                lineWins = lineWins
            };
            client.recieveResult(result);
        }

        private LineWin[] _findLineWins(string[][] symbols)
        {
            int[][,] patterns = Patterns.patterns;
            SymbolType[] symbolTypes = Symbols.getSymbolTypes();

            List<LineWin> lineWins = new List<LineWin>();
            List<int[]> positions = new List<int[]>(); // reused
            for (int i = 0; i < symbolTypes.Length; i++)
            {
                string typeID = symbolTypes[i].iD;

                for (int patternIndex = 0; patternIndex < patterns.Length; patternIndex++)
                {
                    int[,] pattern = patterns[patternIndex];
                    Debug.Log("pattern:" + patternIndex + " lenx:" + pattern.GetLength(1) + " leny:" + pattern.GetLength(0));

                    for (int x = 0; x < pattern.GetLength(1); x++)
                    {
                        for (int y = 0; y < pattern.GetLength(0); y++)
                        {
                            Debug.Log("pattern:" + patternIndex + " x:" + x + " y:" + y);
                            if (pattern[y, x] == 1)
                            {
                                if (symbols[x][y] != typeID)
                                {
                                    x = pattern.GetLength(1);
                                    break;
                                }
                                positions.Add(new int[] { x, y });
                            }
                        }
                    }

                    if (positions.Count >= Model.adjacent)
                    {
                        LineWin lineWin = new LineWin() {
                            winAmount = positions.Count * symbolTypes[i].value,
                            symbolPositions = positions.ToArray()
                        };
                        lineWins.Add(lineWin);
                    }

                    positions.Clear();
                }
            }

            return lineWins.ToArray();
        }

        private float _calculateWinnings(LineWin[] lineWins, float multiplier = 1f)
        {
            float total = 0;
            for (int i = 0; i < lineWins.Length; i++)
            {
                total += lineWins[i].winAmount;
            }
            return total * multiplier;
        }

        // todo: forced results
    }
}


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
            int rows = Model.rows;


            // generate reel positions
            int[] reelPositions = new int[columns];
            string[][] symbols = new string[columns][];
            for (int i = 0; i < strips.Count; i++)
            {
                int reelPos = UnityEngine.Random.Range(0, strips[i].Length);
                reelPositions[i] = reelPos;
                symbols[i] = ReelsData.getSection(i, reelPos, rows);
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
            int columns = symbols.Length;
            int rows = Model.rows;

            int[][,] patterns = Patterns.getPatterns(columns, rows);
            SymbolType[] symbolTypes = Symbols.getSymbolTypes();

            List<LineWin> lineWins = new List<LineWin>();
            List<int[]> positions = new List<int[]>(); // reused
            for (int i = 0; i < symbolTypes.Length; i++)
            {
                string typeID = symbolTypes[i].iD;

                for (int patternIndex = 0; patternIndex < patterns.Length; patternIndex++)
                {
                    int[,] pattern = patterns[patternIndex];

                    for (int x = 0; x < columns; x++)
                    {
                        for (int y = 0; y < rows; y++)
                        {
                            if (pattern[x, y] == 1)
                            {
                                if (symbols[x][y] != typeID)
                                {
                                    x = columns;
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


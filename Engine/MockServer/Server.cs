using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Slot; // would not be in real server

namespace Server
{
    public class Server : MonoBehaviour
    {
        public static Result getResult()
        {
            List<SymbolType[]> strips = ReelsData.strips;
            int[] reelPositions = new int[strips.Count];

            for (int i = 0; i < strips.Count; i++)
            {
                reelPositions[i] = Random.Range(0, strips[i].Length);
            }

            return new Result();
        }

        // todo: forced results
    }
}


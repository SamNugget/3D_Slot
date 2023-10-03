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


            // generate reel positions
            int[] reelPositions = new int[strips.Count];

            for (int i = 0; i < strips.Count; i++)
            {
                reelPositions[i] = Random.Range(0, strips[i].Length);
            }


            // pattern match to get winnings
            SymbolType[] symbols = Symbols.getSymbolTypes();
            for (int i = 0; i < symbols.Length; i++)
            {

            }


            client.recieveResult(new Result() { reelPositions = reelPositions, winnings = 1f });
        }



        // todo: forced results
    }
}


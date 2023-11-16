using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Slot
{
    public class Symbol : MonoBehaviour
    {
        [HideInInspector] public SymbolType type;

        public void reset(SymbolType newType, float reelHeight)
        {
            type = newType;

            Image i = GetComponent<Image>();
            i.enabled = true;
            i.sprite = newType.sprite;

            transform.localPosition -= (Vector3)Reels.directionVector * (reelHeight + 2);
        }

        public bool visible
        {
            get
            {
                return GetComponent<Image>().enabled;
            }
            set
            {
                GetComponent<Image>().enabled = value;
            }
        }
    }
}


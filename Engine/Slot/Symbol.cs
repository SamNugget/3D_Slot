using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Slot
{
    public class Symbol : MonoBehaviour
    {
        [HideInInspector] public SymbolType type;
        [HideInInspector] public Image image;

        public void reset(SymbolType newType, float reelHeight)
        {
            type = newType;

            image.enabled = true;
            image.sprite = newType.sprite;

            transform.localPosition += new Vector3(0f, (reelHeight + 2) * Symbols.size.y);
        }

        public bool visible
        {
            get
            {
                return image.enabled;
            }
            set
            {
                image.enabled = value;
            }
        }

        private void Awake()
        {
            image = transform.GetChild(1).GetComponent<Image>();
        }
    }
}


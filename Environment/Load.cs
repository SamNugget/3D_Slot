using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Environment
{
    public class Load : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI screenText;
        [SerializeField] private float spinDelay;

        private void OnEnable()
        {
            StartCoroutine(spin());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private IEnumerator spin()
        {
            bool horiz = true;
            string text = screenText.text;
            text = text.Substring(0, text.Length - 2);
            while (true)
            {
                screenText.text = text + (horiz ? '-' : '|');

                horiz = !horiz;
                yield return new WaitForSeconds(spinDelay);
            }
        }
    }
}


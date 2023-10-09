using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Slot
{
    [Serializable]
    public class WinStage
    {
        public string name;
        public float minWinnings;
        public float duration;
        public Anim[] animations;

        [Serializable]
        public struct Anim
        {
            public string name;
            public Animator animator;
            public float startDelay;
        }

        public IEnumerator animate()
        {
            float duration = this.duration;

            foreach(Anim anim in animations)
            {
                if (anim.startDelay > 0f)
                {
                    float startDelay = anim.startDelay;

                    duration -= startDelay;
                    yield return new WaitForSeconds(startDelay);
                }

                Animator am = anim.animator;
                am.Play(anim.name);
                AnimatorStateInfo info = am.GetCurrentAnimatorStateInfo(0);
            }

            yield return new WaitForSeconds(duration);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace platformer.utils
{

    public static class Utils
    {
        public static bool IsPlaying(this Animator animator)
        {
            return animator.GetCurrentAnimatorStateInfo(0).length>
                   animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }
    }
}
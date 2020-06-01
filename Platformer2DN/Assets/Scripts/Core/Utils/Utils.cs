using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace platformer.utils
{
    //Class handling all utility functions
    public static class Utils
    {
        //Enxtension method for animator
        //Return true if animator is playing animation
        public static bool IsPlaying(this Animator animator)
        {
            return animator.GetCurrentAnimatorStateInfo(0).length>
                   animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }
    }
}
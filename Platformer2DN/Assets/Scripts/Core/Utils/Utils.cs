using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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

        public static T DeepCopy<T>(this T other)
        {
            using(MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Context=new StreamingContext(StreamingContextStates.Clone);
                formatter.Serialize(ms, other);
                ms.Position=0;
                return (T) formatter.Deserialize(ms);
            }
        }
    }
}
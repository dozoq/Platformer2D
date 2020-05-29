using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace platformer.saving
{
    public interface ISaveable
    {
        object CaptureState();
        void RestoreState(object state);
    }

}
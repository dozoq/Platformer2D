using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace platformer.saving
{
    /// <summary>
    /// Each gameobject that want to save his state needs to implement ISaveable.
    /// Saving will look for each ISaveable component in the scene and save his state.
    /// </summary>
    public interface ISaveable
    {
        // Return an object(could be everything, as object is most generic type, but must be serializable)
        object CaptureState();

        // This function will return exactly the same object(type) as captured in CaptureState.
        // Passed state need to be casted into type of object stored in CaptureState();
        void RestoreState(object state);
    }

}
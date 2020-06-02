using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace platformer.saving
{
    /// <summary>
    /// Vector3 used in saving as default vector3 is not serializable
    /// </summary>
    
    [System.Serializable]
    public class SerializableVector3
    {
        private float x;
        private float y;
        private float z;

        // Constructor
        public SerializableVector3(Vector3 vector)
        {
            this.x = vector.x;
            this.y = vector.y;
            this.z = vector.z;
        }

        // Return new vector3 with currently saved floats in this instance
        public Vector3 DeserializeToVector3()
        {
            return new Vector3(x, y, z);
        }
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace System
{
    public class EntityException : Exception
    {
        public EntityException(GameObject obj) : base(obj == null ? "null" : obj.name)
        {
        }
    }
}

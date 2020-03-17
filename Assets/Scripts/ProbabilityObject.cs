using System;
using UnityEngine;

[Serializable]
public class ProbabilityObject
{
    public EntityCreatorObject currentObject;
    [Min(1)]
    public int probability;

    public bool TryGetObject(ref int randomValue, out EntityCreatorObject resultObject)
    {
        randomValue -= probability;
        if (randomValue < 0)
        {
            resultObject = currentObject;
            return true;
        }
        else
        {
            resultObject = null;
            return false;
        }
    }
}

using Behaviours;
using UnityEngine;

public class BTAnimatorParamAttribute : PropertyAttribute
{
    public ValueType type;

    public BTAnimatorParamAttribute(ValueType type)
    {
        this.type = type;
    }
}

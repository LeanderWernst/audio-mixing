using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This is a struct for storing the relation between
 * the position of a moving game object and its 
 * corresponding value.
 * The first float in the positions array needs to 
 * be the upper position, the second the lower.
 * The first float in the values array needs to be 
 * the corresponding value to positions[0], the second
 * the corresponding value to positions[1]
 */
public struct PositionValueRelation
{
    public float[] positions;
    public float[] values;

    public PositionValueRelation(float[] positions, float[] values)
    {
        this.positions = positions;
        this.values = values;
    }
}
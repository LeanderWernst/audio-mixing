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

/**
 * <summary>
 * This static class stores the relation between 
 * the game object positions and its corresponding values 
 * of a Fader object in its static variable <c>FaderPvr.relation</c>.
 * </summary>
 */
public static class FaderPvr
{
    public static readonly PositionValueRelation[] relation = new PositionValueRelation[]
        {
            new PositionValueRelation(new float[]{0.0236f, 0.02932f}, new float[]{10f, 5f}),
            new PositionValueRelation(new float[]{0.02932f, 0.0351f}, new float[]{5f, 0f}),
            new PositionValueRelation(new float[]{0.0351f, 0.04132f}, new float[]{0f, -5f}),
            new PositionValueRelation(new float[]{0.04132f, 0.04752f}, new float[]{-5f, -10f}),
            new PositionValueRelation(new float[]{0.04752f, 0.05374f}, new float[]{-10f, -15f}),
            new PositionValueRelation(new float[]{0.05374f, 0.05819f}, new float[]{-15f, -20f}),
            new PositionValueRelation(new float[]{0.05819f, 0.06265f}, new float[]{-20f, -30f}),
            new PositionValueRelation(new float[]{0.06265f, 0.06527f}, new float[]{-30f, -40f}),
            new PositionValueRelation(new float[]{0.06527f, 0.06782f}, new float[]{-40f, -80f})
        };
}

/**
 * <summary>
 * This static class stores the relation between 
 * the game object positions and its corresponding values 
 * of a Knob object. Access values through its static method
 * <c>FaderPvr.Relation(KnobType kt)</c>.
 * </summary>
 */
public static class KnobPvr
{
    public static PositionValueRelation[] Relation(KnobType kt)
    {
        switch (kt)
        {
            case KnobType.MicGain:
                return new PositionValueRelation[]
                {
                    new PositionValueRelation(new float[]{-105f, 6.797f}, new float[]{10f, 20f}),
                    new PositionValueRelation(new float[]{6.797f, 80.387f}, new float[]{20f, 35f}),
                    new PositionValueRelation(new float[]{80.387f, 185f}, new float[]{35f, 60f})
                };
            case KnobType.EqGain:
                return new PositionValueRelation[]
                {
                    new PositionValueRelation(new float[]{-105f, 42.84f}, new float[]{-15f, 0f}),
                    new PositionValueRelation(new float[]{42.84f, 185f}, new float[]{0f, 15f}),
                };
            case KnobType.TrebleFreq:
                return new PositionValueRelation[]
                {
                    new PositionValueRelation(new float[]{-105f, 6.797f}, new float[]{2f, 5f}),
                    new PositionValueRelation(new float[]{6.797f, 80.387f}, new float[]{5f, 10f}),
                    new PositionValueRelation(new float[]{80.387f, 185f}, new float[]{10f, 20f})
                };
            case KnobType.HiMidFreq:
                return new PositionValueRelation[]
                {
                    new PositionValueRelation(new float[]{-105f, 6.797f}, new float[]{0.4f, 1f}),
                    new PositionValueRelation(new float[]{6.797f, 80.387f}, new float[]{1f, 3f}),
                    new PositionValueRelation(new float[]{80.387f, 185f}, new float[]{3f, 8f})
                };
            case KnobType.LoMidFreq:
                return new PositionValueRelation[]
                {
                    new PositionValueRelation(new float[]{-105f, 6.797f}, new float[]{0.1f, 0.3f}),
                    new PositionValueRelation(new float[]{6.797f, 80.387f}, new float[]{0.3f, 0.8f}),
                    new PositionValueRelation(new float[]{80.387f, 185f}, new float[]{0.8f, 2f})
                };
            case KnobType.BassFreq:
                return new PositionValueRelation[]
                {
                    new PositionValueRelation(new float[]{-105f, 6.797f}, new float[]{0.02f, 0.05f}),
                    new PositionValueRelation(new float[]{6.797f, 80.387f}, new float[]{0.05f, 0.1f}),
                    new PositionValueRelation(new float[]{80.387f, 185f}, new float[]{0.1f, 0.2f})
                };
            case KnobType.EqWidth:
                return new PositionValueRelation[]
                {
                    new PositionValueRelation(new float[]{-105f, 6.797f}, new float[]{0.1f, 0.3f}),
                    new PositionValueRelation(new float[]{6.797f, 80.387f}, new float[]{0.3f, 0.6f}),
                    new PositionValueRelation(new float[]{80.387f, 185f}, new float[]{0.6f, 2f})
                };
            case KnobType.DSEqControl:
                return new PositionValueRelation[]
                {
                    new PositionValueRelation(new float[]{-105f, 44.874f}, new float[]{-15f, 0f}),
                    new PositionValueRelation(new float[]{44.874f, 185f}, new float[]{0f, 15f}),
                };
            case KnobType.MinusOnePlusOne:
                return new PositionValueRelation[]
                {
                    new PositionValueRelation(new float[]{-105f, 44.874f}, new float[]{-1f, 0f}),
                    new PositionValueRelation(new float[]{44.874f, 185f}, new float[]{0f, 1f}),
                };
            case KnobType.InfTo6DbControl:
                return new PositionValueRelation[]
                {
                    new PositionValueRelation(new float[]{-105f, 6.797f}, new float[]{-80f, -6f}),
                    new PositionValueRelation(new float[]{6.797f, 81.073f}, new float[]{-6f, 0f}),
                    new PositionValueRelation(new float[]{81.073f, 185f}, new float[]{0f, 6f})
                };
            case KnobType.InfTo10DbControl:
                return new PositionValueRelation[]
                {
                    new PositionValueRelation(new float[]{-105f, 6.797f}, new float[]{-80f, -6f}),
                    new PositionValueRelation(new float[]{6.797f, 81.073f}, new float[]{-6f, 0f}),
                    new PositionValueRelation(new float[]{81.073f, 185f}, new float[]{0f, 10f})
                };
            case KnobType.InfTo20DbControl:
                return new PositionValueRelation[]
                {
                    new PositionValueRelation(new float[]{-105f, 6.797f}, new float[]{-80f, -6f}),
                    new PositionValueRelation(new float[]{6.797f, 44.874f}, new float[]{-6f, 0f}),
                    new PositionValueRelation(new float[]{44.874f, 185f}, new float[]{0f, 20f})
                };
            default:
                return new PositionValueRelation[] { };
        }
    }
}
public enum KnobType
{ 
    MicGain,
    EqGain,
    TrebleFreq,
    HiMidFreq,
    LoMidFreq,
    BassFreq,
    EqWidth,
    DSEqControl,
    MinusOnePlusOne,
    InfTo6DbControl,
    InfTo10DbControl,
    InfTo20DbControl
}
using System;


public abstract class Effect
{
    public string name { get; }

    protected Effect(string name)
    {
        this.name = name;
    }
}

/**
 * TODO: Implement this properly later
 *
 * or throw it away, figure it out
 */
public class SpeedEffect : Effect
{
    /**
     * Flat speed increase / decrease
     *
     * adds 0 by default
     */
    public float flat = 0f;

    /**
     * Speed multiplier
     *
     * multiplies by 1 by default
     */
    public float multiplier = 1f;

    public SpeedEffect(string opts) : base(opts)
    {
    }
}

public class Sneak : SpeedEffect
{
    public Sneak() : base("Sneak")
    {
        multiplier = 0.5f;
    }
}
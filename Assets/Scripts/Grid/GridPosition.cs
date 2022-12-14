
using System;
using UnityEngine;

public struct GridPosition : IEquatable<GridPosition>
{
    public int x;
    public int z;
    public static readonly GridPosition Zero = new GridPosition(0, 0);
    public static readonly GridPosition Empty = new GridPosition(int.MinValue, int.MinValue);
    public GridPosition(int x, int z)
    {
        this.x = x;
        this.z = z;
    }
    public GridPosition(float x, float z)
    {
        this.x = Mathf.RoundToInt(x);
        this.z = Mathf.RoundToInt(z);
    }

    public override string ToString()
    {
        return $"{x},{z}";
    }

    public static bool operator ==(GridPosition a, GridPosition b)
    {
        return a.x == b.x && a.z == b.z;
    }
    public static bool operator !=(GridPosition a, GridPosition b)
    {
        return !(a == b);
    }

    public override bool Equals(object obj)
    {
        return obj is GridPosition position &&
               x == position.x &&
               z == position.z;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, z);
    }

    public bool Equals(GridPosition other)
    {
        return this == other;
    }
    public static GridPosition operator +(GridPosition a, GridPosition b)
    {
        return new GridPosition(a.x + b.x, a.z + b.z);
    }
    public static GridPosition operator -(GridPosition a, GridPosition b)
    {
        return new GridPosition(a.x - b.x, a.z - b.z);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Preset names for all available bitmasks
/// </summary>
public class BitmaskNames
{
    private BitmaskNames(byte value)
    {
        this.value = value;
    }

    public static BitmaskNames Empty { get; }               = new BitmaskNames(0b000000);

    public static BitmaskNames UpLeft { get; }              = AxialDirection.UpLeft;
    public static BitmaskNames UpRight { get; }             = AxialDirection.UpRight;
    public static BitmaskNames Right { get; }               = AxialDirection.Right;
    public static BitmaskNames Left { get; }                = AxialDirection.Left;
    public static BitmaskNames DownRight { get; }           = AxialDirection.DownRight;
    public static BitmaskNames DownLeft { get; }            = AxialDirection.DownLeft;

    public static BitmaskNames AllUp { get; }               = UpRight | UpLeft;
    public static BitmaskNames AllDown { get; }             = DownRight | DownLeft;

    public static BitmaskNames DiagonalUpRight { get; }     = UpRight | Right;
    public static BitmaskNames DiagonalDownRight { get; }   = DownRight | Right;
    public static BitmaskNames DiagonalDownLeft { get; }    = DownLeft | Left;
    public static BitmaskNames DiagonalUpLeft { get; }      = UpLeft | Left;
   
    private byte value;

    public static implicit operator BitmaskNames(AxialDirection direction)
    {
        return new BitmaskNames(direction);
    }
    public static implicit operator BitmaskNames(int value)
    {
        return new BitmaskNames((byte)value);
    }
    public static implicit operator BitmaskNames(byte value)
    {
        return new BitmaskNames(value);
    }
    public static implicit operator byte(BitmaskNames name)
    {
        return name.value;
    }
}
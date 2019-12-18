using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AbilityInput
{
    public Vector2 TargetPosition;
    public bool FireButtonStay;
    public bool ShouldReload;
    public bool FireButtonReleased;

    public override string ToString()
    {
        string objectString = "Ability Input { ";

        if (TargetPosition != default)
            objectString += $" {nameof(TargetPosition)}: " + TargetPosition;

        if (FireButtonStay != default)
            objectString += $" {nameof(FireButtonStay)}: " + FireButtonStay;

        if (FireButtonReleased != default)
            objectString += $" {nameof(FireButtonReleased)}: " + FireButtonReleased;

        if (ShouldReload != default)
            objectString += $" {nameof(ShouldReload)}: " + ShouldReload;

        objectString += " }";
        return objectString;
    }
}
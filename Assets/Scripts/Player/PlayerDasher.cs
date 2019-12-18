using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDasher : Dasher
{
    private const float IFrameMultiplier = 1.5f;
    private const int PlayerCollisionLayer = 8;
    private const int ClutterCollisionLayer = 10;
    private const int EnemyCollisionLayer = 9;

    public override void Dash(Vector2 direction)
    {
        base.Dash(Utility.ScaleToOrthographicVector(direction));
    }
    protected override void Update()
    {
        if (InputSuppressor.IsSuppressed)
            return;

        base.Update();
    }
    protected override void DashingStarted()
    {
        entity.AddModifier(typeof(InvincibleModifier), dashTime.Value * IFrameMultiplier);

        base.DashingStarted();
        
        DisableClutterCollision();
        DisableEnemyCollision();
    }
    protected override void DashingFinished()
    {
        base.DashingFinished();
        
        EnableClutterCollision();
        EnableEnemyCollision();
    }
    private void EnableEnemyCollision()
    {
        ToggleEnemyCollision(true);
    }
    private void DisableEnemyCollision()
    {
        ToggleEnemyCollision(false);
    }
    private void ToggleEnemyCollision(bool value)
    {
        Physics2D.IgnoreLayerCollision(PlayerCollisionLayer, EnemyCollisionLayer, !value);
    }
    private void EnableClutterCollision()
    {
        ToggleClutterCollision(true);
    }
    private void DisableClutterCollision()
    {
        ToggleClutterCollision(false);
    }
    private void ToggleClutterCollision(bool value)
    {
        Physics2D.IgnoreLayerCollision(PlayerCollisionLayer, ClutterCollisionLayer, !value);
    }
}

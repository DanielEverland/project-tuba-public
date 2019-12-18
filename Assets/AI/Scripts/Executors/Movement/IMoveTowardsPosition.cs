using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveTowardsPosition : IAIMover
{
    void MoveTowards(Vector2 target);
    bool IsStuck();
}

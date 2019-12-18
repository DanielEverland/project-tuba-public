using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    #region Properties
    public static readonly Vector3 OrthographicScale = new Vector3(
        1,
        1 / Mathf.Cos(OrthogonalCameraAngle * Mathf.Deg2Rad),
        1 / Mathf.Sin(OrthogonalCameraAngle * Mathf.Deg2Rad));

    public const float HexelWidth = 1;
    public const float HexelCeilingHeight = 1;
    public static readonly float HexelHeightMultiplier = 2 / Mathf.Sqrt(3);
    public static readonly float HexelHeight = HexelWidth * HexelHeightMultiplier * OrthographicScale.y;

    /// <summary>
    /// First we get the ratio between height and width
    /// We then subtract it from 1, one being the full width of the texture,
    ///     this gives us the margin from both sides
    /// We divide by 2 so we have the margin of either side
    /// </summary>
    public static readonly float UVHexelEdgeMultiplier = (1 - (HexelWidth / Utility.HexelHeightMultiplier)) / 2;

    public const float OrthogonalCameraAngle = 60;

    private const int PhysicsBufferSize = 40;
    public static RaycastHit2D[] HitBuffer = new RaycastHit2D[PhysicsBufferSize];
    public static Collider2D[] ColliderBuffer = new Collider2D[PhysicsBufferSize];
    
    public static readonly Plane WorldPlane = new Plane(Vector3.back, Vector3.zero);
    
    public static Vector2 MousePositionInWorld
    {
        get
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            WorldPlane.Raycast(ray, out float distance);

            return ray.origin + ray.direction * distance;
        }
    }

    public const string MenuItemRoomBuilder = MenuItemRoot + "Room Builder/";
    public const string MenuItemDebug = MenuItemRoot + "Debug/";
    public const string MenuItemRoot = "Everland Games/";
    #endregion

    #region Functions
    /// <summary>
    /// Will return all <see cref="Entity"/> within an area
    /// </summary>
    public static List<Entity> GetAllEntitiesWithinRadius(Vector2 center, float radius, int layerMask = Physics2D.DefaultRaycastLayers)
    {
        return GetAllInteractablesWithinRadius(center, radius, layerMask)
            .Where(x => x.Entity != null)
            .Select(x => x.Entity)
            .ToList();
    }
    /// <summary>
    /// Will return all <see cref="Interactable"/> within an area
    /// </summary>
    public static List<Interactable> GetAllInteractablesWithinRadius(Vector2 center, float radius, int layerMask = Physics2D.DefaultRaycastLayers)
    {
        List<Interactable> results = new List<Interactable>();
        int hitCount = Physics2D.OverlapCircleNonAlloc(center, radius, ColliderBuffer, layerMask);

        for (int i = 0; i < hitCount; i++)
        {
            Collider2D collider = ColliderBuffer[i];
            Interactable interactable = collider.GetComponent<Interactable>();

            if (interactable != null)
                results.Add(interactable);
        }

        return results;
    }
    /// <summary>
    /// Will add force from <paramref name="center"/> to all objects within <paramref name="radius"/>
    /// </summary>
    public static void AddForceAll(Vector2 center, float radius, float force, ForceMode2D forceMode = ForceMode2D.Force, ForceFalloffMode falloffMode = ForceFalloffMode.Linear, int layerMask = Physics2D.DefaultRaycastLayers)
    {
        int hitCount = Physics2D.CircleCastNonAlloc(center, radius, Vector2.zero, HitBuffer, 0, layerMask);

        for (int i = 0; i < hitCount; i++)
        {
            RaycastHit2D hit = HitBuffer[i];

            Vector2 delta = (Vector2)hit.transform.position - center;

            if (delta.magnitude == 0)
                continue;

            Interactable interactable = hit.collider.GetComponent<Interactable>();
            EntityController controller = interactable?.Entity?.EntityController;

            float falloff = ProcessFalloff(delta.magnitude, falloffMode);

            if (controller != null)
            {
                controller.AddForce(delta.normalized * force * falloff, forceMode);
            }
            else if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(delta.normalized * force * falloff, forceMode);
            }
        }
    }
    /// <summary>
    /// Calculates force over a <paramref name="distance"/> using <paramref name="falloffMode"/>
    /// </summary>
    public static float ProcessFalloff(float distance, ForceFalloffMode falloffMode)
    {
        switch (falloffMode)
        {
            case ForceFalloffMode.None:
                return 1;
            case ForceFalloffMode.Linear:
                return 1 / distance;
            case ForceFalloffMode.Squared:
                return 1 / Mathf.Sqrt(distance);
            default:
                throw new System.NotImplementedException();
        }
    }
    /// <summary>
    /// Will try to get the farthest position in <paramref name="direction"/> starting from <paramref name="start"/>
    /// </summary>
    /// <returns>World-Space Position</returns>
    public static Vector2 GetFarthestWalkablePositionInDirection(Vector2 startingPosition, Vector2 direction, float maxDistance)
    {
        Vector2 positionOffsetPerStep = ScaleToOrthographicVector(direction);
        Vector2 positionToCheck = startingPosition + positionOffsetPerStep;
        Vector2 farthestWalkablePosition = startingPosition;

        while (true)
        {
            if (OrthographicDistance(startingPosition, positionToCheck) > maxDistance)
                break;

            Axial nearestAxialPosition = WorldToAxialPosition(positionToCheck);

            if (IsAxialPositionWalkable(nearestAxialPosition))
                farthestWalkablePosition = positionToCheck;

            positionToCheck += positionOffsetPerStep;
        }

        return farthestWalkablePosition;
    }
    public static Entity GetEntityFromHierarchyTraversal(GameObject gameObject)
    {
        while (gameObject != null)
        {
            Entity entity = gameObject.GetComponent<Entity>();

            if (entity != null)
                return entity;

            gameObject = gameObject.transform.parent?.gameObject;
        }

        return null;
    }
    /// <summary>
    /// Returns the distance similar to <see cref="Vector2.Distance(Vector2, Vector2)"/>, but it's scaled using <see cref="OrthographicScale"/>
    /// </summary>
    public static float OrthographicDistance(Vector2 a, Vector2 b)
    {
        Vector2 delta = b - a;

        delta.x /= OrthographicScale.x;
        delta.y /= OrthographicScale.y;

        return delta.magnitude;
    }
    /// <summary>
    /// Returns all <see cref="KeyCode"/> that are current <see cref="KeyState.Down"/>
    /// </summary>
    public static List<KeyCode> GetAllDownKeys()
    {
        return GetAllKeys(KeyState.Down);
    }
    /// <summary>
    /// Returns all <see cref="KeyCode"/> that are current <see cref="KeyState.Stay"/>
    /// </summary>
    public static List<KeyCode> GetAllStayKeys()
    {
        return GetAllKeys(KeyState.Stay);
    }
    /// <summary>
    /// Returns all <see cref="KeyCode"/> that are current <see cref="KeyState.Up"/>
    /// </summary>
    public static List<KeyCode> GetAllUpKeys()
    {
        return GetAllKeys(KeyState.Up);
    }
    /// <summary>
    /// Returns all <see cref="KeyCode"/> that fulfill a certain <see cref="KeyState"/> criteria
    /// </summary>
    public static List<KeyCode> GetAllKeys(KeyState state)
    {
        List<KeyCode> pressedKeys = new List<KeyCode>();

        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(key) && state.HasFlag(KeyState.Stay))
            {
                pressedKeys.Add(key);
            }
            else if(Input.GetKeyDown(key) && state.HasFlag(KeyState.Down))
            {
                pressedKeys.Add(key);
            }
            else if(Input.GetKeyUp(key) && state.HasFlag(KeyState.Up))
            {
                pressedKeys.Add(key);
            }
        }

        return pressedKeys;
    }
    public static bool IsAxialPositionWalkable(Axial position)
    {
        return IsAxialPositionWalkable(Level.Current.AllTiles, position);
    }
    /// <summary>
    /// Can an entity walk at the given position?
    /// </summary>
    public static bool IsAxialPositionWalkable(Dictionary<Axial, TileData> tiles, Axial position)
    {
        if (tiles.ContainsKey(position))
        {
            return tiles[position].IsWalkable;
        }

        return false;
    }
    /// <summary>
    /// Will convert a vector to account for the scale of the environment,
    /// which is scaled so we can have 3D environments using an orthographic camera
    /// </summary>
    public static Vector2 ScaleToOrthographicVector(Vector2 vector)
    {
        return new Vector2()
        {
            x = vector.x * OrthographicScale.x,
            y = vector.y * OrthographicScale.y,
        };
    }
    /// <summary>
    /// Will take a coordinate and scale it to fit the size of a 3D hexgaon, with the origin being in the middle of the hexagon.
    /// For instance, a y-value of 0.5f will be converted to the top of the hexagon.
    /// </summary>
    /// <param name="coordinate"></param>
    /// <returns></returns>
    public static Vector3 ScaleToHexagonalSize(Vector3 coordinate)
    {
        return new Vector3()
        {
            x = coordinate.x * Utility.OrthographicScale.x,
            y = coordinate.y * Utility.HexelHeightMultiplier * Utility.OrthographicScale.y,
            z = -coordinate.z * Utility.HexelCeilingHeight * Utility.OrthographicScale.z,
        };
}
    /// <summary>
    /// Returns whether a collection of rooms contains a position
    /// </summary>
    public static bool RoomsContainPosition(IEnumerable<RoomData> rooms, Vector2 position)
    {
        foreach (RoomData room in rooms)
        {
            foreach (var keyValuePair in room.Tiles)
            {
                if (keyValuePair.Key + room.Position == position)
                    return true;
            }
        }

        return false;
    }
    /// <summary>
    /// Calculates a bitmask based on the surroundings of a tile
    /// </summary>
    /// <param name="center">The tile whose bitmask we're calculating. Hexagonal coordinate</param>
    /// <param name="allTiles">All tiles in hexagonal coordinates</param>
    /// <returns>The bitmask of <paramref name="center"/></returns>
    public static byte GetBitmask(Axial center, HashSet<Axial> allTiles)
    {
        byte bitmask = 0;

        foreach (AxialDirection direction in AxialDirection.AllDirections)
        {
            if(allTiles.Contains(center + direction))
            {
                bitmask |= direction;
            }
        }

        return bitmask;
    }
    /// <summary>
    /// Converts an axial coordinate to cubed
    /// </summary>
    private static Vector3 AxialToCubedCoordinate(Vector2 axialCoordinate)
    {
        return new Vector3()
        {
            x = axialCoordinate.x,
            y = (-axialCoordinate.x) - axialCoordinate.y,
            z = axialCoordinate.y,
        };
    }
    /// <summary>
    /// Converts a cubed coordinate to axial
    /// </summary>
    private static Vector2 CubedToAxialCoordinate(Vector3 cubeCoordinate)
    {
        return new Vector2(cubeCoordinate.x, cubeCoordinate.z);
    }
    /// <summary>
    /// Converts an axial position to world coordinates
    /// </summary>
    public static Vector2 AxialToWorldPosition(Axial axial)
    {
        Vector2 axialVector = axial;

        return new Vector2()
        {
            x = (axialVector.x - axialVector.y * (Utility.HexelWidth / 2)) * OrthographicScale.x,
            y = (axialVector.y /= Utility.HexelHeightMultiplier) * OrthographicScale.y,
        };
    }
    /// <summary>
    /// Converts a world coordinate into the nearest axial coordinate
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <returns></returns>
    public static Axial WorldToAxialPosition(Vector2 worldPosition)
    {
        worldPosition = RoundToNearestHexagonalPosition(worldPosition);
        float y = worldPosition.y / OrthographicScale.y * Utility.HexelHeightMultiplier;

        return new Axial()
        {
            x = Mathf.RoundToInt((y * (Utility.HexelWidth / 2)) + worldPosition.x),
            y = Mathf.RoundToInt(y),
        };
    }
    /// <summary>
    /// Rounds a world coordinate to the nearest hexagon
    /// </summary>
    public static Vector2 RoundToNearestHexagonalPosition(Vector2 worldPos)
    {
        return HexagonalMath.WorldToHexagonalPosition(worldPos);
    }
    /// <summary>
    /// Evaluates every cell over a grid
    /// </summary>
    public static void EvaluateAxialGrid(Vector2 worldCenter, int width, int height, System.Action<Axial> callback)
    {
        int startX = Mathf.CeilToInt(-((float)width / 2));
        int startY = Mathf.CeilToInt(-((float)height / 2));
        int endX = Mathf.CeilToInt((float)width / 2);
        int endY = Mathf.CeilToInt((float)height / 2);

        Axial axialCenter = Utility.WorldToAxialPosition(worldCenter);

        for (int y = startY; y < endY; y++)
        {
            for (int x = startX; x < endX; x++)
            {
                // We do this to compensate for the x-axis shifting.
                // Impossible to explain in a short comment, feel free to uncomment to see the effect
                if(x >= startX + y && x < endX + y)
                    callback(axialCenter + new Axial(x, y));
            }
        }
    }
    /// <summary>
    /// Enumerates over 6 adjacent axial positions to <paramref name="center"/>
    /// </summary>
    public static void AdjacentHexagons(Axial center, System.Action<Axial> callback)
    {
        for (int i = 0; i < 6; i++)
        {
            callback(center + AxialDirection.AllDirections[i]);
        }
    }
    /// <summary>
    /// Enumerates over 8 adjacent positions to <paramref name="center"/>
    /// </summary>
    public static void AdjacentEight(Vector2 center, System.Action<Vector2> callback)
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                callback(center + new Vector2(x, y));
            }
        }
    }
    /// <summary>
    /// Enumerates over 4 adjacent positions to <paramref name="center"/>
    /// </summary>
    public static void AdjacentFour(Vector2 center, System.Action<Vector2> callback)
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if ((x == 0 && y == 0) || (x != 0 && y != 0))
                    continue;

                callback(center + new Vector2(x, y));
            }
        }
    }
    public static int GetAllObjectsWithinRay(Vector2 origin, Vector2 direction, ref RaycastHit2D[] hitBuffer)
    {
        return GetAllObjectsWithinRay(origin, direction, Physics2D.DefaultRaycastLayers, ref hitBuffer);
    }
    public static int GetAllObjectsWithinRay(Vector2 origin, Vector2 direction, int layermask, ref RaycastHit2D[] hitBuffer)
    {
        return GetAllObjectsWithinRay(origin, direction, direction.magnitude, layermask, ref hitBuffer);
    }
    public static int GetAllObjectsWithinRay(Vector2 origin, Vector2 direction, float distance, int layermask, ref RaycastHit2D[] hitBuffer)
    {
        return Physics2D.RaycastNonAlloc(origin, direction.normalized, hitBuffer, distance, layermask);
    }
    private static int GetAllObjectsWithinRadiusRaycastHit(Vector3 origin, float radius, int layermask, ref RaycastHit2D[] hitBuffer)
    {
        int count = GetAllObjectsWithinRadius(origin, radius, layermask, ref ColliderBuffer);

        // Convert from Collider2D to RaycastHit2D
        for (int i = 0; i < count; i++)
        {
            hitBuffer[i] = Physics2D.Raycast(origin, (ColliderBuffer[i].transform.position - origin).normalized, radius, layermask);
        }

        return count;
    }
    public static int GetAllObjectsWithinRadius(Vector3 origin, float radius, ref Collider2D[] colliderBuffer)
    {
        return GetAllObjectsWithinRadius(origin, radius, Physics2D.DefaultRaycastLayers, ref colliderBuffer);
    }
    public static int GetAllObjectsWithinRadius(Vector3 origin, float radius, int layermask, ref Collider2D[] colliderBuffer)
    {
        return Physics2D.OverlapCircleNonAlloc(origin, radius, colliderBuffer, layermask);
    }
    /// <summary>
    /// Will return the max value of the current segment
    /// </summary>
    /// <param name="currentValue">Current value in total, not in relation to segment</param>
    /// <param name="valuePerSegment">How many values does a segment hold?</param>
    /// <returns>The max value of the current segment</returns>
    public static float GetMaxValueForSegment(float currentValue, float valuePerSegment)
    {
        int currentSegment = Mathf.CeilToInt(currentValue / valuePerSegment);
        return valuePerSegment * currentSegment;
    }
    #endregion
}
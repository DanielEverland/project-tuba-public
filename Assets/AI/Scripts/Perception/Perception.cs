using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeCanvas.Framework;

/// <summary>
/// Allows an AI to see things within the envioronment
/// </summary>
public class Perception : AIBehaviour
{
    [SerializeField]
    private LayerMask sightBlockers = (int)Layer.Environment;
    [SerializeField]
    private float maxSightDistance = 15;
    [SerializeField]
    private bool debug = false;

    public float SightDistance => maxSightDistance;

    private List<Interactable> VisibleThings
    {
        get
        {
            if (canLookAtSurroundings)
                LookAtSurroundings();

            return visibleThings;
        }
    }

    [SerializeField, Readonly]
    private List<Interactable> visibleThings = new List<Interactable>();

    private const float ShowDebugDuration = 0.1f;
    private bool canLookAtSurroundings = true;

    private void Start()
    {
        Tick();
    }
    public List<PerceptionTarget> QueryTag(TagType tag)
    {
        List<PerceptionTarget> results = new List<PerceptionTarget>();

        foreach (Interactable interactable in VisibleThings)
        {
            if (interactable.ContainsTag(tag))
                results.Add(new PerceptionTarget(this, interactable));
        }

        return results;
    }
    public List<PerceptionTarget> QueryTags(IList<TagType> tags)
    {
        List<PerceptionTarget> results = new List<PerceptionTarget>();

        foreach (Interactable interactable in VisibleThings)
        {
            if (interactable.ContainsAny(tags))
                results.Add(new PerceptionTarget(this, interactable));
        }
        
        return results;
    }
    public List<PerceptionTarget> QueryTags(IList<TagType> inclusive, IList<TagType> exclusive)
    {
        List<PerceptionTarget> results = new List<PerceptionTarget>();

        foreach (Interactable interactable in VisibleThings)
        {
            if (interactable.ContainsAny(inclusive) && !interactable.ContainsAny(exclusive))
                results.Add(new PerceptionTarget(this, interactable));
        }

        return results;
    }
    public bool CanSeeTag(TagType tag)
    {
        return visibleThings.Any(x => x.ContainsTag(tag));
    }

    protected override void Tick()
    {
        canLookAtSurroundings = true;
    }
    private void LookAtSurroundings()
    {
        canLookAtSurroundings = false;

        visibleThings.Clear();

        foreach (Interactable interactable in Utility.GetAllInteractablesWithinRadius(transform.position, maxSightDistance, ~sightBlockers))
        {
            if (debug)
                Debug.DrawLine(transform.position, interactable.transform.position, Color.white, ShowDebugDuration);

            PollHit(interactable);
        }
    }
    private void PollHit(Interactable interactable)
    {
        if(CanSeeTarget(this, interactable.transform.position))
            visibleThings.Add(interactable);
    }
    public static bool CanSeeTarget(Perception perception, Vector2 targetPosition)
    {
        Vector2 delta = targetPosition - (Vector2)perception.transform.position;
        Vector2 direction = delta.normalized;
        float targetDistance = delta.magnitude;

        if (targetDistance > perception.SightDistance)
            return false;

        int hits = Physics2D.RaycastNonAlloc(perception.transform.position, direction, Utility.HitBuffer, targetDistance, perception.sightBlockers);

        if (perception.debug && hits > 0)
            Debug.DrawLine(perception.transform.position, Utility.HitBuffer[0].point, Color.red, ShowDebugDuration);

        return hits <= 0;
    }
    private void OnDrawGizmos()
    {
        if(debug)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, maxSightDistance);
        }        
    }
}

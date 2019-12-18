using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternObject : MonoBehaviour
{
    public float SpawnAnimationTime { get; set; }
    public List<PatternElement> Elements { get; set; } = new List<PatternElement>();
        
    private PatternBehaviour behaviour = default;
    private bool isAnimating = default;
    
    public void InitializeBehaviour(PatternBehaviour behaviourPrefab)
    {
        behaviour = Instantiate(behaviourPrefab);
        behaviour.Pattern = this;
        behaviour.Initialize();
    }
    public void AddElement(PatternElement element)
    {
        Elements.Add(element);
        element.transform.SetParent(transform);
    }
    public void SetElementPosition(PatternElement element, Vector2 position)
    {
        element.transform.localPosition = Utility.ScaleToOrthographicVector(position);
    }
    private void OnEnable()
    {
        StartCoroutine(nameof(AnimateSpawning));
    }
    private IEnumerator AnimateSpawning()
    {
        isAnimating = true;

        yield return new WaitUntil(() => Elements.Count > 0);
        
        for (int i = 0; i < Elements.Count; i++)
        {
            PatternElement currentElement = Elements[i];

            if(currentElement != null)
                Elements[i].gameObject.SetActive(false);
        }            

        for (int i = 0; i < Elements.Count; i++)
        {
            PatternElement currentElement = Elements[i];

            if(currentElement != null)
            {
                currentElement.gameObject.SetActive(true);
                currentElement.DisableCollision();

                yield return new WaitForSeconds(SpawnAnimationTime / Elements.Count);
            }            
        }

        for (int i = 0; i < Elements.Count; i++)
        {
            PatternElement currentElement = Elements[i];

            if (currentElement != null)
                currentElement.EnableCollision();
        }

        isAnimating = false;
    }
    private void Update()
    {
        if(!isAnimating)
            behaviour.Update();
    }
}
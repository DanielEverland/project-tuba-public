using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pattern.asset", menuName = Utility.MenuItemRoot + "Pattern", order = 300)]
public class Pattern : ScriptableObject
{
    [SerializeField]
    private float spawnAnimationTime = 0.5f;
    [SerializeField]
    private List<PatternComponent> components = new List<PatternComponent>();
    [SerializeField]
    private PatternBehaviour behaviour = null;
    [SerializeField]
    private PatternElement prefab = null;
    
    public List<PatternComponent> Components => components;
    public PatternBehaviour Behaviour { get => behaviour; set => behaviour = value; }
    
    public PatternObject Spawn()
    {
        GameObject instance = HierarchyManager.CreateGameObject($"Pattern ({name})", HierarchyCategory.Patterns);

        PatternObject patternObject = instance.AddComponent<PatternObject>();
        patternObject.SpawnAnimationTime = spawnAnimationTime;

        Tags tags = instance.AddComponent<Tags>();
        tags.AddType(TagType.PatternObject);

        for (int i = 0; i < components.Count; i++)
        {
            components[i].SpawnChildren(prefab, patternObject);
        }

        if (Behaviour != null)
            patternObject.InitializeBehaviour(Behaviour);

        return patternObject;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = Utility.MenuItemRoot + "TagType", order = 303)]
public class TagType : ScriptableObject
{
    public static TagType Boss
    {
        get
        {
            if (_boss == null)
                _boss = LoadType("Boss");

            return _boss;
        }
    }
    private static TagType _boss;

    public static TagType BossPhase
    {
        get
        {
            if (_bossPhase == null)
                _bossPhase = LoadType("Boss Phase");

            return _bossPhase;
        }
    }
    private static TagType _bossPhase;

    public static TagType Effects
    {
        get
        {
            if (_effects == null)
                _effects = LoadType("Effects");

            return _effects;
        }
    }
    private static TagType _effects;

    public static TagType Enemy
    {
        get
        {
            if (_enemy == null)
                _enemy = LoadType("Enemy");

            return _enemy;
        }
    }
    private static TagType _enemy;

    public static TagType PatternObject
    {
        get
        {
            if (_patternObject == null)
                _patternObject = LoadType("Pattern Object");

            return _patternObject;
        }
    }
    private static TagType _patternObject;

    public static TagType Support
    {
        get
        {
            if (_support == null)
                _support = LoadType("Support");

            return _support;
        }
    }
    private static TagType _support;

    public static TagType AbilityObject
    {
        get
        {
            if (_abilityObject == null)
                _abilityObject = LoadType("Ability Object");

            return _abilityObject;
        }
    }
    private static TagType _abilityObject;

    private static TagType LoadType(string name)
    {
        TagType loadedType = Resources.Load<TagType>(name);

        if (loadedType == null)
            throw new System.NullReferenceException($"Couldn't load type {name}");

        return loadedType;
    }
}

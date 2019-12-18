using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Type = System.Type;

/// <summary>
/// Contains an instance of a given ScriptableObject type <typeparamref name="T"/>
/// </summary>
/// <typeparam name="T">The type selections are based on</typeparam>
[System.Serializable]
public abstract class GenericField<T> where T : ScriptableObject
{
    public Type Type => typeof(T);

    public T Instance { get => instance; set => instance = value; }

    [SerializeField]
    protected T instance;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InitializedMonobehaviour<T1, T2, T3, T4, T5, T6, T7> : InitializedMonobehaviourBase
{
    public abstract void DoInitialize(T1 obj1, T2 obj2, T3 obj3, T4 obj4, T5 obj5, T6 obj6, T7 obj7);
    public virtual void Initialize(T1 obj1, T2 obj2, T3 obj3, T4 obj4, T5 obj5, T6 obj6, T7 obj7)
    {
        IsInitialized = true;
    }
}
public abstract class InitializedMonobehaviour<T1, T2, T3, T4, T5, T6> : InitializedMonobehaviourBase
{
    public abstract void DoInitialize(T1 obj1, T2 obj2, T3 obj3, T4 obj4, T5 obj5, T6 obj6);
    public virtual void Initialize(T1 obj1, T2 obj2, T3 obj3, T4 obj4, T5 obj5, T6 obj6)
    {
        IsInitialized = true;
    }
}
public abstract class InitializedMonobehaviour<T1, T2, T3, T4, T5> : InitializedMonobehaviourBase
{
    public abstract void DoInitialize(T1 obj1, T2 obj2, T3 obj3, T4 obj4, T5 obj5);
    public virtual void Initialize(T1 obj1, T2 obj2, T3 obj3, T4 obj4, T5 obj5)
    {
        IsInitialized = true;
    }
}
public abstract class InitializedMonobehaviour<T1, T2, T3, T4> : InitializedMonobehaviourBase
{
    public abstract void DoInitialize(T1 obj1, T2 obj2, T3 obj3, T4 obj4);
    public virtual void Initialize(T1 obj1, T2 obj2, T3 obj3, T4 obj4)
    {
        IsInitialized = true;
    }
}
public abstract class InitializedMonobehaviour<T1, T2, T3> : InitializedMonobehaviourBase
{
    public abstract void DoInitialize(T1 obj1, T2 obj2, T3 obj3);
    public virtual void Initialize(T1 obj1, T2 obj2, T3 obj3)
    {
        IsInitialized = true;
    }
}
public abstract class InitializedMonobehaviour<T1, T2> : InitializedMonobehaviourBase
{
    public abstract void DoInitialize(T1 obj1, T2 obj2);
    public virtual void Initialize(T1 obj1, T2 obj2)
    {
        IsInitialized = true;
    }
}
public abstract class InitializedMonobehaviour<T1> : InitializedMonobehaviourBase
{
    public abstract void DoInitialize(T1 obj1);
    public virtual void Initialize(T1 obj1)
    {
        IsInitialized = true;
    }
}
public abstract class InitializedMonobehaviourBase : MonoBehaviour
{
    protected bool IsInitialized { get; set; } = false;

    protected virtual void Start()
    {
        if (!IsInitialized)
            throw new System.InvalidOperationException();
    }
}
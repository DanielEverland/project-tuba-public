using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    [SerializeField]
    private Space space = Space.Self;
    [SerializeField]
    private Vector3 direction = Vector3.right;
    [SerializeField]
    protected FloatReference velocity = new FloatReference(100);
    
    protected virtual void Update()
    {
        Move();
    }
    protected virtual void Move()
    {
        transform.position += GetDirection() * velocity.Value * Time.deltaTime;
    }
    protected virtual Vector3 GetDirection() => space == Space.Self ? transform.TransformDirection(direction) : direction;
}
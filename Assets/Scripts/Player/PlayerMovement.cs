using UnityEngine;

[RequireComponent(typeof(EntityController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Entity entity = null;
    [SerializeField]
    private EntityController characterController = null;
    [SerializeField]
    private FloatReference movementSpeed = null;
    [SerializeField]
    private Dasher dasher = null;
    
    private void Update()
    {
        if (entity.ContainsModifier(typeof(StunnedModifier)) || InputSuppressor.IsSuppressed || PauseState.IsPaused)
            return;

        Vector2 input = PollInput();

        if (!dasher.IsDashing)
        {
            Move(input);

            if (PollDash())
                dasher.Dash(input);
        }
    }
    private void Move(Vector2 direction)
    {
        characterController.Move(direction * movementSpeed.Value);
    }
    private bool PollDash()
    {
        return Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button0);
    }
    private Vector2 PollInput()
    {
        return new Vector2()
        {
            x = Input.GetAxisRaw("Horizontal"),
            y = Input.GetAxisRaw("Vertical"),
        }.normalized;
    }
    private void OnValidate()
    {
        if (characterController == null)
            characterController = GetComponent<EntityController>();

        if (entity == null)
            entity = GetComponent<Entity>();
    }
}
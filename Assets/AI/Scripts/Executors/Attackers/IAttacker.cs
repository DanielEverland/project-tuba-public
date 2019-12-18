using NodeCanvas.Framework;

public interface IAttacker
{
    Status OnUpdate();
    void OnStart(Interactable target);
    void OnStop();
}

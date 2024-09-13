using UnityEngine;

public abstract class PlayerDecorator : MonoBehaviour
{
    protected PlayerController player;

    public void SetPlayer(PlayerController player)
    {
        this.player = player;
    }

    public abstract void ApplyEffect();
    public abstract void RemoveEffect();
}

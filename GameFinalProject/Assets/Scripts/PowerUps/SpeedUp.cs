public class SpeedBoost : PlayerDecorator
{
    private float originalSpeed;

    public override void ApplyEffect()
    {
        originalSpeed = player.GetSpeed();
        player.ChangeSpeed(player.GetSpeed() * 1.5f);
    }

    public override void RemoveEffect()
    {
        player.ChangeSpeed(originalSpeed);
    }
}
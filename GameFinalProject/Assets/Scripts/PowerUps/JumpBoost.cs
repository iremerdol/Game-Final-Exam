public class JumpBoost : PlayerDecorator
{
    private float originalJumpPower;

    public override void ApplyEffect()
    {
        originalJumpPower = player.GetJumpPower();
        player.ChangeJumpPower(originalJumpPower * 1.5f);
    }

    public override void RemoveEffect()
    {
        player.ChangeJumpPower(originalJumpPower);
    }
}
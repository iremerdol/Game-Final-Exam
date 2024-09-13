using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState
{
    void EnterState(PlayerController player);
    void UpdateState(PlayerController player);
    void ExitState(PlayerController player);
}

public class IdleState : IPlayerState
{
    public void EnterState(PlayerController player)
    {
        
    }

    public void UpdateState(PlayerController player)
    {
        if (player.IsMoving())
        {
            player.TransitionToState(player.RunningState);
        }
        else if (player.IsJumping())
        {
            player.TransitionToState(player.JumpingState);
        }
        else if (player.IsAttacking())
        {
            player.TransitionToState(player.AttackingState);
        }
    }

    public void ExitState(PlayerController player)
    {
        
    }
}

public class RunningState : IPlayerState
{
    public void EnterState(PlayerController player)
    {
        player.SetAnimation("run");
    }

    public void UpdateState(PlayerController player)
    {
        if (!player.IsMoving())
        {
            player.TransitionToState(player.IdleState);
        }
        else if (player.IsJumping())
        {
            player.TransitionToState(player.JumpingState);
        }
        else if (player.IsAttacking())
        {
            player.TransitionToState(player.AttackingState);
        }
    }

    public void ExitState(PlayerController player)
    {
        player.ResetAnimation("run");
    }
}

public class JumpingState : IPlayerState
{
    public void EnterState(PlayerController player)
    {
        player.SetAnimation("jump");
    }

    public void UpdateState(PlayerController player)
    {
        if (player.IsGrounded())
        {
            player.TransitionToState(player.IdleState);
        }
    }

    public void ExitState(PlayerController player)
    {
        player.ResetAnimation("jump");
    }
}

public class AttackingState : IPlayerState
{
    public void EnterState(PlayerController player)
    {
        player.SetAnimation("attack");
    }

    public void UpdateState(PlayerController player)
    {
        if (!player.IsAttacking())
        {
            player.TransitionToState(player.IdleState);
        }
    }

    public void ExitState(PlayerController player)
    {
        player.ResetAnimation("attack");
    }
}

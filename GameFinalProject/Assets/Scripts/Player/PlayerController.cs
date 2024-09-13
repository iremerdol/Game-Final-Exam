using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public IPlayerState IdleState { get; private set; }
    public IPlayerState RunningState { get; private set; }
    public IPlayerState JumpingState { get; private set; }
    public IPlayerState AttackingState { get; private set; }
    private IPlayerState currentState;

    [Header("Movement Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime;
    private float coyoteCounter;

    [Header("Multiple Jumps")]
    [SerializeField] private int extraJumps;
    private int jumpCounter;

    [Header("Wall Jumping")]
    [SerializeField] private float wallJumpX;
    [SerializeField] private float wallJumpY;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    [Header("Sounds")]
    [SerializeField] private AudioClip jumpSound;

    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;

    private void Awake()
    {
        IdleState = new IdleState();
        RunningState = new RunningState();
        JumpingState = new JumpingState();
        AttackingState = new AttackingState();

        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();

        currentState = IdleState;
        SetAnimation("grounded");
    }

    private void Update(){

        currentState.UpdateState(this);

        horizontalInput = Input.GetAxis("Horizontal");

        //flip the player left-right when moving
        if(horizontalInput > 0.01f){
            transform.localScale = Vector3.one;
        }
        else if(horizontalInput < -0.01f){
            transform.localScale = new Vector3(-1, 1, 1);
        }

        //set animator parameters
        //anim.SetBool("run", horizontalInput != 0);
        //anim.SetBool("grounded", IsGrounded());

        //old wall jump
        /* if(wallJumpCooldown > 0.2f){
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
            
            if(OnWall() && !IsGrounded()){
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            }
            else{
                body.gravityScale = 2;
            }
            
            if(Input.GetKey(KeyCode.Space)){
                Jump();
                if(Input.GetKeyDown(KeyCode.Space) && IsGrounded()){
                    SoundManager.instance.PlaySound(jumpSound);
                }
            }
        }
        else{
            wallJumpCooldown += Time.deltaTime;
        } */

        if(Input.GetKeyDown(KeyCode.Space)){
            Jump();
        }

        if(Input.GetKeyUp(KeyCode.Space)){
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);
        }

        if(OnWall()){
            body.gravityScale = 0;
            body.velocity = Vector2.zero;
        }
        else{
            body.gravityScale = 3;
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
        
            if(IsGrounded()){
                coyoteCounter = coyoteTime;
            }
            else{
                coyoteCounter -= Time.deltaTime;
            
            }
        }
    }

    public void TransitionToState(IPlayerState newState)
    {
        currentState.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }

    private void Jump(){
        if(coyoteCounter < 0 && !OnWall() && jumpCounter <= 0) return;

        SoundManager.Instance.PlaySound(jumpSound);

        if(OnWall()){
            WallJump();
        }
        else{
            if(IsGrounded()){
                body.velocity = new Vector2(body.velocity.x, jumpPower);
                jumpCounter = extraJumps;
            }
            else{
                if(coyoteCounter > 0){
                    body.velocity = new Vector2(body.velocity.x, jumpPower);
                }
                else{
                    if(jumpCounter > 0){
                        body.velocity = new Vector2(body.velocity.x, jumpPower);
                        jumpCounter--;
                    }
                }
            }
            //reset coyote counter to 0 to avoid double jump
            coyoteCounter = 0;    
        }

        //old jump
        /* if(IsGrounded()){
            body.velocity = new Vector2(body.velocity.x, jumpPower);
        }
        else if(OnWall() && !IsGrounded()){
            if(horizontalInput == 0){
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else{
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);
            }
            wallJumpCooldown = 0;
        } */
    }

    public bool IsMoving()
    {
        // Return whether the player is moving
        return Mathf.Abs(Input.GetAxis("Horizontal")) > 0.01f;
    }

    public bool IsJumping()
    {
        // Return whether the player is jumping
        return Input.GetKeyDown(KeyCode.Space);
    }

    public bool IsAttacking()
    {
        // Return whether the player is attacking
        return Input.GetMouseButton(0);
    }

    public bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    public void SetAnimation(string animationName)
    {
        // Set player animation
        //GetComponent<Animator>().SetTrigger(animationName);
        if(animationName == "run"){
            anim.SetBool("run", true);
        }
        else{
            anim.SetTrigger(animationName);
        }
        Debug.Log("Set animation: " + animationName);
    }

    public void ResetAnimation(string animationName)
    {
        // Reset player animation
        if(animationName == "run"){
            anim.SetBool("run", false);
        }
        else{
            anim.ResetTrigger(animationName);
        }
    }

    private void WallJump(){
        body.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpX, wallJumpY));
        wallJumpCooldown = 0;
    }
    private bool OnWall(){
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
    public bool CanAttack(){
        return horizontalInput == 0 && IsGrounded() && !OnWall();
    }
    public void ChangeJumpPower(float newJumpPower){
        jumpPower = newJumpPower;
    }
    public float GetJumpPower(){
        return jumpPower;
    }
    public void ChangeSpeed(float newSpeed){
        speed = newSpeed;
    }
    public float GetSpeed(){
        return speed;
    }
}

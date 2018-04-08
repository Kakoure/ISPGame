using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedGolemController : MonoBehaviour {
    public float speed;
    private Rigidbody2D rg2d;
    private bool facingRight;
    public float hp;
    public float damage;
    private int kbAngle;
    private float kbForce;
    private int lungeDir;
    public Transform groundCheck;
    public Transform forwardsGroundCheck;
    public Transform wallCheck;
    private RaycastHit2D forwardsGroundCheckHit;
    public GameObject playerCheck;
    public GameObject hurtParticles;
    private LookForPlayer playerCheckVars;
    private Animator anim;
    public LayerMask whatIsGround;
    public LayerMask whatIsPlayer;
    public GameObject dmgObject;
    private GameObject attackObject;
    private DamageScript attackvars;
    private GameObject targetObject;
    private PhysicsMaterial2D rGolemMaterial;
    private string state;
    private float stateDuration;
    private float strollSpeed;
    private float strollDir;
    private int randInt;
    private float stateLock;
    private bool seesTarget;
    private bool wallAhead;
    private bool isGrounded;
    private bool hitStunned;
    private float currentSpeedx;
    private bool inPhysicsMode;
    private bool isVisible;
    private Vector2 incomingKb;
    public Vector2 IncomingKb
    {
        get
        {
            return incomingKb;
        }
        set
        {
            incomingKb = value;
        }
    }
    /*
     * States:
     * 
     * "findNextState" - Check for the player, if found pursue, else stay idle or move
     * "pursuit"- move towards player until within attacking range
     * "attack" - swings at teh player
     * "idle" - stand in place
     * "stroll" - walk slowly somewhere
     * "ded" - dies
     */
    void Start () {
        rg2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerCheckVars = playerCheck.GetComponent<LookForPlayer>();
        rGolemMaterial = new PhysicsMaterial2D();
        rg2d.sharedMaterial = rGolemMaterial;
        rg2d.sharedMaterial.friction = 0f;
        facingRight = true;
        state = "findNextState";
        stateLock = 0f;
        inPhysicsMode = false;
        isVisible = false;
        randInt = Random.Range(0, 2);
        if (randInt == 1)
        {
            Flip();
        }
        DoStateStep();
   
        
	}
    private void OnBecameInvisible()
    {
        isVisible = false;
    }
    private void OnBecameVisible()
    {
        isVisible = true;
    }
    void FixedUpdate () {
        if (isVisible || seesTarget)
        {
            //Debug.Log(testcount);
            currentSpeedx = rg2d.velocity.x;
            if (currentSpeedx != 0)
            {
                anim.SetBool("isMoving", true);
            }
            else
            {
                anim.SetBool("isMoving", false);
            }
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, whatIsGround);
            anim.SetBool("isGrounded", isGrounded);
            if (hp <= 0f)
            {
                anim.SetBool("isDead", true);
                state = "ded";
                hitStunned = false;
            }
            if (hitStunned)
            {
                anim.SetBool("isHurt", true);
                state = "hurt";
            }
            if (targetObject != null && (Mathf.Abs(targetObject.transform.position.x - transform.position.x) > 10 || Mathf.Abs(targetObject.transform.position.y - transform.position.y) > 5))
            {
                seesTarget = false;
                targetObject = null;
            }
            else if (targetObject != null && (Mathf.Abs(targetObject.transform.position.x - transform.position.x) < 10 || Mathf.Abs(targetObject.transform.position.y - transform.position.y) < 5))
            {
                seesTarget = true;
            }
            else
            {
                seesTarget = false;
            }
            DoStateStep();
        }
	}

    void Flip()
    {
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
        facingRight = !facingRight;
    }
    public float GetDir()
    {
        if (facingRight)
        {
            return 1f;
        } else
        {
            return -1f;
        }
    }
    public void SetHitStun()
    {
        hitStunned  = true;
    }

    public void SetXVelocity(float input)
    {
        rg2d.velocity = new Vector2(input, rg2d.velocity.y);
    }

    public float GetXVelocity()
    {
        return rg2d.velocity.x;
    }

    public void SetYVelocity(float input)
    {
        rg2d.velocity = new Vector2(rg2d.velocity.x, input);
    }

    public void SetTargetObject(GameObject input)
    {
        targetObject = input;
    }

    public void SetState(string input)
    {
        state = input;
    }

    public string GetState()
    {
        return state;
    }

    public float GetStateLock()
    {
        return stateLock;
    }

    public void SetStateLock(float input)
    {
        stateLock = input;
    }

    void DoStateStep()
    {
        //Debug.Log(state);
        stateLock = Mathf.Max(stateLock - Time.deltaTime, 0f);
        stateDuration = Mathf.Max(stateDuration - Time.deltaTime, 0f);
        switch (state)
        {
            case "findNextState":
                randInt = Random.Range(0, 4);
                switch (randInt)
                {
                    case 1:
                        state = "idle";
                        stateDuration = Random.Range(1f, 3f);
                        DoStateStep();
                        break;
                    case 2:
                        Flip();
                        state = "idle";
                        stateDuration = Random.Range(1f, 3f);
                        DoStateStep();
                        break;
                    case 3:
                        state = "stroll";
                        strollSpeed = Random.Range(1f, 2f);
                        if (Random.Range(0, 2) == 1)
                        {
                            strollDir = -1;
                        } else
                        {
                            strollDir = 1;
                        }
                        stateDuration = Random.Range(1f, 3f);
                        break;
                }
                break;
            case "idle":
                if (!inPhysicsMode) { 
                rg2d.velocity = new Vector2(0, rg2d.velocity.y);
                }
                if (seesTarget)
                {
                    if (targetObject.transform.position.x > transform.position.x && !facingRight)
                    {
                        Flip();
                    }
                    else if (targetObject.transform.position.x < transform.position.x && facingRight)
                    {
                        Flip();
                    }
                }
                if(stateLock == 0f && stateDuration == 0f)
                {
                    state = "findNextState";
                }

                break;
            case "stroll":
                if (seesTarget && Mathf.Abs(targetObject.transform.position.x - transform.position.x) > 1)
                {
                    strollDir = Mathf.Sign(targetObject.transform.position.x - transform.position.x);
                }
                if (!inPhysicsMode)
                {
                    rg2d.velocity = new Vector2(strollDir * strollSpeed, rg2d.velocity.y);
                }

                if (rg2d.velocity.x > 0f && !facingRight)
                {
                    Flip();
                }
                else if (rg2d.velocity.x < 0f && facingRight)
                {
                    Flip();
                }
                if (stateLock == 0f && stateDuration == 0f)
                {
                    state = "findNextState";
                }
                forwardsGroundCheckHit = (Physics2D.Raycast(forwardsGroundCheck.position, new Vector2(0, -1), .1f, whatIsGround));
                if (forwardsGroundCheckHit.collider == null)
                {
                    randInt = Random.Range(0, 2);
                    switch (randInt)
                    {
                        case 0:
                            strollDir *= -1;
                            break;
                        case 1:
                            state = "findNextState";
                            DoStateStep();
                            break;
                    }
                }
                break;
            case "pursuit":
                if (targetObject != null)
                {
                    wallAhead = Physics2D.OverlapCircle(wallCheck.position, .3f, whatIsGround);
                    if (wallAhead && isGrounded)
                    {
                        rg2d.AddForce(new Vector2(0f, 17f), ForceMode2D.Impulse);
                    }
                    if (Mathf.Abs(targetObject.transform.position.x - transform.position.x)> .1f && !inPhysicsMode)
                    {
                        rg2d.velocity = new Vector2(Mathf.Sign(targetObject.transform.position.x - transform.position.x) * speed, rg2d.velocity.y);
                    } else if (!inPhysicsMode)
                    {
                        rg2d.velocity = new Vector2(0, rg2d.velocity.y);
                    }
                    if (!seesTarget)
                    {
                        state = "findNextState";
                    }

                    if (rg2d.velocity.x > 0f && !facingRight)
                    {
                        Flip();
                    }
                    else if (rg2d.velocity.x < 0f && facingRight)
                    {
                        Flip();
                    }
                    anim.SetBool("isAttacking", false);
                    //Debug.Log("WEE WOO WEE WOO WEE WOO");
                } else
                {
                    state = "findNextState";
                    DoStateStep();
                }
                break;
            case "attack":
                anim.SetBool("isAttacking", true);
                if (targetObject != null)
                {
                    if (targetObject.transform.position.x > transform.position.x && !facingRight)
                    {
                        Flip();
                    }
                    else if (targetObject.transform.position.x < transform.position.x && facingRight)
                    {
                        Flip();
                    }
                }
                break;
            case "hurt":
                if(isGrounded && stateLock == 0f)
                {

                    playerCheckVars.SetRange("search");
                    anim.SetBool("isAttacking", false);
                    state = "findNextState";
                    seesTarget = true;
                    ExitPhysicsMode();
                }
                break;
            case "ded":
                hitStunned = false;
                anim.SetBool("isHurt", false);
                gameObject.layer = 27;
                if(rg2d.sharedMaterial.friction == 0)
                {
                    EnterPhysicsMode();
                }
                //rg2d.velocity = new Vector2(0, rg2d.velocity.y);
                break;
            default:
                break;
        }
    }

    void Slash()
    {
        attackObject = Instantiate(dmgObject, transform);
        attackvars = attackObject.GetComponent<DamageScript>();
        if (facingRight) { 
            kbForce = 8f;
            lungeDir = 1;
        } else
        {
            kbForce = -8f;
            lungeDir = -1;
        }
        attackvars.Setup(damage, kbForce, "Evil", new Vector2(1.75f, 0.9f), new Vector2(0.5f, 0f), 4, 45f * lungeDir);
        rg2d.AddForce(new Vector2(10f*lungeDir,0f), ForceMode2D.Impulse);
    }

    void EndAttack()
    {
        state = "idle";
        anim.SetBool("isAttacking", false);
        stateLock = .5f;
        playerCheckVars.SetRange("search");
        seesTarget = true;
        ExitPhysicsMode();
    }

    void EndHitStun() //Called in hurt2 anim
    {
        anim.SetBool("isHurt" ,false);
        hitStunned = false;
        if(isGrounded && stateLock == 0f)
        {
            playerCheckVars.SetRange("search");
            anim.SetBool("isAttacking", false);
            state = "findNextState";
            seesTarget = true;
            ExitPhysicsMode();

        }
        
    }

    public void EnterPhysicsMode()
    {
        rg2d.sharedMaterial.friction = .6f;
        inPhysicsMode = true;
    }

    public void ExitPhysicsMode()
    {
        rg2d.sharedMaterial.friction = 0f;
        inPhysicsMode = false;
    }

    public void ApplyKb()
    {
        SetXVelocity(0f);
        EnterPhysicsMode();
        rg2d.AddForce(incomingKb, ForceMode2D.Impulse);
    }
}

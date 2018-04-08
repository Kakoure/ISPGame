using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenGolemController : MonoBehaviour {
    [SerializeField]
    float speed;
    [SerializeField]
    float fleeSpeedModifier;
    private Rigidbody2D rg2d;
    private PolygonCollider2D polyColl;
    private BoxCollider2D boxColl;
    private bool facingRight;
    public float hp;
    public float damage;
    private int kbAngle;
    private float kbForce;
    public Transform groundCheck;
    public Transform forwardsGroundCheck;
    public Transform gunBarrel;
    public Transform wallCheck;
    private RaycastHit2D forwardsGroundCheckHit;
    public GameObject playerCheck;
    public GameObject hurtParticles;
    private LookForPlayer playerCheckVars;
    private Animator anim;
    public LayerMask whatIsGround;
    public LayerMask whatIsPlayer;
    public GameObject projObject;
    GameObject bulletFired;
    GreenGolemProjController projVars;
    private GameObject targetObject;
    private PhysicsMaterial2D gGolemMaterial;
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
     * "attack" - shoots at teh player
     * "idle" - stand in place
     * "stroll" - walk slowly somewhere
     * "ded" - dies
     * "flee" creates some distance
     */

  
    void Start () {
        rg2d = GetComponent<Rigidbody2D>();
        polyColl = GetComponent<PolygonCollider2D>();
        boxColl = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        playerCheckVars = playerCheck.GetComponent<LookForPlayer>();
        gGolemMaterial = new PhysicsMaterial2D();
        rg2d.sharedMaterial = gGolemMaterial;
        rg2d.sharedMaterial.friction = 0f;
        facingRight = true;
        state = "findNextState";
        stateLock = 0f;
        inPhysicsMode = false;
        randInt = Random.Range(0, 2);
        if (randInt == 1)
        {
            Flip();
        }
        DoStateStep();
        isVisible = false;
    }
    private void OnBecameInvisible()
    {
        isVisible = false;
    }
    private void OnBecameVisible()
    {
        isVisible = true;
    }

    private void FixedUpdate()
    {
        if (isVisible || seesTarget)
        {
            currentSpeedx = rg2d.velocity.x;
            if (currentSpeedx != 0)
            {
                anim.SetBool("isMoving", true);
            }
            else
            {
                anim.SetBool("isMoving", false);
            }
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
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, whatIsGround);
            anim.SetBool("isGrounded", isGrounded);
            //Debug.Log(seesTarget);
            DoStateStep();
        }
    }

    void DoStateStep()
    {
        stateLock = Mathf.Max(stateLock - Time.deltaTime, 0f);
        stateDuration = Mathf.Max(stateDuration - Time.deltaTime, 0f);
        //Debug.Log(state);
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
                        strollSpeed = Random.Range(.5f, .5f);
                        if (Random.Range(0, 2) == 1)
                        {
                            strollDir = -1;
                        }
                        else
                        {
                            strollDir = 1;
                        }
                        stateDuration = Random.Range(1f, 3f);
                        break;
                }
                break;

            case "idle":
                SetXVelocity(0f);
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
                if (stateLock == 0f && stateDuration == 0f)
                {
                    state = "findNextState";
                }

                break;

            case "stroll":
                if (seesTarget && Mathf.Abs(targetObject.transform.position.x - transform.position.x) > 1)
                {
                    strollDir = Mathf.Sign(targetObject.transform.position.x - transform.position.x);
                }
                rg2d.velocity = new Vector2(strollDir * strollSpeed, rg2d.velocity.y);
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

                    wallAhead = Physics2D.OverlapCircle(wallCheck.position, 0.1f, whatIsGround);
                    if (wallAhead && isGrounded)
                    {
                        rg2d.AddForce(new Vector2(0f, 17f), ForceMode2D.Impulse);
                    }
                    if (Mathf.Abs(targetObject.transform.position.x - transform.position.x) > 4f)
                    {
                        rg2d.velocity = new Vector2(Mathf.Sign(targetObject.transform.position.x - transform.position.x) * speed, rg2d.velocity.y);
                    }
                    else
                    {
                        rg2d.velocity = new Vector2(0, rg2d.velocity.y);
                    }
                    forwardsGroundCheckHit = (Physics2D.Raycast(forwardsGroundCheck.position, new Vector2(0, -1), .1f, whatIsGround));
                    if (!forwardsGroundCheckHit)
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

                }
                else
                {
                    state = "findNextState";
                    DoStateStep();
                }
                break;
            case "flee":
                if (targetObject != null)
                {
                    wallAhead = Physics2D.OverlapCircle(wallCheck.position, 0.1f, whatIsGround);
                    if (wallAhead && isGrounded)
                    {
                        rg2d.AddForce(new Vector2(0f, 17f), ForceMode2D.Impulse);
                    }
                    if (!inPhysicsMode)
                    {
                        if (Mathf.Abs(targetObject.transform.position.x - transform.position.x) < 7.5f)
                        {
                            rg2d.velocity = new Vector2(Mathf.Sign(transform.position.x - targetObject.transform.position.x) * speed * fleeSpeedModifier, rg2d.velocity.y);
                        }
                        else
                        {
                            rg2d.velocity = new Vector2(0, rg2d.velocity.y);
                        }
                        if (!seesTarget)
                        {
                            state = "findNextState";
                        }
                    }
                    forwardsGroundCheckHit = (Physics2D.Raycast(forwardsGroundCheck.position, new Vector2(0, -1), .1f, whatIsGround));
                    if (forwardsGroundCheckHit.collider == null || stateLock == 0f)
                    {
                        if(forwardsGroundCheckHit.collider == null)
                        {
                            state = "stroll";
                            anim.SetBool("isAttacking", false);
                            strollSpeed = rg2d.velocity.x;
                            strollDir = (facingRight) ? 1 : -1;
                            stateLock = .1f;
                            DoStateStep();
                            break;
                        }
                        state = "idle";
                        anim.SetBool("isAttacking", false);
                        stateDuration  = .01f;
                        break;
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


                }
                else
                {
                    state = "findNextState";
                    DoStateStep();
                }
                break;
            case "attack":
                anim.SetBool("isAttacking", true);
                if (targetObject != null)
                {
                    /*
                    if (targetObject.transform.position.x > transform.position.x && !facingRight)
                    {
                        Flip();
                    }
                    else if (targetObject.transform.position.x < transform.position.x && facingRight)
                    {
                        Flip();
                    }
                    */
                }
                break;
            case "hurt":
                if (isGrounded && stateLock == 0f)
                {
                    float distance = 2f;
                    if (targetObject != null)
                    {
                        distance = Vector3.Distance(targetObject.transform.position, transform.position);
                    }
                    if (distance < 2f)
                    {
                        playerCheckVars.SetRange("search");
                        anim.SetBool("isAttacking", false);
                        state = "flee";
                        stateLock = .5f;
                        seesTarget = true;
                        ExitPhysicsMode();
                    }
                    else
                    {
                        playerCheckVars.SetRange("search");
                        anim.SetBool("isAttacking", false);
                        state = "findNextState";
                        seesTarget = true;
                        ExitPhysicsMode();
                    }
                }
                break;

            case "ded":
                hitStunned = false;
                anim.SetBool("isHurt", false);
                gameObject.layer = 27;
                if (rg2d.sharedMaterial.friction == 0)
                {
                    EnterPhysicsMode();
                }
                break;

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
        }
        else
        {
            return -1f;
        }
    }
    public void SetXVelocity(float value)
    {
        rg2d.velocity = new Vector2(value, rg2d.velocity.y);
    }
    public string GetState()
    {
        return state;
    }
    public void SetState(string value)
    {
        state = value;

    }
    public float GetStateLock()
    {
        return stateLock;
    }
    public void SetStateLock(float value)
    {
        stateLock = value;
    }
    public void SetTargetObject(GameObject obj)
    {
        targetObject = obj;

    }
    public void SetHitStun()
    {
        hitStunned = true;
    }
    void EndHitStun() //Called in hurt2 anim
    {
        anim.SetBool("isHurt", false);
        hitStunned = false;
        if (isGrounded && stateLock == 0f)
        {
            float distance = Vector3.Distance(targetObject.transform.position, transform.position);
            if (distance < 2)
            {
                playerCheckVars.SetRange("search");
                anim.SetBool("isAttacking", false);
                state = "flee";
                stateLock  = .5f;
                seesTarget = true;
                ExitPhysicsMode();
            }
            else
            {
                playerCheckVars.SetRange("search");
                anim.SetBool("isAttacking", false);
                state = "findNextState";
                seesTarget = true;
                ExitPhysicsMode();
            }
        }

    }
    public void ApplyKb()
    {
        SetXVelocity(0f);
        EnterPhysicsMode();
        rg2d.AddForce(incomingKb, ForceMode2D.Impulse);
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
    public void DeathDeElevation()
    {
        polyColl.enabled = false;
        boxColl.enabled = true;
    }
    void EndAttack()
    {
        state = "flee";
        anim.SetBool("isAttacking", false);
        stateLock = 1f;
        playerCheckVars.SetRange("search");
        seesTarget = true;
        ExitPhysicsMode();
    }
    void Shoot()
    {
        bulletFired = Instantiate(projObject);
        projVars = bulletFired.GetComponent<GreenGolemProjController>();
        projVars.Setup(8 * Mathf.Sign(transform.localScale.x), damage, 7f);
        projVars.transform.position = gunBarrel.position;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public KeyCode rightkey;
    public KeyCode leftkey;
    public KeyCode jumpkey;
    public float speed;
    private Rigidbody2D rg2d;
    private float horvalues;
    private Vector2 horVect;
    private bool facingRight;
    private Animator anim;
    [SerializeField]
    private float jumpPower;
    private bool currentlyJumping;
    public LayerMask whatIsGround;
    private float groundCheckRadius = 0.1f;
    private bool grounded;
    public Transform groundCheck;
    private bool attacking;
    private bool canCombo;
    private int attackType;
    public GameObject damageObj;
    private GameObject attackObj;
    public GameObject hurtParticles;
    private DamageScript attackvars;
    /*
     *Attack Type List:
        0. None
        1. Slap (basic attack)
    */
    private bool canKnockback;
    private bool canControl;
    private bool isHurt;
    private float hurtDuration;
    private PhysicsMaterial2D playerMaterial;
    private float hp;
    public float Hp
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value;
            health.Current = value;
        }
    }
    Vector2 incomingKb;
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
    [SerializeField]
    Stat health;
	void Start () {
        canControl = true;
        attacking = false;
        canCombo = false;
        rg2d = GetComponent<Rigidbody2D>();	
        horvalues = 0f;
        currentlyJumping = false;
        facingRight  = true;
        canKnockback  = true;
        isHurt =  false;
        hurtDuration = 0f;
        hp = health.Maximum;
        health.Maximum = hp;
        health.Current = hp;
        anim = GetComponent<Animator>();
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius);
        playerMaterial = new PhysicsMaterial2D();
        rg2d.sharedMaterial = playerMaterial;
        ExitPhysicsMode();
    }
    private void Update()
    {
        if (canControl)
        {

            horvalues = Input.GetAxisRaw("Horizontal");
            if (Input.GetButtonDown("Fire1") && !attacking)
            {
                attacking = true;
                attackType = 1;
                anim.SetBool("Attack", true);
                anim.SetInteger("AttackType", 1);
            }
            if (attackType == 1 && canCombo && Input.GetButton("Fire1"))
            {
                anim.SetBool("Combo", true);
                anim.SetBool("Attack", true);
                anim.SetInteger("AttackType", 1);
                canCombo = false;
            }

            if (Input.GetKeyDown(jumpkey) && grounded && !attacking)
            {
                grounded = false;
                currentlyJumping = true;

                rg2d.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);

            }

            if (currentlyJumping && Input.GetKeyUp(jumpkey))
            {
                currentlyJumping = false;
                if (rg2d.velocity.y > 4)
                {
                    rg2d.velocity = new Vector2(rg2d.velocity.x, 4f);
                }
            }
        }
    }

    void FixedUpdate () {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        if (currentlyJumping && rg2d.velocity.y == 0f) {
            currentlyJumping = false;    

        }
        hurtDuration = Mathf.Max(hurtDuration - Time.deltaTime, 0f);
        if(grounded && hurtDuration == 0)
        {
            UnHurtPlayer();
        }
        if (!attacking && !isHurt)
        {
            rg2d.velocity = new Vector2(horvalues * speed, rg2d.velocity.y);
        } else if (!isHurt)
        {
            rg2d.velocity = new Vector2(horvalues * speed* .5f, rg2d.velocity.y);
        }
        anim.SetFloat("Speed", Mathf.Abs(horvalues));
        anim.SetFloat("vSpeed", rg2d.velocity.y);
        anim.SetBool("Ground", grounded);
        if (horvalues > 0 && !facingRight && !attacking)
        {
            Flip();
            
        } else if (horvalues < 0 && facingRight && !attacking)
        {
            Flip();
        }
	}
    
    void Flip()
    {
        Vector3 currentScale  = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
        facingRight = !facingRight;
    }
    void SlapLunge()
    {
        attackObj = Instantiate(damageObj, transform);
        attackvars = attackObj.GetComponent<DamageScript>();
        rg2d.velocity = new Vector2(0f, 0f);
        if (facingRight)
        {
            //rg2d.AddForce(new Vector2(lungePower, 0));
            if (!anim.GetBool("Combo"))
            {
                float kbAngle = 45f;
                attackvars.Setup(10f, 4f, "Good", new Vector2(1.3f, 0.6f), new Vector2(0.4f, -0.2f), 1, kbAngle);
            }
            else {
                float kbAngle = 45f;
                attackvars.Setup(10f, 16f, "Good", new Vector2(1.3f, 0.6f), new Vector2(0.4f, -0.2f), 1, kbAngle);

            }
            
        }

        if (!facingRight)
        {
            //rg2d.AddForce(new Vector2(-lungePower, 0));
            if (!anim.GetBool("Combo"))
            {
                float kbAngle = -45f;
                attackvars.Setup(10f, -4f, "Good", new Vector2(1.3f, 0.6f), new Vector2(0.4f, -0.2f), 1, kbAngle);
            }
            else
            {
                float kbAngle = -45f;
                //Debug.Log(kbAngle);
                attackvars.Setup(10f, -16f, "Good", new Vector2(1.3f, 0.6f), new Vector2(0.4f, -0.2f), 1, kbAngle);

            }
           
          
        }
        attackObj.transform.SetParent(transform);
    }
    void ComboOpportunity() {
        canCombo = true;    

    }
    void ResumeCombo()
    {
        //Opportunity to change direction between hits
        if (horvalues > 0 && !facingRight)
        {
            Flip();

        }
        else if (horvalues < 0 && facingRight)
        {
            Flip();
        }
    }
    void AttackEnd()
    {
        attacking = false;
        canCombo = false;
        attackType = 0;
        anim.SetBool("Attack", false);
        anim.SetBool("Combo", false);
        anim.SetInteger("AttackType", 0);

    }

    public void HurtPlayer()
    {
        isHurt = true;
        anim.SetBool("isHurt", true);
        hurtDuration = .25f;
        rg2d.velocity = new Vector2(0, rg2d.velocity.y);
        EnterPhysicsMode();
    }

    public void UnHurtPlayer()
    {
        isHurt = false;
        anim.SetBool("isHurt", false);
        ExitPhysicsMode();
    }

    public void EnterPhysicsMode()
    {
        rg2d.sharedMaterial.friction = .6f;
    }

    public void ExitPhysicsMode()
    {
        rg2d.sharedMaterial.friction = 0f;
    }

    public void ApplyKb()
    {
        rg2d.AddForce(incomingKb, ForceMode2D.Impulse);
    }
    public void EnableControl()
    {
        canControl = true;
    }
    public void DisableControl()
    {
        canControl = false;
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
}

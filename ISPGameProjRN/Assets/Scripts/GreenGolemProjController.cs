using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenGolemProjController : MonoBehaviour {
    float speed;
    public Transform hitboxOrigin;
    public GameObject dmgObject;
    private Rigidbody2D rg2d;
    private Animator anim;
    public LayerMask whatHitsThis;
    GameObject attackObj;
    public GameObject trailParticlesObj;
    public GameObject hitParticlesObj;
    DamageScript attackvars;
    ParticleSystem trailParticles;
    ParticleSystem hitParticles;
    float damage;
    float knockback;
    bool hitCheck;
    bool hasHit;
    bool facingRight;
	
	void Start () {
        rg2d = GetComponent<Rigidbody2D>();
        rg2d.velocity = new Vector2(speed, 0);
        anim = GetComponent<Animator>();
        hasHit = false;
        facingRight = true;
        if (rg2d.velocity.x > 0f && !facingRight)
        {
            Flip();
        }
        else if (rg2d.velocity.x < 0f && facingRight)
        {
            Flip();
        }
        trailParticles = Instantiate(trailParticlesObj, transform).GetComponent<ParticleSystem>();
        trailParticles.transform.position = transform.position;
    }

    private void FixedUpdate()
    {
        if (rg2d.velocity.x > 0f && !facingRight)
        {
            Flip();
        }
        else if (rg2d.velocity.x < 0f && facingRight)
        {
            Flip();
        }
        hitCheck = Physics2D.OverlapCircle(hitboxOrigin.position, 0.1f, whatHitsThis);
        if (hitCheck && !hasHit)
        {
            anim.SetBool("hit", true);
            hasHit = true;
            attackObj = Instantiate(dmgObject, transform);
            attackvars = attackObj.GetComponent<DamageScript>();
            attackvars.Setup(damage, knockback * Mathf.Sign(rg2d.velocity.x), "Evil", new Vector2(.02f, .02f), new Vector2(0.1f, 0.01f), 1, 10f * Mathf.Sign(rg2d.velocity.x));
            trailParticles.Stop();
            hitParticles = Instantiate(hitParticlesObj, transform).GetComponent<ParticleSystem>();
            hitParticles.transform.position = transform.position;
        }
    }
    void Flip()
    {
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
        facingRight = !facingRight;
    }
    public void Setup(float spd, float dmg, float kb)
    {
        speed = spd;
        damage = dmg;
        knockback = kb;
    }
    void Dissipate()
    {
        
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageScript : MonoBehaviour {

    float damage;
    Vector2 dmgSize;
    Vector2 dmgOffset;
    string alignment;
    float knockback;
    float knockBackAngle;
    int lifetime;
    GameObject target;
    PlayerController playerScript;
    RedGolemController rGolemScript;
    GreenGolemController gGolemScript;
    PropController propScript;
    BoxCollider2D range;
    bool autoCalckb;
    /* Good - player, player created instances
     * Evil - Enemies
     * Neutral - inanimate destructible objects
    */


	void Start () {
	}
	
	
	void FixedUpdate () {
        if (transform.parent != null)
        {
            transform.position = transform.parent.position;
        }
        lifetime--;
        if (lifetime < 0)
        {
            Destroy(gameObject);
        }
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody2D>() != null)
        {
            if (!collision.gameObject.tag.Contains(alignment + ","))
            {
                if (autoCalckb)
                {
                    knockBackAngle = Vector2.Angle(new Vector2(transform.position.x, transform.position.y), new Vector2(collision.transform.position.x, collision.transform.position.y));
                }

                target = collision.gameObject;

                if (target.tag.Contains("Player,"))
                {
                    playerScript = target.GetComponent<PlayerController>();
                    playerScript.Hp -= damage;
                    playerScript.HurtPlayer();
                    playerScript.IncomingKb = new Vector2(knockback, 0).Rotate(knockBackAngle);
                    playerScript.Invoke("ApplyKb", 0.01f);
                    GameObject particles = Instantiate(playerScript.hurtParticles);
                    particles.transform.position = playerScript.transform.position;
                    particles.transform.localScale = new Vector3(particles.transform.localScale.x * playerScript.GetDir(), particles.transform.localScale.y, particles.transform.localScale.z);
                }
                else if (target.tag.Contains("Prop,"))
                {
                    propScript = target.GetComponent<PropController>();
                    propScript.hp -= damage;
                } else if (target.tag.Contains("RedGolem,"))
                {
                    rGolemScript = collision.gameObject.GetComponent<RedGolemController>();
                    rGolemScript.hp -= damage;
                    rGolemScript.SetHitStun();
                    rGolemScript.SetTargetObject(transform.parent.gameObject);                                               
                    rGolemScript.IncomingKb = new Vector2(knockback, 0).Rotate(knockBackAngle);
                    rGolemScript.Invoke("ApplyKb", 0.01f);
                    GameObject particles = Instantiate(rGolemScript.hurtParticles);
                    particles.transform.position = rGolemScript.transform.position;
                    particles.transform.localScale = new Vector3(particles.transform.localScale.x*rGolemScript.GetDir(), particles.transform.localScale.y, particles.transform.localScale.z);
                
                } else if (target.tag.Contains("GreenGolem,"))
                {
                    gGolemScript = collision.gameObject.GetComponent<GreenGolemController>();
                    gGolemScript.hp -= damage;
                    gGolemScript.SetHitStun();
                    gGolemScript.SetTargetObject(transform.parent.gameObject);
                    gGolemScript.IncomingKb = new Vector2(knockback, 0).Rotate(knockBackAngle);
                    gGolemScript.Invoke("ApplyKb", 0.01f);
                    GameObject particles = Instantiate(gGolemScript.hurtParticles);
                    particles.transform.position = gGolemScript.transform.position;
                    particles.transform.localScale = new Vector3(particles.transform.localScale.x * gGolemScript.GetDir(), particles.transform.localScale.y, particles.transform.localScale.z);

                }
                /*
                 *  -------------Template for What the added force is---------------------
                 * #use invoke to put a small delay to avoid bugs that involve controller and damage script running simutaneously
                 * otherRigid.AddForce(new Vector2(knockback, 0).Rotate(knockBackAngle), ForceMode2D.Impulse);
                 * */    
                Physics2D.IgnoreCollision(range, collision);
            }
        }
    }
    public void Setup(float dmg, float kb, string align, Vector2 size, Vector2 offset, int duration)
    {
        range = GetComponent<BoxCollider2D>();
        damage = dmg;
        knockback = kb;
        alignment = align;
        dmgSize = size;
        dmgOffset = offset;
        lifetime = duration;
        range.size = dmgSize;
        range.offset = dmgOffset;
        autoCalckb = true;
        
    }
    public void Setup(float dmg, float kb, string align, Vector2 size, Vector2 offset, int duration, float angle)
    {
        range = GetComponent<BoxCollider2D>();
        damage = dmg;
        knockback = kb;
        alignment = align;
        dmgSize = size;
        dmgOffset = offset;
        lifetime = duration;
        knockBackAngle = angle;
        range.size = dmgSize;
        range.offset = dmgOffset;
        autoCalckb = false;
    }
    
        
    
}

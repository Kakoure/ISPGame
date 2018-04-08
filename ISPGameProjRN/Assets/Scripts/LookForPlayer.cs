using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookForPlayer : MonoBehaviour {

    private GameObject owner;
    private RedGolemController rGolemScript;
    private GreenGolemController gGolemScript;
    private BoxCollider2D range;
    private Vector2 searchSize;
    private Vector2 searchOffset;
    private Vector2 attackRange;
    private Vector2 attackOffset;
    private string parenType;
	void Start () {

        owner = transform.parent.gameObject;
        range = GetComponent<BoxCollider2D>();
        if (owner.tag.Contains("RedGolem,")){
            parenType = "RedGolem";
        } else if (owner.tag.Contains("GreenGolem,"))
        {
            parenType = "GreenGolem";
        }
        switch (parenType)
        {
            case "RedGolem":
                rGolemScript = owner.GetComponent<RedGolemController>();
                searchSize = new Vector2(5f, 2.25f);
                searchOffset = new Vector2(2.25f, 0f);
                attackRange = new Vector2(1f, 1f);
                attackOffset = new Vector2(.5f, 0f);
                SetRange("search");
                break;
            case "GreenGolem":
                gGolemScript = owner.GetComponent<GreenGolemController>();
                searchSize = new Vector2(6f, 2.25f);
                searchOffset = new Vector2(2.5f, 0.0f);
                attackRange = new Vector2(8f, .2f);
                attackOffset = new Vector2(3.8f, 0f);
                SetRange("search");
                break;

        }
	}

    private void OnTriggerStay2D(Collider2D collision)
    {
        DoCollisionStuff(collision);
    }
    
    void DoCollisionStuff(Collider2D collision)
    {
        if (collision.gameObject.tag.Contains("Player,"))
        {
            //Debug.Log("ALERT");
            switch (parenType)
            {
                case "RedGolem":
            
                    switch (rGolemScript.GetState())
                    {
                        case "findNextState":
                            if (rGolemScript.GetStateLock() > 0)
                            {
                                break;
                            }
                            else
                            {
                                rGolemScript.SetTargetObject(collision.gameObject);
                                rGolemScript.SetState("pursuit");
                                rGolemScript.SetStateLock(.05f);
                                range.size = attackRange;
                                range.offset = attackOffset;

                            }
                            break;
                        case "idle":
                            if (rGolemScript.GetStateLock() > 0)
                            {
                                break;
                            }
                            else
                            {
                                rGolemScript.SetTargetObject(collision.gameObject);
                                rGolemScript.SetState("pursuit");
                                rGolemScript.SetStateLock(.05f);
                                range.size = attackRange;
                                range.offset = attackOffset;
                                
                            }
                            break;
                        case "stroll":
                            rGolemScript.SetTargetObject(collision.gameObject);
                            rGolemScript.SetState("pursuit");
                            rGolemScript.SetStateLock(.05f);
                            range.size = attackRange;
                            range.offset = attackOffset;
                            break;
                        case "flee":
                            if (rGolemScript.GetStateLock() > 0)
                            {
                                break;
                            }
                            else
                            {
                                rGolemScript.SetTargetObject(collision.gameObject);
                                rGolemScript.SetState("pursuit");
                                rGolemScript.SetStateLock(.05f);
                                range.size = attackRange;
                                range.offset = attackOffset;

                            }
                            break;
                        case "pursuit":
                            if (rGolemScript.GetStateLock() == 0f)
                            {
                                rGolemScript.SetState("attack");
                                rGolemScript.SetXVelocity(0f);
                                rGolemScript.EnterPhysicsMode();
                            }
                            break;
                    }

                    break;

                case "GreenGolem":
                    switch (gGolemScript.GetState())
                    {
                        case "findNextState":
                            if (gGolemScript.GetStateLock() > 0)
                            {
                                break;
                            } else
                            {
                                gGolemScript.SetTargetObject(collision.gameObject);
                                gGolemScript.SetState("pursuit");
                                gGolemScript.SetStateLock(.05f);
                                range.size = attackRange;
                                range.offset = attackOffset;

                            }

                            break;
                        case "idle":
                            if (gGolemScript.GetStateLock() > 0)
                            {
                                break;
                            }
                            else
                            {
                                gGolemScript.SetTargetObject(collision.gameObject);
                                gGolemScript.SetState("pursuit");
                                gGolemScript.SetStateLock(.05f);
                                range.size = attackRange;
                                range.offset = attackOffset;

                            }
                            break;
                        case "stroll":
                            if (gGolemScript.GetStateLock() > 0)
                            {
                                break;
                            }
                            else
                            {
                                gGolemScript.SetTargetObject(collision.gameObject);
                                gGolemScript.SetState("pursuit");
                                gGolemScript.SetStateLock(.05f);
                                range.size = attackRange;
                                range.offset = attackOffset;

                            }
                            break;
                        case "pursuit":
                            if(gGolemScript.GetStateLock() == 0f)
                            {
                                gGolemScript.SetState("attack");
                                gGolemScript.SetXVelocity(0f);
                                gGolemScript.EnterPhysicsMode();
                            }

                            break;
                    }

                    break;
            }
        }
        else
        {
            
        }
        //Debug.Log(range.size);
    }

    public void SetRange(Vector2 size, Vector2 offset)
    {
        range.size = size;
        range.offset = offset;
    }
    public void SetRange(string input)
    {
        if (input == "search")
        {
            range.size = searchSize;
            range.offset = searchOffset;
        } else if (input == "pursuit")
        {
            range.size = attackRange;
            range.offset = attackOffset;
        }
    }
}

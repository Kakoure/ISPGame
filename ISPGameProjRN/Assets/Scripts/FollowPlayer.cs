using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    private Vector3 offset;
    public GameObject target;
    private BgScroll[] backgroundsToScroll;	
	void Start () {
        offset = transform.position - target.transform.position;
        backgroundsToScroll = FindObjectsOfType<BgScroll>();
	}
	
	
	void LateUpdate () {
        transform.position = target.transform.position + offset;
        for (int i = 0; i < backgroundsToScroll.Length; i++)
        {
            backgroundsToScroll[i].DoBGScroll();
        }
	}
}

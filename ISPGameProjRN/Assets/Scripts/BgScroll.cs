using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgScroll : MonoBehaviour {

    public Transform cameraTransform;
    public float imageWidth;
    public float imageYLock;
    private float trueYPos;
    private float yTrack;
    private Transform[] subParts;
    private Vector3 targetPos;
    public float parallaxSpeed;
    public float speedModifierY;
    private float viewZone = 18;
    private int leftIndex;
    private int rightIndex;
    private float lastCameraX;
    private float lastCameraY;
    private float camDeltaX;
    private float camDeltaY;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        transform.position = new Vector3(cameraTransform.position.x, cameraTransform.transform.position.y, transform.position.z);
        targetPos = transform.position;
        trueYPos = transform.position.y;
        subParts = new Transform[transform.childCount];
        for(int i = 0; i < subParts.Length; i++)
        {
            subParts[i] = transform.GetChild(i);
        }
        leftIndex = 0;
        rightIndex = subParts.Length - 1;
        lastCameraX = cameraTransform.position.x;
        lastCameraY = cameraTransform.position.y;
        yTrack = 0;
    }
   /* private void FixedUpdate()
    {
        camDeltaX = cameraTransform.position.x - lastCameraX;
        camDeltaY = cameraTransform.position.y - lastCameraY;
        lastCameraX = cameraTransform.position.x;
        lastCameraY = cameraTransform.position.y;
        yTrack += (camDeltaY * parallaxSpeed * speedModifierY);
        Debug.Log(yTrack);
        trueYPos = FindFill(Mathf.Clamp(yTrack, -3, 3), -3, 3, -imageYLock, imageYLock);
        targetPos = new Vector3(targetPos.x + (camDeltaX * parallaxSpeed), cameraTransform.position.y);
        transform.position = targetPos;
        //trueYPos += (camDeltaY * parallaxSpeed * speedModifierY);
        //transform.position = new Vector3(transform.position.x + (camDeltaX * parallaxSpeed), Mathf.Clamp(trueYPos, cameraTransform.position.y - imageYLock, cameraTransform.position.y + imageYLock));
        if (cameraTransform.position.x < subParts[leftIndex].transform.position.x + viewZone)
        {
            ScrollLeft();
        }
        if (cameraTransform.position.x > subParts[rightIndex].transform.position.x - viewZone)
        {
            ScrollRight();
        }
    }
    */
    public void DoBGScroll()
    {
        camDeltaX = cameraTransform.position.x - lastCameraX;
        camDeltaY = cameraTransform.position.y - lastCameraY;
        lastCameraX = cameraTransform.position.x;
        lastCameraY = cameraTransform.position.y;
        yTrack -= (camDeltaY * parallaxSpeed * speedModifierY);
        //Debug.Log(yTrack);
        trueYPos = FindFill(Mathf.Clamp(yTrack, -5, 5), -5, 5, -imageYLock, imageYLock);
        targetPos = new Vector3(targetPos.x + (camDeltaX * parallaxSpeed), cameraTransform.position.y + trueYPos, transform.position.z);
        transform.position = targetPos;
        //trueYPos += (camDeltaY * parallaxSpeed * speedModifierY);
        //transform.position = new Vector3(transform.position.x + (camDeltaX * parallaxSpeed), Mathf.Clamp(trueYPos, cameraTransform.position.y - imageYLock, cameraTransform.position.y + imageYLock));
        if (cameraTransform.position.x < subParts[leftIndex].transform.position.x + viewZone)
        {
            ScrollLeft();
        }
        if (cameraTransform.position.x > subParts[rightIndex].transform.position.x - viewZone)
        {
            ScrollRight();
        }
    }
    void ScrollRight()
    {
        subParts[leftIndex].position = new Vector3(subParts[rightIndex].position.x + imageWidth, subParts[rightIndex].position.y, transform.position.z);
        leftIndex = rightIndex;
        rightIndex--;
        if(rightIndex < 0)
        {
            rightIndex = subParts.Length - 1;
        }
    }
    void ScrollLeft()
    {
        subParts[rightIndex].position = new Vector3(subParts[leftIndex].position.x - imageWidth, subParts[leftIndex].position.y, transform.position.z);
        rightIndex = leftIndex;
        leftIndex++;
        if (leftIndex == subParts.Length)
        {
            leftIndex = 0;
        }
    }
    float FindFill(float value, float barMin, float barMax, float fillMin, float fillMax)
    {
        float ret;
        ret = ((value - barMin) / (barMax - barMin)) * (fillMax - fillMin) + fillMin;
        return ret;
    }
}

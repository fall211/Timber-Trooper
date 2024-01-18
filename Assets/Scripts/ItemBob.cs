using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBob : MonoBehaviour
{
    public float bobSpeed = 1f;
    public float rotateSpeed = 100f;
    public float maxScale = 3f;
    public float minScale = 2f;

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(1,1,1) * Mathf.Sin(Time.time * bobSpeed)/2 + new Vector3(1,1,1) * (maxScale + minScale)/2;
        transform.Rotate(0, Time.deltaTime * rotateSpeed, 0);
    }
}

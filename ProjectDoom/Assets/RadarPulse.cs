using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarPulse : MonoBehaviour
{
    private Transform pulseTransform;

    private float range;
    private float rangeMax;

    private List<Collider> alreadyPingedColliderList;

    private void Awake()
    {
        pulseTransform = transform.Find("RadarPulseSprite");
        rangeMax = 25f;

        alreadyPingedColliderList = new List<Collider>();
    }

    private void Update()
    {
        float rangeSpeed = 12.5f;
        range += rangeSpeed * Time.deltaTime;
        if (range > rangeMax)
        {
            range = 0f;
            alreadyPingedColliderList.Clear();
        }
        pulseTransform.localScale = new Vector3(range, range);

        RaycastHit[] raycastHitArray = Physics.SphereCastAll(transform.position, range / 2f, Vector3.zero);
        foreach(RaycastHit raycastHit in raycastHitArray)
        {
            if (raycastHit.collider != null)
            {
                //Hit something
                if (alreadyPingedColliderList.Contains(raycastHit.collider))
                {
                    alreadyPingedColliderList.Add(raycastHit.collider);
                    Debug.Log("Yee");
                }
            }
        }
    }
}

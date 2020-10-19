using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarPulse : MonoBehaviour
{
    [SerializeField] private Transform radarPingPf;
    [SerializeField] private Transform playerTransform;

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

        Collider[] raycastHitArray = Physics.OverlapSphere(playerTransform.transform.position, range / 2f);
        foreach(Collider collider in raycastHitArray)
        {
            if (collider != null)
            {
                //Debug.Log("yee");
                //Hit something
                if (!alreadyPingedColliderList.Contains(collider))
                {
                    alreadyPingedColliderList.Add(collider);
                    Debug.Log("Yee");

                    Transform radarPingTransform = Instantiate(radarPingPf, collider.transform.localPosition, Quaternion.identity);
                    RadarPing radarPing = radarPingTransform.GetComponent<RadarPing>();
                    
                    if(collider.gameObject.GetComponent<HealthPickup>() != null)
                    {
                        //Hit health object
                        radarPing.SetColor(new Color(0, 1, 0));
                    }
                    
                }
            }
        }
    }
}

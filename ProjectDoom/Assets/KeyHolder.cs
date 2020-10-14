using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyHolder : MonoBehaviour
{
    public event EventHandler OnKeysChanged;

    public List<Key.KeyType> keyList;

    public void Awake()
    {
        keyList = new List<Key.KeyType>();
    }

    public List<Key.KeyType> GetKeyList()
    {
        return keyList;
    }

    public void AddKey(Key.KeyType keyType)
    {
        Debug.Log("Added Key: " + keyType);
        keyList.Add(keyType);
        OnKeysChanged?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveKey(Key.KeyType keyType)
    {
        keyList.Remove(keyType);
        OnKeysChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool ConstainsKey(Key.KeyType keyType)
    {
        return keyList.Contains(keyType);
    }

    public void OnTriggerEnter(Collider collider)
    {
        Key key = collider.GetComponent<Key>();
        if(key != null)
        {
            AddKey(key.GetKeyType());
            Destroy(key.gameObject);
        }

        //if(Input.GetKeyDown(KeyCode.E))
        //{
            //KeyDoor keyDoor = collider.GetComponent<KeyDoor>();
            //DoorInterface doorInterface = GetComponent<DoorInterface>();
            //if (keyDoor != null)
            //{
                //if (ConstainsKey(keyDoor.GetKeyType()))
                //{
                    //Debug.Log("yee");
                    //doorInterface.OpenDoor();
                //}
            //}
        //} 
    }
}

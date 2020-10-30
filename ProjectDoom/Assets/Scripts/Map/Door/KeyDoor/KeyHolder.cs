using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyHolder : MonoBehaviour
{
    public event EventHandler OnKeysChanged;

    private List<Key.KeyType> keyList;

    private void Awake()
    {
        //Make the list
        keyList = new List<Key.KeyType>();
    }

    private List<Key.KeyType> GetKeyList()
    {
        return keyList;
    }

    public void AddKey(Key.KeyType keyType)
    {
        //Add key
        Debug.Log("Added Key: " + keyType);
        keyList.Add(keyType);
        OnKeysChanged?.Invoke(this, EventArgs.Empty);
    }

    private void RemoveKey(Key.KeyType keyType)
    {
        //Remove key if needed
        keyList.Remove(keyType);
        OnKeysChanged?.Invoke(this, EventArgs.Empty);
    }

    private bool ConstainsKey(Key.KeyType keyType)
    {
        //Check if player has the key
        return keyList.Contains(keyType);
    }

    private void OnTriggerEnter(Collider collider)
    {
        //If player enters the key trigger then pickup key
        Key key = collider.GetComponent<Key>();
        if(key != null)
        {
            AddKey(key.GetKeyType());
            Destroy(key.gameObject);
        }
    }

    public void KeyDoor(Collider collider)
    {
        //Open door that requires key
        KeyDoor keyDoor = collider.GetComponent<KeyDoor>();
        DoorInterface doorInterface = collider.GetComponent<DoorInterface>();
        if (keyDoor != null)
        {
            if (ConstainsKey(keyDoor.GetKeyType()))
            {
                doorInterface.OpenDoor();
            }
        }
    }
}

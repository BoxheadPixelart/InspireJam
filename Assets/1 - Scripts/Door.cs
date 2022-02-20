using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour
{
    public enum Direction
    {
        North,
        South,
        East,
        West
    }
    [OnValueChanged("SetDoorDirection")]
    public Direction currentDirection;
    public LayerMask layerMask;
    public DoorCollider doorCollider;
    
        
        
    // Start is called before the first frame update
    void Start()
    {
        doorCollider.DoorEnterEvent.AddListener(EnterDoor);
        doorCollider.DoorExitEvent.AddListener(ExitDoor);
        Invoke("SetColliderActive",1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SetColliderActive()
    {
        doorCollider.gameObject.SetActive(true);
    }
    

    void SetDoorDirection()
    {
        Vector3 forward; 
        switch (currentDirection)
        {
            case Direction.North:
                forward = Vector3.forward;
                break;
            case Direction.South:
                forward = Vector3.back;
                break;
            case Direction.East:
                forward = Vector3.right;
                break;
            case Direction.West:
                forward = Vector3.left;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        } 
        transform.rotation = Quaternion.LookRotation(forward,Vector3.up);
    }
    void CollectOffset()
    {
        Room room = gameObject.GetComponentInParent<Room>();
    }

    public void EnterDoor(DoorCollider.DoorInfo doorInfo)
    {
        RoomSpawner.instance.activeRoom = doorInfo.room;
        RoomSpawner.instance.SpawnRooms();
    }
    public void ExitDoor(DoorCollider.DoorInfo doorInfo)
    {
      //  RoomSpawner.instance.SpawnRooms();
    }
}

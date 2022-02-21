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
    public float distFromPlayer;
    public bool playerPassthough; 
    public bool isOpen;
    public float openDist; 
    [SerializeField]
    public static List<Door> allDoors = new List<Door>();
    public Animator animator;
    


    private void OnEnable()
    {
        if(!allDoors.Contains(this))
            allDoors.Add(this);
    }
    private void OnDisable()
    {
        if(allDoors.Contains(this))
            allDoors.Remove(this); 
    }
    // Start is called before the first frame update
    void Start()
    {
        doorCollider.DoorEnterEvent.AddListener(EnterDoor);
        doorCollider.DoorExitEvent.AddListener(ExitDoor);
      //  Invoke("SetColliderActive",.01f);
    }

    public void CollectDistFromPlayer()
    {
        
        distFromPlayer = Vector3.Distance(transform.position, RoomSpawner.instance.player.position); 
//        print(distFromPlayer + " is the distance from the player to " + gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOpen)
        {
            if (distFromPlayer < openDist)
            {
                OpenDoor(); 
            }
        }
        else
        {
            if (distFromPlayer > openDist)
            {
                CloseDoor();
            }
        }
    }

    public void OpenDoor()
    {
        isOpen = true; 
        animator.SetBool("OpenStatus",isOpen);
    }
    
    public void CloseDoor()
    {
        isOpen = false; 
        animator.SetBool("OpenStatus",false);
    }
    
    public void SetColliderActive()
    {
        gameObject.SetActive(true);
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
        //ForceCloseDoor(doorInfo); 
        
        
    }
    
    public void ExitDoor(DoorCollider.DoorInfo doorInfo)
    {
        if (doorInfo.side == DoorCollider.DoorInfo.Side.Front)
        {
            print("FRONT");
        }
        if (doorInfo.side == DoorCollider.DoorInfo.Side.Back)
        {
            print("BACK");
        }
        if (doorInfo.side == DoorCollider.DoorInfo.Side.Unknown)
        {
            print("WHO");
        }
      //  RoomSpawner.instance.SpawnRooms();
    }
    void ForceCloseDoor(DoorCollider.DoorInfo d)
    {
        animator.Play("Closing");
        animator.SetBool("OpenStatus",false);
        
        //MoveNextRoom(d);
    }
    void MoveNextRoom(DoorCollider.DoorInfo doorInfo)
    {
        RoomSpawner.instance.otherRooms.Add(RoomSpawner.instance.activeRoom);
        RoomSpawner.instance.otherRooms.Remove(doorInfo.room); 
        RoomSpawner.instance.activeRoom = doorInfo.room;
        RoomSpawner.instance.ClearOtherRooms();
        RoomSpawner.instance.SpawnRooms();
    }
}

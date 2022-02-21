using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class DoorCollider : MonoBehaviour
{
    private Door door;
    private Room room; 
    public class DoorInfo
    {
        public enum Side
        {
            Front,
            Back,
            Unknown
        }
        public Side side;
        public GameObject other;
        public Door door;
        public Room room; 
    }
    
    public UnityEvent<DoorInfo> DoorEnterEvent;
    public UnityEvent<DoorInfo> DoorExitEvent;
    public LayerMask layerMask;
    private void Awake()
    {
        Init();
    }
    private void OnTriggerEnter(Collider other)
    {
        
        DoorInfo hit = new DoorInfo();
        hit.door = door; 
        hit.other = other.gameObject;
        hit.room = room; 
        hit.side = GetSide(); 
        DoorEnterEvent.Invoke(hit);
    }
    private void OnTriggerExit(Collider other)
    {
        
        DoorInfo hit = new DoorInfo();
        hit.door = door; 
        hit.other = other.gameObject;
        hit.room = room; 
        hit.side = GetSide(); 
        DoorExitEvent.Invoke(hit);
    }

    public DoorInfo.Side GetSide()
    {
        RaycastHit frontHit;
        RaycastHit backHit;
        DoorInfo.Side side = DoorInfo.Side.Unknown;
        if (Physics.Raycast(transform.parent.position + Vector3.up, transform.forward, out frontHit, 10, layerMask))
        {
            side = DoorInfo.Side.Front; 
        }
        
        if (Physics.Raycast(transform.parent.position + Vector3.up,  -transform.forward, out backHit, 10, layerMask))
        {
            side = DoorInfo.Side.Back; 
        }
        return side;
    }
    [Button()]
    public void Init()
    {
        door = gameObject.GetComponentInParent<Door>(); //TODO BAKE THIS OUT LATER 
        room = gameObject.GetComponentInParent<Room>();
    }
}

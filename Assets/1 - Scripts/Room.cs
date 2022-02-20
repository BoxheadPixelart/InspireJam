using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Room : MonoBehaviour
{
    [Expandable]
    public RoomData roomData;
    public Transform doorParent;
    public List<Door> allDoors = new List<Door>(); 
    public List<Door> northDoors; 
    public List<Door> southDoors; 
    public List<Door> eastDoors; 
    public List<Door> westDoors;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Button()]
    void InitRoom()
    {
        CollectAllDoors();
        SortDoors();
    }
    [ContextMenu("CollectDoors")]
    void CollectAllDoors()
    {
        allDoors.Clear();
        foreach (Transform child in doorParent)
        {
            Door door = child.GetComponent<Door>();
            if (door)
            {
               allDoors.Add(door);
            }
        }
    }
    [ContextMenu("SortDoors")]
    void SortDoors()
    {
        northDoors.Clear();
        southDoors.Clear();
        eastDoors.Clear();
        westDoors.Clear();
        foreach (Door door in allDoors)
        {
            switch (door.currentDirection)
            {
                case Door.Direction.North:
                  northDoors.Add(door);
                    break;
                case Door.Direction.South:
                   southDoors.Add(door);
                    break;
                case Door.Direction.East:
                    eastDoors.Add(door);
                    break;
                case Door.Direction.West:
                    westDoors.Add(door);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
    }
[Button()]
    void ApplyData()
{
    int buildIndex = gameObject.scene.buildIndex;
    if (buildIndex > -1)
    {
        roomData.SetBuildIndex(buildIndex);
    }
    else
    {
        Exception e = new Exception("SCENE NOT IN BUILD: " + buildIndex); 
        Debug.LogException(e);
    }
    roomData.SetDoorCount(allDoors);
}
    
   
}

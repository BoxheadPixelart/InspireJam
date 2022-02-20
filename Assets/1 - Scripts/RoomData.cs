using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;


[CreateAssetMenu(fileName = "RoomData", menuName = "Data/RoomData", order = 0)]
    public class RoomData : ScriptableObject
{
    public int buildIndex;
    public int totalDoorCount;
    public int[] doorCount = new int[5]; //Index this with the enum. 





    public void SetBuildIndex(int num)
    {
        buildIndex = num; 
    }
    public void SetDoorCount(List<Door> allDoors)
    {
        doorCount = new int[5]; 
        foreach (Door door in allDoors)
        {
            switch (door.currentDirection)
            {
                case Door.Direction.North:
                    doorCount[0] += 1; 
                    break;
                case Door.Direction.South:
                    doorCount[1] += 1; 
                    break;
                case Door.Direction.East:
                    doorCount[2] += 1; 
                    break;
                case Door.Direction.West:
                    doorCount[3] += 1; 
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        doorCount[4] = allDoors.Count;
    }
}


using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

    [CreateAssetMenu(fileName = "RoomLib", menuName = "Data/RoomLib", order = 0)]
    public class RoomLibrary : ScriptableObject
    {
        public RoomData[] roomDatas;
        public List<RoomData> northRooms; //rooms with South Doors; 
        public List<RoomData> southRooms; //rooms with North Doors;
        public List<RoomData> eastRooms; //rooms with South Doors;
        public List<RoomData> westRooms; //rooms with South Doors;

        [Button()]
        public void SortRooms()
        {
            northRooms = new List<RoomData>();
            eastRooms = new List<RoomData>();
            westRooms = new List<RoomData>();
            southRooms = new List<RoomData>();
            
            foreach (RoomData roomData in roomDatas)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (roomData.doorCount[i] > 0)
                    {
                        this[(Door.Direction)i].Add(roomData);
                    }
                }
            }
        }
        
        public List<RoomData> this[Door.Direction choice]
        {
            get
            {
                switch (choice)
                {

                    case Door.Direction.North:
                        return southRooms; 
                        break;
                    case Door.Direction.South:
                        return northRooms; 
                        break;
                    case Door.Direction.East:
                        return westRooms; 
                        break;
                    case Door.Direction.West:
                        return eastRooms; 
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(choice), choice, null);
                }
            }
        }
    }


using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class RoomSpawner : MonoBehaviour
{
    public static RoomSpawner instance;
    public List<Room> otherRooms = new List<Room>();
    public UnityEvent SpawnRoomsEvent;
    public Room activeRoom;
    public RoomLibrary lib;
    public RoomData firstRoom;
    public Transform player;
    public int currentDoorIndex; 
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this; 
        InitFirstRoom(); 
        SpawnRooms();
        SpawnRoomsEvent.AddListener(SpawnRooms);
    }

    // Update is called once per frame
    void Update()
    {
        CollectDoorDistance();
    }

    void CollectDoorDistance()
    {
        if (currentDoorIndex > Door.allDoors.Count - 1)
        {
            currentDoorIndex = 0; 
        }
        currentDoorIndex += 1;
        Door.allDoors[currentDoorIndex].CollectDistFromPlayer();
    }
    void InitFirstRoom()
    {
        StartCoroutine(InitLoadRoutine()); 
    }
    [Button()]
   public void SpawnRooms()//TODO TIMESLICING
    {
        StartCoroutine(SpawnRoomRoutine()); 
        //TODO make code that picks rooms base on direction
    }
    [Button()]
    public void ClearOtherRooms()
    {
        int num = otherRooms.Count;
        for (int i = 0; i < num; i++)
        {
            //otherRooms[i].gameObject.scene; 
            SceneManager.UnloadSceneAsync(otherRooms[i].gameObject.scene);
        }
        otherRooms.Clear();
    }
    IEnumerator SpawnRoomRoutine()
    {
        //otherRooms.Clear();
        yield return new WaitForSeconds(0.01f); 
  
      //  RoomData choice = lib.roomDatas[0]; 
        foreach (Door door in activeRoom.allDoors)
        {
            int index = SceneManager.sceneCount;

           // if (door.currentDirection == Door.Direction.North)
            int randomIndex = Random.Range(0,lib[door.currentDirection].Count); 
            RoomData choice = lib[door.currentDirection][randomIndex]; 
            AsyncOperation op = LoadRoom(choice);
            yield return new WaitUntil(() => op.isDone);
            Room r = GetRoom(index);
            otherRooms.Add(r);
            //The order of the room spawn and clear
            switch (door.currentDirection)
            {
                case Door.Direction.North:
                    Vector3 nOff = r.southDoors[0].transform.localPosition;
                 //   print("total offset " + door.transform.localPosition + nOff);
                    r.transform.position = activeRoom.transform.position + (door.transform.localPosition - nOff);
                  //  print(r.name + "Has been moved");
                    break;
                case Door.Direction.South:
                    Vector3 sOff = r.northDoors[0].transform.localPosition;
                  //  print("total offset " + door.transform.localPosition + sOff);
                    r.transform.position = activeRoom.transform.position + (door.transform.localPosition - sOff); 
                  //  print(r.name + "Has been moved");
                    break;
                case Door.Direction.East:
                    Vector3 wOff = r.westDoors[0].transform.localPosition;
                  //  print("total offset " + door.transform.localPosition + wOff);
                    r.transform.position = activeRoom.transform.position + (door.transform.localPosition - wOff); 
                   // print(r.name + "Has been moved");
                    break;
                case Door.Direction.West:
                    Vector3 eOff = r.eastDoors[0].transform.localPosition;
                  //  print("total offset " + door.transform.localPosition + eOff);
                    r.transform.position = activeRoom.transform.position + (door.transform.localPosition - eOff); 
                  //  print(r.name + "Has been moved");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            r.SetNewPlace();
        }
        int num = activeRoom.allDoors.Count; 
        for (int i = 0; i < num; i++)
        {
            Destroy(activeRoom.allDoors[i].gameObject);
        }
        
        activeRoom.allDoors.Clear(); //TODO CLEAR
      //  ClearOtherRooms();
        yield return null; 
    }
    IEnumerator InitLoadRoutine()
    {
        Scene myScene = new Scene()
        {
            name = "FUCKTHISBROKEN"
        }; 
        int index = SceneManager.sceneCount;
        var op = LoadRoom(firstRoom);
        yield return new WaitUntil(() => op.isDone); 
        myScene = SceneManager.GetSceneAt(index);
        GameObject[] objs = myScene.GetRootGameObjects();
        activeRoom = objs[0].GetComponent<Room>();
        yield return null; 
    }
    AsyncOperation LoadRoom(RoomData data)
    {
        
        Scene myScene = new Scene()
        {
            name = "FUCKTHISBROKEN"
        }; 
       AsyncOperation operation = SceneManager.LoadSceneAsync(data.buildIndex, LoadSceneMode.Additive);
     
       return operation; 
    }
    Room GetRoom(int sceneIndex)
    {
        Scene myScene = SceneManager.GetSceneAt(sceneIndex);
        GameObject[] objs = myScene.GetRootGameObjects();
       return objs[0].GetComponent<Room>();
    }
}

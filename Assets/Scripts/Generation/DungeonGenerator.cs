using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;
using Random = System.Random;

public class DungeonGenerator : MonoBehaviour
{

    [Header("TileMaps")]
    public Tilemap floorTilemap;
    public Tilemap wallTilemap;

    [Header("Indicator Tiles")]
    public TileBase startTile;
    public TileBase bossTile;

    [Header("Room Builder Tiles")]
    public TileBase floorTile;
    public TileBase wallTile;

    int roomWidth = 10;
    int roomHeight = 10;

    [Header("Dungeon Settings")]
    [Range(3, 10)]
    public int dungeonLength = 5;
    public List<Room> rooms = new List<Room>();

    [Header("Debug Options")]
    public bool printOnlyConnections = false;
    void Start()
    {
        GenerateDungeonGraph();
        PrintRooms(printOnlyConnections);
        GenerateDungeon();
    }

    void GenerateDungeonGraph()
    {
        Room startRoom = new Room(0, new Vector2Int(0, 0), RoomType.Start, startTile);
        Room currentRoom = startRoom;
        rooms.Add(startRoom);
        for (int i = 0; i < dungeonLength; i++)
        {
            Vector2Int direction;
            if (new Random().Next(0, 2) == 0)
            {
                direction = new Vector2Int(1, 0);
            }
            else
            {
                direction = new Vector2Int(0, 1);
            }
            Vector2Int roomPosition = new Vector2Int(currentRoom.position.x + direction.x, currentRoom.position.y + direction.y);
            Room newRoom = new Room(i + 1, roomPosition, RoomType.Normal, floorTile, currentRoom);
            if (i == dungeonLength - 1)
            {
                newRoom.SetRoomType(RoomType.Boss);
                newRoom.roomTile = bossTile;
            }
            if (i == 0)
            {
                currentRoom.SetConnectedRoom(newRoom);
            }
            rooms.Add(newRoom);
            currentRoom = newRoom;
        }
    }

    void PrintRooms(bool onlyConnections = false)
    {
        if (onlyConnections)
        {
            foreach (Room roomEntry in rooms)
            {
                if (roomEntry.connectedRoom != null)
                {
                    Debug.Log("Room ID: " + roomEntry.Id + "\n -> Room ID: " + roomEntry.connectedRoom.Id);
                }
            }
            return;
        }

        foreach (Room roomEntry in rooms)
        {
            Debug.Log("Room ID: " + roomEntry.Id + "\nRoom Position: " + roomEntry.position + " \nRoom Type: " + roomEntry.roomType + " \nConnected to: " + $"{roomEntry.Id}" + " " + (roomEntry.connectedRoom != null ? roomEntry.connectedRoom.roomType.ToString() : "None"));
        }

    }

    void GenerateDungeon()
    {
        foreach (Room roomEntry in rooms)
        {
            floorTilemap.SetTile(roomEntry.position, roomEntry.roomTile);
        }
    }

    void GenerateRoom(int offSetX, int offSetY)
    {
        for (int i = offSetX; i < roomWidth + offSetX; i++)
        {
            for (int j = offSetY; j < roomHeight + offSetY; j++)
            {
                floorTilemap.SetTile(new Vector3Int(i, j, 0), floorTile);
            }
        }
        GenerateWallsAroundRoom(roomWidth, roomHeight);
    }

    void GenerateWallsAroundRoom(int roomWidth, int roomHeight)
    {
        for (int x = 0; x < roomWidth; x++)
        {
            wallTilemap.SetTile(new Vector3Int(x, roomHeight, 0), wallTile);
            wallTilemap.SetTile(new Vector3Int(x, -1, 0), wallTile);
        }
        for (int y = -1; y <= roomHeight; y++)
        {
            wallTilemap.SetTile(new Vector3Int(-1, y, 0), wallTile);
            wallTilemap.SetTile(new Vector3Int(roomWidth, y, 0), wallTile);
        }
    }
}
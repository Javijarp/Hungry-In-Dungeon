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
    public TileBase treasureTile;

    [Header("Room Builder Tiles")]
    public TileBase floorTile;
    public TileBase wallTile;

    int roomWidth = 10;
    int roomHeight = 10;

    [Header("Dungeon Settings")]
    [Range(3, 100)]
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

    public void GenerateDungeonGraph()
    {
        Room startRoom = new Room(0, new Vector2Int(0, 0), RoomType.Start, startTile);
        Room currentRoom = startRoom;
        rooms.Add(startRoom);
        for (int i = 1; i < dungeonLength; i++)
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
            Room newRoom = new Room(i, roomPosition, RoomType.Normal, floorTile);

            if (i == dungeonLength - 1)
            {
                newRoom.SetRoomType(RoomType.Boss);
                newRoom.roomTile = bossTile;
            }

            // Connect the current room to the new room
            currentRoom.AddConnectedRoom(newRoom);

            rooms.Add(newRoom);
            currentRoom = newRoom;
        }

        GenerateBranches();
    }

    public void GenerateBranches()
    {
        Vector2Int[] possibleDirections = new Vector2Int[]
        {
                new Vector2Int(1, 0),   // Right
                new Vector2Int(-1, 0), // Left
                new Vector2Int(0, 1),  // Up
                new Vector2Int(0, -1)  // Down
        };

        // Create a copy of the rooms list to iterate over, so we can modify the original
        List<Room> roomsCopy = new List<Room>(rooms);

        foreach (Room roomEntry in roomsCopy)
        {
            if (roomEntry.connectedRooms == null || roomEntry.connectedRooms.Length == 0)
                continue;

            Room mainConnectedRoom = roomEntry.connectedRooms[0];
            Vector2Int mainDirection = new Vector2Int(
                System.Math.Sign(mainConnectedRoom.position.x - roomEntry.position.x),
                System.Math.Sign(mainConnectedRoom.position.y - roomEntry.position.y)
            );


            foreach (Vector2Int direction in possibleDirections)
            {
                if (direction == mainDirection)
                    continue;

                if (new Random().Next(0, 2) == 0)
                {
                    Vector2Int branchPosition = new Vector2Int(
                        roomEntry.position.x + direction.x,
                        roomEntry.position.y + direction.y
                    );

                    // Check if a room already exists at this position
                    bool positionOccupied = rooms.Exists(r => r.position.x == branchPosition.x && r.position.y == branchPosition.y);
                    if (positionOccupied)
                    {
                        Debug.LogWarning($"Position {branchPosition} already occupied, skipping branch from Room {roomEntry.Id}");
                        continue;
                    }

                    Room branchRoom = new Room(
                        rooms.Count,
                        branchPosition,
                        RoomType.Treasure,
                        treasureTile
                    );
                    branchRoom.AddConnectedRoom(roomEntry);

                    roomEntry.AddConnectedRoom(branchRoom);
                    rooms.Add(branchRoom);
                    Debug.LogWarning($"Created branch from Room {roomEntry.Id} in direction {direction}");
                }
            }
        }
    }

    public void PrintRooms(bool onlyConnections = false)
    {
        if (onlyConnections)
        {
            foreach (Room roomEntry in rooms)
            {
                if (roomEntry.connectedRooms != null)
                {
                    Debug.Log("Room ID: " + roomEntry.Id + "\n -> Room ID: " + roomEntry.connectedRooms[0].Id);
                }
            }
            return;
        }

        foreach (Room roomEntry in rooms)
        {
            Debug.Log("Room ID: " + roomEntry.Id + "\nRoom Position: " + roomEntry.position + " \nRoom Type: " + roomEntry.roomType + " \nConnected to: " + $"{roomEntry.Id}" + " " + (roomEntry.connectedRooms != null ? roomEntry.connectedRooms[0].roomType.ToString() : "None"));
        }

    }

    public void GenerateDungeon()
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
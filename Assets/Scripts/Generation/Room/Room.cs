using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room
{
    public int Id;
    public TileBase roomTile;

    public Vector3Int position;
    public RoomType roomType;
#nullable enable
    public Room[]? connectedRooms;

    public Room(int id = 0, Vector2Int position = new Vector2Int(), RoomType roomType = RoomType.Normal, TileBase roomTile = null, Room[]? connectedRooms = null)
    {
        this.Id = id;
        this.position = new Vector3Int(position.x, position.y, 0);
        this.roomType = roomType;

        if (roomTile == null)
        {
            this.roomTile = Resources.Load<TileBase>("Assets/Assets/8bitdungeon_set/tiles/Floor_Tile.asset");
        }
        else
        {
            this.roomTile = roomTile;
        }

        this.connectedRooms = connectedRooms;
    }

    Room[]? getConnectedRooms()
    {
        return connectedRooms ?? null;
    }

    public void PrintConnectedRooms()
    {
        if (connectedRooms == null || connectedRooms.Length == 0)
        {
            Debug.Log("Room ID: " + Id + " has no connected rooms.");
            return;
        }

        foreach (Room room in connectedRooms)
        {
            Debug.Log("Current Room ID: " + Id + " -> " + room.Id);
        }
    }

    public void SetConnectedRooms(Room[] rooms)
    {
        this.connectedRooms = rooms;
    }

    public void AddConnectedRoom(Room room)
    {
        if (this.connectedRooms == null)
        {
            this.connectedRooms = new Room[] { room };
        }
        else
        {
            int length = this.connectedRooms.Length;
            Room[] newConnectedRooms = new Room[length + 1];
            for (int i = 0; i < length; i++)
            {
                newConnectedRooms[i] = this.connectedRooms[i];
            }
            newConnectedRooms[length] = room;
            this.connectedRooms = newConnectedRooms;
        }
    }

    public void SetRoomType(RoomType newType)
    {
        this.roomType = newType;
    }
}
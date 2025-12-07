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
    public Room? connectedRoom;

    public Room(int id = 0, Vector2Int position = new Vector2Int(), RoomType roomType = RoomType.Normal, TileBase roomTile = null, Room? connectedRoom = null)
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

        this.connectedRoom = connectedRoom;
    }

    Room? getConnectedRoom()
    {
        return connectedRoom ?? null;
    }

    public void SetConnectedRoom(Room room)
    {
        this.connectedRoom = room;
    }

    public void SetRoomType(RoomType newType)
    {
        this.roomType = newType;
    }
}
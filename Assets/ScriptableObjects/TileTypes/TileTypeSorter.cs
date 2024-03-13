using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TypedTile", menuName = "ScriptableObjects/Typed Tile", order = 1)]
public class TileTypeSorter : ScriptableObject
{
    public List<TileBase> Path = new List<TileBase>();
    public List<TileBase> Wall = new List<TileBase>();

    public TileType getTileType(string tileName)
    {
        //oh lord
        return Path.Find(s => s.name == tileName) != null ? TileType.Path : Wall.Find(s => s.name == tileName) != null ? TileType.Wall : TileType.None;
    }

}


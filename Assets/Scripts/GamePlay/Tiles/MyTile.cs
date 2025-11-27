using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "MyTile", menuName = "Tiles/MyTile")]
public class MyTile : Tile
{
    [field: SerializeField] public string Id { get; private set; }
}

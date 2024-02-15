using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class SaveHandler : Singleton<SaveHandler> {
    Dictionary<string, Tilemap> tilemaps = new Dictionary<string, Tilemap>();
    Dictionary<TileBase, BuildingObjectBase> tileBaseToBuildingObject = new Dictionary<TileBase, BuildingObjectBase>();
    Dictionary<string, TileBase> guidToTileBase = new Dictionary<string, TileBase>();

    [SerializeField] BoundsInt bounds;
    [SerializeField] string Filename;

    public void Start() {
        InitTilemaps();
        InitTileReferences();
    }

    private void InitTileReferences() {
        BuildingObjectBase[] buildables = Resources.LoadAll<BuildingObjectBase>("Scriptables/Buildables");

        foreach (BuildingObjectBase buildable in buildables) {
            if (!tileBaseToBuildingObject.ContainsKey(buildable.TileBase)) {
                tileBaseToBuildingObject.Add(buildable.TileBase, buildable);
                guidToTileBase.Add(buildable.name, buildable.TileBase);
            } else {
                Debug.LogError("TileBase " + buildable.TileBase.name + " is already in use by " + tileBaseToBuildingObject[buildable.TileBase].name);
            }
        }
    }

    private void InitTilemaps() {
        Tilemap[] maps = FindObjectsOfType<Tilemap>();
        foreach (var map in maps) {
            if (!tilemaps.ContainsKey(map.name))
                tilemaps.Add(map.name, map);
        }
    }

    public void OnSave(string filename) {
        Filename = filename;
        
        List<TilemapData> data = new List<TilemapData>();

        foreach (var mapObj in tilemaps) {
            TilemapData mapData = new TilemapData();
            mapData.key = mapObj.Key;

            BoundsInt boundsForThisMap = mapObj.Value.cellBounds;

            for (int x = boundsForThisMap.xMin; x < boundsForThisMap.xMax; x++) {
                for (int y = boundsForThisMap.yMin; y < boundsForThisMap.yMax; y++) {
                    Vector3Int pos = new Vector3Int(x, y, 0);
                    TileBase tile = mapObj.Value.GetTile(pos);

                    if (tile != null && tileBaseToBuildingObject.ContainsKey(tile)) {
                        String guid = tileBaseToBuildingObject[tile].name;
                        TileInfo ti = new TileInfo(pos, guid);
                        mapData.tiles.Add(ti);
                    }
                }
            }

            data.Add(mapData);
        }
        FileHandler.SaveToJSON<TilemapData>(data, Filename);
    }

    public void OnLoad(string filename) {
        List<TilemapData> data = FileHandler.ReadListFromJSON<TilemapData>(filename);

        foreach (var mapData in data) {
            if (!tilemaps.ContainsKey(mapData.key)) {
                Debug.LogError("Found saved data for tilemap called '" + mapData.key + "', but Tilemap does not exist in scene.");
                continue;
            }

            var map = tilemaps[mapData.key];

            map.ClearAllTiles();

            if (mapData.tiles != null && mapData.tiles.Count > 0) {
                foreach (var tile in mapData.tiles) {

                    if (guidToTileBase.ContainsKey(tile.guidForBuildable)) {
                        map.SetTile(tile.position, guidToTileBase[tile.guidForBuildable]);
                    } else {
                        Debug.LogError("Refernce " + tile.guidForBuildable + " could not be found.");
                    }

                }
            }
        }
    }
}


[Serializable]
public class TilemapData {
    public string key; // the key of your dictionary for the tilemap - here: the name of the map in the hierarchy
    public List<TileInfo> tiles = new List<TileInfo>();
}

[Serializable]
public class TileInfo {
    public string guidForBuildable;
    public Vector3Int position;

    public TileInfo(Vector3Int pos, string guid) {
        position = pos;
        guidForBuildable = guid;
    }
}
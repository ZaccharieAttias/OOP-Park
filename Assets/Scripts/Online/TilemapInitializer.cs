using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapInitializer : Singleton<TilemapInitializer> {
    [SerializeField] List<BuildingCategory> categoriesToCreateTilemapFor;
    [SerializeField] Transform grid;

    private void Start() {
        CreateMaps();
    }

    private void CreateMaps() {
        foreach (BuildingCategory category in categoriesToCreateTilemapFor) {
            // Create new GameObject
            GameObject obj = new GameObject("Tilemap_" + category.name);
            //set layer to Ground if category.name contains "Ground"
            if (category.name.Contains("Floor"))
                obj.layer = LayerMask.NameToLayer("Ground");


            // Assign Tilemap Features
            Tilemap map = obj.AddComponent<Tilemap>();
            TilemapRenderer tr = obj.AddComponent<TilemapRenderer>();
            TilemapCollider2D tilemapCollider = obj.AddComponent<TilemapCollider2D>();

            // Set Parent
            obj.transform.SetParent(grid);

            // Here you can add settings ...
            tr.sortingOrder = category.SortingOrder;
            //set material of tr to FixGaps
            tr.material = Resources.Load<Material>("Physics/FixGaps");
            tilemapCollider.sharedMaterial = Resources.Load<PhysicsMaterial2D>("Physics/No Friction");
            

            category.Tilemap = map;
        }
    }
}
using Assets.PixelFantasy.PixelTileEngine.Scripts;
using UnityEngine;
using UnityEngine.UI;


public class InventoryB : MonoBehaviour
{
    public SpriteCollectionPF SpriteCollection;
    public LevelBuilderB LevelBuilder;
    public Transform Grid;
    public GameObject InventoryItem;

    public void Start()
    {
        SwitchTab(0);
    }

    public void SwitchTab(int tab)
    {
        foreach (Transform child in Grid)
        {
            Destroy(child.gameObject);
        }

        CreateInventoryItem("Empty", SpriteCollection.DeleteSprite, tab, -1);

        switch (tab)
        {
            case 0:
            case 1:
                var tilesets = tab == 0 ? SpriteCollection.GroundTilesets : SpriteCollection.CoverTilesets;

                for (var i = 0; i < tilesets.Count; i++)
                {
                    CreateInventoryItem(tilesets[i].Name, tilesets[i].Sprites[tab == 0 ? 20 : 0], tab, i);
                }
                break;
            case 2:
                var sprites = SpriteCollection.PropsSprites;

                for (var i = 0; i < sprites.Count; i++)
                {
                    CreateInventoryItem(sprites[i].name, sprites[i], tab, i);
                }
                break;
            case 3:
                var walltilesets = SpriteCollection.WallTilesets;

                for (var i = 0; i < walltilesets.Count; i++)
                {
                    CreateInventoryItem(walltilesets[i].name, walltilesets[i], tab, i);
                }
                break;
            case 4:
                var gameplayObjects = SpriteCollection.GamePlaySprite;

                for (var i = 0; i < gameplayObjects.Count; i++)
                {
                    CreateInventoryItem(gameplayObjects[i].name, gameplayObjects[i], tab, i);
                }
                break;
        }
    }

    private void CreateInventoryItem(string itemName, Sprite sprite, int type, int index)
    {
        var item = Instantiate(InventoryItem, Grid);
        var toggle = item.GetComponent<Toggle>();

        item.name = itemName;
        item.transform.Find("Icon").GetComponent<Image>().sprite = sprite;
        toggle.onValueChanged.AddListener(value => { if (value) LevelBuilder.SwitchTile(type, index); });
        toggle.group = Grid.GetComponent<ToggleGroup>();
        toggle.isOn = index == 0;
    }
}
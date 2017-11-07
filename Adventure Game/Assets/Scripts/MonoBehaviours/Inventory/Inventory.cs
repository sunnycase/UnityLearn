using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public const int NumItemSlots = 4;

    public Image[] itemImages = new Image[NumItemSlots];
    public Item[] items = new Item[NumItemSlots];

    public void AddItem(Item item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if(items[i] == null)
            {
                items[i] = item;
                itemImages[i].sprite = item.sprite;
                itemImages[i].enabled = true;
                return;
            }
        }
    }
    public void RemoveItem(Item item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == item)
            {
                items[i] = null;
                itemImages[i].sprite = null;
                itemImages[i].enabled = false;
                return;
            }
        }
    }
}

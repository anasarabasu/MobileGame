using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System.Linq;
using System.Reflection;
using System;

public class InventorySystem : MonoBehaviour, ISaveable {
    public static InventorySystem instance;
    private void Awake() => instance = this;

    public List<ItemData> inventoryContents = new();
    internal void AddItem(ItemData item) {
        bool itemExists = false;
        item.inventory.amount ++;

        foreach (ItemData existingItem in inventoryContents) {
            if(existingItem.inventory == item.inventory) {
                itemExists = true;
            }
        }
        if(!itemExists) 
            inventoryContents.Add(item);

        UpdateInventoryUI();
    }

    [SerializeField] GameObject InventorySlotPrefab;
    [SerializeField] Transform InventoryPanel;
    public void UpdateInventoryUI() {
        ItemData itemToDelete = null;

        foreach (Transform reloadItem in InventoryPanel.GetComponentInChildren<Transform>()) 
            Destroy(reloadItem.gameObject);

        foreach (ItemData item in inventoryContents) {
            GameObject itemSlot = Instantiate(InventorySlotPrefab, InventoryPanel);
            itemSlot.GetComponent<FocusItem>().itemData = item;
            itemSlot.GetComponentInChildren<Image>().sprite = item.inventory.sprite;

            if(item.inventory.amount > 1)
                itemSlot.GetComponentInChildren<TextMeshProUGUI>().SetText(item.inventory.amount.ToString());
            else if(item.inventory.amount == 1)
               itemSlot.GetComponentInChildren<TextMeshProUGUI>().SetText("");
            else {
                itemToDelete = item;
                Destroy(itemSlot);
                ItemInfo.instance.HideInfoPanel();
            }
        }

        inventoryContents.Remove(itemToDelete);
    }
    private bool showInventory = false;
    public void _ToggleInventory() {  
        //not in combat | there is probably a better way to do this but i dont have the time    
        //be in awe of my spaghetti code > <   
        if(!showInventory) {
            InventoryPanel.DOLocalMoveY(30, 0.5f);
            showInventory = true;
            Joystick.MovementState(false);
        }
        else {
            InventoryPanel.DOLocalMoveY(150, 0.5f);
            showInventory = false;

            ItemInfo.instance.infoPanel.DOLocalMoveY(150, 0.5f);
            Joystick.MovementState(true);
        }  
    }

    public void Save(DataRoot data) {
        List<int> inventoryID = new();
        foreach (var item in inventoryContents) 
            inventoryID.Add(item.GetInstanceID());
        
        data.inventoryData = inventoryContents;
    }

    public void Load(DataRoot data) {
        inventoryContents = data.inventoryData; 
        UpdateInventoryUI();
    }
}
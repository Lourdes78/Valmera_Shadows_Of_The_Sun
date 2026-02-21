using UnityEngine;
using System.IO;
using System.Linq;


public class SaveSystem
{
    private const string SAVE_FILE = "save.json";

    public void Save(PlayerState player)
    {
        var wrapper = new SaveData();
        wrapper.Player = CreatePlayerData(player);

        string json = JsonUtility.ToJson(wrapper, true);

        string path = GetPath();

        System.IO.File.WriteAllText(path, json);

        Debug.Log("Save path: " + path);   // AFEGEIX AIXÒ
        Debug.Log("Game Saved.");
    }

    public SaveData Load()
    {
        if (!File.Exists(GetPath()))
        {
            Debug.LogWarning("No save file found.");
            return null;
        }

        string json = File.ReadAllText(GetPath());
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        Debug.Log("Game Loaded.");
        Debug.Log("Loaded Level from JSON: " + data.Player.Level);

        return data;
    }

    private string GetPath()
    {
        return Path.Combine(Application.persistentDataPath, SAVE_FILE);
    }

    private PlayerSaveData CreatePlayerData(PlayerState player)
    {
        PlayerSaveData data = new PlayerSaveData();

        // XP
        data.Level = player.XPSystem.Level;
        data.CurrentXP = player.XPSystem.CurrentXP;

        // Stats
        data.Stats = new PlayerStatsSaveData
        {
            CurrentHealth = player.Stats.CurrentHealth,
            MaxHealth = player.Stats.MaxHealth,
            CurrentStamina = player.Stats.CurrentStamina,
            MaxStamina = player.Stats.MaxStamina
        };

        // Reputation
        var repDict = player.ReputationSystem.GetAll();

        data.Reputation = new ReputationSaveData
        {
            Factions = repDict.Keys.Select(f => f.ToString()).ToArray(),
            Values = repDict.Values.ToArray()
        };

        // Inventory
        var slots = player.InventorySystem.Slots;
        InventorySlotSaveData[] slotData = new InventorySlotSaveData[slots.Count];

        for (int i = 0; i < slots.Count; i++)
        {
            if (!slots[i].IsEmpty)
            {
                slotData[i] = new InventorySlotSaveData
                {
                    ItemID = slots[i].Item.Definition.Id,
                    Quantity = slots[i].Item.Quantity
                };
            }
            else
            {
                slotData[i] = new InventorySlotSaveData();
            }
        }

        data.Inventory = new InventorySaveData
        {
            Slots = slotData
        };

        return data;
    }
}
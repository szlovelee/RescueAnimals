using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Component.Entities;
using Unity.VisualScripting;
using UnityEngine;

namespace EnumTypes
{
    public class DataManager : MonoBehaviour
    {
        private const string FileName = "player.json";
        private static DataManager _instance;
        public bool IsWrite { get; private set; } = false;

        public static DataManager Instance
        {
            get
            {
                if (_instance != null) return _instance;

                var go = new GameObject
                {
                    name = nameof(DataManager)
                };
                _instance = go.AddComponent<DataManager>();
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != this && _instance != null)
            {
                Destroy(gameObject);
            }
        }

        public Player LoadPlayerInfo(AnimalData animalData, RankSystem rankSystem)
        {
            var parent = Application.persistentDataPath;
            var filePath = Path.Combine(parent, FileName);
            var json = File.ReadAllText(filePath);
            var saveData = JsonUtility.FromJson<SaveData>(json);
            Debug.Log(json);
            var player = gameObject.AddComponent<Player>();
            player.level = saveData.Level;
            player.atk = saveData.Atk;
            player.gold = saveData.Gold;
            player.exp = saveData.Exp;
            player.maxScore = saveData.MaxScore;
            var reinforce = animalData.AnimalReinforceData;
            rankSystem.rankings = saveData.RankSystemData;
            for (int i = 0; i < saveData.ReinforceSaveData.Count; i++)
            {
                if (i >= reinforce.Count) break;
                reinforce[i].reinforceLevel = saveData.ReinforceSaveData[i].reinforceLevel;
            }

            return player;
        }

        public void SavePlayer(Player player, AnimalData animalData, RankSystem rankSystem)
        {
            var reinforce = animalData
                .AnimalReinforceData
                .Select(data => new ReinforceSaveData(data.animalType, data.reinforceLevel))
                .ToList();

            IsWrite = true;
            var parent = Application.persistentDataPath;
            var filePath = Path.Combine(parent, FileName);
            SaveData saveData = new SaveData(
                player.level,
                player.exp,
                player.gold,
                player.maxScore,
                player.atk,
                reinforceSaveData: reinforce
            );
            saveData.RankSystemData = rankSystem.GetRankings();
            var json = JsonUtility.ToJson(saveData);
            File.WriteAllText(filePath, json);
            IsWrite = false;
        }
    }
}
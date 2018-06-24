using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour {
    [Header("Game Presets")]
    public GameObject[] LevelsPool;
    public int selectedLevel;
    public float beginingTimer;
    public float waveTimer;
    public GameObject[] EnemyPrefab;
    public int[] EnemyLevel;
    public int waveCount = 0;

    [Header("UI")]
    public GameObject levelAnnounceText;

	// Use this for initialization
	void Start () {
        //Устанавлиавем размер массива EnemyLevel равным размеру массива EnemyPrefab
        EnemyLevel = new int[EnemyPrefab.Length];
        //Выставляем уровни для каждого типа противника = 1
        for (int i = 0; i < EnemyLevel.Length; i++) {
            EnemyLevel[i] = 1;
        }
        //Выбираем случайный уровень из списка LevelsPool
        selectedLevel = Random.Range(0, LevelsPool.Length);
        LoadLevel(selectedLevel);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void LoadLevel(int levelid) {
        GameObject newLevel = LevelsPool[selectedLevel];
        //Делаем уровень активным
        newLevel.SetActive(true);
        WaveManager waveManager = newLevel.GetComponent<WaveManager>();
        //Запускаем аннонс названия уровня
        AnnounceLevelName(waveManager);
        //Устанавливаем начальные значения для класса WaveManager
        waveManager.GameSetup(beginingTimer, waveTimer);


    }

    void AnnounceLevelName(WaveManager level) {
        levelAnnounceText.GetComponent<Text>().text = "Selected level - " + level.levelName;
        levelAnnounceText.GetComponent<FadeIn>().Activate(3f);
    }
}

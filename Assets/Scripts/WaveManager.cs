using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour {
    [Header("Level Values")]
    public string levelName = "Training Polygon";
    public Transform WaypointsParent;
    private List<Transform> waypoints = new List<Transform>();
    private GameMaster gameMaster;

    [Header("Waves Values")]
    public float startTimer;
    public float waveTimer;
    private float timer;
    private bool gameStarted = false;
    private bool waveSpawned = true;
    public float delayTimer;
    public int enemyPerWave = 10;

	// Use this for initialization
	void Start () {
        gameMaster = GameObject.Find("GameMaster").GetComponent<GameMaster>();
	}
	
	// Update is called once per frame
	void Update () {
        //Если вы настройки игры выполнены в методе GameSetup, то запускаем игру (таймеры)
        //В игре 2 таймера:
        //startTimer - Это одноразовый таймер, который может отличатся от waveTimer. Он нужен для того, чтобы дать игроку больше времени на подготовку
        //waveTimer - Это повторяющийся таймер между последовательными волнами противников
        if (gameStarted) {
            //Если стартовый таймер больше нуля, то убавляем его
            if (startTimer > 0) {
                startTimer -= Time.deltaTime;
            }
            //Если стартовый таймер равен нулю, то проверяем была ли создана первая волна
            else {
                //Если первая волна не была создана, то создаем ее
                if (gameMaster.waveCount == 0) {
                    if (waveSpawned)
                    {
                        waveSpawned = false;
                        SpawnNewWave();
                    }
                }
                //Если первая волна была создана, то проверяем таймер между волнами
                else {
                    //Если волна была полностью выпущена
                    if (waveSpawned)
                    {
                        //Если таймер между волнами больше нуля, то убавляем его
                        if (timer > 0)
                        {
                            timer -= Time.deltaTime;
                        }
                        //Если таймер между волнами меньше или равен нулю, то выпускаем следующую волну
                        else
                        {
                            waveSpawned = false;
                            SpawnNewWave();
                        }
                    }
                }
            }
        }
	}

    public void GameSetup(float _startTimer, float _waveTimer) {
        //Очищаем список Вейпоинтов и добавляем все чайлд трансформы поочереди в качестве вейпоинта
        waypoints.Clear();
        for (int i = 0; i < WaypointsParent.childCount; i++)
        {
            waypoints.Add(WaypointsParent.GetChild(i));
        }
        startTimer = _startTimer;
        waveTimer = _waveTimer;
        gameStarted = true;
    }

    void SpawnNewWave() {
        //Выбираем случайного противника
        int randomEnemy = Random.Range(0, gameMaster.EnemyPrefab.Length);
        StartCoroutine(SpawnEnemy(randomEnemy, enemyPerWave));

    }

    IEnumerator SpawnEnemy(int enemyID, int count){
        for (int i = 0; i < count; i++) {
            GameObject currentEnemy = (GameObject)Instantiate(gameMaster.EnemyPrefab[enemyID], waypoints[0].position, Quaternion.identity);
            currentEnemy.GetComponent<Enemy>().SetupWaypoints(waypoints);
            //Ждем перед следующим противником
            yield return new WaitForSeconds(delayTimer);
        }

        //Увеличиваем номер волны на 1
        gameMaster.waveCount++;
        //Устанавливаем таймер
        timer = waveTimer;
        //waveSpawned = true;
        waveSpawned = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveController : MonoBehaviour
{
    public static WaveController instance;
    [Header("Points for little enemy")]
    [SerializeField] public List<GameObject> pointsForLittleEnemy;
    [Header("Points for Big or Middle enemy")]
    [SerializeField] public List<GameObject> pointsForBirOrMiddle;

    [Header("Waves")]
    [SerializeField] List<WaveObj> waveObjs;
    
    [Header("Spawn Areas")]
    [SerializeField] MeshCollider bigSpawnArea;
    [SerializeField] MeshCollider leftSpawnArea;
    [SerializeField] MeshCollider rightSpawnArea;
    bool win = false;

    int waveCounter = 0;
    [SerializeField] public int enemiesActive;
    [SerializeField] public List<GameObject> enemyInGame;
    float waveTimer;
    [SerializeField] List<GameObject> enemies = new List<GameObject>();
    //[SerializeField] public List<GameObject> enemieCounter;
    Coroutine coroutine;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        for (int i = 0; i < waveObjs.Count; i++)
        {
            for (int j = 0; j < waveObjs[i].amountLittleEnemy; j++)
            {
                enemiesActive += 1;
            }
            for (int j = 0; j < waveObjs[i].amountLittleEnemyFast; j++)
            {
                enemiesActive += 1;
            }
            for (int j = 0; j < waveObjs[i].amountMiddleEnemy3; j++)
            {
                enemiesActive += 1;
            }
            for (int j = 0; j < waveObjs[i].amountMiddleEnemy6; j++)
            {
                enemiesActive += 1;
            }
            for (int j = 0; j < waveObjs[i].amountBigEnemy; j++)
            {
                enemiesActive += 1;
            }
        }
    }
    private void FixedUpdate()
    {
        for (int i = 0; i < enemyInGame.Count; i++)
        {
            if (enemyInGame[i] == null)
            {
                enemyInGame.RemoveRange(i, 1);
            }
        }
        if (coroutine == null && waveCounter < waveObjs.Count)
        {
            waveTimer += Time.deltaTime;
            //Debug.Log(waveTimer);

            if (waveTimer >= waveObjs[waveCounter].spawnWaveDelay)
            {
                
                    coroutine = StartCoroutine(Spawn());
            }
        }
        if (enemiesActive <= 0 && win == false)
        {
            win = true;
            string nameScene;
            nameScene = SceneManager.GetActiveScene().name;
            
            UIManager.instance.Win();

        }
    }

    void SpawnEnemy(MeshCollider spawnArea, GameObject enemyForSpawn)
    {
        Vector3 sizeEnemy = new Vector3(enemyForSpawn.GetComponent<BoxCollider>().size.x / 2,
            enemyForSpawn.GetComponent<BoxCollider>().size.y / 2,
            enemyForSpawn.GetComponent<BoxCollider>().size.z / 2);
        float x = 0f, z = 0f;
        int repeatCouner = 0;
        x = Random.Range(spawnArea.transform.position.x - Random.Range(0, spawnArea.bounds.extents.x), spawnArea.transform.position.x + Random.Range(0, spawnArea.bounds.extents.x));
        z = Random.Range(spawnArea.transform.position.z - Random.Range(0, spawnArea.bounds.extents.z), spawnArea.transform.position.z + Random.Range(0, spawnArea.bounds.extents.z));
        Vector3 spawnPos = new Vector3(x, 0.8f, z);
        while(Physics.CheckBox(spawnPos, sizeEnemy) == true)
        {
            x = Random.Range(spawnArea.transform.position.x - Random.Range(0, spawnArea.bounds.extents.x), spawnArea.transform.position.x + Random.Range(0, spawnArea.bounds.extents.x));
            z = Random.Range(spawnArea.transform.position.z - Random.Range(0, spawnArea.bounds.extents.z), spawnArea.transform.position.z + Random.Range(0, spawnArea.bounds.extents.z));
            spawnPos = new Vector3(x, 0.8f, z);

            repeatCouner++;
            if (repeatCouner >= 150)
            {
                break;
            }
        }

       enemyInGame.Add(Instantiate(enemyForSpawn, spawnPos, Quaternion.identity));
        
        
    }

    IEnumerator Spawn()
    {
        for (int j = 0; j < waveObjs[waveCounter].amountLittleEnemy; j++)
        {
            enemies.Add(waveObjs[waveCounter].enemyLittle);
                
        }

        for (int j = 0; j < waveObjs[waveCounter].amountLittleEnemyFast; j++)
        {
            enemies.Add(waveObjs[waveCounter].enemyLittleFast);

        }

        for (int j = 0; j < waveObjs[waveCounter].amountMiddleEnemy3; j++)
        {
            enemies.Add(waveObjs[waveCounter].enemyMiddle3);
                
        }

        for (int j = 0; j < waveObjs[waveCounter].amountMiddleEnemy6; j++)
        {
            enemies.Add(waveObjs[waveCounter].enemyMiddle6);

        }

        for (int j = 0; j < waveObjs[waveCounter].amountBigEnemy; j++)
        {
            enemies.Add(waveObjs[waveCounter].enemyBig);
                
        }


            
            int randEnemy = 0;
            int randArea = 0;
            while (enemies.Count != 0)
            {

                randEnemy = Random.Range(0, enemies.Count);
                yield return new WaitForSeconds(waveObjs[waveCounter].spawnMobDelay);
                if (enemies[randEnemy] == waveObjs[waveCounter].enemyBig)
                {
                    SpawnEnemy(bigSpawnArea, enemies[randEnemy]);
                }

                if (enemies[randEnemy] == waveObjs[waveCounter].enemyMiddle3)
                {
                    SpawnEnemy(bigSpawnArea, enemies[randEnemy]);
                }
                if (enemies[randEnemy] == waveObjs[waveCounter].enemyMiddle6)
                {
                    SpawnEnemy(bigSpawnArea, enemies[randEnemy]);
                }

            if (enemies[randEnemy] == waveObjs[waveCounter].enemyLittle)
                {

                    randArea = Random.Range(0, 3);
                    //0 - up, 1 - left, 2 - right

                    if (randArea == 0)
                    {
                        SpawnEnemy(bigSpawnArea, enemies[randEnemy]);
                    }

                    if (randArea == 1)
                    {
                        SpawnEnemy(leftSpawnArea, enemies[randEnemy]);
                    }

                    if (randArea == 2)
                    {
                        SpawnEnemy(rightSpawnArea, enemies[randEnemy]);
                    }

                }
            if (enemies[randEnemy] == waveObjs[waveCounter].enemyLittleFast)
            {

                randArea = Random.Range(0, 3);
                //0 - up, 1 - left, 2 - right

                if (randArea == 0)
                {
                    SpawnEnemy(bigSpawnArea, enemies[randEnemy]);
                }

                if (randArea == 1)
                {
                    SpawnEnemy(leftSpawnArea, enemies[randEnemy]);
                }

                if (randArea == 2)
                {
                    SpawnEnemy(rightSpawnArea, enemies[randEnemy]);
                }

            }
            enemies.RemoveRange(randEnemy, 1);
                    
            }
            
            waveTimer = 0;
            waveCounter += 1;

            coroutine = null;
        yield return null;
    }
}

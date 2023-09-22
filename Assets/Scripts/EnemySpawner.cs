using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class EnemySpawner : MonoBehaviour{
    [SerializeField] int delay = 0;
    [Space]
    public int amountOfMellee1 = 0;
    public int amountOfMellee2 = 0;
    public int amountOfAcid = 0;
    public int amountOfAcidRange = 0;
    public int amountOfRange = 0;
    public int amountOfBoss1 = 0;


    //public int wave = 0;
    [SerializeField] GameObject[] enemiesPrefs;
    [SerializeField] float spawnDelta = 0.2f;
    
    [SerializeField] Transform[] spawns;

    float timer;
    int spawnedEnemies;
    public bool startSpawn;

    public static int destroyedEnemies = 0;

    int waveMain;
    void Start(){
        startSpawn = false;
        spawnedEnemies = 0;
        destroyedEnemies = 0;
        //waveMain = wave;
    }

    public void AddAllEnemies() {
        GameManager.amountOfEnemies += amountOfMellee1;
        GameManager.amountOfEnemies += amountOfMellee2;
        GameManager.amountOfEnemies += amountOfAcid;
        GameManager.amountOfEnemies += amountOfRange;
        GameManager.amountOfEnemies += amountOfAcidRange;
        GameManager.amountOfEnemies += amountOfBoss1;
    }


    public void StartSpawning() {
        StartCoroutine(StartSp());
    }
    //F3E5A2 - level borders, bottom
    IEnumerator StartSp() {
        yield return new WaitForSeconds(delay);
        //wave--;
        startSpawn = true;
        enToSpawn = 0;


        
    }

    int enToSpawn = 0;

    void Update() {
        if (startSpawn) {
            timer += Time.deltaTime;
            //if (enToSpawn == 0)
            //    InstEnemies(0, amountOfMellee1, spawns[1]);
            //else if (enToSpawn == 1)
            //    InstEnemies(1, amountOfMellee2, spawns[0]);
            //else if (enToSpawn == 2)
            //    InstEnemies(enemiesPrefs[2], amountOfAcid, spawns[2]);
            //else if (enToSpawn == 3)
            //    InstEnemies(enemiesPrefs[3], amountOfAcidRange, spawns[1]);
            //else if (enToSpawn == 2)
            //    InstEnemies(2, amountOfRange, spawns[1]);
            //else
            //    InstEnemies(enemiesPrefs[5], amountOfBoss1, spawns[1]);

            if (enToSpawn == 0)
                InstEnemies(enemiesPrefs[0], amountOfMellee1, spawns[1]);
            else if (enToSpawn == 1)
                InstEnemies(enemiesPrefs[1], amountOfMellee2, spawns[0]);
            else if (enToSpawn == 2)
                InstEnemies(enemiesPrefs[2], amountOfAcid, spawns[2]);
            else if (enToSpawn == 3)
                InstEnemies(enemiesPrefs[3], amountOfAcidRange, spawns[1]);
            else if (enToSpawn == 4)
                InstEnemies(enemiesPrefs[4], amountOfRange, spawns[1]);
            else
                InstEnemies(enemiesPrefs[5], amountOfBoss1, spawns[1]);


        }
    }


    void InstEnemies(GameObject enemyToSpawn, int amountOfEnemies, Transform pos) {
        if (timer > spawnDelta) {

            if (spawnedEnemies < amountOfEnemies) {
                Instantiate(enemyToSpawn, new Vector3(Random.Range(pos.position.x - 3, pos.position.x + 3), enemyToSpawn.transform.position.y, Random.Range(pos.position.z - 1, pos.position.z + 1)), Quaternion.identity);
                timer = 0;
                spawnedEnemies++;
            } else {
                enToSpawn++;
                spawnedEnemies = 0;
                if (enToSpawn == 6) {
                    spawnedEnemies = 0;
                    timer = 0;
                    startSpawn = false;
                }
            }
        }
    }

    void InstEnemies(int name, int amountOfEnemies, Transform pos) {
        if (timer > spawnDelta) {
            if (spawnedEnemies < amountOfEnemies) {

                AiController en;
                if (name == 0)
                    en = GameManager.enemies_1.Dequeue();
                else if (name == 1) {
                    en = GameManager.enemies_2.Dequeue();
                } else {
                    en = GameManager.enemies_3.Dequeue();
                }
                en.StartWalk();
                en.gameObject.transform.SetPositionAndRotation(new Vector3(Random.Range(pos.position.x - 3, pos.position.x + 3), 0, Random.Range(pos.position.z - 1, pos.position.z + 1)), Quaternion.identity);

                if (name == 0)
                    GameManager.enemies_1.Enqueue(en);
                else if (name == 1) {
                    GameManager.enemies_2.Enqueue(en);
                } else {
                    GameManager.enemies_3.Enqueue(en);
                }


                timer = 0;
                spawnedEnemies++;
            } else {
                enToSpawn++;
                spawnedEnemies = 0;
                if (enToSpawn == 6) {
                    spawnedEnemies = 0;
                    timer = 0;
                    startSpawn = false;
                }
            }
        }
    }
}

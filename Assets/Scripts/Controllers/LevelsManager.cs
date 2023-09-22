using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class LevelsManager : MonoBehaviour{

    public List<Box> allLevels = new List<Box>();
    [SerializeField] EnemySpawner[] spawners;

    [SerializeField] bool disableUpgrade;
    [SerializeField] GameObject upgrade1;


    private void Start() {

        for (int i = 0; i < allLevels.Count; i++) {
            FindObjectOfType<UI>().levels.Add(allLevels[i]);

            if (GameManager.currentLevel < 10) {

                if (i < GameManager.currentLevel % 3)
                    allLevels[i].OpenDoor();

                if (i <= GameManager.currentLevel % 3) {
                    allLevels[i].raise = false;
                    allLevels[i].gameObject.SetActive(true);
                }
            } 
            else {

                if (i < (GameManager.currentLevel - 10) % 8)
                    allLevels[i].OpenDoor();

                if (i <= (GameManager.currentLevel - 10) % 8) {
                    allLevels[i].raise = false;
                    allLevels[i].gameObject.SetActive(true);
                }
            }
        }

        if(GameManager.currentLevel == 0) {
            int c = PlayerPrefs.GetInt("Tutorial");
            if (c == 2) {
                PlayerPrefs.SetInt("Tutorial", 1);
                PlayerPrefs.SetInt("MoneyAmount", 0);
            }
            
        }

        if (GameManager.currentLevel == 1) {
            int c = PlayerPrefs.GetInt("Tutorial");
            if (c == 5) {
                PlayerPrefs.SetInt("Tutorial", 4);
                PlayerPrefs.SetInt("MoneyAmount", 0);
            } 
            else if (c == 8) {
                PlayerPrefs.SetInt("Tutorial", 7);
                PlayerPrefs.SetInt("MoneyAmount", 10);
            }
        }

        if (GameManager.currentLevel == 2) {

            int c = PlayerPrefs.GetInt("Tutorial");
            if (c == 10) {
                PlayerPrefs.SetInt("Tutorial", 9);
                PlayerPrefs.SetInt("MoneyAmount", 0);
            } 
            else if (c == 13) {
                PlayerPrefs.SetInt("Tutorial", 12);
                PlayerPrefs.SetInt("MoneyAmount", 36);
            }

        }
        //PlayerPrefs.SetInt("Tutorial", 0);

        GameManager.tutorial = PlayerPrefs.GetInt("Tutorial");


        //PlayerPrefs.SetInt("MoneyAmount", GameManager.moneyAmount);
        GameManager.moneyAmount = PlayerPrefs.GetInt("MoneyAmount");

        GameObject.Find("NavMesh").GetComponent<NavMeshSurface>().BuildNavMesh();

    }

    public void StartSpawn() {
        for(int i = 0; i < spawners.Length; i++) {
            spawners[i].StartSpawning();
        }
    }

    private void Update() {
        if (disableUpgrade) {
            if(GameManager.tutorial > 6)
                upgrade1.gameObject.SetActive(false);
        }
    }
}

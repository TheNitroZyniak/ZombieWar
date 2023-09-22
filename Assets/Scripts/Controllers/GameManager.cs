using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour{

    public static int moneyAmount;
    public static int currentLevel;
    public static int amountOfEnemies, earnedMoney;
    [SerializeField] GameObject[] islandPrefs;
    [SerializeField] GameObject[] tutorialObjs;

    [SerializeField] GameObject[] enemyPrefabs;


    [SerializeField] GameObject player;
    [SerializeField] CameraFollow cam1;

    public static int tutorial;
    public static bool notFirstLoad = false;

    public static Queue<AiController> enemies_1 = new Queue<AiController>();
    public static Queue<AiController> enemies_2 = new Queue<AiController>();
    public static Queue<AiController> enemies_3 = new Queue<AiController>();

    //public static List<AiController> allEnemies = new List<AiController>();

    private void Awake() {
        //allEnemies.Clear();

        PlayerPrefs.SetInt("Tutorial", 15);
        PlayerPrefs.SetInt("MoneyAmount", 2540);
        PlayerPrefs.SetInt("CurrentLevel", 3);
        PlayerPrefs.SetInt("AllowedWeapon", 1);

        currentLevel = PlayerPrefs.GetInt("CurrentLevel");
        moneyAmount = PlayerPrefs.GetInt("MoneyAmount");
        tutorial = PlayerPrefs.GetInt("Tutorial");
    }

    private void Start() {
        amountOfEnemies = 0;
        earnedMoney = 0;

        if (!notFirstLoad) {
            //CreateObjects();
            notFirstLoad = true;
        }
      
        player.transform.position = new Vector3(0, 0, 0);
        player.GetComponent<PlayerController>().enabled = true;

        if(currentLevel < 3) 
            Instantiate(islandPrefs[0]);
        else {
            tutorialObjs[0].SetActive(false);
            tutorialObjs[1].SetActive(false);
            tutorialObjs[2].SetActive(true);
            if (currentLevel < 6) 
                Instantiate(islandPrefs[1]);          
            else if (currentLevel < 9) 
                Instantiate(islandPrefs[2]);      
            else if (currentLevel == 9) 
                Instantiate(islandPrefs[3]);          
            else if (currentLevel < 17) 
                Instantiate(islandPrefs[4]);            
            else 
                Instantiate(islandPrefs[5]);         
        } 
    }


    void CreateObjects() {

        for (int i = 0; i < 180; i++) {
            AiController newEn1 = Instantiate(enemyPrefabs[0].GetComponent<AiController>());
            DontDestroyOnLoad(newEn1);
            enemies_1.Enqueue(newEn1);
        }

        for (int i = 0; i < 120; i++) {
            AiController newEn2 = Instantiate(enemyPrefabs[1].GetComponent<AiController>());
            DontDestroyOnLoad(newEn2);
            enemies_2.Enqueue(newEn2);
        }

        for (int i = 0; i < 40; i++) {
            AiController newEn3 = Instantiate(enemyPrefabs[2].GetComponent<AiController>());
            DontDestroyOnLoad(newEn3);
            enemies_3.Enqueue(newEn3);
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Loading : MonoBehaviour{

    [SerializeField] GameObject[] enemyPrefabs;


    private void Start() {

        int currentLevel = PlayerPrefs.GetInt("CurrentLevel");

        for (int i = 0; i < 180; i++) {
            AiController newEn1 = Instantiate(enemyPrefabs[0].GetComponent<AiController>());
            DontDestroyOnLoad(newEn1);
            GameManager.enemies_1.Enqueue(newEn1);
        }

        for (int i = 0; i < 120; i++) {
            AiController newEn2 = Instantiate(enemyPrefabs[1].GetComponent<AiController>());
            DontDestroyOnLoad(newEn2);
            GameManager.enemies_2.Enqueue(newEn2);
        }

        for (int i = 0; i < 40; i++) {
            AiController newEn3 = Instantiate(enemyPrefabs[2].GetComponent<AiController>());
            DontDestroyOnLoad(newEn3);
            GameManager.enemies_3.Enqueue(newEn3);
        }

        SceneManager.LoadScene("GameScene");
    }


}

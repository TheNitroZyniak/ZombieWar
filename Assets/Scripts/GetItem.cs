using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GetItem : MonoBehaviour{
    [SerializeField] Image loadingBar;
    [SerializeField] GameObject canvasObjects;
    [SerializeField] int moneyToOpen;
    [SerializeField] bool openLevel, endLevel;
    [SerializeField] GameObject prefToInst;
    [SerializeField] GameObject Popup;
    float timeToOpen;


    private void OnEnable() {
        //if(!setTurret)
            //newLevel.text = (GameManager.currentLevel + 1).ToString();
    }

    private void OnTriggerEnter(Collider other) {
        if (GameManager.moneyAmount >= moneyToOpen) {
            if(!openLevel) {
                if (GameManager.tutorial == 10) {
                    GameManager.tutorial = 11;
                    PlayerPrefs.SetInt("Tutorial", GameManager.tutorial);
                };

                Popup.SetActive(true);
            }
        }
    }

    private void OnTriggerStay(Collider other) {

        if (GameManager.moneyAmount >= moneyToOpen) {

            if (other.gameObject.CompareTag("Player")) {

                if (openLevel) {
                    timeToOpen += Time.deltaTime;
                    loadingBar.fillAmount = (float)timeToOpen / 2f;

                    if (timeToOpen > 2) {
                        GameObject[] ds = GameObject.FindGameObjectsWithTag("OpenDoor");
                        for (int i = 0; i < ds.Length; i++) ds[i].SetActive(false);


                        FindObjectOfType<MusicManager>().PlaySound(2);

                        GameManager.moneyAmount -= moneyToOpen;
                        if (!endLevel) {
                            prefToInst.SetActive(true);

                            if (GameManager.tutorial == 8) {
                                PlayerPrefs.SetInt("Tutorial", GameManager.tutorial);
                                GameManager.tutorial = 9;
                            }

                        }
                        else {
                            GameObject[] gets = GameObject.FindGameObjectsWithTag("GetItem");
                            for (int i = 0; i < gets.Length; i++)
                                gets[i].transform.GetChild(0).gameObject.SetActive(false);
                            FindObjectOfType<UI>().OpenMenu1(false);
                        }

                        LevelComplete lvlComplete = new LevelComplete();
                        lvlComplete.level = GameManager.currentLevel;
                        lvlComplete.timeSpent = 50;
                        lvlComplete.daysSinceReg = 0;

                        //AppMetrica.Instance.SendEventsBuffer();
                        string json = JsonUtility.ToJson(lvlComplete);
                        AppMetrica.Instance.ReportEvent("level_complete", json);

                        GameManager.currentLevel++;

                        
                        PlayerPrefs.SetInt("CurrentLevel", GameManager.currentLevel);
                        PlayerPrefs.SetInt("MoneyAmount", GameManager.moneyAmount);

                        //if (GameManager.currentLevel == 10)
                        //    SceneManager.LoadScene("GameScene");
                        //if (GameManager.currentLevel == 17)
                        //    SceneManager.LoadScene("GameScene");
                        
                        canvasObjects.SetActive(false);
                        timeToOpen = 0;
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            if (!openLevel) {
                Popup.SetActive(false);
            }
            else {
                timeToOpen = 0;
                loadingBar.fillAmount = 0;
            }
        
        }

    }


    public void BuyObject() {
        FindFirstObjectByType<UI>().OpenMenu();
        GameManager.moneyAmount -= moneyToOpen;
        Instantiate(prefToInst, new Vector3(transform.position.x, 1.05f, transform.position.z), Quaternion.identity);

        if (GameManager.tutorial == 11) {
            PlayerPrefs.SetInt("Tutorial", GameManager.tutorial);
            GameManager.tutorial = 12;
        }

        timeToOpen = 0;
        canvasObjects.SetActive(false);
    }
}

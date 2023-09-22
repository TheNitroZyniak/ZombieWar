using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
//using UnityStandardAssets.Water;


public class UI : MonoBehaviour{

    [SerializeField] Image levelProgress;
    [SerializeField] EnemySpawner enemySpawner;
    [SerializeField] GameObject victoryPopup, losePopup;
    [SerializeField] PlayerController pL;
    [SerializeField] GameObject menuUI, gameUI;
    [SerializeField] TextMeshProUGUI moneyAmount;
    [SerializeField] GameObject doorPartPref, doorPartPrefCity, gameDonePopup;

    [SerializeField] TextMeshProUGUI[] upgrade;

    [SerializeField] TextMeshProUGUI remainEnemies;
    TextMeshProUGUI currentLevelText;

    [SerializeField] GameObject[] levelSliders;
    Image levelSlider;

    public List<Box> levels;

    public static bool soundsEnabled;

    bool gameStarted;

    [SerializeField] Toggle soundsCheckBox;

    private void Start() {
        if (GameManager.currentLevel < 10) {
            levelSliders[0].SetActive(true);
            levelSliders[1].SetActive(false);
            levelSlider = levelSliders[0].transform.GetChild(1).GetComponent<Image>();
            currentLevelText = levelSliders[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        } 
        else {
            levelSliders[1].SetActive(true);
            levelSliders[0].SetActive(false);
            levelSlider = levelSliders[1].transform.GetChild(1).GetComponent<Image>();
            currentLevelText = levelSliders[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        }


        int i = PlayerPrefs.GetInt("SoundsEnabled");
        if (i == 0) {
            soundsCheckBox.isOn = true;
            soundsEnabled = true;
        }
        else {
            soundsCheckBox.isOn = false;
            soundsEnabled = false;
        }

    }

    private void Update() {

        if (!pL.gameWon) {
            UpdateSlider();
            ShowPopup();
        }

        if (pL.gameLost) {
            ShowLosePopup();
        }

        moneyAmount.text = GameManager.moneyAmount.ToString();
        currentLevelText.text = "Level " + (GameManager.currentLevel + 1).ToString();

        if (GameManager.currentLevel < 10)
            levelSlider.fillAmount = (float)(GameManager.currentLevel) / 10f;
        else
            levelSlider.fillAmount = (float)(GameManager.currentLevel - 10) / 8f;

    }

    void UpdateSlider() {
        levelProgress.fillAmount = (float) EnemySpawner.destroyedEnemies / (float) GameManager.amountOfEnemies;
        remainEnemies.text = "Remain: " + (GameManager.amountOfEnemies - EnemySpawner.destroyedEnemies).ToString();


    }
    float t;
    void ShowPopup() {
        if(EnemySpawner.destroyedEnemies == GameManager.amountOfEnemies && gameStarted && GameManager.amountOfEnemies != 0) {
            victoryPopup.SetActive(true);
        }
    }

    void ShowLosePopup() {
        //losePopup.SetActive(true);
    }
    

    public void OnNextLose() {
        menuUI.SetActive(true);
        gameUI.SetActive(false);
        PlayerPrefs.SetInt("MoneyAmount", GameManager.moneyAmount);
        pL.gameLost = false;
        EnemySpawner.destroyedEnemies = 0;
        GameManager.amountOfEnemies = 0;
        SceneManager.LoadScene("GameScene");
    }

    public void OnNextLevel() {
        if (GameManager.currentLevel != 17)
            menuUI.SetActive(true);
        else
            gameDonePopup.SetActive(true);

        gameUI.SetActive(false);

        pL.gameWon = false;
        EnemySpawner.destroyedEnemies = 0;
        GameManager.amountOfEnemies = 0;
        PlayerPrefs.SetInt("MoneyAmount", GameManager.moneyAmount);
        if (GameManager.currentLevel < 10) {

            levels[(GameManager.currentLevel) % 3].OpenWin();

        } 
        else {
            if(GameManager.currentLevel != 17) 
                levels[(GameManager.currentLevel - 10) % 8].OpenWin();
        }
        GameObject[] gets = GameObject.FindGameObjectsWithTag("GetItem");
        for (int i = 0; i < gets.Length; i++)
            gets[i].transform.GetChild(0).gameObject.SetActive(true);

        GameObject[] weps = GameObject.FindGameObjectsWithTag("GetWeapon");
        for (int i = 0; i < weps.Length; i++)
            weps[i].transform.GetChild(0).gameObject.SetActive(true);

        GameObject[] items = GameObject.FindGameObjectsWithTag("DoorMain");
        for (int i = 0; i < items.Length; i++) {
            //items[i].transform.GetChild(3).gameObject.SetActive(true);

            Transform root = items[i].transform.GetChild(0);

            root.GetComponent<Health>().SetHealth();
            root.GetComponent<Door>().doorCounter = 0;
            root.GetComponent<Door>().parts.Clear();
            root.gameObject.SetActive(true);

            foreach (Transform childs in root) {
                childs.gameObject.SetActive(false);
                Destroy(childs.gameObject, 0.2f);
            }

            StartCoroutine(BuildDoor(root, 0));

        }
        //pL.gameWon = true;

    }

    float[] pos = new float[3] { 4.225f, 3.15f, 1.7f };
    int[] rot = new int[3] { 90, 80, 97 };

    IEnumerator BuildDoor(Transform root, int start) {
        Transform go;
        if(GameManager.currentLevel < 10)
            go = Instantiate(doorPartPref).transform;
        else
            go = Instantiate(doorPartPrefCity).transform;
        
        go.SetParent(root);
        go.SetLocalPositionAndRotation(new Vector3(0, pos[start], 0), Quaternion.Euler(0, 0, rot[start]));

        yield return new WaitForSeconds(0.2f);
        start++;
        if (start < 3)
            StartCoroutine(BuildDoor(root, start));
        
    }

    public void OpenMenu() {
        //menuUI.SetActive(true);
    }


    public void OpenMenu1(bool active) {
        menuUI.SetActive(active);
    }

    public void SetSounds(bool enabled) {
        soundsEnabled = enabled;
        if (soundsEnabled)
            PlayerPrefs.SetInt("SoundsEnabled", 0);
        else
            PlayerPrefs.SetInt("SoundsEnabled", 1);
    }

    public void StartGame() {

        if (GameManager.tutorial == 1) { 
            GameManager.tutorial = 2;
            PlayerPrefs.SetInt("Tutorial", GameManager.tutorial);
        }
        if (GameManager.tutorial == 4) { 
            GameManager.tutorial = 5;
            PlayerPrefs.SetInt("Tutorial", GameManager.tutorial);
        }

        if (GameManager.tutorial == 7) {
            GameManager.tutorial = 8;
            PlayerPrefs.SetInt("Tutorial", GameManager.tutorial);
        }

        if (GameManager.tutorial == 9) {
            GameManager.tutorial = 10;
            PlayerPrefs.SetInt("Tutorial", GameManager.tutorial);
        }
        
        if (GameManager.tutorial == 12) {
            GameManager.tutorial = 13;
            PlayerPrefs.SetInt("Tutorial", GameManager.tutorial);
        }

        GameObject[] spawners = GameObject.FindGameObjectsWithTag("EnemySpawner");
        GameObject[] items = GameObject.FindGameObjectsWithTag("GetItem");
        GameObject[] weps = GameObject.FindGameObjectsWithTag("GetWeapon");

        for (int i = 0; i < items.Length; i++)
            items[i].transform.GetChild(0).gameObject.SetActive(false);

        for (int i = 0; i < weps.Length; i++)
            weps[i].transform.GetChild(0).gameObject.SetActive(false);

        for (int i = 0; i < spawners.Length; i++) {
            //if(spawners[i].GetComponent<EnemySpawner>().wave == 0)
                spawners[i].GetComponent<EnemySpawner>().AddAllEnemies();
                spawners[i].GetComponent<EnemySpawner>().StartSpawning();
        }

        //pL.enemies.Clear();
        gameStarted = true;

        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");
        for (int i = 0; i < doors.Length; i++)
            doors[i].gameObject.SetActive(false);


        GameObject.Find("NavMesh").GetComponent<NavMeshSurface>().BuildNavMesh();

        for (int i = 0; i < doors.Length; i++)
            doors[i].gameObject.SetActive(true);

        StartLevel newLevel = new StartLevel();
        newLevel.level = GameManager.currentLevel;
        newLevel.daysSinceReg = 0;

        //AppMetrica.Instance.SendEventsBuffer();
        string json = JsonUtility.ToJson(newLevel);
        AppMetrica.Instance.ReportEvent("level_start", json);
    }

    public void OpenInventory() {
        OpenMenu1(false);
        SceneManager.LoadScene("Inventory", LoadSceneMode.Additive);
    }


    public void Reset() {
        PlayerPrefs.SetInt("Tutorial", 0);
        PlayerPrefs.SetInt("MoneyAmount", 0);
        PlayerPrefs.SetInt("CurrentLevel", 0);
        PlayerPrefs.SetInt("AllowedWeapon", 0);

        PlayerPrefs.SetInt("GetRPG", 0);
        PlayerPrefs.SetInt("GetAK", 0);

        PlayerPrefs.SetInt("SoundsEnabled", 0);

        for (int i = 0; i < pL.guns.Length; i++) {
            pL.guns[i].Reset();
        }
        SceneManager.LoadScene("GameScene");
    }


    public void OnMusic(int active) { 
    
    }

    public void OnSounds(int active) {

    }

}

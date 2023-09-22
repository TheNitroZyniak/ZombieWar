using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseWeapon : MonoBehaviour{

    public GameObject[] allGuns;
    [SerializeField] GameObject[] gunImages;

    [SerializeField] int levelToOpen;
    PlayerController pl;

    private void Start() {
        pl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        for (int i = 0; i < allGuns.Length; i++) {
            allGuns[i].gameObject.SetActive(false);
        }

        allGuns[PlayerController.currentWeapon].gameObject.SetActive(true);

        if(PlayerController.allowedWeapon >= levelToOpen) {
            gunImages[1].SetActive(true);
            gunImages[0].SetActive(false);
        } 
        else {
            gunImages[0].SetActive(true);
            gunImages[1].SetActive(false);
        }
    }

    public void OnClick(int gunToOpen) {
        if (PlayerController.allowedWeapon >= levelToOpen) {
            for (int i = 0; i < allGuns.Length; i++) {
                pl.guns[i].gameObject.SetActive(false);
                allGuns[i].gameObject.SetActive(false);
            }

            PlayerController.currentWeapon = gunToOpen;
            allGuns[gunToOpen].SetActive(true);
            pl.guns[gunToOpen].gameObject.SetActive(true);
        }
    }


    public void Back() {
        FindObjectOfType<UI>().OpenMenu1(true);
        SceneManager.UnloadSceneAsync(1);
    }
}

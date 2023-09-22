using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Upgrade : MonoBehaviour{

    [SerializeField] GameObject Popup;
    [SerializeField] TextMeshProUGUI[] upgrade;

    PlayerController pl;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {

            pl = other.gameObject.GetComponent<PlayerController>();

            if (GameManager.tutorial == 2) { 
                GameManager.tutorial = 3;
                PlayerPrefs.SetInt("Tutorial", GameManager.tutorial);
            }
            if (GameManager.tutorial == 5) { 
                GameManager.tutorial = 6;
                PlayerPrefs.SetInt("Tutorial", GameManager.tutorial);
            }

            string dmMoney = pl.guns[PlayerController.currentWeapon].moneyToUpgradeDamage[pl.guns[PlayerController.currentWeapon].damageLevel].ToString();
            string distMoney = pl.guns[PlayerController.currentWeapon].moneyToUpgradeDistance[pl.guns[PlayerController.currentWeapon].distanceLevel].ToString();

            

            if (pl.guns[PlayerController.currentWeapon].damageLevel < 4)
                upgrade[0].text = dmMoney;
            else
                upgrade[0].text = "Max";

            if (pl.guns[PlayerController.currentWeapon].distanceLevel < 4)
                upgrade[1].text = distMoney;
            else
                upgrade[1].text = "Max";


            Popup.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")) {           
            Popup.SetActive(false);
        }
    }


    public void UpgradeDamage() {

        int d = pl.guns[PlayerController.currentWeapon].moneyToUpgradeDamage[pl.guns[PlayerController.currentWeapon].damageLevel];

        if (GameManager.moneyAmount >= d) {
            FindObjectOfType<MusicManager>().PlaySound(1);
            int dm = PlayerPrefs.GetInt(pl.guns[PlayerController.currentWeapon].name + "Damage");
            
            pl.guns[PlayerController.currentWeapon].SetDamage();


            if (GameManager.tutorial == 3) {
                GameManager.tutorial = 4;
                PlayerPrefs.SetInt("Tutorial", GameManager.tutorial);
            }

            GameManager.moneyAmount -= d;

            if (pl.guns[PlayerController.currentWeapon].damageLevel < 4)
                upgrade[0].text = pl.guns[PlayerController.currentWeapon].moneyToUpgradeDamage[pl.guns[PlayerController.currentWeapon].damageLevel].ToString();
            else
                upgrade[0].text = "Max";


        } 
        else {
            FindObjectOfType<MusicManager>().PlaySound(0);
        }
    }

    public void UpgradeDistance() {
        int d = pl.guns[PlayerController.currentWeapon].moneyToUpgradeDistance[pl.guns[PlayerController.currentWeapon].distanceLevel];

        if (GameManager.moneyAmount >= d) {
            FindObjectOfType<MusicManager>().PlaySound(1);
            int dist = PlayerPrefs.GetInt(pl.guns[PlayerController.currentWeapon].name + "Distance");
            pl.guns[PlayerController.currentWeapon].SetDistance(dist + 2);


            GameManager.moneyAmount -= d;
            if (GameManager.tutorial == 6) {
                GameManager.tutorial = 7;
                PlayerPrefs.SetInt("Tutorial", GameManager.tutorial);
            }

            if (pl.guns[PlayerController.currentWeapon].distanceLevel < 4)
                upgrade[1].text = pl.guns[PlayerController.currentWeapon].moneyToUpgradeDistance[pl.guns[PlayerController.currentWeapon].distanceLevel].ToString();
            else
                upgrade[1].text = "Max";
        } 
        else {
            FindObjectOfType<MusicManager>().PlaySound(0);
        }
    }
}

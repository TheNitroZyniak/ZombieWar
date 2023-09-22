using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetWeapon : MonoBehaviour{

    [SerializeField] int weaponCount;
    [SerializeField] Transform gun;
    bool isTaken;

    private void Start() {
        int k = PlayerPrefs.GetInt(gameObject.name);
        if(k == 1) 
            gameObject.SetActive(false);
    }

    private void OnEnable() {
        int k = PlayerPrefs.GetInt(gameObject.name);
        if (k == 1)
            gameObject.SetActive(false);
    }

    private void Update() {
        gun.Rotate(Vector3.up, 50 * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            isTaken = true;
            if (PlayerController.allowedWeapon < weaponCount) {
                PlayerController.allowedWeapon = weaponCount;
                PlayerPrefs.SetInt("AllowedWeapon", PlayerController.allowedWeapon);
            }
            PlayerController.currentWeapon = weaponCount;
            other.gameObject.GetComponent<PlayerController>().ChangeWeapon();

            PlayerPrefs.SetInt(gameObject.name, 1);

            gameObject.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Turret : MonoBehaviour{

    [SerializeField] Transform enemyToLookAt;
    [SerializeField] Weapon gun;
    public List<AiController> enemies = new List<AiController>();

    [SerializeField] MeshRenderer cyl;

    private void Start() {
        GameObject[] ens = GameObject.FindGameObjectsWithTag("Enemy");
        for(int i = 0; i < ens.Length; i++) {
            enemies.Add(ens[i].GetComponent<AiController>());
        }
        if(GameManager.currentLevel >= 10) {
            cyl.material.color = Color.grey;
        }
    }

    private void Update() {
        if (!PlayerController.isDead) {
            enemyToLookAt = gun.Attack(enemies, enemyToLookAt, transform);
            if (gun.isShooting) {
                gun.transform.LookAt(enemyToLookAt);
            }
        }
    }
}

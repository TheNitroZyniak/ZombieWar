using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour{

    [SerializeField] Transform shootPoint;
    [SerializeField] bool isPlayerWeapon, showBG;
    [SerializeField] bool isSplash;
    [SerializeField] AudioSource[] audioSources;
    [SerializeField] Bullet bulletPref;
    [SerializeField] float reloadTime = 0.25f;

    public int[] moneyToUpgradeDamage;
    public int[] moneyToUpgradeDistance;

    public int[] damageByLevel;
    public int[] distanceByLevel;


    public int damageLevel, distanceLevel;

    [HideInInspector]
    public bool isShooting;

    float t;

    private void Start() {

        //PlayerPrefs.SetInt(gameObject.name + "Damage", 0);
        //PlayerPrefs.SetInt(gameObject.name + "Distance", 0);

        damageLevel = PlayerPrefs.GetInt(gameObject.name + "Damage");
        distanceLevel = PlayerPrefs.GetInt(gameObject.name + "Distance");


        if(gameObject.name == "TurretA_Base_B_low") {
            if(GameManager.currentLevel < 6) 
                damageLevel = distanceLevel = 0;
            else if(GameManager.currentLevel < 9)
                damageLevel = distanceLevel = 1;
            else
                damageLevel = distanceLevel = 2;
        }
    }

    public Transform Attack(List<AiController> enemies, Transform lookEnemy, Transform parrent) {
        t += Time.deltaTime;
        if (t > reloadTime) {
            t = 0;

                int enemyToShoot = 0;
                float dist = 10000;

            for (int i = 0; i < enemies.Count; i++) {
                if (enemies[i].startMoving) {
                    if (!enemies[i].isDead && !enemies[i].isAboutToDie) {

                        float dist2 = Vector3.Distance(transform.position, enemies[i].transform.position);

                        if (showBG) {
                            if (dist2 < distanceByLevel[distanceLevel]) {
                                enemies[i].ShowBG(true);
                            } else enemies[i].ShowBG(false);
                        }

                        if (dist2 < dist) {
                            dist = dist2;
                            enemyToShoot = i;
                        }
                    }
                }
            }
                if (dist < distanceByLevel[distanceLevel]) {
                    isShooting = true;

                if (UI.soundsEnabled) {
                    for (int i = 0; i < audioSources.Length; i++) {
                        if (!audioSources[i].isPlaying) {
                            audioSources[i].Play();
                            break;
                        }
                    }
                }
                if (isPlayerWeapon) {
                    parrent.LookAt(enemies[enemyToShoot].transform);
                    parrent.rotation = Quaternion.Euler(0, parrent.rotation.eulerAngles.y + 55, 0);
                }
                
                Bullet bullet = Instantiate(bulletPref, shootPoint.position, Quaternion.identity);

                lookEnemy = enemies[enemyToShoot].transform;
                enemies[enemyToShoot]._health.ChangeHealth(-damageByLevel[damageLevel]);

                enemies[enemyToShoot].enHp = enemies[enemyToShoot]._health._currentHealth;
                if (enemies[enemyToShoot]._health._currentHealth <= 0) {
                    enemies[enemyToShoot].isAboutToDie = true;
                }

                List<AiController> chosenEnemies = new List<AiController> {
                    enemies[enemyToShoot]
                };

                if (isSplash) {
                    for (int i = 0; i < enemies.Count; i++) {
                        if (enemies[i] != null && enemies[i].gameObject.activeSelf) {
                            if (!enemies[i].isAboutToDie && !enemies[i].isDead) {
                                if (Vector3.Distance(enemies[enemyToShoot].transform.position, enemies[i].transform.position) < 2) {

                                    chosenEnemies.Add(enemies[i]);
                                    enemies[i]._health.ChangeHealth(-damageByLevel[damageLevel] / 4);
                                    enemies[i].enHp = enemies[i]._health._currentHealth;

                                    if (enemies[i]._health._currentHealth <= 0) {
                                        enemies[i].isAboutToDie = true;
                                    }
                                }
                            }
                        }
                    }
                }

                bullet.Throw(chosenEnemies, isSplash);
            } 
            else {
                isShooting = false;
            }
        }

        return lookEnemy;
    }


    public void SetDamage() {
        damageLevel++;
        PlayerPrefs.SetInt(gameObject.name + "Damage", damageLevel);
    }

    public void SetDistance(int newDistance) {
        distanceLevel++;
        PlayerPrefs.SetInt(gameObject.name + "Distance", distanceLevel);
    }



    public void Reset() {
        PlayerPrefs.SetInt(gameObject.name + "Damage", 0);
        PlayerPrefs.SetInt(gameObject.name + "Distance", 0);
    }
}

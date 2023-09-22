using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.AI.Navigation;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour{
    
    public static bool shooting;
    [SerializeField] Joystick movementJs;
    [SerializeField] float speed = 8;
    [SerializeField] Animator anim; 
    [SerializeField] CharacterController _ctr;
    [SerializeField] GameObject playerCanvas;
    [SerializeField] UI uI;

    public Weapon[] guns;
    public static int currentWeapon = 0;

    public List<AiController> enemies;

    Vector3 moveDirection;
    [SerializeField] Transform enemyToLookAt;
    public event Action<bool> PlayerDead;


    public static bool isDead;
    public bool gameWon, gameLost;

    public static int allowedWeapon;


    private void Start() {
        isDead = false;
        gameWon = false;
        allowedWeapon = PlayerPrefs.GetInt("AllowedWeapon");

        for (int i = 0; i < guns.Length; i++) guns[i].gameObject.SetActive(false);
        guns[currentWeapon].gameObject.SetActive(true);
    }

    void Update() {
        if (!gameLost) {
            if (!isDead) {

                Vector3 moveDirection = new Vector3(movementJs.Horizontal, 0, movementJs.Vertical) * speed;

                

                if (moveDirection != Vector3.zero) {
                    if (GameManager.tutorial == 0) {
                        GameManager.tutorial = 1;
                        PlayerPrefs.SetInt("Tutorial", GameManager.tutorial);
                    }

                    anim.SetBool("isIdle", false);
                    if (moveDirection != Vector3.zero) {
                        anim.SetBool("isAiming", false);
                        anim.SetBool("isMoving", true);
                    }

                } else {
                    anim.SetBool("isAiming", false);
                    anim.SetBool("isMoving", false);
                    anim.SetBool("isIdle", true);

                }

                Move();



                enemyToLookAt = guns[currentWeapon].Attack(enemies, enemyToLookAt, transform);
                if (guns[currentWeapon].isShooting) {
                    if (moveDirection == Vector3.zero) {
                        anim.SetBool("isIdle", false);
                        anim.SetBool("isAiming", true);
                    }
                    transform.LookAt(enemyToLookAt);
                    transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 55, 0);
                }



            } 
            else {
                PlayerDead?.Invoke(true);
                playerCanvas.SetActive(false);
                gameLost = true;
                
            }
        }

    }

    void Move() {
        moveDirection = new Vector3(movementJs.Horizontal, 0, movementJs.Vertical) * speed;

        if (moveDirection != Vector3.zero) {

            if (!shooting) {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 30);
            }
                _ctr.Move(moveDirection * Time.deltaTime);
            } 

    }


    private void OnTriggerEnter(Collider other) {

        if (other.gameObject.CompareTag("NextLevelBlock")) {

            FindObjectOfType<CameraFollow>().enabled = false;

            GetComponent<PlayerAuto>().enabled = true;
            enabled = false;

            //StartCoroutine(NewScene());    
        }

        if (other.gameObject.CompareTag("NewScene")) {
            SceneManager.LoadScene("GameScene");
        }
    }



    public void ChangeWeapon() {
        for (int i = 0; i < guns.Length; i++) {
            guns[i].gameObject.SetActive(false);
        }

        guns[currentWeapon].gameObject.SetActive(true);
    }
}
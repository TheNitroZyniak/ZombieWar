using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using Unity.AI.Navigation;

public class Helper : MonoBehaviour{

    [SerializeField] float speed = 8;
    [SerializeField] Animator anim;
    public Weapon[] guns;
    public List<AiController> enemies;
    [SerializeField] Transform enemyToLookAt;
    public static bool isDead;
    NavMeshAgent agent;
    PlayerController player;
    public int toPlayerDist;
    Vector3 destination;
    bool isPlayerMoving;

    void Start() {

        

        agent = GetComponent<NavMeshAgent>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        //player.GetComponent<PlayerController>().PlayerDead += OnPlayerDead;


        destination = player.transform.position;

        isDead = true;
        //currentIsland = GameManager.currentLevel / 3;
        StartCoroutine(Move());
    }


    IEnumerator Move() {
        yield return new WaitForSeconds(1);
        isDead = false;
    }

    private void Update() {
        if (!isDead) {


            destination = player.transform.position;
            agent.SetDestination(destination);
            transform.LookAt(destination);
            float minDist = Vector3.Distance(transform.position, destination);

            if(enemies.Count > 0)
                enemyToLookAt = guns[0].Attack(enemies, enemyToLookAt, transform);

            // Тут менять 
            if (minDist < 5) {
                isPlayerMoving = false;

                if (guns[0].isShooting) {
                    anim.SetBool("isMoving", false);
                    anim.SetBool("isIdle", false);
                    anim.SetBool("isAiming", true);
                    if (enemies.Count > 0)
                        transform.LookAt(enemyToLookAt);
                    transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 55, 0);
                } 
                else {
                    anim.SetBool("isAiming", false);
                    anim.SetBool("isMoving", false);
                    anim.SetBool("isIdle", true);
                }
                transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                agent.speed = 0;
            }
            if (minDist >= 5 && minDist <= 7) {
                if (isPlayerMoving) {
                    if (guns[0].isShooting) {
                        anim.SetBool("isIdle", false);
                        anim.SetBool("isMoving", true);
                        anim.SetBool("isAiming", false);
                        if (enemies.Count > 0)
                            transform.LookAt(enemyToLookAt);
                        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 55, 0);
                    } else {
                        anim.SetBool("isAiming", false);
                        anim.SetBool("isMoving", true);
                        anim.SetBool("isIdle", false);
                    }

                    transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                    agent.speed = speed;
                } 
                else {
                    if (guns[0].isShooting) {
                        anim.SetBool("isMoving", false);
                        anim.SetBool("isIdle", false);
                        anim.SetBool("isAiming", true);
                        if (enemies.Count > 0)
                            transform.LookAt(enemyToLookAt);
                        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 55, 0);
                    } else {
                        anim.SetBool("isAiming", false);
                        anim.SetBool("isMoving", false);
                        anim.SetBool("isIdle", true);
                    }
                    transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                    agent.speed = 0;
                }
            }
            if(minDist > 7) {
                isPlayerMoving = true;

                if (guns[0].isShooting) {
                    anim.SetBool("isIdle", false);
                    anim.SetBool("isMoving", true);
                    anim.SetBool("isAiming", false);
                    if (enemies.Count > 0)
                        transform.LookAt(enemyToLookAt);
                    transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 55, 0);
                }
                else {
                    anim.SetBool("isAiming", false);
                    anim.SetBool("isMoving", true);
                    anim.SetBool("isIdle", false);
                }

                transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                agent.speed = speed;
            }



        }
    }
}

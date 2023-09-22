using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiController : MonoBehaviour{
    public string Name;
    [SerializeField] PlayerController player;
    [SerializeField] GameObject HealthBar;
    [SerializeField] Grenade grenadePref;
    public Health _health;
    [SerializeField] Transform[] destinations;
    NavMeshAgent agent;

    public int attackDist;
    public float speed;

    public int[] damage;
    public int[] moneyOnKill;
    int currentIsland;

    public AudioSource source;
    public Animator anim;
    bool throwed;
    public bool isDead, isAboutToDie;
    public bool startMoving;

    public bool isAcid;

    public Transform destination;
    public static int doorDestroyed;
    public int enHp;
    public bool isBoss;
    public bool showBGOrNot;
    bool move;

    void Start(){
        agent = GetComponent<NavMeshAgent>();
        source = GetComponent<AudioSource>();
        ResetEnemy();
        //transform.localScale = Vector3.zero;
    }



    IEnumerator Raise() {
        float a = 0;
        Vector3 endScale = transform.localScale;
        while (a <= 1) {
            transform.localScale = Vector3.Lerp(Vector3.zero, endScale, a);
            yield return new WaitForFixedUpdate();
            a += 0.125f;
        }
        transform.localScale = endScale;

    }


    void FixedUpdate() {
        if (startMoving) {
            destination = FindDestination();

            if (!isAcid) {


                FindEnemy();
            } else {
                FindEnemyAcid();
            }
        } 
        else {
            if (isBoss) {
                transform.rotation = Quaternion.Euler(0, 205,0);

                if (GameManager.amountOfEnemies - EnemySpawner.destroyedEnemies == 1) {
                    anim.SetBool("isMoving", true);
                    move = true;
                    speed = 2;
                    startMoving = true;
                }

            }
        }
    }

    Transform FindDestination() {
        float minDist = 10000;
        int destToAttack = 0;
        for (int i = 0; i < destinations.Length; i++) {
            if (destinations[i].gameObject.activeSelf) {
                float dist = Vector3.Distance(transform.position, destinations[i].position);
                if (dist < minDist) {
                    minDist = dist;
                    destToAttack = i;
                }
            }
        }
        return destinations[destToAttack];
    }


    void FindEnemy() {
        float minDist = Vector3.Distance(transform.position, destination.position);
        if (minDist < attackDist) {
            anim.SetBool("isMoving", false);
            anim.SetBool("isAttacking", true);
            agent.speed = 0;
            Attack();
            transform.LookAt(destination);
        } 
        else{
            anim.SetBool("isMoving", true);
            anim.SetBool("isAttacking", false);

            agent.speed = speed;
            agent.SetDestination(destination.position);
            transform.LookAt(destination);
        }
    }

    float acidTime = 1;
    void FindEnemyAcid() {
        float minDist = Vector3.Distance(transform.position, destination.position);
        anim.SetBool("isMoving", true);
        anim.SetBool("isAttacking", false);
        agent.SetDestination(destination.position);
        transform.LookAt(destination);
        if (minDist < attackDist) {
            acidTime += Time.deltaTime;
            if(acidTime > 1) {
                destination.GetComponent<Health>().ChangeHealth(-damage[currentIsland]);
                destination.GetComponent<Health>().ChangeSlider();
                acidTime = 0;
            }
        } 
        else {
            acidTime = 1;
        }
    }

    public void ShowBG(bool show) {
        if(showBGOrNot)
            HealthBar.SetActive(show);
    }

    void Attack() {
        AnimatorStateInfo animState = anim.GetCurrentAnimatorStateInfo(0);
        float currentTime = animState.normalizedTime % 1;

        if (currentTime < 0.1f) {
            if (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "ZombieAttack")
                throwed = false;
            if (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Throw")
                throwed = false;
        }
        if (currentTime > 0.35f && !throwed) {
            if (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "ZombieAttack") {
                throwed = true;
                destination.GetComponent<Health>().ChangeHealth(-damage[currentIsland]);
                destination.GetComponent<Health>().ChangeSlider();
            }
            if (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Throw") {
                Grenade grenade = Instantiate(grenadePref, transform.position, Quaternion.identity);
                grenade.Throw(destination, damage[currentIsland]);
                throwed = true;
            }
        }
    }

    void OnPlayerDead(bool dead) {
        startMoving = false;
        agent.speed = 0;
        anim.SetBool("isWinning", true);
        HealthBar.SetActive(false);
    }

    public void OnDead() {
        float rotY = transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0, Random.Range(rotY - 45, rotY + 45),0);
        startMoving = false;
        
        agent.speed = 0;
        agent.enabled = false;
        GetComponent<Collider>().enabled = false;
        player.GetComponent<PlayerController>().PlayerDead -= OnPlayerDead;
        GameManager.earnedMoney += moneyOnKill[currentIsland];
        GameManager.moneyAmount += moneyOnKill[currentIsland];
        HealthBar.SetActive(false);
        anim.SetBool("isDead", true);

        //anim.SetBool("isWinning", true);
    }

    public void ChangeEnSlider(int enemyHp) {
        if (!isDead) {
            _health.ChangeSliderEnemy(enemyHp);
            if (enemyHp <= 0) {
                isDead = true;
                DestroyEn();
            }
        }
    }
    public void DestroyEn() {
        _health.DestroyEnemy();
    }


    public void ResetEnemy() {
        //transform.position = new Vector3(0, 0, -45);
        _health.ResetHealth();
        agent.enabled = true;
        if(!isBoss)
            anim.SetBool("isMoving", true);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isWinning", false);
        anim.SetBool("isDead", false);
        isDead = false;
        isAboutToDie = false;
        startMoving = false;
        //transform.position = new Vector3(0, 0, -45);

        SetStartParams();
    }


    public void SetStartParams() {
        doorDestroyed = 0;
        

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player.GetComponent<PlayerController>().PlayerDead += OnPlayerDead;
        player.enemies.Add(this);
        //print(player.enemies.Count);

        GameObject go = GameObject.FindGameObjectWithTag("Helper");
        Helper helper = null;

        if (go != null)
            helper = GameObject.FindGameObjectWithTag("Helper").GetComponent<Helper>();


        if (helper != null)
            helper.enemies.Add(this);

        GameObject[] turrets = GameObject.FindGameObjectsWithTag("Turret");
        for (int i = 0; i < turrets.Length; i++) {
            turrets[i].GetComponent<Turret>().enemies.Add(this);
        }

        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");


        if (helper == null)
            destinations = new Transform[doors.Length + 1];
        else
            destinations = new Transform[doors.Length + 2];


        for (int i = 0; i < doors.Length; i++) {
            destinations[i] = doors[i].transform;
        }

        destinations[destinations.Length - 1] = player.transform;

        if (helper == null)
            destinations[destinations.Length - 1] = player.transform;
        else {
            destinations[destinations.Length - 2] = helper.transform;
            destinations[destinations.Length - 1] = player.transform;
        }
        StartCoroutine(Raise());
        currentIsland = GameManager.currentLevel / 3;
        if (GameManager.currentLevel < 10) {
            currentIsland = GameManager.currentLevel / 3;
        } else {
            currentIsland = GameManager.currentLevel / 3 + 1;
        }

        if (isBoss) speed = 0;

        if(!isBoss)
            startMoving = true;

        //gameObject.SetActive(false);
    }


    public void StartWalk() {
        gameObject.SetActive(true);
        //agent.enabled = true;
        startMoving = true;
    }
}

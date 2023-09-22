using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour{
    [Header("Health stats")]
    [SerializeField] int _maxHealth = 100;
    public int _currentHealth = 100;
    public event Action<float> HealthChanged;
    [SerializeField] SkinnedMeshRenderer rend;
    Color startColor;

    private void Awake() {
        _currentHealth = _maxHealth;
        if(rend != null)
            startColor = rend.material.color;
    }

    public void ChangeHealth(int value) {
        _currentHealth += value;
    }

    public void SetHealth() {
        _currentHealth = _maxHealth;
        ChangeSlider();
    }


    public void ChangeSlider() {
        if (_currentHealth <= 0) {

            if (gameObject.CompareTag("Door")) {
                AiController.doorDestroyed++;
                //gameObject.SetActive(false);
            }
            if (gameObject.CompareTag("Enemy")) {
                
            }
            if (gameObject.CompareTag("Player") || gameObject.CompareTag("Helper")) {
                PlayerController.isDead = true;
                gameObject.GetComponent<Animator>().SetBool("isDead", true);

                LevelFailed lvlFailed = new LevelFailed();
                lvlFailed.level = GameManager.currentLevel;
                lvlFailed.timeSpent = 34;
                lvlFailed.daysSinceReg = 0;

                //AppMetrica.Instance.SendEventsBuffer();
                string json = JsonUtility.ToJson(lvlFailed);
                AppMetrica.Instance.ReportEvent("level_fail", json);

            }
        } 
        else {
            float _currentHealthAsPercantage = (float)+_currentHealth / _maxHealth;
            HealthChanged?.Invoke(_currentHealthAsPercantage);
        }
        
    }
    public void ChangeSliderEnemy(int health) {
        float _currentHealthAsPercantage = (float)+health / _maxHealth;
        HealthChanged?.Invoke(_currentHealthAsPercantage);
        StartCoroutine(returnColorBack());

    }

    IEnumerator returnColorBack() {
        rend.material.color = new Color(0.8f, 0.8f, 0.8f, 1f);
        yield return new WaitForSeconds(0.1f);
        rend.material.color = startColor;
    }


    public void DestroyEnemy() {
        GetComponent<AiController>().OnDead();
        EnemySpawner.destroyedEnemies++;
        StartCoroutine(Fade());
    }


    IEnumerator Fade() {
        yield return new WaitForSeconds(5);
        float a = 1;
        while(a > 0) {
            rend.material.color = new Color(startColor.r, startColor.g, startColor.b, a);
            a -= 0.02f;
            yield return new WaitForFixedUpdate();
        }
        rend.material.color = rend.material.color = new Color(startColor.r, startColor.g, startColor.b, 0);


        gameObject.SetActive(false);
        Destroy(gameObject, 1);
        //GetComponent<AiController>().ResetEnemy();
    }

    public void ResetHealth() {
        rend.material.color = startColor;

        _currentHealth = _maxHealth;
        ChangeSlider();
    }
}

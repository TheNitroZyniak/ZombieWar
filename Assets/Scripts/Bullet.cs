using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour{

    [SerializeField] GameObject explosionPref;
    public int enemyHp = 0;

    public void Throw(List<AiController> enemies, bool isSplash) {
        StartCoroutine(FlyToEnemy(enemies, isSplash));
    }

    IEnumerator FlyToEnemy(List<AiController> enemies, bool isSplash) {
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(enemies[0].transform.position.x, 1.5f, enemies[0].transform.position.z);
        float a = 0;
        float S = Vector3.Distance(startPos, endPos);
        float v = 60;
        float alphaChange = 0.02f;

        while (a <= S) {
            Vector3 enPos = new Vector3(enemies[0].transform.position.x, 1.5f, enemies[0].transform.position.z);
            transform.position = Vector3.Lerp(startPos, enPos, (float)a / (float)S);
            transform.LookAt(enPos);
            yield return new WaitForFixedUpdate();
            a += alphaChange * v;
        }

        if (enemies[0].gameObject.activeSelf) {
            enemies[0].ChangeEnSlider(enemies[0].enHp);
        }

        if (isSplash) {
            Destroy(Instantiate(explosionPref, transform.position, Quaternion.identity), 1);
            for (int i = 1; i < enemies.Count; i++) {
                enemies[i].ChangeEnSlider(enemies[i].enHp);
            }
        }
        //if (UI.soundsEnabled)
            //source.Play();

        transform.position = new Vector3(-100, -100, -100);
        Destroy(gameObject, 1);
    }
}

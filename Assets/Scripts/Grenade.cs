using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour{
    [SerializeField] GameObject explosionPref, bombPlace;
    [SerializeField] bool isAcid;

    public void Throw(Transform enemyPos, int damage) {

        StartCoroutine(FlyToPoint(enemyPos, damage));

    }

    IEnumerator FlyToPoint(Transform enemyPos, int damage) {

        Vector3 endPos = enemyPos.position;

        GameObject go = Instantiate(bombPlace, new Vector3(endPos.x, 0.03f, endPos.z), bombPlace.transform.rotation);
        Transform bombT = go.transform.GetChild(0);

        Vector3 startPos = transform.position;
        float a = 0;
        float S = Vector3.Distance(startPos, endPos);
        float v = 8;

        float alphaChange = 0.02f;

        while (a <= S) {
            float f = (float)a / (float)S;
            transform.position = Vector3.Lerp(startPos, endPos, f);
            bombT.localScale = new Vector3(f, f, 1);

            yield return new WaitForFixedUpdate();
            a += alphaChange * v;
        }

        transform.position = new Vector3(-100, -100, -100);

        if (!isAcid) {
            float dist = Vector3.Distance(enemyPos.position, endPos);


            if (dist < 2) {
                if (enemyPos.gameObject.activeSelf) {
                    enemyPos.GetComponent<Health>().ChangeHealth(-damage);
                    enemyPos.GetComponent<Health>().ChangeSlider();

                }
            }

            Destroy(Instantiate(explosionPref, new Vector3(bombT.position.x, 1, bombT.position.z), Quaternion.identity), 1);
        } 
        else {
            Destroy(Instantiate(explosionPref, new Vector3(bombT.position.x, 1, bombT.position.z), Quaternion.identity), 3);
        }

        Destroy(gameObject, 1);
        Destroy(go);
    }
}

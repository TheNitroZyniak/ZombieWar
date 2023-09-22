using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Repair : MonoBehaviour{

    [SerializeField] Health itemToRepair;
    [SerializeField] Image loadingBar;
    [SerializeField] int moneyToOpen;

    [SerializeField] GameObject doorPartPref;
    [SerializeField] GameObject dustSmoke;
    float timeToOpen;

    private void OnTriggerStay(Collider other) {
        if (GameManager.moneyAmount >= moneyToOpen) {

            if (other.gameObject.CompareTag("Player")) {

                timeToOpen += Time.deltaTime;
                loadingBar.fillAmount = (float)timeToOpen / 2f;

                if (timeToOpen > 2) {
                    itemToRepair.SetHealth();
                    itemToRepair.gameObject.GetComponent<Door>().doorCounter = 0;
                    itemToRepair.gameObject.GetComponent<Door>().parts.Clear();
                    Transform root = gameObject.transform.parent.transform.parent.GetChild(0);
                    root.gameObject.SetActive(true);

                    foreach(Transform childs in root) {
                        childs.gameObject.SetActive(false);
                        Destroy(childs.gameObject, 0.2f);
                    }

                    Transform go = Instantiate(doorPartPref).transform;
                    go.SetParent(root);
                    go.SetLocalPositionAndRotation(new Vector3(0, 4.225f, 0), Quaternion.Euler(0, 0, 90));
                    //Instantiate(dustSmoke, go.transform.position, Quaternion.identity);

                    Transform go2 = Instantiate(doorPartPref).transform;
                    go2.SetParent(root);
                    go2.SetLocalPositionAndRotation(new Vector3(0, 3.15f, 0), Quaternion.Euler(0, 0, 80));
                    //Instantiate(dustSmoke, go2.transform.position, Quaternion.identity);

                    Transform go3 = Instantiate(doorPartPref).transform;
                    go3.SetParent(root);
                    go3.SetLocalPositionAndRotation(new Vector3(0, 1.7f, 0), Quaternion.Euler(0, 0, 97));
                    //Instantiate(dustSmoke, go3.transform.position, Quaternion.identity);

                    timeToOpen = 0;
                    loadingBar.fillAmount = 0;
                    transform.parent.gameObject.SetActive(false);
                }


            }
        }
    }



    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            timeToOpen = 0;
            loadingBar.fillAmount = 0;
        }
    }

}

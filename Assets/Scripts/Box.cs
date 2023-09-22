using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using Unity.AI.Navigation;

public class Box : MonoBehaviour {
    [SerializeField] GameObject winObjects;
    [SerializeField] GameObject nextDoor;

    public bool raise;

    private void Start() {
        
    }


    void OnEnable() {
        if (raise)
            StartCoroutine(Raise());
        else
            transform.localScale = Vector3.one;
    }

    IEnumerator Raise() {
        float a = 0;

        while(a <= 1) {
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, a);
            yield return new WaitForFixedUpdate();
            a += 0.1f;
        }

        transform.localScale = Vector3.one;
        StartCoroutine(CreateMesh());
    }

    IEnumerator CreateMesh() {
        yield return new WaitForSeconds(0.1f);
        GameObject.Find("NavMesh").GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    public void OpenWin() {
        winObjects.SetActive(true);
    }

    public void OpenDoor() {
        nextDoor.SetActive(false);
    }

}

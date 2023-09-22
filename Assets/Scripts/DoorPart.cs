using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPart : MonoBehaviour{
    [SerializeField] MeshRenderer meshRend;
    [SerializeField] Rigidbody rb;

    private void Start() {
        transform.parent.transform.parent.gameObject.GetComponent<Door>().parts.Add(this);
    }

    public void ThrowParts() {

        rb.transform.SetParent(null);
        rb.useGravity = true;
        rb.isKinematic = false;
        int throwForward = Random.Range(0, 2);
        if (throwForward == 0) {
            rb.velocity = new Vector3(Random.Range(-5, 5), Random.Range(1, 4), Random.Range(1, 4));
        } else {
            rb.velocity = new Vector3(Random.Range(-5, 5), Random.Range(1, 4), Random.Range(-4, -1));
        }

        throwForward = Random.Range(0, 2);
        if (throwForward == 0) {
            rb.angularVelocity = new Vector3(0, Random.Range(1, 6), 0);
        } else {
            rb.angularVelocity = new Vector3(0, Random.Range(-6, -1), 0);
        }
        StartCoroutine(FadeDoor());

    }


    IEnumerator FadeDoor() {
        Color startColor = meshRend.material.color;
        yield return new WaitForSeconds(3);
        float a = 1;
        while (a > 0) {
            meshRend.material.color = new Color(startColor.r, startColor.g, startColor.b, a);
            a -= 0.02f;
            yield return new WaitForFixedUpdate();
        }
        meshRend.material.color = meshRend.material.color = new Color(startColor.r, startColor.g, startColor.b, 0);
        gameObject.SetActive(false);
        Destroy(gameObject, 1);

    }

}

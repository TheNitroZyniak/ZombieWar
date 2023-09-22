using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PlayerAuto : MonoBehaviour{
    [SerializeField] CharacterController _ctr;
    [SerializeField] Animator anim;


    private void OnEnable() {
        //if (transform.position.x < 15) {
        //    Debug.Log("Ass we can");
        //    while(transform.position.x > -10)
        //        transform.SetPositionAndRotation(new Vector3(-20, 0, 0), Quaternion.identity);
        //}
    }

    private void LateUpdate() {
        Vector3 moveDirection = Vector3.right * 8;

        anim.SetBool("isIdle", false);
        anim.SetBool("isAiming", false);
        anim.SetBool("isMoving", true);
        transform.rotation = Quaternion.Euler(0, 90, 0);
        
        _ctr.Move(moveDirection * Time.deltaTime);


        //if(transform.position.x > 0 && transform.position.x < 10) {
            
        //    transform.position = Vector3.zero;
        //    FindObjectOfType<UI>().OpenMenu1(true);
        //    GameObject go = GameObject.Find("Main Camera");
        //    go.GetComponent<CameraFollow>().enabled = true;


        //    GetComponent<PlayerController>().enabled = true;
        //    enabled = false;
        //}

    }


    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("NextLevelMove")) {
            SceneManager.LoadScene("GameScene");
        }
    }
}

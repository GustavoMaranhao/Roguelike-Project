using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skelletonController : MonoBehaviour {

    public float speed;
    public Animator anim;
    bool isAttacking;

    // Use this for initialization
    void Start(){
        anim.SetBool("isWalking", false);
        isAttacking = false;
        speed = 3.0f;
    }
	
	// Update is called once per frame
	void Update(){
        Vector3 direction = Vector3.zero;
        if(Input.anyKey){

            if (Input.GetButton("Fire1"))
            {
                isAttacking = true;
                anim.SetTrigger("slash");
                return;
            }

            if (!isAttacking && (Input.GetButton("X Axis")))
            {
                float translationV = Input.GetAxis("Vertical");
                float translationH = Input.GetAxis("Horizontal");
                direction = new Vector3(translationH, 0f, translationV);
                direction += Quaternion.Euler(new Vector3(0, 90, 0)) * direction;
                transform.forward = direction;
                transform.position += direction * speed * Time.deltaTime;
                anim.SetBool("isWalking", true);
            }
        }
        else{
            anim.SetBool("isWalking", false);
        }

        if (isAttacking && !anim.GetCurrentAnimatorStateInfo(0).IsName("SwordSlash"))
        {
            isAttacking = false;
            anim.ResetTrigger("slash");
        }

        //rotation   
        //cast to Vector2 sets z to 0
        Vector2 positionOnScreen = (Vector2)Camera.main.WorldToViewportPoint(transform.position);
        Vector2 mouseOnScreen    = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition + Vector3.forward * 5f);
        float angle = AngleBetween2Points(positionOnScreen, mouseOnScreen);
        //angle+45f to transform to isometric
        transform.rotation = Quaternion.Euler(new Vector3(0f, -(angle+45f), 0f));

    }

    float AngleBetween2Points(Vector3 a, Vector3 b){
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
}

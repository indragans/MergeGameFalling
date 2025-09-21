using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Transform tr;
   // Animator anim;
    private Vector3 movement;

    public float limitX = 2f;   // batas kanan-kiri
    public float speed = 10f;   // kecepatan mengikuti jari
    

    void Start()
    {
        tr = GetComponent<Transform>();
        //anim = GetComponent<Animator>();   // ambil animator di player
    }

    void Update()
    {
        #if UNITY_EDITOR || UNITY_STANDALONE   // kalau dijalankan di PC
        HandleKeyboardMovement();
#elif UNITY_ANDROID || UNITY_IOS       // kalau di Android/iOS
            HandleTouchMovement();
#endif
    
    }

   

    // --- FUNGSI UNTUK PC ---
    void HandleKeyboardMovement()
    {
        float move = 0f;

        if (Input.GetKey("right"))
            move = 1f;
        else if (Input.GetKey("left"))
            move = -1f;

        // geser player
        tr.position += new Vector3(move * speed * Time.deltaTime, 0f, 0f);

        // clamp biar gak keluar batas
        tr.position = new Vector3(Mathf.Clamp(tr.position.x, -limitX, limitX), tr.position.y, tr.position.z);

        // animasi jalan
       // anim.SetBool("WalkLeft", move < 0);
       // anim.SetBool("WalkRight", move > 0);

        // balik arah (flip)
        if (move < 0)
            tr.localScale = new Vector3(1f, 1f, 1f);
        else if (move > 0)
            tr.localScale = new Vector3(1f, 1f, 1f);
    }


    // --- FUNGSI UNTUK ANDROID ---
    void HandleTouchMovement()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            Vector3 pos = Camera.main.ScreenToWorldPoint(
                new Vector3(touch.position.x, touch.position.y, Camera.main.nearClipPlane)
            );

            float targetX = Mathf.Clamp(pos.x, -limitX, limitX);

            // smooth lerp biar halus
            Vector3 newPos = Vector3.Lerp(
                tr.position,
                new Vector3(targetX, tr.position.y, tr.position.z),
                Time.deltaTime * speed
            );

            // cek apakah posisinya berubah
            bool WalkLeft = Mathf.Abs(newPos.x - tr.position.x) > 0.001f;

            tr.position = newPos;

            //anim.SetBool("WalkLeft", WalkLeft);
        }
        /*else
        {
           anim.SetBool("WalkLeft", false);
        }*/
    }
}

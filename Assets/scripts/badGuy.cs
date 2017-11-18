using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class badGuy : MonoBehaviour {

    public enum Dir { left, right, up, down };
    public Dir direction;
    public float speed;
    public AudioClip badGuySound;


    Rigidbody2D rb;

    //move on awake?
        //needs a trigger buddy

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        gameObject.tag = "enemy";
        
    }
	
	// Update is called once per frame
	void Update () {
        moveDir();
    }

    public void moveDir()
    {
        if(direction == Dir.left)
        {
            Vector3 move = new Vector3(-speed, rb.velocity.y, 0f);
            rb.velocity = move;
        }
        if(direction == Dir.right)
        {
            Vector3 move = new Vector3(speed, rb.velocity.y, 0f);
            rb.velocity = move;
        }
        if (direction == Dir.down)
        {
            Vector3 move = new Vector3(rb.velocity.x, -speed, 0f);
            rb.velocity = move;
        }
        if (direction == Dir.up)
        {
            Vector3 move = new Vector3(rb.velocity.x, speed, 0f);
            rb.velocity = move;
        }
    }

    public void _triggered()
    {

    }

    //ghost behavior

}

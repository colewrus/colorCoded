using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PhysControl : System.Object
{
    [Tooltip("Y VELOCITY CONSTRAINT: Limits how fast you can move along the Y axis at any give time")]
    public float yVelocity_Constraint;
    public float xVelocity_Constraint;
    [Tooltip("This determines how much you will bounce off of an enemy when you stomp on them. Set to 0 if you aren't jumping on enemy")]
    public float enemyBounce_Amount;
}



public class playerMovement : MonoBehaviour {
	public enum GameType { platformer, endlessRunner };

    public GameType myGame;
    public Transform groundCheckObject;
    public float groundCheckDistance;
    public bool grounded;
    public float jumpTimer;

    public float jumpForce;
    public float speed;

    bool playerContorl; //am i cool with the player controlling the character?
    //why wouldn't I be?
    //Animations, events, stun lock
    public PhysControl physControl;
    Rigidbody2D rb; //grabs the object's rigidBody

    bool beingPushed;
    Vector2 pushDir;
    float pushPower;

    void Start()
    {       
        beingPushed = false; 
        rb = GetComponent<Rigidbody2D>();
        Debug.Log("You chose " + myGame + "! Good Luck designer!");
        grounded = true;
    }



    void FixedUpdate()
    {
        //jump contrainer - keeps player from flying to far too fast
        if (rb.velocity.y > physControl.yVelocity_Constraint)
        {         
            Vector3 pullDown = new Vector3(rb.velocity.x, physControl.yVelocity_Constraint, 0f);            
            rb.velocity = pullDown;         
        }
        if(myGame == GameType.platformer)
            platformer_Controls();
    }


    void platformer_Controls()
    {
        float x = Input.GetAxis("Horizontal");
        Vector3 move = new Vector3(x * speed, rb.velocity.y, 0f);
        rb.velocity = move;
        if (Input.GetKey(KeyCode.Space))
        {
            if (grounded)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                grounded = false;             
            }
            else
            {
            }
        }

        //If you change the animations it needs to be named properly or the string needs to be changed
        if (x > 0)
        {
            transform.gameObject.GetComponent<Animator>().Play("right");
        }
        if (x < 0)
        {
            transform.gameObject.GetComponent<Animator>().Play("left");
        }
        if (x == 0)
        {
            transform.gameObject.GetComponent<Animator>().Play("idle");
        }
    }




    IEnumerator JumpDelay(float t)
    {        
        yield return new WaitForSeconds(t);
        grounded = true;
    }
    void endlessRunner_Controls()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.transform.tag == "ground")
        {
            grounded = true;
        }else if (collision.gameObject.tag == "door")
        {
            Debug.Log("hit door");
            if (gameManager.instance.current_Collect >= collision.gameObject.GetComponent<genericObjects>().coinCost)
            {
                collision.gameObject.SetActive(false);
            }
        }else
        {

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.transform.tag == "ground")
        {
            grounded = false;
        }
    }

    void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "collectible") {
            //make it run a function in the object script that runs the following, this allows the player to set the varibles for the objects
            //UI change
            //playsound
            //col.gameObject.GetComponent<genericObjects>().playSound();           
                        
        }

        if(col.gameObject.tag == "killzone")
        {
            Color tmp = gameObject.GetComponent<SpriteRenderer>().color;
            tmp.a = 0;
            gameObject.GetComponent<SpriteRenderer>().color = tmp;
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            StartCoroutine(PlayerRespawn(1.1f));
        }

        if (col.gameObject.tag == "coin")
        {
            gameManager.instance.current_Collect++;
            gameManager.instance.coins.text = "Coins: " + gameManager.instance.current_Collect;
            Debug.Log(gameManager.instance.current_Collect);
            col.gameObject.SetActive(false);
        }

        if (col.gameObject.tag == "enemy")
        {
            gameManager.instance.current_Enemy++;
            //if col.sound != null
                //play sound
            Vector3 move = new Vector3(rb.velocity.x, physControl.enemyBounce_Amount, 0f);
            rb.velocity = move;
            col.gameObject.SetActive(false);
        }else if(col.gameObject.tag == "pusher"){    
            //if col.sound != null
                //play sound       
            rb.AddForce(transform.up * col.GetComponent<genericObjects>().pushPower, ForceMode2D.Impulse);
        }
        if (col.gameObject.tag == "door")
        {
            Debug.Log(col.gameObject.GetComponent<genericObjects>().coinCost);
            if(gameManager.instance.current_Collect >= col.gameObject.GetComponent<genericObjects>().coinCost)
            {
                Debug.Log(gameManager.instance.collectCount + ", " + col.gameObject.GetComponent<genericObjects>().coinCost);
                col.gameObject.GetComponent<Animator>().Play("flash");                
            }           
        }else if (col.gameObject.name == "spawner")
        {
            Debug.Log("spawner");
            col.gameObject.GetComponent<spawner>().loopSpawn = false;
            Color tmp = gameObject.GetComponent<SpriteRenderer>().color;
            tmp.a = 0.3f;
            col.gameObject.GetComponent<SpriteRenderer>().color = tmp;
        }

        if(col.gameObject.tag == "victory")
        {
            gameManager.instance.winText.gameObject.SetActive(true);
        }
	}
     
    IEnumerator PlayerRespawn(float t)
    {
        yield return new WaitForSeconds(t);
        transform.position = GameObject.Find("spawnPoint").transform.position;
        Color tmp = gameObject.GetComponent<SpriteRenderer>().color;
        tmp.a = 1;
        gameObject.GetComponent<SpriteRenderer>().color = tmp;
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.5f;
    }

}



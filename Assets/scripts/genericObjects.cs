using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public enum objType
{
    collectible,
    powerUp,
    pusher,
    door
    //victoryObject
};

public class genericObjects : MonoBehaviour {

	

	public objType myIdentity;
     
    public AudioClip mySound;
    
    //collectible variables
    public float value;
    public Text myCollectible;


    //pusher variables
    public bool visible_Pusher;
    public float pushPower;
    GameObject particlesEffectObj;

    //door variables
    public int coinCost;
    public bool passable;
    public bool teleport;
    public Vector2 teleport_Destination;

	// Use this for initialization
	void Start () {
		if (myIdentity == objType.collectible) {
			gameObject.tag = "collectible";
			gameObject.GetComponent<BoxCollider2D> ().isTrigger = true;
		}

        if(myIdentity == objType.pusher)
        {
            gameObject.tag = "pusher";
            gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            visible_Pusher = true;
        }
        if(myIdentity == objType.door)
        {
            gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            if (passable)
            {
                gameObject.GetComponent<CircleCollider2D>().isTrigger = true;
            }else if (!passable)
            {
                gameObject.GetComponent<CircleCollider2D>().isTrigger = false;
            }
            

        }
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

     public void playSound()
    {
        Debug.Log(mySound);
    }
}


[CustomEditor(typeof(genericObjects))]
public class genericObjectsEditor : Editor
{
    override public void OnInspectorGUI()
    {
        genericObjects myGui = target as genericObjects; //reference the script that has the variables we want to use

        // These guys should be visible no matter what
        myGui.myIdentity = (objType)EditorGUILayout.EnumPopup(myGui.myIdentity);
        myGui.mySound = (AudioClip)EditorGUILayout.ObjectField("My Sound", myGui.mySound, typeof(AudioClip), true);


        //different options for different objects

        if (myGui.myIdentity == objType.pusher) // PUSH OBJECT
        {
            myGui.pushPower = EditorGUILayout.FloatField("Push Power", myGui.pushPower);            
        }

        if(myGui.myIdentity == objType.collectible)
        {
            myGui.value = EditorGUILayout.FloatField("Value", myGui.value);
        }
        if(myGui.myIdentity == objType.door)
        {
            myGui.coinCost = EditorGUILayout.IntField("Coin Cost", myGui.coinCost);
            myGui.passable = EditorGUILayout.Toggle("Passable", myGui.passable);
            myGui.teleport = EditorGUILayout.Toggle("Teleport?", myGui.teleport);
            if(myGui.teleport)
                myGui.teleport_Destination = EditorGUILayout.Vector2Field("Destination", myGui.teleport_Destination);
        }
    }
}

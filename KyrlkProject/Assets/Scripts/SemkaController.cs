using UnityEngine;
using System.Collections;

public class SemkaController : MonoBehaviour {
    private Rigidbody2D myRigidbody;

    public float Y;
    
    public bool onGround = false;
	// Use this for initialization
	void Start () {
        myRigidbody = GetComponent<Rigidbody2D>();

    }
	
	// Update is called once per frame
	void Update () {
        Y = myRigidbody.position.y;
        if (myRigidbody.position.y <= 0)
            onGround = true;
   }

   
    void GetOnGround (bool check)
    {
        check = onGround;
    }
}

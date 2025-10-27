using UnityEngine;
using System.Collections;
using UnityEngine.Animations;
public class playerControler : MonoBehaviour
{
    //Movement Variables
    public float runSpeed;
    Rigidbody myRB;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myRB = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        float move = Input.GetAxis("Horizontal");
        

        myRB.linearVelocity = new Vector3(move * runSpeed, myRB.linearVelocity.y, 0);
    }
}

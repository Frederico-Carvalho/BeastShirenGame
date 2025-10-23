using UnityEngine;
using System.Collections;
using UnityEngine.Animations;
public class playerControler : MonoBehaviour
{
    //Movement Variables
    public float runSpeed;
    Rigidbody myRB;
    Animator myAnim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myRB = GetComponent<Rigidbody>();
        myAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        float move = Input.GetAxis("Horizontal");
        myAnim.SetFloat("speed", Mathf.Abs(move));

        myRB.linearVelocity = new Vector3(move * runSpeed, myRB.linearVelocity.y, 0);
    }
}

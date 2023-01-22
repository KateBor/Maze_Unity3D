using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody))] //не шарю
public class ControlBall : MonoBehaviour
{
  private bool _isGrounded;
  private Rigidbody _rb;

  public float speed;
    // Start is called before the first frame update
    void Start()
    {
    _rb = GetComponent<Rigidbody>();
  }

  void FixedUpdate()
  {
    MovementLogic();
  }


  private void MovementLogic()
  {
    float moveHorizontal = Input.GetAxis("Horizontal");

    float moveVertical = Input.GetAxis("Vertical");

    Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

    _rb.AddForce(movement * speed);
  }


  void OnCollisionEnter(Collision collision)
  {
    IsGroundedUpate(collision, true);
  }

  void OnCollisionExit(Collision collision)
  {
    IsGroundedUpate(collision, false);
  }

  private void IsGroundedUpate(Collision collision, bool value)
  {
    if (collision.gameObject.tag == ("Ground"))
    {
      _isGrounded = value;
    }
  }
}

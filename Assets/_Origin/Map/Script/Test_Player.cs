using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Player : MonoBehaviour
{

    public float speed = 0;
    private void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.forward * speed;
        }
            if (Input.GetKey(KeyCode.S))
        {
            transform.position -= Vector3.forward * speed;
        }
                if (Input.GetKey(KeyCode.A))
        {
            transform.position -= Vector3.right * speed;
        }
                    if (Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * speed;
        }

                    if(Input.GetKeyDown(KeyCode.Space))
        {
            transform.position += Vector3.up * 5;
        }
    }

}

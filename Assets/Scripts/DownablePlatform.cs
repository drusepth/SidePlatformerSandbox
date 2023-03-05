using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownablePlatform : MonoBehaviour
{
    private PlatformEffector2D effector;
    public float waitTime = 0.5f;

    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
    }

    void Update()
    {
        /*
         * Trigger a "through platform" descent when pressing jump while DOWN is held
        float verticalInput = Input.GetAxis("Vertical");
        bool pressingDownInput = verticalInput < -0.9f;

        if (!pressingDownInput)
        {
            waitTime = 0.5f;
            effector.rotationalOffset = 0;
        }

        if (pressingDownInput)
        {
            if (waitTime <= 0)
            {
                effector.rotationalOffset = 180f;
                waitTime = 0.5f;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
        */
    }
}

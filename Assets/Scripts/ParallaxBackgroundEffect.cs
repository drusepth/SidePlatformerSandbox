using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackgroundEffect : MonoBehaviour
{
    private float imageTileLength, currentPosition;
    public GameObject mainCamera;
    public float parallaxStrength;

    void Start()
    {
        currentPosition = transform.position.x;
        imageTileLength = GetComponent<SpriteRenderer>().bounds.size.x;

        Debug.Log(gameObject.name + " startPos is " + currentPosition + " / length = " + imageTileLength);
    }

    void FixedUpdate()
    {
        float tilingBoundPosition = (mainCamera.transform.position.x * (1 - parallaxStrength));
        float parallaxOffset = (mainCamera.transform.position.x * parallaxStrength);

        // Parallax effect
        transform.position = new Vector2(currentPosition + parallaxOffset, transform.position.y);

        // Looping effect
        if (tilingBoundPosition > currentPosition + imageTileLength)
        {
            //Debug.Log("Moving " + gameObject.name + " two units to the right");
            currentPosition += imageTileLength * 2f;
        }
        else if (tilingBoundPosition < currentPosition - imageTileLength)
        {
            //Debug.Log("Moving " + gameObject.name + " two units to the left");
            currentPosition -= imageTileLength * 2f;
        }
    }
}

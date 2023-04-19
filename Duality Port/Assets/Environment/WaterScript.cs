using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterScript : MonoBehaviour
{
    [SerializeField] GameObject layer1;
    [SerializeField] GameObject layer3;

    [SerializeField] Transform Water;

    private int startX = 0;
    private int endX = 2;

    private Vector3 waterPos;
    // Start is called before the first frame update
    void Start()
    {
        waterPos = Water.position;
    }

    // Update is called once per frame
    void Update()
    {   
        layer1.transform.position = calcPos(0, .15f);
        layer3.transform.position = calcPos(1.25f, .1f);
        
    }

    private Vector3 calcPos(float yOffset, float speed) {
        return Vector3.Lerp(new Vector3(startX, yOffset, 0) + waterPos, new Vector3(endX, yOffset) + waterPos, Mathf.PingPong(Time.time * speed, 1.0f));
    }
}

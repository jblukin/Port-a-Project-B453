using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageAnimatorScript : MonoBehaviour
{
    
    [SerializeField, Range(0, 1)] private float delay; //delay between animation frames
    
    [SerializeField] private Image imageToAnimate;

    [SerializeField] private Sprite[] animationFrames;

    private int frameIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(PlayAnimation());
    }

    void OnEnable()
    {

        StartCoroutine(PlayAnimation());

    }

    IEnumerator PlayAnimation() 
    {

        yield return new WaitForSeconds(delay);

        if(frameIndex >= animationFrames.Length)
            frameIndex = 0;

        imageToAnimate.sprite = animationFrames[frameIndex];

        frameIndex++;

        StartCoroutine(PlayAnimation());

    }
}

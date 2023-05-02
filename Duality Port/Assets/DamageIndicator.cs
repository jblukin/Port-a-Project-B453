using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageIndicator : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] TMP_Text tmp;

    public float yVel = 5f;
    public float xVel = 5f;
    public float timeToLive = 1f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tmp = GetComponent<TMP_Text>();

        rb.velocity = new Vector2(Random.Range(-xVel, xVel), yVel);
        Destroy(gameObject, timeToLive);
    }

    public void SetText(string text) {
        tmp.SetText(text);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FauxEnemyScript : MonoBehaviour
{

    [SerializeField] protected float _health = 100.0f;



    // Start is called before the first frame update
    void Start()
    {
        
        if(_health < 100.0f)
            _health = 100.0f;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TakeDamage(AttackData data)
    {

        _health-=data.damagePerHit;

        if(_health <= 0.0f) {

            Instantiate(this.gameObject, new Vector2(Random.Range(-18f, 18f), -3.5f), Quaternion.identity);

            Destroy(this.gameObject);

        }

    }
}

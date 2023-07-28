using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Fire : MonoBehaviour
{
    private Transform _transform;
    
    private Quaternion target;

    private Vector3 dir;
    
    [SerializeField] private float _bullet_speed = 0.01f;
    
    // Start is called before the first frame update

    // Update is called once per frame
    private void Update()
    {
        if ((transform.position.y > 5.5f) || (transform.position.y < -5.5f) || (transform.position.x > 13.5f) || (transform.position.y < -13.5f))
        {
            Destroy(this.gameObject);
        }
        else
        {
            transform.position += -dir * _bullet_speed * Time.deltaTime;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Bot"))
        {
            Destroy(this.gameObject);
        }
    }

    public void ShootDir(Vector3 dir)
    {
        this.dir = dir;
    }
}

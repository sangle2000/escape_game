using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Fire : MonoBehaviour
{
    private Transform _transform;
    
    private Quaternion target;

    private Vector3 dir;

    private Camera mainCamera;

    [SerializeField] private float _bullet_speed = 0.01f;

    // Start is called before the first frame update
    private void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);

        if (viewportPosition.x < 0f || viewportPosition.x > 1f || viewportPosition.y < 0f || viewportPosition.y > 1f)
        {
            Destroy(this.gameObject);
        } else
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

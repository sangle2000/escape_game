using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using Color = System.Drawing.Color;
using Random = UnityEngine.Random;

public class BotMovement : MonoBehaviour
{
    [SerializeField] private int health = 5;
    [SerializeField] private float speed = 0.001f;

    private Transform _transform;
    private GameObject _player;

    private Animator _anim;

    private SpriteRenderer _spr;

    private int score;
    
    private float _pos_x;
    private float _pos_y;

    private bool EnemyKilled = false;

    private Vector3 _move_dir = new Vector3(); 

    // Start is called before the first frame update
    void Start()
    {
        _transform = GetComponent<Transform>();
        _spr = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _player = GameObject.Find("Player");

        _pos_x = _transform.position.x;
        _pos_y = _transform.position.y;

        score = 1;
        
        _transform.position = new Vector2(_pos_x, _pos_y);
    }

    // Update is called once per frame
    void Update()
    {
        _transform.position += _move_dir*speed;
        try
        {
            UpdateMovement();
        }
        catch
        {
            
        }
    }

    private void UpdateMovement()
    {
        if (_player.transform.position.x - _transform.position.x < -0.1)
        {
            _spr.flipX = true;
        } else if (_player.transform.position.x - _transform.position.x > 0.1)
        {
            _spr.flipX = false;
        }

        _move_dir = (_player.transform.position - _transform.position).normalized;
    }
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Bullet"))
        {
            health -= 1;
        }

        if (health == 0)
        {
            EnemyKilled = true;

            _player.GetComponent<PlayerMovement>().GetScore(EnemyKilled);
            
            Destroy(this.gameObject);
        }
        else
        {
            EnemyKilled = false;
        }
    }
    
    
}
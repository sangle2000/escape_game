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

    private Camera mainCamera;

    private Rigidbody2D _rb;

    private Transform _transform;
    private GameObject _player;

    private Animator _anim;

    private SpriteRenderer _spr;

    private float _pos_x;
    private float _pos_y;

    private bool EnemyKilled = false;
    private bool _stop_enemy = false;
    private bool DropCoin;

    private Vector3 _move_dir = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
        _transform = GetComponent<Transform>();
        _spr = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();

        mainCamera = Camera.main;

        _player = GameObject.Find("Player");

        _pos_x = _transform.position.x;
        _pos_y = _transform.position.y;

        _transform.position = new Vector2(_pos_x, _pos_y);

        System.Random random = new System.Random();

        int coin = random.Next(0, 10);

        DropCoin = coin > 3 ? true : false;
    }

    // Update is called once per frame
    void Update()
    {
        _transform.position += _move_dir * speed;

        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);

        /*if (viewportPosition.x < 0f || viewportPosition.x > 1f || viewportPosition.y < 0f || viewportPosition.y > 1f)
        {
            if (transform.childCount == 0)
            {
                GameObject prefabObject = (GameObject)LoadPrefabFromFile("Square");

                GameObject pNewObject =
                    (GameObject)Instantiate(prefabObject, _move_dir, Quaternion.identity);

                pNewObject.transform.SetParent(transform);
            } else
            {
                transform.GetChild(0).transform.position = _move_dir;
            }
        } else
        {
            Destroy(transform.FindChild("Square (Clone)"));
        }*/

        try
        {
            UpdateMovement();
        }
        catch
        {

        }
    }
/*    private UnityEngine.Object LoadPrefabFromFile(string filename)
    {
#if UNITY_EDITOR
        var loadedObject = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/" + filename + ".prefab");
        if (loadedObject == null)
        {
            throw new FileNotFoundException("...no file found - please check the configuration");
        }
        return loadedObject;
#endif
    }*/

    private void UpdateMovement()
    {
        if (_player.transform.position.x - _transform.position.x < -0.1)
        {
            _spr.flipX = true;
        }
        else if (_player.transform.position.x - _transform.position.x > 0.1)
        {
            _spr.flipX = false;
        }

        _move_dir = (_player.transform.position - _transform.position).normalized;
        _transform.rotation = Quaternion.EulerRotation(0f, 0f, 0f);
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

            if (DropCoin)
            {
                //GameObject prefabObject = (GameObject)LoadPrefabFromFile("Coin");

                GameObject prefabObject = Resources.Load<GameObject>("Coin");

                GameObject pNewObject =
                    (GameObject)Instantiate(prefabObject, transform.position, Quaternion.identity);

                pNewObject.name = "Coin";
                pNewObject.transform.SetParent(GameObject.Find("Item").transform);
            }

            Destroy(this.gameObject);
        }
        else
        {
            EnemyKilled = false;
        }
    }

    public void EndGame()
    {
        speed = 0;
    }
}
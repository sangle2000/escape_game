using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private Transform _transform;
    private Animator _animator;
    private Rigidbody2D _rb;

    private SpriteRenderer _spr;
    public GameObject _backgroundGO;
    private Renderer _renderer;

    public FixedJoystick Joystick;

    private Vector2 move;

    public int score;
    private GameObject _bot;

    [SerializeField] private GameObject _bullet;
    [SerializeField] private HealthHandle _healthHandle;

    [SerializeField] private float _speed = 0.01f;

    [SerializeField] private int _health = 10;
    [SerializeField] private int _current_health = 10;
    [SerializeField] private float _scope = 10f;

    [SerializeField] private Text _coint_text;
    [SerializeField] private float _speed_shot = 3;

    private float pos_x;
    private float pos_y;

    private bool _spawn_enemy = true;
    private bool _fire_ready = true;

    private int money = 0;
    private float distance = 100f;

    //private Vector3 _mousePosition;

    // Start is called before the first frame update
    void Start()
    {
        _transform = GetComponent<Transform>();
        _spr = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _renderer = _backgroundGO.GetComponent<Renderer>();

        /*pos_x = _transform.position.x;
        pos_y = _transform.position.y;*/

        score = 0;

        _animator.SetBool("running", false);
        _healthHandle.SetMaxHealth(_health);
    }

    // Update is called once per frame
    void Update()
    {
        move.x = Joystick.Horizontal;
        move.y = Joystick.Vertical;

        /*_rb.velocity = new Vector2(pos_x, pos_y);
        Movement();*/
        CheckShoot();
    }

    private void FixedUpdate()
    {
        if (move.x < 0)
        {
            _spr.flipX = true;

            _animator.SetBool("running", true);
        }
        else if (move.x > 0)
        {
            _spr.flipX = false;

            _animator.SetBool("running", true);
        }
        else
        {
            _animator.SetBool("running", false);
        }

        _rb.MovePosition(_rb.position + move * _speed * Time.fixedDeltaTime);
    }

    /*private void Movement()
    {

        if (Input.GetKey("a"))
        {
            pos_x = -_speed;
            _spr.flipX = true;

            _animator.SetBool("running", true);
        }
        else if (Input.GetKey("d"))
        {

            pos_x = _speed;
            _spr.flipX = false;

            _animator.SetBool("running", true);
        } else
        {
            pos_x = 0;
        }

        if (Input.GetKey("w"))
        {
            pos_y = _speed;

            _animator.SetBool("running", true);
        }
        else if (Input.GetKey("s"))
        {
            pos_y = -_speed;

            _animator.SetBool("running", true);
        } else
        {
            pos_y = 0;
        }

        if (!Input.GetKey("a") && !Input.GetKey("d") && !Input.GetKey("w") && !Input.GetKey("s"))
        {
            _animator.SetBool("running", false);
        }

        if (_speed == 0)
        {
            _animator.SetBool("running", true);
        }

    }*/

    private void CheckShoot()
    {
        bool count_bot = false;

        foreach (var gameObj in FindObjectsOfType(typeof(GameObject)) as GameObject[])
        {
            if (gameObj.name == "Bot")
            {
                count_bot = true;
            }
        }

        if (!count_bot && GameObject.Find("SpawnPoint") == null)
        {
            WarnToSpawn();
        }
    }

    private void FireCountDown()
    {
        _fire_ready = true;
    }

    public void Shoot()
    {
        if (_fire_ready)
        {
            GameObject go = null;
            bool hasEnemy = false;
            distance = 100f;

            foreach (var gameObj in FindObjectsOfType(typeof(GameObject)) as GameObject[])
            {
                if (gameObj.name == "Bot")
                {
                    hasEnemy = true;
                    if (Vector3.Distance(_transform.position, gameObj.transform.position) < distance)
                    {
                        go = gameObj;
                        distance = Vector3.Distance(_transform.position, gameObj.transform.position);
                    }
                }
            }

            _bot = go;

            if (hasEnemy && _bot != null)
            {
                GameObject pNewObject = Instantiate(_bullet, _transform.position, Quaternion.identity);

                Vector3 shootDir = (_transform.position - _bot.transform.position).normalized;

                pNewObject.GetComponent<Fire>().ShootDir(shootDir);
            }
            _fire_ready = false;
            Invoke("FireCountDown", _speed_shot);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Bot"))
        {
            if (_current_health > 1)
            {
                _current_health -= 1;
                _healthHandle.SetHealth(_current_health);
                StartCoroutine(Hit());
            }
            else
            {
                SceneManager.LoadScene(3);
            }
        }

        if (collider.CompareTag("Coin"))
        {
            money += 1;
            CoinNumber(money);
        }
    }

    private void CoinNumber(int coin)
    {
        _coint_text.text = coin.ToString();
    }

    IEnumerator Hit()
    {
        yield return new WaitForSeconds(0.1f);
        _spr.color = new UnityEngine.Color(1, 1, 1, .1f);
        StartCoroutine(HitDelay());
    }

    IEnumerator HitDelay()
    {
        yield return new WaitForSeconds(0.1f);
        _spr.color = new UnityEngine.Color(1, 1, 1, 1);
    }

    public void GetScore(bool KillEnemy)
    {
        if (KillEnemy)
        {
            score += 1;
        }
    }

    private void WarnToSpawn()
    {
        for (int i = 0; i < System.Int32.Parse((score / 2).ToString()) + 1; i++)
        {

            // Load a prefab from the "Resources" folder
            GameObject _spawn_point = Resources.Load<GameObject>("SpawnPoint");

            Vector3[] corners = new Vector3[2];

            corners[0] = _renderer.bounds.min;
            corners[1] = _renderer.bounds.max;

            float random_x = UnityEngine.Random.Range((corners[0].x) + 0.5f, (corners[1].x) - 0.5f);

            float random_y = UnityEngine.Random.Range((corners[0].y) + 0.5f, (corners[1].y) - 0.5f);

            GameObject pNewObject =
                (GameObject)Instantiate(_spawn_point, new Vector3(random_x, random_y, 0), Quaternion.identity);

            pNewObject.name = "SpawnPoint";

            pNewObject.GetComponent<SpawnEnemy>().Setup(new Vector3(random_x, random_y, 0));
        }
    }
}
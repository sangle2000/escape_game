using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private Transform _transform;
    private Animator _animator;

    private SpriteRenderer _spr;

    public int score;

    [SerializeField] private GameObject _bullet;
    [SerializeField] private float _speed = 0.01f;
    [SerializeField] private int health = 5;

    private float pos_x;
    private float pos_y;

    // Start is called before the first frame update
    void Start()
    {
        _transform = GetComponent<Transform>();
        _spr = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        pos_x = _transform.position.x;
        pos_y = _transform.position.y;
        score = 0;

        _animator.SetBool("running", false);
    }

    // Update is called once per frame
    void Update()
    {
        _transform.position = new Vector2(pos_x, pos_y);
        Movement();
        Shoot();
    }

    private void Movement()
    {
        if (Input.GetKey("a"))
        {
            if (!(pos_x <= -13.5f))
            {
                pos_x -= _speed;
                _spr.flipX = true;
            }

            _animator.SetBool("running", true);
        }
        else if (Input.GetKey("d"))
        {
            if (!(pos_x >= 13.5f))
            {
                pos_x += _speed;
                _spr.flipX = false;
            }

            _animator.SetBool("running", true);
        }

        if (Input.GetKey("w"))
        {
            if (!(pos_y >= 4.4f))
            {
                pos_y += _speed;
            }

            _animator.SetBool("running", true);
        }
        else if (Input.GetKey("s"))
        {
            if (!(pos_y <= -4.3f))
            {
                pos_y -= _speed;
            }

            _animator.SetBool("running", true);
        }

        if (!Input.GetKey("a") && !Input.GetKey("d") && !Input.GetKey("w") && !Input.GetKey("s"))
        {
            _animator.SetBool("running", false);
        }
    }

    private void Shoot()
    {
        GameObject go = null;
        float distance = 20f;
        foreach (var gameObj in FindObjectsOfType(typeof(GameObject)) as GameObject[])
        {
            if (gameObj.name == "Bot")
            {
                if (Vector3.Distance(_transform.position, gameObj.transform.position) < distance)
                {
                    go = gameObj;
                    distance = Vector3.Distance(_transform.position, gameObj.transform.position);
                }
            }
        }

        GameObject _bot = go;

        if (_bot != null)
        {
            if (Input.GetKeyDown("space"))
            {
                GameObject pNewObject = Instantiate(_bullet, _transform.position, Quaternion.identity);

                Vector3 shootDir = (_transform.position - _bot.transform.position).normalized;

                pNewObject.GetComponent<Fire>().ShootDir(shootDir);
            }
        }
        else if (GameObject.Find("SpawnPoint") == null)
        {
            WarnToSpawn();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Bot"))
        {
            if (health > 1)
            {
                health -= 1;
                StartCoroutine(Hit());
            }
            else
            {
                Destroy(this.gameObject);
                RestartLevel();
            }
        }
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
            GameObject _spawn_point = (GameObject)LoadPrefabFromFile("SpawnPoint");

            float random_x = Random.Range(-13f, 13f);

            float random_y = Random.Range(-4.5f, 4.5f);

            GameObject pNewObject =
                (GameObject)Instantiate(_spawn_point, new Vector3(random_x, random_y, 0), Quaternion.identity);

            pNewObject.name = "SpawnPoint";

            pNewObject.GetComponent<SpawnEnemy>().Setup(new Vector3(random_x, random_y, 0));
        }
    }

    private UnityEngine.Object LoadPrefabFromFile(string filename)
    {
        var loadedObject = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/" + filename + ".prefab");
        if (loadedObject == null)
        {
            throw new FileNotFoundException("...no file found - please check the configuration");
        }

        return loadedObject;
    }

    void RestartLevel() //Restarts the level
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
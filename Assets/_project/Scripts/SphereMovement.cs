using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SphereMovement : MonoBehaviour
{
    public float swipeSensibility = 7f;
    public float speedBounceInSeconds = 0.3f;
    public float posZTarget;
    public float posXmax = 3f; //-2.5f;

    public bool _gameStarted;
    public bool _isGameOver;

    public int _spawnPlatformCount;
    public int _numberOfPlatformToSpawnedAtStart;
    public int _point;
    public int _collectivePoint;

    public GameObject _camGame;
    public GameObject _platformPrefab;
    public GameObject _tuto;
    public GameObject _RetryPanel;
    public GameObject _RetryBTN;
    public GameObject _collectiveTaken;

    public Rigidbody _rigidbody;

    public Text _pointCountTxt;

    private void Start()
    {
        for (int i = 0; i < _numberOfPlatformToSpawnedAtStart; i++)
        {
            SpawnPlatform();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Collective")
        {
            _collectivePoint += 1;
            Instantiate(_collectiveTaken, other.transform.position, Quaternion.identity);
            other.gameObject.SetActive(false);
        }
    }

    void CheckIfPlayerIsGrounded()
    {
        if (_isGameOver)
            return;

        if (posZTarget > 4)
        {
            RaycastHit hit;

            Vector3 down = transform.TransformDirection(Vector3.down);

            if (Physics.Raycast(transform.position, down, out hit))
            {
                Platform platform = hit.transform.GetComponent<Platform>();

                if (platform != null)
                {
                    platform.OnPlayerBounce();
                    if (Vector3.Distance(platform.transform.position, transform.position) < 0.6f)
                    {
                        GameManager.Instance.CallPerfecttMsg();
                        GameManager.Instance._comboCount += 1;
                    }
                    else
                    {
                        GameManager.Instance.CallGreatMsg();
                    }
                    Add1Point();
                }
            }
            else
            {
                GameOver();
            }
        }
    }

    void Add1Point()
    {
        if (_isGameOver)
            return;

        _point += 1;
        _pointCountTxt.text = _point.ToString();
    }

    public void GameOver()
    {
        Debug.LogError("GameOver");
        _isGameOver = true;
        GameManager.Instance.GameOver();
        _RetryPanel.SetActive(true);
        GameManager.Instance._comboMsg.gameObject.SetActive(false);
        _RetryBTN.transform.DOPunchScale(new Vector3(1, 1, 1), 0.5f);
        Rigidbody rig = gameObject.AddComponent<Rigidbody>();
        rig.AddForce(transform.forward * 300);
    }



    void PlayerMove()
    {
        CheckIfPlayerIsGrounded();

        if (_isGameOver)
            return;

        speedBounceInSeconds -= 0.0002f;

        if (speedBounceInSeconds < 0.15f)
            speedBounceInSeconds = 0.15f;
        Ease playerAnimEase = Ease.OutCubic; //SetEase(Ease.OutQuad);

        transform.DOMoveY(2.5f, speedBounceInSeconds).SetLoops(2, LoopType.Yoyo).SetEase(playerAnimEase);

        transform.position = new Vector3(transform.position.x, transform.position.y, posZTarget);

        posZTarget += 5f;

			transform.DOMoveZ(posZTarget, speedBounceInSeconds * 2f)
				.SetEase(Ease.Linear)
				.OnUpdate(() => {
					OnUpdate();
					UpdateCamPosZ();
				})
				.OnComplete(() => {
					PlayerMove();
				});
    }

    void UpdateCamPosZ()
    {
        var pCam = _camGame.transform.position;
        pCam.z = transform.position.z - 8.63f;
        _camGame.transform.position = pCam;

        pCam = _camGame.transform.position;
        pCam.z = transform.position.z - 8.63f;
        _camGame.transform.position = pCam;
    }

    void OnUpdate()
    {
        if (_isGameOver)
            return;

        if (Input.GetMouseButton(0))
        {
            if (!_gameStarted)
                _gameStarted = true;

            var playerPos = transform.position;

            playerPos.x = Mathf.Lerp(playerPos.x, GetPositionTouchX(), 10f * Time.deltaTime);

            transform.position = playerPos;
        }
    }

    float GetPositionTouchX()
    {

        float x = 0;

        if (Application.isMobilePlatform)
        {
            if (Input.touchCount > 0)
            {

                Touch touch = Input.GetTouch(0);

                x = touch.position.x / Screen.width - 0.5f;
            }
        }
        else
        {
            x = Input.mousePosition.x / Screen.width - 0.5f;
        }

        if (x < -0.5f)
            x = -0.5f;

        if (x > 0.5f)
            x = 0.5f;

        return swipeSensibility * x;
    }

    public void SpawnPlatform()
    {
        if (_isGameOver)
            return;

        GameObject go = Instantiate(_platformPrefab) as GameObject;

        Transform t = go.transform;

        SpawnPlatform(t);
    }

    public void SpawnPlatform(Transform t)
    {
        if (_isGameOver)
            return;

        _spawnPlatformCount++;

        float posX = Random.Range(-posXmax, posXmax);

        if (Random.Range(0, 100f) < 70f)
        {
            if (Random.Range(0, 100f) < 50f)
            {
                posX = Random.Range(-posXmax, -1f);
            }
            else
            {
                posX = Random.Range(1f, posXmax);
            }
        }
        t.transform.position = new Vector3(posX, -2f, _spawnPlatformCount * 5f);
        t.transform.DOMoveY(0.3f, 1f);
    }

    private void Update()
    {
        if (!_gameStarted)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _gameStarted = true;
                GameManager.Instance.StartGame();
                _tuto.SetActive(false);
                PlayerMove();
            }
        }

    }
}

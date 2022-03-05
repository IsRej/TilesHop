using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Boxophobic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    public PolyverseSkies _valuskye;
    public bool DayToNight;
    public bool _gameStarted;
    public int _comboCount;
    public int _skyNo;
    public float _timer;
    public Material[] _allSkyboxes;
    public AudioSource _music;
    public Text _comboMsg;

    private void Awake()
    {
        Instance = this;
    }

    public void StartGame()
    {
        _music.Play();
        _gameStarted = true;
    }

    public void GameOver()
    {
        _music.Stop();
    }

public void CallGreatMsg()
    {
        _comboMsg.transform.DOScale(new Vector3(0, 0, 0), 0.01f);
        _comboMsg.transform.DOScale(new Vector3(1, 1, 1), 0.25f);
        Color color1;
        ColorUtility.TryParseHtmlString("#55FF55", out color1);
        _comboMsg.text = "Great";
        _comboMsg.color = color1;
        Color color2;
        ColorUtility.TryParseHtmlString("#00FFC5", out color2);
        _comboMsg.gameObject.GetComponent<Outline>().effectColor = color2;
        _comboCount = 0;
    }

    public void CallPerfecttMsg()
    {
        if (_comboCount > 0)
        {
            _comboMsg.transform.DOScale(new Vector3(0, 0, 0), 0.01f);
            _comboMsg.transform.DOScale(new Vector3(1, 1, 1), 0.25f);
            Color color1;
            ColorUtility.TryParseHtmlString("#FF55BF", out color1);
            _comboMsg.text = "Perfect " + _comboCount + "X";
            _comboMsg.color = color1;
            Color color2;
            ColorUtility.TryParseHtmlString("#FF0066", out color2);
            _comboMsg.gameObject.GetComponent<Outline>().effectColor = color2;
        }
        else
        {
            _comboMsg.transform.DOScale(new Vector3(0, 0, 0), 0.01f);
            _comboMsg.transform.DOScale(new Vector3(1, 1, 1), 0.25f);
            Color color1;
            ColorUtility.TryParseHtmlString("#FF55BF", out color1);
            _comboMsg.text = "Perfect";
            _comboMsg.color = color1;
            Color color2;
            ColorUtility.TryParseHtmlString("#FF0066", out color2);
            _comboMsg.gameObject.GetComponent<Outline>().effectColor = color2;
        }
    }

    public void CloseMsg()
    {
        _comboMsg.transform.DOScale(new Vector3(0, 0, 0), 0.25f);
    }

    public void Retry()
    {
        SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameStarted)
        {
            _timer += Time.deltaTime;
            if (_timer > 15f)
            {
                _music.pitch = 1.02f;
            }
            else if (_timer > 15f)
            {
                _music.pitch = 1.04f;
            }
            else if (_timer > 30f)
            {
                _music.pitch = 1.06f;
            }
            else if (_timer > 45f)
            {
                _music.pitch = 1.08f;
            }
            else if (_timer > 60f)
            {
                _music.pitch = 1.1f;
            }
            else if (_timer > 75f)
            {
                _music.pitch = 1.12f;
            }
            else if (_timer > 90f)
            {
                _music.pitch = 1.14f;
            }
            else if (_timer > 105f)
            {
                _music.pitch = 1.16f;
            }
            else if (_timer > 130f)
            {
                _music.pitch = 1.18f;
            }
            else if (_timer > 145f)
            {
                _music.pitch = 1.2f;
            }
            if (_valuskye.timeOfDay >= 1)
            {
                DayToNight = false;
                if (_skyNo <= 9)
                {
                    _skyNo += 1;
                }
                else
                {
                    _skyNo = 0;
                }
                _valuskye.skyboxDay = _allSkyboxes[_skyNo];
            }
            else if (_valuskye.timeOfDay <= 0)
            {
                DayToNight = true;
                if (_skyNo <= 9)
                {
                    _skyNo += 1;
                }
                else
                {
                    _skyNo = 0;
                }
                _valuskye.skyboxNight = _allSkyboxes[_skyNo];
            }
            if (DayToNight)
            {
                _valuskye.timeOfDay += 0.05f * Time.deltaTime;
            }
            else
            {
                _valuskye.timeOfDay -= 0.05f * Time.deltaTime;
            }
        }
    }
}

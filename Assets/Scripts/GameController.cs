using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	[SerializeField] public int remainingMirrors;
    public MeshRenderer ground;
    public Material[] groundMats;
    public GameObject backGround;
    public ParticleSystem[] pss;
    public GameObject[] objs;
    public Animator worldAnimator;
    public Text welldoneText;
    public GameObject[] stars;
    public GameObject buttons;
    public int starsGetThisLevel;
    public float twoStarLowerBound;
    public float threeStarLowerBound;
    public float timer;
    public bool stopTimer;
    public bool isGameStarted;
    public GameObject worldLights;
    private static GameController _instance;

    public static GameController Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    private void Update()
    {
        if (worldAnimator != null)
        {
            worldAnimator.SetBool("endLevel", false);
            if (worldAnimator.GetCurrentAnimatorStateInfo(0).IsName("lastState"))
            {
                Destroy(worldAnimator.gameObject);
                foreach(ParticleSystem ps in pss)
                {
                    ps.Stop();
                }
            }
        }
        if (!stopTimer)
        {
            timer += Time.deltaTime;
        }
        if (stopTimer)
        {
            if (Input.GetMouseButtonDown(0))
            {
                StopAllCoroutines();
                ImmediateLoad();
            }
        }
    }
    public void EndLevel()
    {
        PlayerPrefs.SetInt("levelIndex", SceneManager.GetActiveScene().buildIndex + 1);
        stopTimer = true;
        StartCoroutine(EndLevelRoutine());
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    private IEnumerator EndLevelRoutine()
    {
        //Color waterTargetColor = groundMats[0].color;
        //Color groundTargetColor = groundMats[1].color;
        ground.materials = groundMats;
        worldLights.SetActive(true);
        //while (Mathf.Abs(ground.materials[0].color.r - waterTargetColor.r) > .02f || (Mathf.Abs(ground.materials[1].color.r - groundTargetColor.r) > .02f))
        //{
        //    yield return new WaitForSeconds(Time.fixedDeltaTime);
        //    ground.materials[0].color = Color.Lerp(ground.materials[0].color, waterTargetColor, 0.15f);
        //    ground.materials[1].color = Color.Lerp(ground.materials[1].color, groundTargetColor, 0.15f);
        //}

        yield return new WaitForSeconds(.5f);
        foreach (ParticleSystem ps in pss)
        {
            ps.gameObject.SetActive(true);
            ps.Play();
            yield return new WaitForSeconds(.1f);
        }
        worldAnimator.SetBool("endLevel", true);
        StartCoroutine(BackgroundCoroutine());
        yield return new WaitForSeconds(.2f);
        foreach (GameObject obj in objs)
        {
            obj.SetActive(false);
        }
    }

    public void LoadStartScene()
    {
        SceneManager.LoadScene(0);
    }

    private IEnumerator BackgroundCoroutine()
    {
        yield return new WaitForSeconds(2f);
        backGround.SetActive(true);
        while (backGround.transform.localScale.x < 1440)
        {
            yield return new WaitForSeconds(Time.fixedDeltaTime);
            backGround.transform.localScale *= 1.5f;
        }
        yield return new WaitForSeconds(.5f);
        welldoneText.gameObject.SetActive(true);
        welldoneText.GetComponent<Animator>().SetBool("levelEnd", true);
        yield return new WaitForSeconds(.5f);
        if (timer <= threeStarLowerBound)
        {
            starsGetThisLevel = 3;
        }
        else if(timer <= twoStarLowerBound)
        {
            starsGetThisLevel = 2;
        }
        else 
        {
            starsGetThisLevel = 1; 
        }
        for (int i = 0; i < starsGetThisLevel; i++)
        {
            stars[i].SetActive(true);
        }
        yield return new WaitForSeconds(.5f);
        buttons.SetActive(true);
    }
   
    public void ImmediateLoad()
    {
        if(worldAnimator != null)
        {
            Destroy(worldAnimator.gameObject);
        }
        foreach (GameObject obj in objs)
        {
            obj.SetActive(false);
        }
        backGround.SetActive(true);
        backGround.transform.localScale = new Vector3(1440, 960, 1);
        welldoneText.gameObject.SetActive(true);
        if (timer <= threeStarLowerBound)
        {
            starsGetThisLevel = 3;
        }
        else if (timer <= twoStarLowerBound)
        {
            starsGetThisLevel = 2;
        }
        else
        {
            starsGetThisLevel = 1;
        }
        for (int i = 0; i < starsGetThisLevel; i++)
        {
            stars[i].SetActive(true);
        }
        buttons.SetActive(true);
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}


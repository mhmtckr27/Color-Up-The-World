using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
	private void Update()
	{
	}

	private void OnMouseDown()
	{
		int levelIndex = PlayerPrefs.GetInt("levelIndex", 1);
		SceneManager.LoadScene(1);
	}
}

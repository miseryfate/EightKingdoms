using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour {
    public float timer;
    public float baseTimer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //каждый фрейм уменьшаем таймер. Когда таймер = 0 - выключаем объект
        if (timer > 0f) {
            timer -= Time.deltaTime;
        }
        else {
            gameObject.SetActive(false);
        }

        //Если к объекту прикреплен класс Text, то постепенно делаем его прозрачным
        if (GetComponent<Text>() != null) {
            GetComponent<Text>().color = new Color(GetComponent<Text>().color.r, GetComponent<Text>().color.b, GetComponent<Text>().color.g, timer / baseTimer);
        }
	}

    public void Activate (float _timer) {
        //Устанавливаем текущий и исходный таймер и включаем объект
        gameObject.SetActive(true);
        baseTimer = _timer;
        timer = _timer;
    }
}

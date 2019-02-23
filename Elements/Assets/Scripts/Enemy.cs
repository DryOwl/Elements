using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    private enum PlayerStatus { Normal, FireBarrier };
    public float enemy_Hp = 20;
    public string enemy_Status = "";
	// Use this for initialization
	void Start () {	
	}
	
	// Update is called once per frame
	void Update () {
        if(enemy_Hp <= 0)
        {
            Destroy(gameObject);
        }
	}
}

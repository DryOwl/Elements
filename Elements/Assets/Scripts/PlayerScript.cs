using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {
    private enum PlayerStatus { Normal, FireBarrier, BubbleBarrier, Haste };
    public int player_status = (int)PlayerStatus.Normal;
    public float player_Damage = 0;
    public bool barrier_Dealing = false;

    GameManager GM;
    // Use this for initialization
    void Start () {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (player_status == (int)PlayerStatus.FireBarrier)
            {
                if (barrier_Dealing)
                {
                    other.gameObject.GetComponent<Enemy>().enemy_Hp -= player_Damage;
                    barrier_Dealing = false;
                    StartCoroutine(BarrierDealing());
                }
            }
        }
    }
    IEnumerator BarrierDealing()
    {
        yield return new WaitForSeconds(1.0f);
        barrier_Dealing = true;
    }
    public void SetValues()
    {
        player_status = GM.player_status;
        player_Damage = GM.player_Damage;
        barrier_Dealing = GM.barrier_Dealing;
    }
}

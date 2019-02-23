using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour {
    public float damage;
    public int chargingLevel;
    float size;
    // Use this for initialization
    void Start () {
        size = transform.localScale.x * (chargingLevel + 1)*2;
        transform.localScale = new Vector3(0.001f, 1f, 0.001f);
		
	}
	
	// Update is called once per frame
	void Update () {
        Enlarge();
		
	}
    void Enlarge()
    {
        if(size >= transform.localScale.x)
        {
            transform.localScale += new Vector3(5, 5, 5) * Time.deltaTime*(chargingLevel+1);
        }
        else
        {
            StartCoroutine(DestroyWithDelay());
        }
    }
    IEnumerator DestroyWithDelay()
    {
        yield return new WaitForSeconds(0.05f);
        for(int i = 0; i < GameObject.FindGameObjectsWithTag("Enemy").Length; i++)
        {
            if((GameObject.FindGameObjectsWithTag("Enemy")[i].transform.position - transform.position).magnitude < size)
            {
                GameObject.FindGameObjectsWithTag("Enemy")[i].GetComponent<Enemy>().enemy_Hp -= damage;
            }
        }
        Destroy(gameObject);
    }    
}

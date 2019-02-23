using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {
    public string attackName;
    public float damage = 0;
    public GameObject Explosion;
    public int chargingLevel;
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            if(attackName == "FireBall")
            {
                GameObject explosion = (GameObject)Instantiate(Explosion);
                explosion.transform.position = transform.position;
                explosion.GetComponent<ExplosionScript>().damage = damage;
                explosion.GetComponent<ExplosionScript>().chargingLevel = chargingLevel;
                Destroy(gameObject);
            }
            else if(attackName == "WaterShower")
            {
                other.gameObject.GetComponent<Enemy>().enemy_Hp -= damage;
                other.gameObject.GetComponent<Enemy>().enemy_Status = "Wet";
                Destroy(gameObject);
            }
            else
            {
                other.gameObject.GetComponent<Enemy>().enemy_Hp -= damage;
                Destroy(gameObject);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour {
    public int element;
    public float manaConsumption;
    public float coolTime;
    public float damage;
    public float duration;
    public string magicName;
    public float moveSpeed;
    
    enum Gems { Fire, Aqua, Wind, Earth, Shining, Dark, Ice, Eletric }

    public GameObject FireBall;
    public GameObject WaterShower;
    public GameObject WindCutter;
    // Use this for initialization
    void Start () {
        element = 7;
	}
	
	// Update is called once per frame
	public void SetMagicStat () {
        switch (magicName)
        {
            case "FireBall":
                element = (int)Gems.Fire;
                manaConsumption = 3;
                coolTime = 1;
                damage = 5;
                moveSpeed = 500;
                break;
            case "FireBarrier":
                element = (int)Gems.Fire;
                manaConsumption = 10;
                coolTime = 5;
                break;
            case "WaterShower":
                element = (int)Gems.Aqua;
                manaConsumption = 1;
                coolTime = 0.5f;
                damage = 1;
                moveSpeed = 500;
                break;
            case "BubbleBarrier":
                element = (int)Gems.Aqua;
                manaConsumption = 10;
                coolTime = 5;
                break;
            case "WindCutter":
                element = (int)Gems.Wind;
                manaConsumption = 1;
                coolTime = 0.33f;
                damage = 1;
                moveSpeed = 1000;
                break;
            case "Haste":
                element = (int)Gems.Wind;
                manaConsumption = 20;
                coolTime = 5;
                break;
            case "Burst":
                element = (int)Gems.Earth;
                manaConsumption = 5;
                coolTime = 1f;
                damage = 8;
                moveSpeed = 500;
                break;
            /*case "":
                break;
            case "":
                break;
            case "":
                break;
            case "":
                break;
            case "":
                break;*/
            default:
                element = 7;
                break;
        }
	}
}

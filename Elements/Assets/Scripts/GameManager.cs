using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    //Player 구성요소
    public GameObject PlayerObject;
    public Material[] PlayerMaterials;
    Renderer PlayerRenderer;
    private enum PlayerStatus {Normal, FireBarrier, BubbleBarrier, Haste };
    private CharacterController playerController;
    private Vector3 player_MoveDirection = Vector3.zero;
    private Vector3 player_FaceDirection = Vector3.up.normalized;
    private float player_MoveSpeed = 10.0f;
    //private float player_Hp = 50;
    private float player_Mp = 50;
    //private int player_GemSlot = 1;
    public int player_status = (int)PlayerStatus.Normal;
    public float player_Damage = 0;
    public bool barrier_Dealing = false;
    

    //마법
    private KeyCode[] MagicKey = new KeyCode[] {KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F };
    
    private float chargingTime = 0;
    private int chargingLevel = 0;
    private int hasteStack = 0;

    //젬
    public class Gem
    {
        public int element;
        public int level = 0;
        public float coolTime = 0;
        public string magic_1;
        public string magic_2;
        public Gem(int element, string magic_1, string magic_2)
        {
            this.element = element;
            this.magic_1 = magic_1;
            this.magic_2 = magic_2;
        }
    }
    public Gem Fire = new Gem(0, "FireBall", "FireBarrier");
    public Gem Aqua = new Gem(1, "WaterShower", "BubbleBarrier");
    public Gem Wind = new Gem(2, "WindCutter", "Haste");
    public Gem Earth = new Gem(3, "", "");
    public Gem Shining = new Gem(4, "", "");
    public Gem Dark = new Gem(5, "", "");
    public Gem Ice = new Gem(6, "", "");
    public Gem Yellow = new Gem(7, "", "");
    private Gem[] registeredGem = new Gem[4];
    

    //화면상태
    private bool map = false;

    // Use this for initialization
    void Start() {
        map = true;
        PlayerObject = (GameObject)Instantiate(PlayerObject);
        PlayerObject.transform.position = Vector3.zero;
        playerController = PlayerObject.AddComponent<CharacterController>() as CharacterController;
        PlayerRenderer = PlayerObject.GetComponent<Renderer>();
        PlayerRenderer.enabled = true;
        registeredGem = new Gem[] {Fire, Aqua, Wind, Earth};
        Fire.level = 3;
        Aqua.level = 3;
        Wind.level = 3;
	}
	
	// Update is called once per frame
	void Update () {
        //맵상에서 player의 움직임
        if (map)
        {
            if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.RightArrow))
            {
                player_MoveDirection = new Vector3(1, 1, 0).normalized;
                player_FaceDirection = new Vector3(0, 0, -45);
            }else if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftArrow))
            {
                player_MoveDirection = new Vector3(-1, 1, 0).normalized;
                player_FaceDirection = new Vector3(0, 0, 45);
            }else if(Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.RightArrow))
            {
                player_MoveDirection = new Vector3(1, -1, 0).normalized;
                player_FaceDirection = new Vector3(0, 0, -135);
            }else if(Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.LeftArrow))
            {
                player_MoveDirection = new Vector3(-1, -1, 0).normalized;
                player_FaceDirection = new Vector3(0, 0, 135);
            }else if (Input.GetKey(KeyCode.UpArrow))
            {
                player_MoveDirection = Vector3.up;
                player_FaceDirection = Vector3.zero;
            }else if (Input.GetKey(KeyCode.RightArrow))
            {
                player_MoveDirection = Vector3.right;
                player_FaceDirection = new Vector3(0, 0, - 90);
            }else if (Input.GetKey(KeyCode.DownArrow))
            {
                player_MoveDirection = Vector3.down;
                player_FaceDirection = new Vector3(0, 0, 180);
            }else if (Input.GetKey(KeyCode.LeftArrow))
            {
                player_MoveDirection = Vector3.left;
                player_FaceDirection = new Vector3(0, 0, 90);
            }
            else
            {
                player_MoveDirection = Vector3.zero;
            }
            playerController.Move(player_MoveDirection * player_MoveSpeed * Time.deltaTime);
            PlayerObject.transform.eulerAngles = player_FaceDirection;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            for (int i = 0; i < MagicKey.Length; i++)
            {
                if(registeredGem[i].coolTime == 0)
                {
                    if (Input.GetKeyDown(MagicKey[i]))
                    {
                        if(registeredGem[i].magic_2 != "Haste")
                        {
                            UseMagic(i, registeredGem[i].magic_2);
                            hasteStack = 0;
                        }
                        else
                        {
                            if(hasteStack < 3)
                            {
                                hasteStack++;
                            }
                        }
                    }
                }
            }
        }else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            for (int i = 0; i < MagicKey.Length; i++)
            {
                if (registeredGem[i].coolTime == 0 && registeredGem[i].magic_2 == "Haste" && hasteStack > 0)
                {
                    UseMagic(i, registeredGem[i].magic_2);

                }
            }
        }
        else
        {
            for(int i = 0; i < MagicKey.Length; i++)
            {
                if(registeredGem[i].coolTime == 0)
                {
                    if (Input.GetKeyDown(MagicKey[i]))
                    {
                        if (registeredGem[i].magic_1 == "FireBall")
                        {
                            chargingTime = Time.time;
                        }
                        else
                        {
                            UseMagic(i, registeredGem[i].magic_1, chargingLevel);
                        }

                    }
                    else if (Input.GetKey(MagicKey[i]))
                    {
                        if (registeredGem[i].magic_1 == "WindCutter")
                        {
                            UseMagic(i, registeredGem[i].magic_1);
                        } else if (registeredGem[i].magic_1 == "FireBall")
                        {
                            chargingLevel = (int)(Time.time - chargingTime);
                        }
                    }
                    else if (Input.GetKeyUp(MagicKey[i]))
                    {
                        if (registeredGem[i].magic_1 == "FireBall")
                        {
                            UseMagic(i, registeredGem[i].magic_1, chargingLevel);
                        }
                    }
                }
            }
        }
    }







    //함수들
    public void UseMagic(int i, string magicName, int chargingLevel = 0)
    {
        Magic magic = GetComponent<Magic>();
        magic.magicName = magicName;
        magic.SetMagicStat();
        registeredGem[i].coolTime = magic.coolTime;
        player_Mp -= magic.manaConsumption;
        switch (magic.magicName)
        {
            case "FireBall":
                if(player_status != (int)PlayerStatus.FireBarrier)
                {
                    GameObject FireBall = (GameObject)Instantiate(magic.FireBall);
                    if (chargingLevel > registeredGem[i].level)
                    {
                        chargingLevel = registeredGem[i].level;
                    }
                    FireBall.GetComponent<Attack>().damage = magic.damage * (chargingLevel + 1);
                    FireBall.GetComponent<Attack>().attackName = "FireBall";
                    FireBall.GetComponent<Attack>().chargingLevel = chargingLevel;
                    FireBall.transform.position = PlayerObject.transform.position;
                    FireBall.transform.eulerAngles = PlayerObject.transform.eulerAngles;
                    FireBall.GetComponent<Rigidbody>().velocity = PlayerObject.transform.up.normalized * magic.moveSpeed * Time.deltaTime;
                }
                break;
            case "FireBarrier":
                if(player_status == (int)PlayerStatus.FireBarrier)
                {
                    player_status = (int)PlayerStatus.Normal;
                    player_Damage = 0;
                    player_MoveSpeed = 10.0f;
                    barrier_Dealing = false;
                }else{
                    player_status = (int)PlayerStatus.FireBarrier;
                    player_Damage = registeredGem[i].level;
                    player_MoveSpeed = 10.0f;
                    barrier_Dealing = true;
                }
                PlayerObject.GetComponent<PlayerScript>().SetValues();
                SetStatus(player_status);
                break;
            case "WaterShower":
                GameObject WaterShower = (GameObject)Instantiate(magic.WaterShower);
                WaterShower.GetComponent<Attack>().damage = registeredGem[i].level;
                WaterShower.GetComponent<Attack>().attackName = "WaterShower";
                WaterShower.transform.position = PlayerObject.transform.position;
                WaterShower.transform.eulerAngles = PlayerObject.transform.eulerAngles;
                WaterShower.GetComponent<Rigidbody>().velocity = PlayerObject.transform.up.normalized * magic.moveSpeed * Time.deltaTime;
                break;
            case "BubbleBarrier":
                if(player_status == (int)PlayerStatus.BubbleBarrier)
                {
                    player_status = (int)PlayerStatus.Normal;
                    player_MoveSpeed = 10.0f;
                }
                else
                {
                    player_status = (int)PlayerStatus.BubbleBarrier;
                    player_Damage = 0;
                    player_MoveSpeed = 10.0f;
                    barrier_Dealing = false;
                }
                PlayerObject.GetComponent<PlayerScript>().SetValues();
                SetStatus(player_status);
                break;
            case "WindCutter":
                GameObject[] WindCutters = new GameObject[registeredGem[i].level];
                for (int j = 0; j < registeredGem[i].level; j++)
                {
                    WindCutters[j] = (GameObject)Instantiate(magic.WindCutter);
                    WindCutters[j].GetComponent<Attack>().damage = magic.damage;
                    WindCutters[j].GetComponent<Attack>().attackName = "WindCutter";
                    WindCutters[j].transform.position = PlayerObject.transform.position;
                    WindCutters[j].transform.eulerAngles = PlayerObject.transform.eulerAngles;
                }
                if (registeredGem[i].level == 1)
                {
                    WindCutters[0].GetComponent<Rigidbody>().velocity = PlayerObject.transform.up.normalized * magic.moveSpeed * Time.deltaTime;
                }else if(registeredGem[i].level == 2)
                {
                    float angle = Mathf.PI / 180 * 10;
                    Vector3 moveDirection = PlayerObject.transform.up.normalized;
                    Vector3 moveDirection_1 = new Vector3(moveDirection.x * Mathf.Cos(angle) - moveDirection.y * Mathf.Sin(angle), moveDirection.x * Mathf.Sin(angle) + moveDirection.y * Mathf.Cos(angle), 0).normalized;
                    Vector3 moveDirection_2 = new Vector3(moveDirection.x * Mathf.Cos(-angle) - moveDirection.y * Mathf.Sin(-angle), moveDirection.x * Mathf.Sin(-angle) + moveDirection.y * Mathf.Cos(-angle), 0).normalized;
                    WindCutters[0].GetComponent<Rigidbody>().velocity = moveDirection_1 * magic.moveSpeed * Time.deltaTime;
                    WindCutters[1].GetComponent<Rigidbody>().velocity = moveDirection_2 * magic.moveSpeed * Time.deltaTime;

                }else if(registeredGem[i].level == 3)
                {
                    float angle = Mathf.PI / 180 * 15;
                    Vector3 moveDirection = PlayerObject.transform.up.normalized;
                    Vector3 moveDirection_1 = new Vector3(moveDirection.x * Mathf.Cos(angle) - moveDirection.y * Mathf.Sin(angle), moveDirection.x * Mathf.Sin(angle) + moveDirection.y * Mathf.Cos(angle), 0).normalized;
                    Vector3 moveDirection_2 = new Vector3(moveDirection.x * Mathf.Cos(-angle) - moveDirection.y * Mathf.Sin(-angle), moveDirection.x * Mathf.Sin(-angle) + moveDirection.y * Mathf.Cos(-angle), 0).normalized;
                    WindCutters[0].GetComponent<Rigidbody>().velocity = moveDirection * magic.moveSpeed * Time.deltaTime;
                    WindCutters[1].GetComponent<Rigidbody>().velocity = moveDirection_1 * magic.moveSpeed * Time.deltaTime;
                    WindCutters[2].GetComponent<Rigidbody>().velocity = moveDirection_2 * magic.moveSpeed * Time.deltaTime;
                }
                break;
            case "Haste":
                if (player_status == (int)PlayerStatus.Haste)
                {
                    player_status = (int)PlayerStatus.Normal;
                    player_MoveSpeed = 10.0f;
                }
                else
                {
                    player_status = (int)PlayerStatus.Haste;
                    player_Damage = 0;
                    if(registeredGem[i].level <= hasteStack)
                    {
                        hasteStack = registeredGem[i].level;
                    }
                    player_MoveSpeed = player_MoveSpeed * (1.0f + (hasteStack * 0.25f));
                    barrier_Dealing = false;
                }
                PlayerObject.GetComponent<PlayerScript>().SetValues();
                SetStatus(player_status);
                break;
            default:
                break;
        }
        StartCoroutine(CoolDown(registeredGem[i]));
    }
    public void SetStatus(int status)
    {
        PlayerRenderer.sharedMaterial = PlayerMaterials[status];
    }




    //Coroutines
    public IEnumerator CoolDown(Gem gem)
    {
        yield return new WaitForSeconds(gem.coolTime);
        gem.coolTime = 0;
    }
}

using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class move : MonoBehaviour
{
    public float speed;
    public Rigidbody rb;
    public int score;
    public SpriteRenderer scoreSprite1, scoreSprite2, scoreSprite3, scoreSprite4;
    public SpriteRenderer gameOver;
    private Boolean lost;
    public AudioClip die, hit ,point, wing;
    AudioSource audio;
    public Sprite blueBird, yellowBird, redBird;
    private SpriteRenderer spriteRenderer; 
    public RuntimeAnimatorController flapY ,flapB, flapR;
    public Sprite dayBackground, nightBackground;
    public SpriteRenderer background1, background2, background3, background4;
    public Sprite greenPipe,redPipe;
    public SpriteRenderer pipe1, pipe2;
    public GameObject pipes, pipePass;
    public Transform pipeSpawn, pipePassSpawn, pipePassSpawnRotated;
    float tempTime;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        score = 0;
        gameOver.enabled = false;
        lost = false;
        audio = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        System.Random r = new System.Random();
        int birdColor = r.Next(1, 4); //for either yellow,blue,red bird color (1,2,3)
        int backgroundColor = r.Next(1, 3); //for either day or night background (1,2)
        int pipeColor = r.Next(1, 3); //for either green or red pipe (1,2)
        if (birdColor == 1) 
        {
            spriteRenderer.sprite = yellowBird;
            this.GetComponent<Animator>().runtimeAnimatorController = flapY as RuntimeAnimatorController;
        } else if (birdColor == 2)
        {
            spriteRenderer.sprite = blueBird;
            this.GetComponent<Animator>().runtimeAnimatorController = flapB as RuntimeAnimatorController;
        }
        else
        {
            spriteRenderer.sprite = redBird;
            this.GetComponent<Animator>().runtimeAnimatorController = flapR as RuntimeAnimatorController;
        }
        if (backgroundColor == 1) 
        {
            setSprite(background1, dayBackground);
            setSprite(background2, dayBackground);
            setSprite(background3, dayBackground);
            setSprite(background4, dayBackground);
        } else {
            setSprite(background1, nightBackground);
            setSprite(background2, nightBackground);
            setSprite(background3, nightBackground);
            setSprite(background4, nightBackground);
        }
        if (pipeColor == 1)
        {
            setSprite(pipe1, greenPipe);
            setSprite(pipe2, greenPipe);
        }
        else
        {
            setSprite(pipe1, redPipe);
            setSprite(pipe2, redPipe);
        }
    }
    // Update is called once per frame
    void Update()
    {

        if (lost == false)
        {
            Vector3 tempVect = new Vector3(1, 0, 0);
            tempVect = tempVect.normalized * speed * Time.deltaTime;
            rb.MovePosition(transform.position + tempVect);

            if (Input.GetMouseButtonDown(0))
            {
                audio.PlayOneShot(wing, 0.7F);
                rb.AddForce(new Vector3(0, 7, 0), ForceMode.Impulse); 
            }
            tempTime += Time.deltaTime;
            if (tempTime > 2.5)
            {
                tempTime = 0;
                pipeCreator();
            }
        }
        if (lost == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene("gameplay");
            }
        }

    }
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == ("base") || col.gameObject.name == ("pipe1") || col.gameObject.name == ("pipe2") || col.gameObject.name == ("pipe2(Clone)") || col.gameObject.name == ("pipe1(Clone)"))
        {
            audio.PlayOneShot(hit, 0.7F);
            audio.PlayOneShot(die, 0.7F);
            gameOver.enabled = true;
            lost = true;
        }
        if (col.gameObject.name == ("pipePass") || col.gameObject.name == ("pipePass(Clone)"))
        {
            audio.PlayOneShot(point, 0.7F);
            Destroy(col.gameObject);
            score++; //ERROR if > 4 digits!!!
            String scoreStr = score.ToString(); //to check each digit
            int length = scoreStr.Length; //to check number of digits
            if (length==1)
            {
                setScore(1, (scoreStr[0]) - '0');
            }
            else if (length==2) {
                setScore(1, (scoreStr[1]) - '0');
                setScore(2, (scoreStr[0]) - '0');
            }
            else if (length == 3)
            {
                setScore(1, (scoreStr[2]) - '0');
                setScore(2, (scoreStr[1]) - '0');
                setScore(3, (scoreStr[0]) - '0');
            }
            else if (length == 4)
            {
                setScore(1, (scoreStr[3]) - '0');
                setScore(2, (scoreStr[2]) - '0');
                setScore(3, (scoreStr[1]) - '0');
                setScore(4, (scoreStr[0]) - '0');
            }
        }
    }
    void setNumber(SpriteRenderer sr,int number) { 
        sr.sprite= Resources.Load<Sprite>(number.ToString());
    }
    void setSprite(SpriteRenderer sr, Sprite s)
    {
        sr.sprite = s;
    }
    void setScore(int location,int number)
    {
        if (location==1)
        {
            setNumber(scoreSprite1,number);

        }else if (location==2)
        {
            setNumber(scoreSprite2, number);

        }
        else if (location == 3)
        {
            setNumber(scoreSprite3, number);

        }
        else if (location == 4)
        {
            setNumber(scoreSprite4, number);
        }
    }
    void pipeCreator() {
        //randomize if the pipe is top or bottom
        System.Random r = new System.Random();
        int up = r.Next(1, 3); //1 or 2
        int level = r.Next(1, 4); //1 or 2 or 3 
        int xaxis = r.Next(0, 3); //0 or 1 or 2 Distance-x
        int plus2 = r.Next(0, 2); //NO OR HEIGHT
        int plus3 = r.Next(0, 3); //NO OR HEIGHT
        int plus4 = r.Next(0, 4); //NO OR HEIGHT
        if (up == 1)
        {
            if (level == 1)
            {
                if (plus4 == 0)
                {
                    createGo(pipes, pipePassSpawn, xaxis, -2);//LOW
                }
                else if (plus4 == 1)
                {
                    createGo(pipes, pipePassSpawn, xaxis, -2);//LOW
                    createGo(pipes, pipePassSpawnRotated, xaxis, -1);//LOW
                }
                else if(plus4==2)
                {
                    createGo(pipes, pipePassSpawn, xaxis, -2);//LOW
                    createGo(pipes, pipePassSpawnRotated, xaxis, 0);//MID
                }
                else
                {
                    createGo(pipes, pipePassSpawn, xaxis, -2);//LOW
                    createGo(pipes, pipePassSpawnRotated, xaxis, 1);//HIGH
                }
            }
            else if (level == 2)
            {
                if (plus3==0)
                {
                    createGo(pipes, pipePassSpawn, xaxis, -1);//MID
                }
                else if (plus3 == 1)
                {
                    createGo(pipes, pipePassSpawn, xaxis, -1);//MID
                    createGo(pipes, pipePassSpawnRotated, xaxis, 0);//MID
                }
                else
                {
                    createGo(pipes, pipePassSpawn, xaxis, -1);//MID
                    createGo(pipes, pipePassSpawnRotated, xaxis, 1);//HIGH
                }
            }
            else
            {
                if (plus2 == 0)
                {
                    createGo(pipes, pipePassSpawn, xaxis, 0);//HIGH

                }
                else
                {
                    createGo(pipes, pipePassSpawn, xaxis, 0);//HIGH
                    createGo(pipes, pipePassSpawnRotated, xaxis, 1);//HIGH

                }
            }
            createGo(pipePass, pipePassSpawn, xaxis, 3);
        }
        else
        {//.rotation is new Quaternion(x, y , z, 1);
            if (level == 1)
            {
                if (plus2 == 0)
                {
                    createGo(pipes, pipePassSpawnRotated, xaxis, -1);//LOW
                }
                else
                {
                    createGo(pipes, pipePassSpawnRotated, xaxis, -1);//LOW
                    createGo(pipes, pipePassSpawn, xaxis, -2);//LOW
                }
            }
            else if (level == 2)
            {
                if (plus3 == 0)
                {
                    createGo(pipes, pipePassSpawnRotated, xaxis, 0);//MID
                }
                else if (plus3 == 1)
                {
                    createGo(pipes, pipePassSpawnRotated, xaxis, 0);//MID
                    createGo(pipes, pipePassSpawn, xaxis, -2);//LOW
                }
                else
                {
                    createGo(pipes, pipePassSpawnRotated, xaxis, 0);//MID
                    createGo(pipes, pipePassSpawn, xaxis, -1);//MID
                }
            }
            else
            {
                if (plus2 == 0)
                {
                    createGo(pipes, pipePassSpawnRotated, xaxis, 1);//HIGH
                    createGo(pipes, pipePassSpawn, xaxis, -2);//LOW
                }
                else
                {
                    createGo(pipes, pipePassSpawnRotated, xaxis, 1);//HIGH
                    createGo(pipes, pipePassSpawn, xaxis, -1);//MID
                }
            }
            createGo(pipePass, pipePassSpawnRotated, xaxis, -3);
        }
    }
    void createGo(GameObject go, Transform t, int xaxis, int yaxis) {
        Instantiate(go, t.position + new Vector3(xaxis, yaxis, 0), t.rotation);
    }
}
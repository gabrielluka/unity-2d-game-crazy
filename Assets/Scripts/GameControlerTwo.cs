using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControlerTwo : MonoBehaviour
{

    private Camera camera;
    public Transform player;

    public Transform limiterCamLeft, limiterCamRigth, limiterCamTop, limiterCamBottom;
    public float speedCam;


    [Header("Audio")]
    public AudioSource soundEffect;
    public AudioSource musicSource;

    public AudioClip jumpSound;
    public AudioClip attackSound;
    public AudioClip stepSoundA;
    public AudioClip stepSoundB;
    public AudioClip coinSound;
    public AudioClip deathSound;
    public AudioClip damageSound;

    public GameObject panelGameOver;

    public int coindCount;
    public Text coinsTxT;
    public Image[] lifes;
    public int life;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        lifeController();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate() {
        CameraHandler();
    }

    void CameraHandler() {
        float camPosX = player.position.x;
        float camPosY = player.position.y;

        if(camera.transform.position.x < limiterCamLeft.position.x && player.position.x < limiterCamLeft.position.x) {
            camPosX = limiterCamLeft.position.x;
        } else if (camera.transform.position.x > limiterCamRigth.position.x && player.position.x > limiterCamRigth.position.x){
            camPosX =  limiterCamRigth.position.x;
        } 
        if (camera.transform.position.y < limiterCamBottom.position.y && player.position.y < limiterCamBottom.position.y){
            camPosY =  limiterCamBottom.position.y;
        }  else if (camera.transform.position.y > limiterCamTop.position.y && player.position.y > limiterCamTop.position.y){
            camPosY =  limiterCamTop.position.y;
        } 

        Vector3 cameraPosition = new Vector3(camPosX, camPosY, camera.transform.position.z);
        camera.transform.position = Vector3.Lerp(camera.transform.position, cameraPosition, speedCam * Time.deltaTime);
    } 


    public void playSound(AudioClip soundClip, float volume) {
        soundEffect.PlayOneShot(soundClip, volume);
    }


    void lifeController() {
        foreach(Image l in lifes) {
            l.enabled = false;
        }
        for(int i = 0; i < life; i++) {
            lifes[i].enabled = true;
        }
    }

    public void getHit() {
        life -=1;
        lifeController();
        if(life <= 0 ) {
            player.gameObject.SetActive(false);
            panelGameOver.SetActive(true);
        }
    }

    public void getCoin() {
        coindCount += 1;
        coinsTxT.text = coindCount.ToString();
    }
}



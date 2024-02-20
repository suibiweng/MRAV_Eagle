using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MRAVctrl : MonoBehaviour
{


    public GameObject [] BackGroundFX;
    public GameObject [] OneShotFx;



    public OSC osc;

    // Start is called before the first frame update
 void Start()
    {

        osc=FindObjectOfType<OSC>();
        osc.SetAllMessageHandler(Recive);
      //  osc.SetAddressHandler(Recive);
        
    }

    void Recive(OscMessage oscMessage){


        switch(oscMessage.address)
        
        {
            //Background
            case "/Ableton/SweepVol":
            print( oscMessage.GetFloat(0));
            break;

            //OneShot
            case "/Ableton/S":
            print( oscMessage.GetFloat(0));
            break;


        }
      

    }




    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B)){
            TriggerBackGround((int)Random.Range(0,BackGroundFX.Length));


        }

        if(Input.GetKeyDown(KeyCode.S)){
            TriggerOnshot((int)Random.Range(0,OneShotFx.Length));


        }

        
    }

    void TriggerOnshot(int i){

        GameObject fx =Instantiate(OneShotFx[i],new Vector3(Random.Range(-3,3),Random.Range(-2,3),Random.Range(3,5)),Quaternion.identity);

        fx.AddComponent<OneShotFly>();
        fx.GetComponent<OneShotFly>().speedf=0.02f;

    





    }

    void TriggerBackGround(int i){

        if(BackGroundFX[i].activeInHierarchy ==true){
            BackGroundFX[i].SetActive(false);

        }else{

            BackGroundFX[i].SetActive(true);

        }



    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePointController : MonoBehaviour
{
    public int num1;
    public int num2;
    public GameObject[] gameObj;

    public bool load;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player") && !load )
        {
            ES3.Save("num1", num1);
            ES3.Save("num2", num2);
            ES3.Save<GameObject[]>("gameObj", gameObj);
        }
        else if(other.tag.Equals("Player") && load)
        {
            int number1 = ES3.Load<int>("num1");
            int number2 = ES3.Load<int>("num2");
            GameObject[] obj = ES3.Load<GameObject[]>("gameObj");
            
            Debug.Log("number 1: " + number1);
            Debug.Log("number 2: " + number2);

            for(int i = 0; i < obj.Length; i++)
            {
                Debug.Log("obj[" + i + "]: " + obj[i].name);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CryptexRow : MonoBehaviour
{
    // Start is called before the first frame update

    private float timer;
    private int counterA, counterB, counterC, counterD, counterE, rowNumber;
    private bool onRowA, onRowB, onRowC, onRowD, onRowE, finished;
    [SerializeField] Animator rowAnim1, rowAnim2, rowAnim3, rowAnim4, rowAnim5;
    

    private void Start()
    {
        finished = false;
        timer = 0;
        counterA = 1;
        counterB = 1;
        counterC = 1;
        counterD = 1;
        counterE = 1;
        rowNumber = 1;
        onRowA = true;
        onRowB = false;
        onRowC = false;
        onRowD = false;
        onRowE = false;
    }

    // Update is called once per frame
    private void Update()
    {
       
        
        timer = timer + Time.deltaTime;

        if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) && rowNumber <=5 && rowNumber>=1)
        {
            rowNumber++;
        }

        if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) ) && rowNumber <= 5 && rowNumber >= 1)
        {
            rowNumber--;
        }

        

        if (rowNumber == 1)
        {
            rowAnim1.SetInteger("counter", counterA);
            
            onRowA = true;
            onRowB = false;
            onRowC = false;
            onRowD = false;
            onRowE = false;
        }
        else if (rowNumber == 2)
        {
            rowAnim2.SetInteger("counter", counterB);
            onRowA = false;
            onRowB = true;
            onRowC = false;
            onRowD = false;
            onRowE = false;
        }
        else if (rowNumber == 3)
        {
            rowAnim3.SetInteger("counter", counterC);
            onRowA = false;
            onRowB = false;
            onRowC = true;
            onRowD = false;
            onRowE = false;
        }
        else if (rowNumber == 4)
        {
            rowAnim4.SetInteger("counter", counterD);
            onRowA = false;
            onRowB = false;
            onRowC = false;
            onRowD = true;
            onRowE = false;
        }
        else if (rowNumber == 5)
        {
            rowAnim5.SetInteger("counter", counterE);
            onRowA = false;
            onRowB = false;
            onRowC = false;
            onRowD = false;
            onRowE = true;
        }
        else if (rowNumber < 1)
        {
            rowNumber = 1;
        }
        else if (rowNumber > 5)
        {
            rowNumber = 5;
        }


        if (onRowA)
            {
            
                counterA = getCounter(counterA);
            }
            else if (onRowB)
            {
                counterB = getCounter(counterB);
            }
            else if (onRowC)
            {
                counterC = getCounter(counterC);
            }
            else if (onRowD)
            {
                counterD = getCounter(counterD);
            }
            else if (onRowE)
            {
                counterE = getCounter(counterE);
                
            }

            if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Mouse0)) && counterA == 16 && counterB == 9 && counterC == 26 && counterD == 26 && counterE == 1)
            {
                CorrectWord();
            }




    }

   

    private void CorrectWord()
    {
        PickupItem.counter = 2; //change to puzzle done
        PickupItem.puzzleDone = true;
        PickupItem.wasPlayingCryptext = true;
        GameObject.Destroy(this);
    }

    private int getCounter(int letterNo)
    {
        

        if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) || Input.GetAxis("Mouse ScrollWheel") < 0f) && timer > 1.2)
        {
            letterNo = letterNo + 1;
            timer = 0;
        }
        else if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetAxis("Mouse ScrollWheel") > 0f) && timer > 1.2)
        {
            letterNo = letterNo - 1;
            timer = 0;
        }
        if (letterNo < 1)
        {
            letterNo = 26;
        }
        else if (letterNo > 26)
        {
            letterNo = 1;
        }
        
        return letterNo;
    }
    

}

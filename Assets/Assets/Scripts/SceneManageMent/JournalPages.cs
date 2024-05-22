using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalPages : MonoBehaviour
{
    

    public void changeToPage2()
    {
        PauseScreen.isOnPage1 = false;
        PauseScreen.isOnPage2 = true;
    }

    public void backToPage1()
    {
        PauseScreen.isOnPage1 = true;
        PauseScreen.isOnPage2 = false;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StgeSelect : MonoBehaviour
{
   public void Change_Button(int StageNumber)
   {
        string SceneName = "";

        switch(StageNumber)
        {
            case 1:
                SceneName = "SampleScene";
                break;
            case 2:
                SceneName = "Stage2";
                break;
        }

        SceneManager.LoadScene(SceneName);
   }
}

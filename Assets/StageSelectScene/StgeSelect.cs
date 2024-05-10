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
                SceneName = "Stage1";
                break;
            case 2:
                SceneName = "Stage2";
                break;
        }

        SceneManager.LoadScene(SceneName);
   }
}

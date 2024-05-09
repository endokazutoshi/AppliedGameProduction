using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StgeSelect : MonoBehaviour
{
   public void Change_Button()
   {
        SceneManager.LoadScene("Stage1");
   }
   public void Change_Button2()
   {
        SceneManager.LoadScene("Stage2");
   }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestMoveScene : MonoBehaviour
{
    public void MoveBackClick()
    {
        SceneManager.LoadScene("StageChoiceScene");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
  public string Name;
  public void changeName(string newName){
    Name = newName;
  }
  public void playGame(){
    DontDestroyOnLoad(this);
    SceneManager.LoadScene(0);
  }
  public string getName(){
    return Name;
  }
}

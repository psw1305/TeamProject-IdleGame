using UnityEngine;
using UnityEngine.SceneManagement;

namespace SciFiArsenal
{

public class PixelArsenalSceneSelect : MonoBehaviour
{
	public bool GUIHide = false;
	public bool GUIHide2 = false;
	public bool GUIHide3 = false;
	
    public void LoadSceneDemo1()
    {
        SceneManager.LoadScene("demo_missiles");
    }
    public void LoadSceneDemo2()
    {
        SceneManager.LoadScene("demo_beams");
    }
    public void LoadSceneDemo3()
    {
        SceneManager.LoadScene("3");
    }
    public void LoadSceneDemo4()
    {
        SceneManager.LoadScene("4");
    }
    public void LoadSceneDemo5()
    {
        SceneManager.LoadScene("5");
    }
    public void LoadSceneDemo6()
    {
        SceneManager.LoadScene("6");
    }
    public void LoadSceneDemo7()
    {
        SceneManager.LoadScene("7");
    }
    public void LoadSceneDemo8()
    {
        SceneManager.LoadScene("8");
    }
    public void LoadSceneDemo9()
    {
        SceneManager.LoadScene("9");
    }
    public void LoadSceneDemo10()
    {
        SceneManager.LoadScene("10");
    }
	public void LoadSceneDemo11()
    {
        SceneManager.LoadScene("11");
    }
	public void LoadSceneDemo12()
    {
        SceneManager.LoadScene("12");
    }
	public void LoadSceneDemo13()
    {
        SceneManager.LoadScene("13");
    }
	public void LoadSceneDemo14()
    {
        SceneManager.LoadScene("14");
    }
	public void LoadSceneDemo15()
    {
        SceneManager.LoadScene("15");
    }
	
	void Update ()
	 {
 
     if(Input.GetKeyDown(KeyCode.J))
	 {
         GUIHide = !GUIHide;
     
         if (GUIHide)
		 {
             GameObject.Find("CanvasSceneSelect2").GetComponent<Canvas> ().enabled = false;
         }
		 else
		 {
             GameObject.Find("CanvasSceneSelect2").GetComponent<Canvas> ().enabled = true;
         }
     }
	      if(Input.GetKeyDown(KeyCode.K))
	 {
         GUIHide2 = !GUIHide2;
     
         if (GUIHide2)
		 {
             GameObject.Find("Canvas").GetComponent<Canvas> ().enabled = false;
         }
		 else
		 {
             GameObject.Find("Canvas").GetComponent<Canvas> ().enabled = true;
         }
     }
		if(Input.GetKeyDown(KeyCode.L))
	 {
         GUIHide3 = !GUIHide3;
     
         if (GUIHide3)
		 {
             GameObject.Find("CanvasTips").GetComponent<Canvas> ().enabled = false;
         }
		 else
		 {
             GameObject.Find("CanvasTips").GetComponent<Canvas> ().enabled = true;
         }
     }
	 }
}
}
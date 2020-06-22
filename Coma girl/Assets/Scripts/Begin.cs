using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(AudioSource))]


public class Begin : MonoBehaviour
{
    public AudioClip beep;//鼠标点击菜单时的音效
    AudioSource audioMenu;//音频播放器
    public GameObject InstallPanel;
    bool isTop = true;
    public GameObject Menu;
    public GameObject BagPanel;
   
    // Start is called before the first frame update
    void Start()
    {
        audioMenu = GetComponent<AudioSource>();
        if (audioMenu == null)
            audioMenu = gameObject.AddComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isTop == true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Time.timeScale = 0;
                isTop = false;
                Menu.SetActive(true);

            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Time.timeScale = 1;
                isTop = true;
                Menu.SetActive(false);

            }
        }
    }
    //开始
    public void OnClick()
    {
        audioMenu.PlayOneShot(beep);
        SceneManager.LoadScene("Game");
    }
    //继续
    public void ContinueClick()
    {
        audioMenu.PlayOneShot(beep);
        Time.timeScale = 1f;
    }

    //设置
  
    public void IstallClick()
    {
        audioMenu.PlayOneShot(beep);
        InstallPanel.SetActive(true);
    }

    public void IstallQuitClick()
    {
        audioMenu.PlayOneShot(beep);
        InstallPanel.SetActive(false);
    }
    //结束
    public void QuitClick()
    {
        audioMenu.PlayOneShot(beep);
        Application.Quit();
    }

    public void BagClick()
    {
        audioMenu.PlayOneShot(beep);
        BagPanel.SetActive(true);
    }
    //背包
    public void bagQuitClick()
    {
        audioMenu.PlayOneShot(beep);
        BagPanel.SetActive(false);
    }
}

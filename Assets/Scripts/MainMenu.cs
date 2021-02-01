using UnityEngine;
using UnityEngine.SceneManagement;

namespace WaifuTaxi
{
    public class MainMenu : MonoBehaviour
    {

        void Start()
        {
            AudioManager.Instance.PlayMusic("menu", true);
        } 

        public void OnPlayButtonClick()
        {
            AudioManager.Instance.StopPlayingMusic();
            SceneManager.LoadScene("MainGame", LoadSceneMode.Single);
        }
    }
}
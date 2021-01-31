using UnityEngine;
using UnityEngine.SceneManagement;

namespace WaifuTaxi
{
    public class MainMenu : MonoBehaviour
    {
        public void OnPlayButtonClick()
        {
            SceneManager.LoadScene("UIScene", LoadSceneMode.Single);
        }
    }
}
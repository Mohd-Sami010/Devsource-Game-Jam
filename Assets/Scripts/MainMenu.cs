using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button githubButton;

    private void Start()
    {
        playButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        });
        githubButton.onClick.AddListener(() => {
            Application.OpenURL("https://github.com/Mohd-Sami010/Devsource-Game-Jam");});
    }
}

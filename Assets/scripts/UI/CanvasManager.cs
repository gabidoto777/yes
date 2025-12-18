using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    [Header("Button References")]
    public Button startButton;
    public Button settingsButton;
    public Button quitButton;
    public Button backSettingsButton;
    public Button resumeButton;
    public Button returnToMenuButton;

    [Header("HUD References")]
    public TMP_Text livesText;

    [Header("Menu References")]
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject pauseMenu;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (startButton)
            startButton.onClick.AddListener(() => SceneManager.LoadScene(0));

        if (settingsButton)
            settingsButton.onClick.AddListener(() => SetMenus(settingsMenu, mainMenu));

        if (backSettingsButton)
            backSettingsButton.onClick.AddListener(() => SetMenus(mainMenu, settingsMenu));

        if (resumeButton)
            resumeButton.onClick.AddListener(() => SetMenus(null, pauseMenu));

        if (returnToMenuButton)
            returnToMenuButton.onClick.AddListener(() => SceneManager.LoadScene(0));

        if (quitButton)
            quitButton.onClick.AddListener(QuitGame);

        if (livesText)
        {
            livesText.text = $"Lives: {GameManager.Instance.lives}";
            GameManager.Instance.OnLifeValueChanged += (int newLives) => livesText.text = $"Lives: {newLives}";
        }
    }

    private void SetMenus(GameObject menuToEnable, GameObject menuToDisable)
    {
        if (menuToEnable) menuToEnable.SetActive(true);
        if (menuToDisable) menuToDisable.SetActive(false);
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    // Update is called once per frame
    void Update()
    {

        if (!pauseMenu) return;

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (pauseMenu.activeSelf)
            {
                SetMenus(null, pauseMenu);
                return;
            }

            SetMenus(pauseMenu, null);
        }

    }
}

using UnityEngine.SceneManagement;

public class WinPopup : PopupPanel
{
    public void OnNextButtonPress()
         => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}

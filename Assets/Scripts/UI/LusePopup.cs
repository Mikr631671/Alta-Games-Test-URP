using UnityEngine.SceneManagement;

public class LusePopup : PopupPanel
{
    public void OnSoCloseButtonPress()
        => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}

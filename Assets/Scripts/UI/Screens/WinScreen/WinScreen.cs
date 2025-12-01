using BaseEngine;
using UnityEngine;

public class WinScreen : BaseScreen
{
    private void Start()
    {
    }
    void OnDestroy()
    {
    }

    private void OnPlayAgainButtonClicked()
    {
        GameController.Instance.DisableInput();
        GameController.Instance.HandlePlayGameAgain();
        GameController.Instance.EnableInput();
    }
}

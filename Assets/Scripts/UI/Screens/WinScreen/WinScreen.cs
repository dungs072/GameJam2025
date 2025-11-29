using BaseEngine;
using UnityEngine;

public class WinScreen : BaseScreen
{
    [SerializeField] private MagicButton playAgainButton;
    private void Start()
    {
        playAgainButton.AddListener(OnPlayAgainButtonClicked);
    }
    void OnDestroy()
    {
        playAgainButton.RemoveListener(OnPlayAgainButtonClicked);
    }

    private void OnPlayAgainButtonClicked()
    {
        GameController.Instance.DisableInput();
        GameController.Instance.HandlePlayGameAgain();
        GameController.Instance.EnableInput();
    }
}

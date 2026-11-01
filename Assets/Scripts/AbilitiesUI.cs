using UnityEngine;

public class AbilitiesUI : MonoBehaviour
{
    [SerializeField] private GameObject moveLeftIcon;
    [SerializeField] private GameObject moveRightIcon;
    [SerializeField] private GameObject jumpIcon;
    [SerializeField] private GameObject doubleJumpIcon;
    [SerializeField] private GameObject dashIcon;

    private void Start()
    {
        PlayerController.Instance.OnAbilityUnlocked += UpdateAbilityIcons;
        UpdateAbilityIcons();
    }
    private void UpdateAbilityIcons()
    {
        moveLeftIcon.SetActive(PlayerController.Instance.IsMoveLeftUnlocked);
        moveRightIcon.SetActive(PlayerController.Instance.IsMoveRightUnlocked);
        jumpIcon.SetActive(PlayerController.Instance.IsJumpUnlocked);
        doubleJumpIcon.SetActive(PlayerController.Instance.IsDoubleJumpUnlocked);
        dashIcon.SetActive(PlayerController.Instance.IsDashUnlocked);
    }
}

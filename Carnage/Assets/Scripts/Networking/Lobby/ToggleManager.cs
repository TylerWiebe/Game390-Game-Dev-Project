using UnityEngine;
using UnityEngine.UI;

public class ToggleManager : MonoBehaviour
{
    public Toggle NameToggle;
    public InputField NameInput;

    public Toggle PinToggle;
    public InputField PinInput;

    public GameObject EmptyNameError;
    public GameObject EmptyPinError;
    public GameObject CouldNotFindRoomError;

    public void ToggleChanged()
    {
        if (NameToggle.isOn)
            IsNameValid(NameInput.text);

        if (PinToggle.isOn)
            IsPinValid(PinInput.text);

        if (NameToggle.isOn && PinToggle.isOn)
        {
            if (IsNameValid(NameInput.text) && IsPinValid(PinInput.text))
            {
                LobbyManager.JoinRoom(int.Parse(PinInput.text));
                LobbyManager.SetNickName(NameInput.text);
            }
            else
            {
                NameToggle.isOn = false;
                PinToggle.isOn = false;
            }
        }
    }

    private bool IsNameValid(string name)
    {
        if (name == "" || name == null)
        {
            EmptyNameError.SetActive(true);
            NameToggle.isOn = false;
            return false;
        }
        else
        {
            EmptyNameError.SetActive(false);
            return true;
        }
    }

    private bool IsPinValid(string pin)
    {
        if (pin == "" || pin == null)
        {
            EmptyPinError.SetActive(true);
            PinToggle.isOn = false;
            return false;
        }
        else
        {
            EmptyPinError.SetActive(false);
            return true;
        }
    }
}

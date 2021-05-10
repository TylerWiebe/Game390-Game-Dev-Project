using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;// Required when using Event data.


public class RemoveConnectionErrorText : MonoBehaviour, ISelectHandler
{
    public GameObject CouldNotFindRoomError;

    public void OnSelect(BaseEventData eventData)
    {
        CouldNotFindRoomError.SetActive(false);
    }
}

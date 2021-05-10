using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class ParkingVictoryWindow : MonoBehaviour
{
    [SerializeField]
    private GameObject display;
    [SerializeField]
    private Text PlayerColumn;
    [SerializeField]
    private Text PlacementColumn;
    [SerializeField]
    private Text ScoreColumn;

    public GameObject ParkingSpotMovesMessage;
    public GameObject PositionPlacementMessage;
    public GameObject AccumulatedPointsMessage;
    public GameObject StuckHelpOffer;

    private int placement = 1;

    private void Start()
    {
        display.SetActive(false);
    }

    public void DisplayVictory(Player[] players)
    {
        display.SetActive(true);
        ParkingSpotMovesMessage.SetActive(false);
        PositionPlacementMessage.SetActive(false);
        AccumulatedPointsMessage.SetActive(false);
        StuckHelpOffer.SetActive(false);

        PlayerColumn.text = "";
        PlacementColumn.text = "";
        ScoreColumn.text = "";

        foreach (Player player in players)
        {
            PlayerColumn.text += player.NickName + "\n";
            PlacementColumn.text += placement + "\n";
            placement += 1;
            ScoreColumn.text += player.CustomProperties["playerScore"] + "\n";
        }
    }
}

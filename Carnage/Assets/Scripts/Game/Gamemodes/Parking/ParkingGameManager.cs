using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class ParkingGameManager : MultiplayerGameManager
{
    [SerializeField]
    private float returnToRoomSetupDelay = 20;
    [SerializeField]
    private ParkingVictoryWindow victoryWindow;
    [SerializeField]
    public ParkingDetector ParkingDetector;
    [SerializeField]
    private ParkingLotManager parkingLotManager;

    public GameObjectEvent OnVictory = new GameObjectEvent();
    public Text ParkingCountdownText;

    public static new ParkingGameManager Instance { get; private set; }
    public int ParkingSpotDuration; //in seconds
    public ScoreParkingGame scoreParkingGame; 
    
    private bool running = false;
    private float parkedTime = 0;
    private PlayerCarController localPlayer;
    public SpawningSystem SpawningSystem;
    public Text StuckHelpOffer;
    public Image PowerupDisplay;
    private PhotonView photonView;
    private IEnumerator moveParkingSpotCoroutine;
    private IEnumerator countdownCoroutine;

    public enum GameMode {
        FirstToPark = 0,
        KingOfTheHill = 1
    }

    private new void Awake()
    {
        base.Awake();
        Assert.IsNull(Instance);
        Instance = this;

        // Sync Settings
        UnityEngine.Random.InitState(RoomSettings.Seed);
    }

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        SpawningSystem = GetComponent<SpawningSystem>();
        if (MultiplayerPlayer.LocalPlayer == null && !RoomSettings.IsGameInProgress) {
            SpawningSystem.SpawnPlayer(this);
            if (!running)
                MultiplayerPlayer.LocalPlayer.GetComponent<PlayerCarController>().enabled = false;
        }
        StartGame();
    }

    public void AddParkingDetector(ParkingDetector PD)
    {
        if (ParkingDetector != null)
        {
            ParkingDetector.OnPlayerParkingStay.RemoveAllListeners();
            ParkingDetector.OnPlayerParkingExit.RemoveAllListeners();
        }

        PD.OnPlayerParkingStay.AddListener(UpdateParkedTime);
        PD.OnPlayerParkingExit.AddListener(ResetParkedTime);
        ParkingDetector = PD;
    }
    
    private void StartGame()
    {
        running = true;
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            PlayerCarController pcc = player.GetComponent<PlayerCarController>();
            if (pcc != null)
                pcc.enabled = true;
        }

        if (PhotonNetwork.IsMasterClient && ParkingGameSettings.GameMode == GameMode.KingOfTheHill)
        {
            moveParkingSpotCoroutine = WaitToMoveParkingSpot(ParkingSpotDuration);
            StartCoroutine(moveParkingSpotCoroutine);
            GameState.TimeUntilParkingSpotMoves = ParkingSpotDuration;
            countdownCoroutine = Countdown(1);
            StartCoroutine(countdownCoroutine);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        if (PhotonNetwork.IsMasterClient)
        {
            ExitGames.Client.Photon.Hashtable properties = PhotonNetwork.CurrentRoom.CustomProperties;
            properties.Remove(otherPlayer.UserId);
            PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
        }
    }

    private async void UpdateParkedTime()
    {
        if (ParkingGameSettings.GameMode == GameMode.FirstToPark)
        {
            parkedTime += Time.deltaTime;
            GameState.TimeInParkingSpot = parkedTime;
            GameState.PlayerInParkingSpot = PhotonNetwork.LocalPlayer.NickName;
        }
        else if (ParkingGameSettings.GameMode == GameMode.KingOfTheHill)
            PlayerProperties.PlayerScore += 1;

        if (parkedTime > ParkingGameSettings.VictoryCountdown && ParkingGameSettings.GameMode == GameMode.FirstToPark && running && GameState.ParkingSpotsLeft > 0)
        {
            running = false;
            photonView.RPC("SuspendParkingCountdownText", RpcTarget.All, 5000);
            parkingLotManager.GenerateNewParkingSpot();
            
            if (ParkingGameSettings.GameMode == GameMode.FirstToPark) {
                PlayerProperties.PlayerScore = 100 * (GameState.ParkingSpotsLeft / ParkingGameSettings.NumberOfTimesParkingSpotMoves);
                GameObject oldPlayerCar = MultiplayerPlayer.LocalPlayer;
                oldPlayerCar.GetComponent<PhotonView>().RPC("DisableCar", RpcTarget.All);
                SpawnPlayerAsNPC();
            }
            await Task.Delay(1000);            

            GameState.ParkingSpotsLeft -= 1;
            running = true;
        }
        else if (parkedTime > ParkingGameSettings.VictoryCountdown && running && GameState.ParkingSpotsLeft == 0)
        {
            running = false;
            PhotonNetwork.RaiseEvent(EventCodes.Victory, PhotonNetwork.LocalPlayer, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
        }
    }

    private IEnumerator WaitToMoveParkingSpot(int duration)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            while (true)
            {
                yield return new WaitForSeconds(duration);
                StopCoroutine(countdownCoroutine);
                GameState.TimeUntilParkingSpotMoves = ParkingSpotDuration;
                countdownCoroutine = Countdown(1);
                StartCoroutine(countdownCoroutine);

                if (GameState.ParkingSpotsLeft > 0)
                {
                    running = false;
                    parkingLotManager.GenerateNewParkingSpot();
                    GameState.ParkingSpotsLeft -= 1;
                    running = true;

                }
                else if (GameState.ParkingSpotsLeft == 0)
                {
                    running = false;
                    PhotonNetwork.RaiseEvent(EventCodes.Victory, PhotonNetwork.LocalPlayer, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
                    StopCoroutine(countdownCoroutine);
                    break;
                }
            }
        }
    }

    private IEnumerator Countdown(int duration)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            while (true)
            {
                yield return new WaitForSeconds(duration);

                GameState.TimeUntilParkingSpotMoves -= 1;
                if (GameState.TimeUntilParkingSpotMoves < 0)
                    GameState.TimeUntilParkingSpotMoves = 0;
            }
        }
    }

    private void SpawnPlayerAsNPC() {
        PlayerProperties.Skin = CarCustomizations.Skins.TaxiCar;
        GameObject npc = NPCManager.cars.First();
        Transform t = npc.transform;
        PhotonNetwork.Destroy(npc);
        localPlayer = InstantiatePlayer("TaxiCar", t.position, t.rotation).GetComponent<PlayerCarController>();
        PhotonNetwork.RaiseEvent(2, null, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
    }

    [PunRPC]
    private async void SuspendParkingCountdownText(int duration) //in milliseconds
    {
        ParkingCountdownText.enabled = false;
        await Task.Delay(duration);
        ParkingCountdownText.enabled = true;
        ParkingCountdownText.text = "";
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey("timeInParkingSpot"))
        {
            UpdateTimer();
        }
    }

    private void UpdateTimer()
    {
        if (GameState.TimeInParkingSpot == 0 && GameState.PlayerInParkingSpot == "" && running)
            ParkingCountdownText.text = "";
        else if (GameState.TimeInParkingSpot <= ParkingGameSettings.VictoryCountdown && running)
            ParkingCountdownText.text = GameState.PlayerInParkingSpot + " wins in " + Math.Round(ParkingGameSettings.VictoryCountdown - GameState.TimeInParkingSpot, 0).ToString() + "s";
        else
            ParkingCountdownText.text = "";
    }

    private void ResetParkedTime()
    {
        parkedTime = 0;
    }

    private void Victory(Player player)
    {
        running = false;
        victoryWindow.DisplayVictory(scoreParkingGame.CurrentScores());
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine("ReturnToRoomSetup");
        }
    }

    private System.Collections.IEnumerator ReturnToRoomSetup()
    {
        yield return new WaitForSecondsRealtime(returnToRoomSetupDelay);
        LoadScene(Scenes.RoomSetup);
        RoomSettings.IsGameInProgress = false;
    }

    public override void OnEvent(EventData eventData)
    {
		base.OnEvent(eventData);
        switch(eventData.Code)
        {
            case EventCodes.Victory:
                Victory((Player) eventData.CustomData);
                break;
        }
    }

    private void Update() {
        if (localPlayer == null && MultiplayerPlayer.LocalPlayer != null) {
            localPlayer = MultiplayerPlayer.LocalPlayer.GetComponent<PlayerCarController>();
        }
        if (localPlayer != null) {
            if (localPlayer.Stuck) {
                StuckHelpOffer.gameObject.SetActive(true);
            }
            else {
                StuckHelpOffer.gameObject.SetActive(false);
            }
        }
    }
    
}

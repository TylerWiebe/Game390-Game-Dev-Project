public class ParkingGameSettings : RoomSettings
{
    public static float VictoryCountdown
    {
        get
        {
            return GetRoomProperty<float>("victory_time");
        }
        set
        {
            SetRoomProperty("victory_time", value);
        }
    }

    public static float NumberOfTimesParkingSpotMoves
    {
        get
        {
            return GetRoomProperty<float>("times_parking_spot_moves");
        }
        set
        {
            SetRoomProperty("times_parking_spot_moves", value);
        }
    }

    public static ParkingGameManager.GameMode GameMode
    {
        get
        {
            return GetRoomProperty<ParkingGameManager.GameMode>("gamemode");
        }
        set
        {
            SetRoomProperty("gamemode", value);
        }
    }
}
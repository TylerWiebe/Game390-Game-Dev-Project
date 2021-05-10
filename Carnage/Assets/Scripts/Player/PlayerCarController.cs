using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine.Audio;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[System.Serializable]
public class Axle
{
    public WheelCollider LeftWheel;
    public WheelCollider RightWheel;
    public WheelSkid LeftSkid;
    public WheelSkid RightSkid;
    public bool Motor;
    public bool Steering;

    public float RPM
    {
        get
        {
            return (LeftWheel.rpm + RightWheel.rpm) / 2;
        }
    }
    public float LeftWheelSlip
    {
        get
        {
            if (LeftWheel.GetGroundHit(out WheelHit wh))
            {
                return wh.forwardSlip;
            }
            else
            {
                return 0;
            }
        }
    }

    public float SkidIntensity {
        get {
            return Mathf.Max(LeftSkid.Intensity, RightSkid.Intensity);
        }
    }

    public float RightWheelSlip
    {
        get
        {
            if (RightWheel.GetGroundHit(out WheelHit wh))
            {
                return wh.forwardSlip;
            }
            else
            {
                return 0;
            }
        }
    }

    public float Slip
    {
        get
        {
            return (LeftWheelSlip + RightWheelSlip) / 2;
        }
    }
}

[RequireComponent(typeof(Rigidbody))]
public class PlayerCarController : MonoBehaviourPunCallbacks
{
    // Unity Events
    public UnityEvent HonkHorn;
    public GameObjectEvent OnSkidStart = new GameObjectEvent();
    public GameObjectEvent OnSkidEnd = new GameObjectEvent();


    // Lists
    public List<Axle> axleInfos;

    // Main Stats
    public float MaxAccleration = 600;
    public float Braking = 600;
    public float MaxSpeed = 25;
    public float Traction = 1;
    public float Stability = 0.88f;

    public float MaxSteeringAngle = 30;
    public float Deceleration = 300;
    public float DriftForce = 1;

    // Flip Control
    public bool FlipControl = true;
    public float FlipRollForce = 20;

    // Mid-Air Control
    public bool AirControl = true;
    public float AirControlForce = 2;

    // Wiggle Control
    public bool WiggleControl = true;
    public float WiggleForce = 2;

    // Wheel Collider Friction Attributes
    public float FFESlip = 1f;
    public float FFEValue = 2f;
    public float FFASlip = 0.8f;
    public float FFAValue = 0.8f;
    public float FFStiff = 1;
    public float SFESlip = 0.2f;
    public float SFEValue = 2;
    public float SFASlip = 0.5f;
    public float SFAValue = 1.5f;
    public float SFStiff = 1;

    private Rigidbody rb;
    private int stuckCounter = 0;
    public int minStuckTime = 2000;
    
    //SFX variables
    public float CollisionSFXThreshhold = 1;
    public float CollisionSfxCooldown = 2;
    private float CurrentCoolDownTimer = 0;
    public AudioSource CollisionAS;
    public AudioMixer MyMixer;
    
    // Unity Audio Clips
    public AudioClip[] CollisionSFX;
    public AudioClip[] MetalPropSFX;
    public AudioClip[] WoodPropsSFX;
    public AudioClip[] PlantPropsSFX;
    public AudioClip[] StonePropsSFX;
    public AudioClip[] ElectricalPropsSFX;
    //boost powerup
    public bool isBoost = false;
    private IEnumerator dynamicTraction;

    private bool InReverse
    {
        get
        {
            return (transform.InverseTransformVector(rb.velocity).z < -0.01f);
        }
    }

    private bool InDrive
    {
        get
        {
            return (transform.InverseTransformVector(rb.velocity).z > 0.01f);
        }
    }

    private bool Flipped
    {
        get
        {
            int count = 0;
            foreach (Axle axleInfo in axleInfos)
            {
                if (axleInfo.LeftWheel.isGrounded)
                    count += 1;
                if (axleInfo.RightWheel.isGrounded)
                    count += 1;
            }
            return (count / axleInfos.Count / 2 <= 0.5f) && (rb.velocity.magnitude < 0.5f);
        }
    }

    private bool MidAir
    {
        get
        {
            foreach (Axle axleInfo in axleInfos)
            {
                if (axleInfo.LeftWheel.isGrounded || axleInfo.RightWheel.isGrounded)
                    return false;
            }
            return true;
        }
    }

    public WheelFrictionCurve ForwardWheelFriction
    {
        get {
            return axleInfos[0].LeftWheel.forwardFriction;
        }
        set {
            foreach (Axle axleInfo in axleInfos)
            {
                axleInfo.LeftWheel.forwardFriction = value;
                axleInfo.RightWheel.forwardFriction = value;
            }
        }
    }

    public WheelFrictionCurve SideWheelFriction
    {
        get {
            return axleInfos[0].LeftWheel.sidewaysFriction;
        }
        set {
            foreach (Axle axleInfo in axleInfos)
            {
                axleInfo.LeftWheel.sidewaysFriction = value;
                axleInfo.RightWheel.sidewaysFriction = value;
            }
        }
    }

    public bool Skidding {
        get {
            foreach (Axle axleInfo in axleInfos)
            {
                if (axleInfo.SkidIntensity > 0.1) {
                    return true;
                }
            }
            return false;
        }
    }

    public float SkidIntensity {
        get {
            float intensity = 0;
            foreach (Axle axleInfo in axleInfos)
            {
                if (axleInfo.SkidIntensity > intensity) {
                    intensity = axleInfo.SkidIntensity;
                }
            }
            return intensity;
        }
    }

    private bool isParked = false;
    private void parking() {
        isParked = true;
    }
    private void stoppedParking() {
        isParked = false;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        ParkingGameManager.Instance.ParkingDetector.OnPlayerParkingEnter.AddListener(parking);
        ParkingGameManager.Instance.ParkingDetector.OnPlayerParkingExit.AddListener(stoppedParking);

        WheelFrictionCurve ff = new WheelFrictionCurve
        {
            extremumSlip = FFESlip,
            extremumValue = FFEValue,
            asymptoteSlip = FFASlip,
            asymptoteValue = FFAValue,
            stiffness = FFStiff
        };

        WheelFrictionCurve sf = new WheelFrictionCurve
        {
            extremumSlip = SFESlip,
            extremumValue = SFEValue * Traction,
            asymptoteSlip = SFASlip,
            asymptoteValue = SFAValue * Traction,
            stiffness = SFStiff
        };

        foreach (Axle axleInfo in axleInfos)
        {
            axleInfo.LeftWheel.forceAppPointDistance = Stability;
            axleInfo.RightWheel.forceAppPointDistance = Stability;

            axleInfo.LeftWheel.forwardFriction = ff;
            axleInfo.LeftWheel.sidewaysFriction = sf;

            axleInfo.RightWheel.forwardFriction = ff;
            axleInfo.RightWheel.sidewaysFriction = sf;
        }

        dynamicTraction = DynamicTraction();
        StartCoroutine(dynamicTraction);
    }

    public bool Stuck = false;
    bool previouslySkidding = false;
    private bool IsMidAir = false;
    private void Update()
    {
        IsMidAir = MidAir;
        if (Skidding && !previouslySkidding && IsMidAir == false) {
            OnSkidStart.Invoke(gameObject);
            previouslySkidding = true;
        }
        else if ((IsMidAir && previouslySkidding))
        {
            OnSkidEnd.Invoke(gameObject);
            previouslySkidding = false;
        }
        else if ((!Skidding && previouslySkidding) ) {
            OnSkidEnd.Invoke(gameObject);
            previouslySkidding = false;
        }

        if (!photonView.IsMine)
        {
            return;
        }

        if (CurrentCoolDownTimer > 0)
        {
            CurrentCoolDownTimer -= Time.deltaTime;
        }
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            //HonkHorn.Invoke();
            Debug.Log("horn has been honked");
            photonView.RPC("HonkHorn", RpcTarget.All);
        }

        float motor;

        if (isBoost)
            motor = MaxAccleration * 1;
        else
            motor = MaxAccleration * Input.GetAxis("Vertical");

        if (!isParked && (Flipped || rb.velocity.magnitude < 0.5f)) {
            stuckCounter += 2;
            if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
                stuckCounter--;
            if (stuckCounter > minStuckTime) {
                // Limit how high counter can go
                if (stuckCounter > minStuckTime + 10) {
                    stuckCounter = minStuckTime + 10;
                }
                Stuck = true;
            }
        }
        else {
            stuckCounter -= 2;
            if (stuckCounter < minStuckTime) {
                Stuck = false;
                if (stuckCounter < 0)
                stuckCounter = 0;
            }
        }

        if ((Stuck && Input.GetKeyDown(KeyCode.Space)) || transform.position.y < -5) {
            Transform spawnpoint = ParkingGameManager.Instance.SpawningSystem.GetAvailableSpawnpoint();
            transform.position = spawnpoint.position;
            transform.rotation = spawnpoint.rotation;
        }
        
    }

    private IEnumerator DynamicTraction()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            float speedTraction = 1 - rb.velocity.magnitude / 75;
            float skidTraction = 1 - SkidIntensity / 4;
            Traction = Mathf.Max(0.35f, Mathf.Min(1, speedTraction * skidTraction));
            WheelFrictionCurve sf = SideWheelFriction;
            sf.extremumValue = SFEValue * Traction;
            sf.asymptoteValue = SFAValue * Traction;
            SideWheelFriction = sf;
        }
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        float motor;
        if (isBoost)
        {
            motor = MaxAccleration * 1;
        }
        else
        {
            motor = MaxAccleration * Input.GetAxis("Vertical");
            if (Input.GetKey(KeyCode.Space))
                motor = 0;
        }
        float steering = MaxSteeringAngle * Input.GetAxis("Horizontal");

        foreach (Axle axleInfo in axleInfos)
        {
            axleInfo.LeftWheel.brakeTorque = 0;
            axleInfo.RightWheel.brakeTorque = 0;
            axleInfo.LeftWheel.motorTorque = 0;
            axleInfo.RightWheel.motorTorque = 0;

            if (axleInfo.Steering)
            {
                Steer(axleInfo, steering);
            }
            if (axleInfo.Motor && rb.velocity.magnitude < MaxSpeed && axleInfo.Slip < 0.5)
            {
                Accelerate(axleInfo, motor);
            }
            if (motor == 0f)
            {
                Decelerate(axleInfo);
            }
            if (Input.GetKey(KeyCode.Space) && (motor < 0 && InDrive) || (motor > 0 && InReverse))
            {
                Brake(axleInfo);
            }
            if (Input.GetKey(KeyCode.Space))
            {
                rb.AddRelativeTorque(Vector3.up * Input.GetAxis("Horizontal") * DriftForce, ForceMode.Acceleration);
            }
            ApplyLocalPositionToWheelVisuals(axleInfo.LeftWheel);
            ApplyLocalPositionToWheelVisuals(axleInfo.RightWheel);
        }

        Manouver();
    }

    private void Manouver()
    {
        if (FlipControl && Flipped)
        {
            rb.AddRelativeTorque(Vector3.back * FlipRollForce * Input.GetAxis("Horizontal"), ForceMode.Acceleration);
            rb.AddRelativeTorque(Vector3.right * FlipRollForce * Input.GetAxis("Vertical"), ForceMode.Acceleration);
        }
        else if (AirControl && MidAir)
        {
            rb.AddRelativeTorque(Vector3.back * AirControlForce * Input.GetAxis("Horizontal"), ForceMode.Acceleration);
            rb.AddRelativeTorque(Vector3.right * AirControlForce * Input.GetAxis("Vertical"), ForceMode.Acceleration);
        }
        else
        {
            rb.AddRelativeTorque(Vector3.back * WiggleForce * Input.GetAxis("Horizontal"), ForceMode.Acceleration);
            rb.AddRelativeForce(Vector3.forward * WiggleForce * Input.GetAxis("Vertical"), ForceMode.Acceleration);
        }
    }

    private void Accelerate(Axle axleInfo, float motor)
    {
        axleInfo.LeftWheel.motorTorque = motor;
        axleInfo.RightWheel.motorTorque = motor;
    }
    private void Decelerate(Axle axleInfo)
    {
        axleInfo.LeftWheel.brakeTorque = Deceleration;
        axleInfo.RightWheel.brakeTorque = Deceleration;
    }
    private void Steer(Axle axleInfo, float steering)
    {
        axleInfo.LeftWheel.steerAngle = steering;
        axleInfo.RightWheel.steerAngle = steering;
    }
    private void Brake(Axle axleInfo)
    {
        axleInfo.LeftWheel.brakeTorque = Braking;
        axleInfo.RightWheel.brakeTorque = Braking;
    }

    private void ApplyLocalPositionToWheelVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        collider.GetWorldPose(out Vector3 position, out Quaternion rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Tags.Player))
        {
            Rebound(collision, RoomSettings.BumperCarRebound);
            if (CurrentCoolDownTimer <= 0 && collision.relativeVelocity.magnitude >= CollisionSFXThreshhold)
            {
                CurrentCoolDownTimer = CollisionSfxCooldown;
                int collisionSfxID = Random.Range(0, CollisionSFX.Length);
                PlaCollisionSFXAt(CollisionSFX[collisionSfxID], collision.contacts[1].point, 1);
                
            }
        }
        else if (collision.gameObject.CompareTag(Tags.MetalProps))
        {
            if (CurrentCoolDownTimer <= 0 && collision.relativeVelocity.magnitude >= CollisionSFXThreshhold)
            {
                CurrentCoolDownTimer = CollisionSfxCooldown;
                int collisionSfxID = Random.Range(0, CollisionSFX.Length);
                PlaCollisionSFXAt(CollisionSFX[collisionSfxID], collision.contacts[0].point, 0.5f);
                collisionSfxID = Random.Range(0, MetalPropSFX.Length);
                PlaCollisionSFXAt(MetalPropSFX[collisionSfxID], collision.contacts[0].point, 1);
            }
        }
        else if (collision.gameObject.CompareTag(Tags.PlantProps))
        {
            if (CurrentCoolDownTimer <= 0 && collision.relativeVelocity.magnitude >= CollisionSFXThreshhold)
            {
                CurrentCoolDownTimer = CollisionSfxCooldown;
                int collisionSfxID = Random.Range(0, CollisionSFX.Length);
                PlaCollisionSFXAt(CollisionSFX[collisionSfxID], collision.contacts[0].point, 1);
                collisionSfxID = Random.Range(0, PlantPropsSFX.Length);
                PlaCollisionSFXAt(PlantPropsSFX[collisionSfxID], collision.contacts[0].point, 1);
            }
        }
        else if (collision.gameObject.CompareTag(Tags.WoodProps))
        {
            if (CurrentCoolDownTimer <= 0 && collision.relativeVelocity.magnitude >= CollisionSFXThreshhold)
            {
                CurrentCoolDownTimer = CollisionSfxCooldown;
                int collisionSfxID = Random.Range(0, CollisionSFX.Length);
                PlaCollisionSFXAt(CollisionSFX[collisionSfxID], collision.contacts[0].point, 1);
                collisionSfxID = Random.Range(0, WoodPropsSFX.Length);
                PlaCollisionSFXAt(WoodPropsSFX[collisionSfxID], collision.contacts[0].point, 1);
            }
        }
        else if (collision.gameObject.CompareTag(Tags.StoneProps))
        {
            if (CurrentCoolDownTimer <= 0 && collision.relativeVelocity.magnitude >= CollisionSFXThreshhold)
            {
                CurrentCoolDownTimer = CollisionSfxCooldown;
                int collisionSfxID = Random.Range(0, CollisionSFX.Length);
                PlaCollisionSFXAt(CollisionSFX[collisionSfxID], collision.contacts[0].point, 1);
                collisionSfxID = Random.Range(0, StonePropsSFX.Length);
                PlaCollisionSFXAt(StonePropsSFX[collisionSfxID], collision.contacts[0].point, 1);
            }
        }
        else if (collision.gameObject.CompareTag(Tags.ElectricalProps))
        {
            if (CurrentCoolDownTimer <= 0 && collision.relativeVelocity.magnitude >= CollisionSFXThreshhold)
            {
                CurrentCoolDownTimer = CollisionSfxCooldown;
                int collisionSfxID = Random.Range(0, CollisionSFX.Length);
                PlaCollisionSFXAt(CollisionSFX[collisionSfxID], collision.contacts[0].point, 1);
                collisionSfxID = Random.Range(0, ElectricalPropsSFX.Length);
                PlaCollisionSFXAt(StonePropsSFX[collisionSfxID], collision.contacts[0].point, 1);
            }
        }
    }

    private void Rebound(Collision collision, float rebound)
    {
        List<ContactPoint> points = new List<ContactPoint>();
        collision.GetContacts(points);
        Vector3 force = collision.relativeVelocity * rebound;
        rb.AddForce(force, ForceMode.VelocityChange);
    }

    private AudioSource PlaCollisionSFXAt(AudioClip AC, Vector3 pos, float volume)
    {
        GameObject tempGO = new GameObject("TempAudio"); // create the temp object
        tempGO.transform.position = pos; // set its position
        AudioSource aSource = tempGO.AddComponent<AudioSource>(); // add an audio source
        aSource.clip = AC; // define the clip
        aSource.outputAudioMixerGroup = MyMixer.FindMatchingGroups("Master/SFX/Collisions")[0];
        aSource.volume = volume;
        // set other aSource properties here, if desired
        aSource.Play(); // start the sound
        Destroy(tempGO, AC.length); // destroy object after clip duration
        return aSource; // return the AudioSource reference
    }

    [PunRPC]
    private void DisableCar()
    {
        rb.isKinematic = true;
        GetComponent<MultiplayerPlayer>().DisableOwnership();
        this.enabled = false;
    }

    private void OnDestroy()
    {
        if (dynamicTraction != null)
            StopCoroutine(dynamicTraction);
    }
}
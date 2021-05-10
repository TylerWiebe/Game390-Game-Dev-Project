using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody), typeof(Navigator))]
public class NPCCarController : MonoBehaviour
{
    public NPCManager Manager;
    // Lists
    public List<Axle> AxleInfos;

    // Main Stats
    public float MaxAccleration = 600;
    public float Braking = 600;
    public float MaxSpeed = 25;
    public float Traction = 1;
    public float Stability = 0.88f;

    public float MaxSteeringAngle = 30;
    public float Deceleration = 300;

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
    private Navigator navigator;
    private PhotonView pv;

    private float verticalInput = 0;
    private float horizontalInput = 0;
    private bool shouldBrake = false;
    private bool shouldUnbrake = false;
    private bool brake;

    private bool Flipped
    {
        get
        {
            int count = 0;
            foreach (Axle axleInfo in AxleInfos)
            {
                if (axleInfo.LeftWheel.isGrounded)
                    count += 1;
                if (axleInfo.RightWheel.isGrounded)
                    count += 1;
            }
            return (count / AxleInfos.Count / 2 <= 0.5f) && (rb.velocity.magnitude < 0.5f);
        }
    }

    private bool Crashed
    {
        get
        {
            return rb.velocity.magnitude < 0.5f && !brake;
        }
    }

    private int destructionCounter = 0;
    public int destructionTime = 800;
    public int waitCounter = 0;
    public int normalWaitDestructionTime = 2400;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        navigator = GetComponent<Navigator>();
        pv = GetComponent<PhotonView>();

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

        foreach (Axle axleInfo in AxleInfos)
        {
            axleInfo.LeftWheel.forceAppPointDistance = Stability;
            axleInfo.RightWheel.forceAppPointDistance = Stability;

            axleInfo.LeftWheel.forwardFriction = ff;
            axleInfo.LeftWheel.sidewaysFriction = sf;

            axleInfo.RightWheel.forwardFriction = ff;
            axleInfo.RightWheel.sidewaysFriction = sf;
        }
    }

    private void Update() {
        if (PhotonNetwork.IsMasterClient && (Flipped || Crashed)) {
            destructionCounter++;
            if (destructionCounter > destructionTime) {
                PhotonNetwork.Destroy(gameObject);
            }
        }
        else {
            destructionCounter = 0;
            if (PhotonNetwork.IsMasterClient && brake) {
                waitCounter++;
                if (waitCounter > normalWaitDestructionTime) {
                    PhotonNetwork.Destroy(gameObject);
                }
            }
            else {
                waitCounter = 0;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!pv.IsMine)
            return;

        Vector3 direction = transform.InverseTransformPoint(navigator.TargetPoint.transform.position).normalized;

        if (shouldBrake)
        {
            brake = true;
        }
        if (shouldUnbrake && !shouldBrake)
        {
            brake = false;
        }
        shouldBrake = false;
        shouldUnbrake = false;

        if (brake)
        {
            verticalInput = 0;
        }
        else
        {
            verticalInput = Mathf.Abs(direction.z);
        }

        horizontalInput = direction.x;

        float motor = MaxAccleration * verticalInput;
        float steering = MaxSteeringAngle * horizontalInput;

        foreach (Axle axleInfo in AxleInfos)
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
            if (brake)
            {
                Brake(axleInfo);
            }
            ApplyLocalPositionToWheelVisuals(axleInfo.LeftWheel);
            ApplyLocalPositionToWheelVisuals(axleInfo.RightWheel);
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
            Rebound(collision, RoomSettings.BumperCarRebound);

    }

    private void Rebound(Collision collision, float rebound)
    {
        List<ContactPoint> points = new List<ContactPoint>();
        collision.GetContacts(points);
        Vector3 force = collision.relativeVelocity * rebound;
        rb.AddForce(force, ForceMode.VelocityChange);
    }

    public void ShouldBrake(bool brake)
    {
        if(brake)
        {
            shouldBrake = true;
        }
        else
        {
            shouldUnbrake = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag(Tags.Player) && other.name != "BrakeTrigger")
            ShouldBrake(true);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.root.CompareTag(Tags.Player) && other.name != "BrakeTrigger")
            ShouldBrake(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.root.CompareTag(Tags.Player) && other.name != "BrakeTrigger")
            ShouldBrake(false);
    }

    public void OnDestroy() {
        if (Manager != null)
            Manager.CarDestroyed(gameObject);
    }

}
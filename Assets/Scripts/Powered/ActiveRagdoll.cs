using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ActiveRagdoll : MonoBehaviour
{
    [SerializeField, Range(0,1)]
    private float m_physicsWeight = 1;

    [SerializeField]
    private float m_motorSpring = 100;

    [SerializeField]
    private float m_motorDamper = 10;

    [SerializeField]
    private float m_rootMotorSpring = 100;

    [SerializeField]
    private float m_rootMotorDamper = 10;


    [SerializeField, Range(0, 10)]
    private float m_pinSpring = 1;

    [SerializeField, Range(0, 10)]
    private float m_pinDamper = 1;


    [SerializeField]
    private Transform m_visualHierachy;

    [SerializeField]
    private Transform m_skeletonHierarchy;

    [SerializeField]
    private Rigidbody m_root;

    [SerializeField]
    private Transform m_rootVisual;

    private ConfigurableJoint[] m_joints;

    private Dictionary<Transform, Transform> m_mapping = new Dictionary<Transform, Transform>();
    private Dictionary<Transform, Transform> m_reverseMapping = new Dictionary<Transform, Transform>();
    private Dictionary<ConfigurableJoint, Transform> m_jointMapping = new Dictionary<ConfigurableJoint, Transform>();

    private bool m_ragdoll = false;

    struct Pose
    {
        public Vector3 Position;
        public Quaternion Rotation;
    }

    struct JointInfo
    {
        public Transform VisualTransform;
        public ConfigurableJoint Joint;
        public Quaternion BaseRotation;
        public Rigidbody Rigidbody;
    }


    private Transform[] m_visualTransforms;
    private Pose[] m_restPose;
    private Pose[] m_lastAnimPose;
    private Rigidbody[] m_bodies;
    private List<JointInfo> m_jointInfo = new List<JointInfo>();
    private Animator m_animator;

    private ConfigurableJoint m_rootJoint;
    private Rigidbody m_rootBody;
    private Quaternion m_rootBaseRotation;

    // Start is called before the first frame update
    void Start()
    {
        m_joints = m_skeletonHierarchy.GetComponentsInChildren<ConfigurableJoint>();
        foreach (var joint in m_joints)
        {
            joint.rotationDriveMode = RotationDriveMode.Slerp;
            var drive = joint.slerpDrive;
        }

        m_rootJoint = m_root.GetComponent<ConfigurableJoint>();
        m_rootBody = m_root.GetComponent<Rigidbody>();
        m_rootBaseRotation = m_rootVisual.localRotation;

        Transform[] visuals = m_visualHierachy.GetComponentsInChildren<Transform>();
        Transform[] skeleton = m_skeletonHierarchy.GetComponentsInChildren<Transform>();
        m_bodies = m_skeletonHierarchy.GetComponentsInChildren<Rigidbody>();
        m_animator = GetComponent<Animator>();
        m_visualTransforms = visuals;
        m_restPose = new Pose[visuals.Length];
        m_lastAnimPose = new Pose[visuals.Length];

        for (int i = 0; i < visuals.Length; i++)
        {
            for (int j = 0; j < skeleton.Length; j++)
            {
                if (visuals[i].gameObject.name == skeleton[j].gameObject.name)
                {
                    m_mapping[visuals[i]] = skeleton[j];
                    m_reverseMapping[skeleton[j]] = visuals[i];

                    ConfigurableJoint joint = skeleton[j].GetComponent<ConfigurableJoint>();
                    Rigidbody rb = skeleton[j].GetComponent<Rigidbody>();
                    if (joint && rb)
                    {
                        m_jointMapping[joint] = visuals[i];                        
                        m_jointInfo.Add(new JointInfo
                        {
                            VisualTransform = visuals[i],
                            Joint = joint,
                            BaseRotation = visuals[i].localRotation,
                            Rigidbody = skeleton[i].GetComponent<Rigidbody>()
                        });
                    }
                }
            }
        }

        for (int i = 0; i < m_visualTransforms.Length; i++)
        {
            m_restPose[i] = new Pose
            {
                Position = m_visualTransforms[i].localPosition,
                Rotation = m_visualTransforms[i].localRotation
            };
        }

        SetRagdoll(false);
    }

    [ContextMenu("Test Ragdoll")]
    public void TestRagdoll()
    {
        SetRagdoll(true);
    }

    public void SetRagdoll(bool ragdoll)
    {
        m_ragdoll = ragdoll;

        if (m_ragdoll)
        {
            foreach (var body in m_bodies)
            {
                body.isKinematic = false;
            }
            m_animator.applyRootMotion = false;
        }
        else
        {
            foreach (var body in m_bodies)
            {
                body.isKinematic = true;
            }
            m_animator.applyRootMotion = true;
        }
    }

    private void FixedUpdate()
    {
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //for (int i = 0; i < m_visualTransforms.Length; i++)
        //{
        //    m_visualTransforms[i].localPosition = m_restPose[i].Position;
        //    m_visualTransforms[i].localRotation = m_restPose[i].Rotation;
        //}

        if (m_ragdoll && m_pinSpring > 0)
        {
            foreach (var info in m_jointInfo)
            {
                Vector3 point = info.Joint.transform.TransformPoint(info.Joint.anchor);
                Vector3 target = m_rootVisual.InverseTransformPoint(info.VisualTransform.TransformPoint(info.Joint.anchor));
                target = m_root.transform.TransformPoint(target);
                Vector3 diff = point - target;

                //Vector3 dir = diff.normalized;
                //info.Rigidbody.
                ////float mp = 1.0f / (1.0f / info.Rigidbody.mass + )

                Vector3 velocityAtPoint = info.Rigidbody.GetRelativePointVelocity(info.Joint.anchor);
                Vector3 springPinForce = -info.Rigidbody.mass / Time.fixedDeltaTime * m_pinSpring * diff - velocityAtPoint * m_pinDamper;
                info.Rigidbody.AddForceAtPosition(springPinForce, point, ForceMode.Impulse);
            }
        }

        for (int i = 0; i < m_visualTransforms.Length; i++)
        {
            m_lastAnimPose[i] = new Pose
            {
                Position = m_visualTransforms[i].localPosition,
                Rotation = m_visualTransforms[i].localRotation
            };
        }

        if (m_ragdoll)
        {
            if (m_rootJoint)
            {
                var joint = m_rootJoint;
                var drive = joint.slerpDrive;
                drive.positionSpring = m_rootMotorSpring;
                drive.positionDamper = m_rootMotorDamper;
                joint.slerpDrive = drive;
                joint.SetTargetRotationLocal(m_rootVisual.localRotation, m_rootBaseRotation);
            }

            foreach (var info in m_jointInfo)
            {
                var joint = info.Joint;
                var drive = joint.slerpDrive;
                drive.positionSpring = m_motorSpring;
                drive.positionDamper = m_motorDamper;
                joint.slerpDrive = drive;
                joint.SetTargetRotationLocal(info.VisualTransform.localRotation, info.BaseRotation);
            }

            foreach (var pair in m_mapping)
            {
                Transform visual = pair.Key;
                Transform bone = pair.Value;

                Rigidbody rb = pair.Value.GetComponent<Rigidbody>();
                if (rb && rb.isKinematic)
                {
                    bone.localPosition = visual.localPosition;
                    bone.localRotation = visual.localRotation;
                    continue;
                }

                visual.localPosition = Vector3.Lerp(visual.localPosition, bone.localPosition, m_physicsWeight);
                visual.localRotation = Quaternion.Slerp(visual.localRotation, bone.localRotation, m_physicsWeight);
            }
        }
        else
        {
            for (int i = 0; i < m_visualTransforms.Length; i++)
            {
                Transform visual = m_visualTransforms[i];
                Transform bone = m_mapping[visual];

                bone.localPosition = m_lastAnimPose[i].Position;
                bone.localRotation = m_lastAnimPose[i].Rotation;

            }
        }
    }
}

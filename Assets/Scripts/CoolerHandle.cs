using UnityEngine;

public class CoolerHandle : MonoBehaviour
{
    public float interactDistance = 2f;
    private bool isAttached = false;
    private ConfigurableJoint joint;
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        // grab
        if (!isAttached && distance < interactDistance && Input.GetMouseButtonDown(1))
        {
            Attach();
        }

        // release
        if (isAttached && Input.GetMouseButtonUp(1))
        {
            Release();
        }
    }

    void Attach()
    {
        isAttached = true;

        // create joint so cooler drags physically
        Rigidbody coolerRB = transform.parent.GetComponent<Rigidbody>();
        joint = coolerRB.gameObject.AddComponent<ConfigurableJoint>();
        joint.connectedBody = player.GetComponent<Rigidbody>();

        // physics settings for dragging feel
        joint.xMotion = ConfigurableJointMotion.Limited;
        joint.yMotion = ConfigurableJointMotion.Limited;
        joint.zMotion = ConfigurableJointMotion.Limited;

        SoftJointLimit limit = new SoftJointLimit();
        limit.limit = 0.5f; // cooler stays half-meter behind
        joint.linearLimit = limit;

        joint.angularXMotion = ConfigurableJointMotion.Free;
        joint.angularYMotion = ConfigurableJointMotion.Free;
        joint.angularZMotion = ConfigurableJointMotion.Free;
    }

    void Release()
    {
        isAttached = false;
        Destroy(joint);
    }
}

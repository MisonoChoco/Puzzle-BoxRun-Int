using System.Collections;
using UnityEngine;

public class BridgeTrigger : MonoBehaviour
{
    [SerializeField] private Transform bridge;
    private BridgeMove bridgeScript;

    private void Start()
    {
        bridgeScript = bridge.GetComponent<BridgeMove>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(CheckTrigger(collision.transform));
    }

    private IEnumerator CheckTrigger(Transform obj)
    {
        yield return new WaitForSeconds(0.15f);

        float dx = Mathf.Abs(transform.position.x - obj.position.x);
        float dz = Mathf.Abs(transform.position.z - obj.position.z);

        if (dx < 0.4f && dz < 0.4f)
            bridgeScript.direction *= -1;
    }
}
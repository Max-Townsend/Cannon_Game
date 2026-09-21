using System.Collections;
using UnityEngine;

public class cannonBall : MonoBehaviour
{
    public GameObject can;
    public GameObject target;
    public GameObject tartohit;

    private Vector3 stopPosition;
    private bool reachedTarget = false;
    public bool fired = false;
    private float inertia = 0.9999f;
    private bool canMove = true;

    private void Update()
    {
        if (Vector3.Distance(transform.position, target.transform.position) <= 0.1f)
        {
            if (!reachedTarget)
            {
                stopPosition = transform.position;
                reachedTarget = true;
                StartCoroutine(ShowFeedback());
            }
        }
        else if (fired && canMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, inertia * 15f + can.GetComponent<canonRotation>().backMove);
            inertia *= 0.99f;
            inertia = Mathf.Max(inertia, 0.3f);
        }
        if (reachedTarget)
        {
            transform.position = stopPosition;
        }
    }

    private IEnumerator ShowFeedback()
    {
        canMove = false;
        GetComponent<Rigidbody>().isKinematic = false;
        yield return new WaitForSeconds(0.02f);
        GetComponent<Rigidbody>().isKinematic = true;
        tartohit.GetComponent<Rigidbody>().detectCollisions = true;
        yield return new WaitForSeconds(0.45f);
        transform.position = can.transform.position;
        tartohit.GetComponent<Renderer>().enabled = true;
        fired = false;
        inertia = 0.9999f;
        canMove = true;
        reachedTarget = false;
        yield return new WaitForSeconds(0.034f);
        transform.position = can.transform.position;
        yield return null;
    }
}

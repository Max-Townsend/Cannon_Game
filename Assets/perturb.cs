using UnityEngine;

public class perturb : MonoBehaviour
{
    public GameObject can;
    public float currP = 0f;
    private Vector3 canPos;

    private void Start()
    {
        canPos = can.transform.position;
    }

    public void rotate()
    {
        transform.RotateAround(canPos, Vector3.up, currP);
    }
}

using UnityEngine;

public class explode : MonoBehaviour
{
    public AudioSource tarSplo;
    public GameObject explosion;
    public GameObject bTrial;
    public blockTrial bt;
    private float sploT = 0f;

    private void Start()
    {
        bt = bTrial.GetComponent<blockTrial>();
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "cballtag")
        {
            if (!explosion.GetComponent<ParticleSystem>().isPlaying || Time.time - sploT > 0.5)
            {
                sploT = Time.time;
                explosion.GetComponent<ParticleSystem>().Play();
                GetComponent<Renderer>().enabled = false;
                GetComponent<Rigidbody>().detectCollisions = false;
                bt.tarHit = "1";
                bt.blockHits += 1;
                tarSplo.Stop();
                tarSplo.Play();
            }
        }
    }
}

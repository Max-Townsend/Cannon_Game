using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class canonRotation : MonoBehaviour
{
    private const int RotationSpeed = 125;
    private const int RotationStep = 1;
    private const float HoldDelay = 0.15f;
    private const float Retention = 0.99854831f;
    private const float LearningRate = 0.03848635f;
    private const int GeneralizationWidth = 30;

    public GameObject postPrac;
    public GameObject pracGuide;
    public GameObject instrCanv;
    public GameObject instrPlane;
    public GameObject pauseCube;
    public GameObject gameCube;
    public AudioSource canAudio;
    public GameObject tarToHit;
    public GameObject tarTest;
    public GameObject endPoint;
    public perturb ep;
    public GameObject cBall;
    public GameObject canSplo;
    public float backMove = 20f;
    public float firedTime = -1.5f;
    public GameObject bTrial;
    public blockTrial bt;
    public GameObject eCont;
    public string keyPresses = "";
    public string timeStamps = "";
    public string aimAngles = "";
    public bool paused = false;
    public bool instrSeen = false;
    public TextMeshProUGUI blockScore;

    private Vector3 startingLocation;
    private Quaternion startingRotation;
    private float aPressTime;
    private float dPressTime;
    private bool fired;
    private float ballRecoilScale = 10f;
    private Vector3 startTarPos;
    private Vector3 canSploPos;
    private Vector3 endPointPosition;
    private Vector3 aimPosition;
    private Quaternion aimRotation;
    private bool trialPositioned;
    private float cannonAngle;
    private float previousCannonAngle;
    private bool canRotate = true;
    private bool trialEnded;
    private float aimAngle;
    private Quaternion startingAimRotation;
    private bool showingBlockFeedback;
    private Vector3 startingTargetPosition;
    private List<float> adaptation = new List<float>();

    private void Start()
    {
        pracGuide.SetActive(false);
        postPrac.SetActive(false);
        for (int i = 0; i < 360; i++)
        {
            adaptation.Add(0f);
        }
        startingLocation = transform.position;
        startingRotation = transform.rotation;
        startTarPos = tarTest.transform.position;
        startingAimRotation = tarTest.transform.rotation;
        canSploPos = canSplo.transform.position;
        bt = bTrial.GetComponent<blockTrial>();
        ep = endPoint.GetComponent<perturb>();
        endPointPosition = endPoint.transform.position;
        instrCanv.SetActive(false);
        instrPlane.SetActive(false);
        gameCube.SetActive(true);
        pauseCube.SetActive(false);
        Cursor.visible = false;
        startingTargetPosition = tarToHit.transform.position;
    }

    private void Update()
    {
        if (!showingBlockFeedback && !bt.expComplete)
        {
            if (!Application.isEditor && !Screen.fullScreen)
            {
                paused = true;
                pauseCube.SetActive(true);
                instrCanv.SetActive(false);
                instrPlane.SetActive(false);
                pracGuide.SetActive(false);
                postPrac.SetActive(false);
            }
            else if (!instrSeen)
            {
                paused = false;
                pauseCube.SetActive(false);
                instrCanv.SetActive(true);
                instrPlane.SetActive(true);
                if (Input.anyKey && Time.time > 10f)
                {
                    instrSeen = true;
                }
            }
            else if (blockScore.enabled)
            {
                StartCoroutine(ShowBlockFeedback());
                if (bt.trialNum == 0)
                {
                    adaptation = ScaleValues(adaptation, 0f);
                }
            }
            else
            {
                pracGuide.SetActive(false);
                pauseCube.SetActive(false);
                instrPlane.SetActive(false);
                postPrac.SetActive(false);
                if (bt.trialNum < -15)
                {
                    pracGuide.SetActive(true);
                }
                else if (bt.trialNum < -14)
                {
                    postPrac.SetActive(true);
                }
                instrCanv.SetActive(false);
                Cursor.visible = false;
                paused = false;
                if (!trialPositioned)
                {
                    bt.updateVisFB();
                    ep.rotate();
                    trialPositioned = true;
                    tarToHit.transform.position = startingTargetPosition;
                    tarToHit.transform.RotateAround(transform.position, Vector3.up, bt.tarPos);
                }

                if (Input.GetKeyDown(KeyCode.Space) && Time.time - firedTime > 1.5f)
                {
                    if (!fired && Vector3.Distance(transform.position, cBall.transform.position) <= 10)
                    {
                        if (!canSplo.GetComponent<ParticleSystem>().isPlaying)
                        {
                            cannonAngle = transform.localEulerAngles.y;
                            cannonAngle = (cannonAngle > 180f) ? cannonAngle - 360f : cannonAngle;
                            ApplyModel();
                            canSplo.GetComponent<ParticleSystem>().Play();
                            aimPosition = tarTest.transform.position;
                            aimRotation = tarTest.transform.rotation;
                            canAudio.Stop();
                            canAudio.Play();
                        }
                    }
                    trialEnded = false;
                    fired = true;
                    firedTime = Time.time;
                    cBall.GetComponent<cannonBall>().fired = true;
                    endPointPosition = endPoint.transform.position;
                    keyPresses += "S,";
                    if (bt.currP == 0)
                    {
                        ballRecoilScale = 0;
                    }
                    else
                    {
                        ballRecoilScale = 0.5f;
                    }
                }
                if (!fired)
                {
                    if (canRotate)
                    {
                        RotateCannon();
                    }
                    float unsignedAimAngle = Quaternion.Angle(transform.rotation, bTrial.transform.rotation);
                    float referenceAngle = Quaternion.Angle(transform.rotation, eCont.transform.rotation);
                    if (referenceAngle < 90)
                    {
                        aimAngle = 360 - unsignedAimAngle;
                    }
                    else
                    {
                        aimAngle = unsignedAimAngle;
                    }
                    aimAngles += aimAngle.ToString() + ",";
                }
            }
            if (Vector3.Distance(endPoint.transform.position, cBall.transform.position) <= 0.1 && !trialEnded && fired)
            {
                StartCoroutine(EndTrial());
            }
            else if (fired)
            {
                float recoilDirection = ep.currP < 0 ? 1f : -1f;
                float recoilAngle = recoilDirection * (Quaternion.Angle(transform.rotation, endPoint.transform.rotation)) * backMove / 100;
                transform.RotateAround(transform.position, Vector3.up, recoilAngle);
                transform.position = Vector3.MoveTowards(transform.position, tarTest.transform.position, -backMove);
                endPoint.transform.position = endPointPosition;
                canSplo.transform.position = Vector3.MoveTowards(canSplo.transform.position, transform.position, -backMove);
                cBall.transform.position = Vector3.MoveTowards(cBall.transform.position, tarTest.transform.position, backMove * ballRecoilScale);
                ballRecoilScale *= 0.875f;
                backMove *= 0.7f;
                tarTest.transform.position = aimPosition;
                tarTest.transform.rotation = aimRotation;
            }
            timeStamps += Time.time.ToString() + ",";
        }
    }

    private IEnumerator ShowBlockFeedback()
    {
        showingBlockFeedback = true;
        pracGuide.SetActive(false);
        yield return new WaitForSeconds(5f);
        blockScore.enabled = false;
        showingBlockFeedback = false;
    }

    private void RotateCannon()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            aPressTime = Time.time;
            transform.RotateAround(startingLocation, -Vector3.up, RotationStep);
            keyPresses += "A,";
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            dPressTime = Time.time;
            transform.RotateAround(startingLocation, Vector3.up, RotationStep);
            keyPresses += "D,";
        }

        if (Input.GetKey(KeyCode.A))
        {
            if (Time.time - aPressTime > HoldDelay)
            {
                transform.RotateAround(startingLocation, -Vector3.up, (int)(RotationSpeed * Time.deltaTime));
                keyPresses += "Ahold,";
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (Time.time - dPressTime > HoldDelay)
            {
                transform.RotateAround(startingLocation, Vector3.up, (int)(RotationSpeed * Time.deltaTime));
                keyPresses += "Dhold,";
            }
        }
    }

    private IEnumerator EndTrial()
    {
        // Save the model state before this trial updates it.
        bt.valArray = string.Join(",", adaptation);
        UpdateModel();
        trialEnded = true;
        fired = false;
        canRotate = false;
        if (keyPresses.Length >= 1)
        {
            bt.keyPresses = keyPresses;
            bt.timeStamps = timeStamps;
            bt.aimAngles = aimAngles;
            bt.finAim = aimAngle;
            timeStamps = "";
            aimAngles = "";
            keyPresses = "";
        }
        yield return new WaitForSeconds(0.05f);
        bt.pushPerturb();
        yield return new WaitForSeconds(0.445f);
        ResetCannon();
        yield return null;
    }

    private void ResetCannon()
    {
        trialPositioned = false;
        canRotate = true;
        transform.rotation = startingRotation;
        transform.position = startingLocation;
        tarTest.transform.position = startTarPos;
        tarTest.transform.rotation = startingAimRotation;
        endPoint.transform.rotation = startingAimRotation;
        endPoint.transform.position = startTarPos;
        canSplo.transform.position = canSploPos;
        float targetAngle = (bt.tarPos > 180f) ? bt.tarPos - 360f : bt.tarPos;
        int startingAngleOffset = 0;
        // The first perturbed trial uses the baseline starting-angle selection.
        if (!bt.firstPerturb)
        {
            cannonAngle = targetAngle - ep.currP;
        }
        while (Mathf.Abs(targetAngle - (cannonAngle + ep.currP)) < 5f || Mathf.Abs(previousCannonAngle - cannonAngle) < 10f)
        {
            float randomOffset = ((0.5f - Random.value) * 200f) + ep.currP;
            startingAngleOffset = (int)randomOffset;
            transform.rotation *= Quaternion.Euler(Vector3.up * (targetAngle - startingAngleOffset));
            cannonAngle = transform.localEulerAngles.y;
            cannonAngle = (cannonAngle > 180f) ? cannonAngle - 360f : cannonAngle;
            transform.rotation *= Quaternion.Euler(-Vector3.up * (targetAngle - startingAngleOffset));
        }
        transform.rotation *= Quaternion.Euler(Vector3.up * (targetAngle - startingAngleOffset));
        previousCannonAngle = cannonAngle;
        backMove = 20f;
    }

    private void ApplyModel()
    {
        int aimIndex = ((int)aimAngle == 360) ? 0 : (int)aimAngle;
        float modelOutput = adaptation[aimIndex];
        endPoint.transform.RotateAround(startingLocation, Vector3.up, modelOutput);
    }

    private void UpdateModel()
    {
        float feedbackAngle = endPoint.transform.localEulerAngles.y;
        feedbackAngle = (feedbackAngle > 180f) ? feedbackAngle - 360f : feedbackAngle;
        List<float> generalization = CreateGeneralization();
        // Centre the circular generalisation kernel on the current aim angle.
        int shift = ((int)aimAngle <= 180) ? 180 - (int)aimAngle : 540 - (int)aimAngle;
        for (int a = 0; a < shift; a++)
        {
            float first = generalization[0];
            generalization.RemoveAt(0);
            generalization.Add(first);
        }
        adaptation = AddValues(ScaleValues(adaptation, Retention), ScaleValues(generalization, LearningRate * -feedbackAngle));
    }

    private List<float> ScaleValues(List<float> values, float scale)
    {
        for (int a = 0; a < values.Count; a++)
        {
            values[a] = values[a] * scale;
        }
        return values;
    }

    private List<float> AddValues(List<float> values, List<float> other)
    {
        for (int a = 0; a < values.Count; a++)
        {
            values[a] = values[a] + other[a];
        }
        return values;
    }

    private List<float> CreateGeneralization()
    {
        List<float> generalization = new List<float>();
        for (int i = -180; i < 180; i++)
        {
            generalization.Add(Gaussian(Mathf.Abs((float)i)));
        }
        return generalization;
    }

    private float Gaussian(float x)
    {
        return Mathf.Exp(-(x * x) / (2 * GeneralizationWidth * GeneralizationWidth));
    }
}

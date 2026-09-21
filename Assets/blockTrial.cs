using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class blockTrial : MonoBehaviour
{
    private const int BlockSize = 100;
    private const int BaselineLength = 40;
    private const float NoiseMean = 0f;
    private const float NoiseStandardDeviation = 3f;

    public float tarPos = 0f;
    public float currP;
    public int trialNum;
    public int blockHits = 0;
    public int pIndex = 0;
    public GameObject endPoint;
    public perturb ep;
    public GameObject expCont;
    public ExperimentController cont;
    public float finAim;
    public TextMeshProUGUI blockScore;
    public GameObject missText;
    public GameObject hitText;
    public string keyPresses;
    public string timeStamps;
    public string aimAngles;
    public string tarHit = "0";
    public string endPointFB;
    public string valArray;
    public bool expComplete = false;
    public bool firstPerturb = false;

    private List<int> remainingTargets = new List<int> { 0, 45, 90, 135, 180, 225, 270, 315 };
    private float visualFeedback;

    private void Start()
    {
        ep = endPoint.GetComponent<perturb>();
        cont = expCont.GetComponent<ExperimentController>();
        tarHit = "0";
        missText.SetActive(false);
        hitText.SetActive(false);
        blockScore.enabled = false;
        blockScore.text = "Congratulations, you hit " + blockHits.ToString() + " trials out of 40 in that block! \n\n Get ready for the next block of trials.";
    }

    public void updateVisFB()
    {
        // Box-Muller noise is added to the angular feedback.
        float u1 = 1.0f - Random.value;
        float u2 = 1.0f - Random.value;
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) *
                     Mathf.Sin(2.0f * Mathf.PI * u2);
        float randNormal = NoiseMean + NoiseStandardDeviation * randStdNormal;
        visualFeedback = currP + randNormal;
        ep.currP = visualFeedback;
        endPointFB = (visualFeedback + finAim).ToString();
    }

    public float pushPerturb()
    {
        if (tarHit == "0")
        {
            StartCoroutine(ShowMiss());
        }
        else
        {
            StartCoroutine(ShowHit());
        }
        // Select the perturbation before advancing to the next trial.
        if (trialNum < BaselineLength || trialNum > 400 - BaselineLength)
        {
            currP = 0f;
        }
        else
        {
            firstPerturb = trialNum == BaselineLength;
            currP = -15f;
        }
        updateVisFB();
        SaveTrial();
        NextTrial();
        return currP;
    }

    private IEnumerator ShowMiss()
    {
        missText.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        missText.SetActive(false);
    }

    private IEnumerator ShowHit()
    {
        hitText.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        hitText.SetActive(false);
    }

    private void SaveTrial()
    {
        cont.putData(
            trialNum.ToString(),
            keyPresses,
            timeStamps,
            aimAngles,
            tarHit,
            endPointFB,
            currP.ToString(),
            valArray,
            tarPos.ToString());
    }

    private void StartBlock()
    {
        blockScore.text = "Congratulations, you hit " + blockHits.ToString() + " trials out of " + trialNum.ToString() + " so far! \n\n Press any key to continue.";
        blockScore.enabled = true;

        pIndex += 1;
        if (pIndex >= 4 || trialNum == 400)
        {
            expComplete = true;
            Cursor.visible = true;
            blockScore.text = "Congratulations, you hit " + blockHits.ToString() + " trials out of 400 trials! \n\n You have now completed the experiment \n\n You may now leave this page.";
        }
    }

    private void NextTrial()
    {
        tarHit = "0";
        trialNum += 1;
        if (trialNum % 8 == 0)
        {
            remainingTargets = new List<int> { 0, 45, 90, 135, 180, 225, 270, 315 };
        }
        if (trialNum >= 0)
        {
            int targetIndex = Random.Range(0, remainingTargets.Count);
            tarPos = remainingTargets[targetIndex];
            remainingTargets.RemoveAt(targetIndex);
        }

        if (trialNum == 0)
        {
            blockScore.text = "Congratulations, you hit " + blockHits.ToString() + " trials out of 15 in the training block! \n\n Get ready for the experimental trials.";
            blockScore.enabled = true;
            blockHits = 0;
        }
        else if (trialNum == -15)
        {
            blockScore.text = "Congratulations, you hit " + blockHits.ToString() + " trials out of 5 in the practice block! \n\n Get ready for the training block.";
            blockScore.enabled = true;
            blockHits = 0;
        }
        else if (trialNum % BlockSize == 0)
        {
            StartBlock();
        }
    }
}

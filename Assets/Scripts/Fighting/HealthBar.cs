using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    readonly float chaseSpeed = 4f;
    [SerializeField]
    private Image front;
    [SerializeField]
    private Image back;
    [SerializeField]
    private float lerpTimer;

    public void UpdateHealth()
    {
        float fillF = front.fillAmount;
        float fillB = back.fillAmount;
        float fraction = player.GetComponent<PlayerManager>().health / 200;
        if (fillB > fraction)
        {
            front.fillAmount = fraction;
            back.color = Color.red; //for now
            lerpTimer += Time.deltaTime;
            float percent = lerpTimer / chaseSpeed;
            back.fillAmount = Mathf.Lerp(fillB, fraction, percent);
        }
        if (fillF < fraction)
        {
            back.fillAmount = fraction;
            back.color = Color.green; //for now
            lerpTimer += Time.deltaTime;
            float percent = lerpTimer / chaseSpeed;
            front.fillAmount = Mathf.Lerp(fillF, fraction, percent);
        }
        if (player.GetComponent<PlayerManager>().hurt == true)
        {
            lerpTimer = 0;
        }
    }


    // Update is called once per frame
    void Update()
    {
        UpdateHealth();
    }
}

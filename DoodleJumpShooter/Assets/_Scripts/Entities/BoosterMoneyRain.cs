using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterMoneyRain : Boosters
{
    [SerializeField] int moneyCount = 15;
    [SerializeField] float spawnIntervall = 0.15f;
    [SerializeField] AudioClip generateSound;

    public override void OnActivate()
    {
        base.OnActivate();
        StartCoroutine(Rain());
    }

    IEnumerator Rain() {
        for (int i = 1; i <= moneyCount; i++) {
            GameManager.Instance.PlaySound(generateSound);
            var newMoney = Instantiate(money, transform.position, Quaternion.identity);

            yield return new WaitForSeconds(spawnIntervall);
        } 
    }
}

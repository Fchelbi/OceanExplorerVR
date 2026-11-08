using System.Collections.Generic;
using UnityEngine;

public class RescueTool : MonoBehaviour
{
    [Header("Références")]
    public GameObject buoyPrefab;
    public Transform throwPoint;
    public Transform seat;

    [Header("Réglages")]
    public int capacity = 1;
    public float throwRange = 20f;
    public float throwSpeed = 15f;
    public float pullRange = 12f;

    public List<Victim> onBoard = new List<Victim>();
    RescueBuoy currentBuoy;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) ThrowBuoy();
        if (Input.GetKeyDown(KeyCode.E)) PullVictim();
    }

    void ThrowBuoy()
    {
        if (currentBuoy != null) { GameManager.Instance.ShowMessage("Une bouée est déjà à l'eau !"); return; }
        if (onBoard.Count >= capacity) { GameManager.Instance.ShowMessage("Véhicule plein ! Retourne à la plage."); return; }

        Vector3 dir = transform.forward;
        Victim target = FindNearestVictim();
        if (target != null)
            dir = (target.transform.position - throwPoint.position).normalized;

        GameObject buoy = Instantiate(buoyPrefab, throwPoint.position, Quaternion.identity);
        buoy.GetComponent<Rigidbody>().linearVelocity = dir * throwSpeed;
        currentBuoy = buoy.GetComponent<RescueBuoy>();
    }

    Victim FindNearestVictim()
    {
        GameObject[] all = GameObject.FindGameObjectsWithTag("Victim");
        Victim best = null;
        float bestDist = throwRange;

        foreach (GameObject g in all)
        {
            Victim v = g.GetComponent<Victim>();
            if (v == null || v.state != VictimState.Drowning) continue;

            float d = Vector3.Distance(throwPoint.position, g.transform.position);
            if (d < bestDist) { bestDist = d; best = v; }
        }
        return best;
    }

    void PullVictim()
    {
        if (currentBuoy == null || currentBuoy.victim == null)
        {
            GameManager.Instance.ShowMessage("Lance d'abord la bouée sur une victime (Espace).");
            return;
        }

        Victim v = currentBuoy.victim;
        if (v.state != VictimState.OnBuoy) return;

        if (Vector3.Distance(transform.position, v.transform.position) > pullRange)
        {
            GameManager.Instance.ShowMessage("Trop loin ! Rapproche-toi de la victime.");
            return;
        }
        v.StartPull(this);
    }

    public void Board(Victim v)
    {
        onBoard.Add(v);
        if (currentBuoy != null) Destroy(currentBuoy.gameObject);

        v.transform.SetParent(seat);
        v.transform.localPosition = new Vector3(0f, 0f, -(onBoard.Count - 1) * 0.5f);

        GameManager.Instance.ShowMessage("Victime à bord (" + onBoard.Count + "/" + capacity + ") ! Retourne à la plage.");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BeachZone") && onBoard.Count > 0)
        {
            foreach (Victim v in onBoard)
            {
                Vector3 landPos = GameManager.Instance.beachDropPoint.position
                                + new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-1f, 1f));
                v.Disembark(landPos, GameManager.Instance.medicalPoint.position);
            }
            GameManager.Instance.OnVictimsSaved(onBoard.Count);
            onBoard.Clear();
        }
    }
}
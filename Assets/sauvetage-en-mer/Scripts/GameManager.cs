using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Scénarios")]
    public GameObject victimPrefab;
    public Transform[] nearSpawns;
    public Transform[] farSpawns;
    public GameObject bigShip;
    public GameObject nearMarker;
    public GameObject farMarker;

    [Header("Véhicules")]
    public RescueTool boat;
    public RescueTool jetski;
    public CameraFollow cameraFollow;

    [Header("Plage")]
    public Transform beachDropPoint;
    public Transform medicalPoint;

    [Header("UI")]
    public GameObject choicePanel;
    public GameObject endPanel;
    public TMP_Text alertText;
    public TMP_Text infoText;
    public TMP_Text messageText;
    public TMP_Text endText;

    int scenario;
    int totalVictims;
    int savedVictims;
    float timer;
    float penalty;
    float messageTimer;
    bool playing;
    RescueTool activeTool;
    Transform currentTarget;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SetVehicleActive(boat, false);
        SetVehicleActive(jetski, false);
        endPanel.SetActive(false);
        messageText.text = "";

        scenario = Random.Range(1, 3);   // 1 ou 2
        SpawnScenario();
    }

    void SpawnScenario()
    {
        Transform[] spawns = (scenario == 1) ? nearSpawns : farSpawns;

        foreach (Transform s in spawns)
            Instantiate(victimPrefab, s.position, Quaternion.identity);
        totalVictims = spawns.Length;

        bigShip.SetActive(scenario == 2);
        nearMarker.SetActive(scenario == 1);
        farMarker.SetActive(scenario == 2);
        currentTarget = (scenario == 1) ? nearMarker.transform : farMarker.transform;

        alertText.text = (scenario == 1)
            ? "ALERTE : une personne se noie près de la plage !"
            : "ALERTE : un grand navire est en détresse au large ! Plusieurs personnes sont à l'eau !";
    }

    public void ChooseVehicle(int choice)
    {
        if (playing) return;

        if (choice != scenario)
        {
            penalty += 10f;
            ShowMessage("Mauvais choix ! Près de la plage : bateau. Au large : jet-ski. (+10 s)");
            return;
        }

        choicePanel.SetActive(false);
        activeTool = (choice == 1) ? boat : jetski;
        SetVehicleActive(activeTool, true);
        cameraFollow.target = activeTool.transform;
        playing = true;
        ShowMessage("Bon choix ! Suis le marqueur jaune.");
    }

    void SetVehicleActive(RescueTool tool, bool value)
    {
        tool.enabled = value;
        tool.GetComponent<VehicleController>().enabled = value;
    }

    void Update()
    {
        if (playing)
        {
            timer += Time.deltaTime;

            Transform target = (activeTool.onBoard.Count > 0) ? beachDropPoint : currentTarget;
            float dist = Vector3.Distance(activeTool.transform.position, target.position);

            infoText.text = "Sauvés : " + savedVictims + "/" + totalVictims
                          + "\nTemps : " + timer.ToString("0") + " s"
                          + "\nDistance : " + dist.ToString("0") + " m";
        }

        if (messageTimer > 0f)
        {
            messageTimer -= Time.deltaTime;
            if (messageTimer <= 0f) messageText.text = "";
        }
    }

    public void ShowMessage(string msg)
    {
        messageText.text = msg;
        messageTimer = 4f;
    }

    public void OnVictimsSaved(int count)
    {
        savedVictims += count;
        if (savedVictims >= totalVictims) EndMission();
    }

    void EndMission()
    {
        playing = false;
        endPanel.SetActive(true);
        endText.text = "Mission réussie !\nTemps : " + timer.ToString("0") + " s"
                     + "\nPénalités : " + penalty.ToString("0") + " s"
                     + "\nScore total : " + (timer + penalty).ToString("0") + " s";
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
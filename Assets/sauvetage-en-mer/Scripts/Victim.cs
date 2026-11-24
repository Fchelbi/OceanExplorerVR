using UnityEngine;
using UnityEngine.AI;

public enum VictimState { Drowning, OnBuoy, Pulled, Climbing, OnBoard, Safe }

public class Victim : MonoBehaviour
{
    public VictimState state = VictimState.Drowning;
    public float bobHeight = 0.1f;
    public float bobSpeed = 2f;
    public float pullSpeed = 5f;

    [Header("Montée à bord")]
    public Animator climbAnimator;          // l'Animator de l'objet Visual
    public string climbStateName = "Climb"; // nom exact du clip
    public float climbDuration = 1.5f;      // durée du clip en secondes
    public float sidePadding = 0.6f;        // distance entre la coque et la victime
    public float hopHeight = 0.5f;          // hauteur du saut au-dessus du pont
    [Range(0f, 0.8f)]
    public float enterDelay = 0.35f;        // part de l'animation passée à se hisser avant d'avancer vers le siège

    [Header("Hauteur du personnage")]
    public Transform model;                 // Man relax (trouvé automatiquement si vide)
    public float standY = 0f;               // Y local debout (pieds sur le point d'ancrage)
    public float standRotY = 0f;            // rotation Y locale du modèle une fois à bord (0 = face à l'avant)

    float waterY;                           // Y local du modèle dans l'eau
    Quaternion waterRot;                    // rotation locale du modèle dans l'eau (180)

    float baseY;
    float phase;
    float sideSign = 1f;
    float sideDistance = 1.5f;
    float climbTimer;
    Vector3 climbStartPos;
    Quaternion climbStartRot;

    RescueBuoy buoy;
    RescueTool tool;
    NavMeshAgent agent;
    Collider col;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        col = GetComponent<Collider>();
        phase = Random.Range(0f, 6.28f);

        if (climbAnimator != null)
        {
            climbAnimator.transform.localPosition = Vector3.zero;
            climbAnimator.transform.localRotation = Quaternion.identity;
            climbAnimator.enabled = false;   // il ne joue que pendant la montée

            if (model == null && climbAnimator.transform.childCount > 0)
                model = climbAnimator.transform.GetChild(0);
        }

        if (model != null)
        {
            waterY = model.localPosition.y;
            waterRot = model.localRotation;  // mémorise le 180° du prefab
        }
    }

    void Start()
    {
        baseY = transform.position.y;
    }

    void Update()
    {
        switch (state)
        {
            case VictimState.Drowning:
            case VictimState.OnBuoy:
                Bob();
                break;
            case VictimState.Pulled:
                UpdatePulled();
                break;
            case VictimState.Climbing:
                UpdateClimbing();
                break;
        }

        if (buoy != null && (state == VictimState.OnBuoy || state == VictimState.Pulled))
            buoy.transform.position = transform.position;
    }

    void Bob()
    {
        Vector3 p = transform.position;
        p.y = baseY + Mathf.Sin(Time.time * bobSpeed + phase) * bobHeight;
        transform.position = p;
    }

    // Emplacement final de cette victime sur le véhicule
    Vector3 SeatSlot()
    {
        return tool.seat.TransformPoint(new Vector3(0f, 0f, -tool.onBoard.Count * 0.5f));
    }

    void SetModelY(float y)
    {
        if (model == null) return;
        Vector3 p = model.localPosition;
        p.y = y;
        model.localPosition = p;
    }

    // Phase 1 : la victime est tirée jusqu'au côté du véhicule
    void UpdatePulled()
    {
        Vector3 target = SeatSlot() + tool.transform.right * sideSign * sideDistance;
        target.y = baseY;

        transform.position = Vector3.MoveTowards(transform.position, target, pullSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.1f)
            StartClimb();
    }

    // Phase 2 : animation enregistrée + déplacement jusqu'au siège
    void StartClimb()
    {
        state = VictimState.Climbing;
        climbTimer = 0f;
        climbStartPos = transform.position;
        climbStartRot = transform.rotation;

        if (climbAnimator != null)
        {
            climbAnimator.enabled = true;
            climbAnimator.Play(climbStateName, 0, 0f);
        }
    }

    void UpdateClimbing()
    {
        climbTimer += Time.deltaTime;
        float t = Mathf.Clamp01(climbTimer / climbDuration);
        Vector3 end = SeatSlot();

        // 1) il se hisse d'abord, 2) il avance ensuite au-dessus du pont
        float up = Mathf.SmoothStep(0f, 1f, t);
        float forward = Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(enterDelay, 1f, t));
        float rise = Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(0f, 0.6f, t));

        Vector3 pos;
        pos.x = Mathf.Lerp(climbStartPos.x, end.x, forward);
        pos.z = Mathf.Lerp(climbStartPos.z, end.z, forward);
        pos.y = Mathf.Lerp(climbStartPos.y, end.y, up) + Mathf.Sin(t * Mathf.PI) * hopHeight;
        transform.position = pos;

        transform.rotation = Quaternion.Slerp(climbStartRot, tool.transform.rotation, up);

        // les pieds sortent de l'eau avant le passage au-dessus du bord
        SetModelY(Mathf.Lerp(waterY, standY, rise));

        // et il se tourne vers l'avant pendant le saut
        if (model != null)
            model.localRotation = Quaternion.Slerp(waterRot, Quaternion.Euler(0f, standRotY, 0f), up);

        if (t >= 1f) EndClimb();
    }

    void EndClimb()
    {
        if (climbAnimator != null)
        {
            climbAnimator.enabled = false;
            climbAnimator.transform.localPosition = Vector3.zero;
            climbAnimator.transform.localRotation = Quaternion.identity;
        }

        SetModelY(standY);
        if (model != null)
            model.localRotation = Quaternion.Euler(0f, standRotY, 0f);

        state = VictimState.OnBoard;
        tool.Board(this);
    }

    public void GrabBuoy(RescueBuoy b)
    {
        buoy = b;
        state = VictimState.OnBuoy;
    }

    public void StartPull(RescueTool t)
    {
        tool = t;
        col.enabled = false;

        // côté du véhicule où se trouve la victime
        float side = Vector3.Dot(transform.position - t.transform.position, t.transform.right);
        sideSign = (side >= 0f) ? 1f : -1f;

        // distance entre l'axe du véhicule et le bord réel du collider, du côté où il monte
        BoxCollider bc = t.GetComponent<BoxCollider>();
        float half = 0.5f;
        if (bc != null)
        {
            float edge = sideSign * bc.center.x + bc.size.x * 0.5f;
            half = Mathf.Max(0.3f, edge * Mathf.Abs(t.transform.lossyScale.x));
        }
        sideDistance = half + sidePadding;

        state = VictimState.Pulled;
    }

    public void Disembark(Vector3 landPos, Vector3 destination)
    {
        state = VictimState.Safe;
        transform.SetParent(null);
        transform.position = landPos;
        col.enabled = true;

        SetModelY(standY);
        if (model != null)
            model.localRotation = Quaternion.Euler(0f, standRotY, 0f);

        agent.baseOffset = 0f;   // pieds posés sur le sable
        agent.enabled = true;
        agent.Warp(landPos);
        agent.SetDestination(destination);

        Animation walk = GetComponentInChildren<Animation>();
        if (walk != null) walk.Play();

        Destroy(gameObject, 10f);
    }
}
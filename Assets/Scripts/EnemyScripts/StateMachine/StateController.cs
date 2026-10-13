using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class StateController : MonoBehaviour
{
    public IState CurrentState {  get; private set; }
    public Attack CurrentAttack;

    public Punch biteAttack;
    public Shoot shootAttack;
    public Dash dashAttack;

    public Transform Player { get; private set; }
    
    public Vector2 EnemyToPlayer { get; private set; }
    public float DistanceToPlayer { get; private set; }

    public Animator Animator { get; private set; }

    public bool locked = false; // Locks the state

    public Light2D AttackIndicator;

    private Vector3 ZEROVEC = Vector3.zero;

    // DEBUG
    [SerializeField] private TextMeshProUGUI debugText;

    private void Awake()
    {
        Animator = GetComponent<Animator>();

        biteAttack = new Punch(gameObject,
            damage: new Damage(10, Damage.Type.PHYSICAL), 
            cooldown: 2f,
            travelSpeed:0,
            knockbackStrength:6 // i guess this is the min b4 it bugs out
        );
        shootAttack = new Shoot(gameObject,
            damage: new Damage(10, Damage.Type.PHYSICAL),
            cooldown: 1f,
            travelSpeed: 10,
            lifetime: 2,
            piercing: false
        );
        dashAttack = new Dash(gameObject,
            damage: new Damage(10, Damage.Type.PHYSICAL),
            cooldown: 1f,
            travelSpeed: 0.25f,
            lifetime: 1f
        );
    }

    private void Start()
    {
        Player = FindFirstObjectByType<PlayerController>()?.transform;
        AttackIndicator = transform.Find("AttackIndicator").GetComponent<Light2D>();
        //Player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void ChangeState(IState newState)
    {
        CurrentState?.OnExit(this);
        CurrentState = newState;
        CurrentState.OnEntry(this);
        debugText.text = CurrentState.GetName();

        // uncomment this for baby mode
        // if (biteAttack == null || shootAttack == null || dashAttack == null)
        // {
        //     return;
        // }
        // biteAttack.ResetCD();
        // shootAttack.ResetCD();
        // dashAttack.ResetCD();
    }

    private void Update()
    {
        if (locked)
        {
            return;
        }
        if (Player == null || CurrentState == null) return;

        EnemyToPlayer = Player.position - transform.position;
        DistanceToPlayer = EnemyToPlayer.magnitude;

        CurrentState.OnUpdate(this);
        //RotateToPlayer();
    }

    public void AttackPlayer(Vector3 origin = default, Vector3 target = default)
    {
        if (CurrentAttack is Shoot)
        {
            StartCoroutine(shootAttack.Execute(
                origin: transform.position, 
                target: EnemyToPlayer));
        }
        else if (CurrentAttack is Punch)
        {
            StartCoroutine(biteAttack.Execute(
                origin: transform.position, 
                target: new Vector3(70,170) // x is range, y is width
            )); 
        }
        else if (CurrentAttack is Dash)
        {
            StartCoroutine(dashAttack.Execute(
                origin: origin,
                target: target
            ));
        }
    }

    private float RotateSpeed = 5f;

    public void RotateToPlayer()
    {
        Vector2 direction = Player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, RotateSpeed * Time.deltaTime);
    }

    public void TryTriggerAnimation(string triggerName)
    {
        if (Animator == null) {return;}

        for (int i=0; i<Animator.parameterCount; i++)
        {
            if (Animator.parameters[i].name == triggerName)
            {
                Animator.SetTrigger(triggerName);
            }
        }
    }
}

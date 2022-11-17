using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : BaseController, IDamageSource, IDamageable
{
    protected EnemyBaseState _currentState;
    [SerializeField] protected bool _isStationary = false;

    protected Seeker _seeker;
    protected Path _path;
    protected AISensor _aiSensor;
    protected Dictionary<string, EnemyBaseState> _states;

    [SerializeField] protected float _repathRate;
    [SerializeField] protected float _patrolRate;
    [SerializeField] protected float _patrolRadius;
    protected Vector3 _spawnPosition;

    public EnemyBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public Seeker GetSeeker { get { return _seeker; } }
    public AISensor GetAISensor { get { return _aiSensor; } }
    public Path Path { get { return _path; } set { _path = value; } }
    public float RepathRate { get { return _repathRate; } }
    public float PatrolRate { get { return _patrolRate; } }
    public float PatrolRadius { get { return _patrolRadius; } }
    public bool IsStationary { get { return _isStationary; } }
    public Vector3 SpawnPosition { get { return _spawnPosition; } }

    protected override void Awake()
    {
        base.Awake();

        _seeker = GetComponent<Seeker>();
        _aiSensor = GetComponent<AISensor>();
    }

    private void OnEnable()
    {
        _healthPoints = _maxHealthPoints;
        _spawnPosition = transform.position;

        InitializeStates();
    }

    protected override void FixedUpdate()
    {
        _currentState?.OnUpdate();

        //_controller.Move(_appliedVelocity * Time.fixedDeltaTime);
    }

    public void OnHit(DamageInfo damageInfo)
    {
        _currentState?.OnHit(damageInfo);

        //Debug.Log($"{damageInfo.source} hit {transform.name} for {damageInfo.damage}, current health is {_healthPoints}.");
    }

    public int CalculateDamage()
    {
        return 5;
    }

    public EnemyBaseState GetState(string stateName)
    {
        return _states[stateName];
    }

    private void InitializeStates()
    {
        _states = new Dictionary<string, EnemyBaseState>();

        _states.Add("Idle", new EnemyIdleState(this));
        _states.Add("Patrol", new EnemyPatrolState(this));
        _states.Add("Follow", new EnemyFollowState(this));
        _states.Add("Hit", new EnemyHitState(this));
        _states.Add("Attack", new EnemyAttackState(this));

        _currentState = GetState("Idle");
        _currentState.OnEnter();
    }
}

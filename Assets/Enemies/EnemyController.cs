using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : BaseController, IDamageSource, IDamageable
{
    protected EnemyBaseState _currentState;
    [SerializeField] protected EnemyData _data;

    protected CharacterController _charController;
    protected SkinnedMeshRenderer _skinnedMesh;
    protected Seeker _seeker;
    protected Path _path;
    protected AISensor _aiSensor;
    protected Dictionary<string, EnemyBaseState> _states;

    protected Vector3 _spawnPosition;

    public EnemyBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public CharacterController GetCharacterController { get { return _charController; } }
    public Seeker GetSeeker { get { return _seeker; } }
    public AISensor GetAISensor { get { return _aiSensor; } }
    public Path Path { get { return _path; } set { _path = value; } }
    public float RepathRate { get { return _data.repathRate; } }
    public float PatrolRate { get { return _data.patrolRate; } }
    public float PatrolRadius { get { return _data.patrolRadius; } }
    public bool IsStationary { get { return _data.isStationary; } }
    public Vector3 SpawnPosition { get { return _spawnPosition; } }

    protected override void Awake()
    {
        base.Awake();

        _charController = GetComponent<CharacterController>();
        _seeker = GetComponent<Seeker>();
        _aiSensor = GetComponent<AISensor>();
        _skinnedMesh = GetComponentInChildren<SkinnedMeshRenderer>();

        _spawnPosition = transform.position;

        SetEnemy();
        InitializeStates();
    }

    private void OnEnable()
    {
        _healthPoints = _maxHealthPoints;
        transform.position = _spawnPosition;
    }

    public void OnDeath()
    {
        gameObject.SetActive(false);
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
        _states.Add("Death", new EnemyDeathState(this));

        _currentState = GetState("Idle");
        _currentState.OnEnter();
    }

    private void SetEnemy()
    {
        _maxHealthPoints = _data.maxHealth;
        _movementSpeed = _data.movementSpeed;
        _skinnedMesh.sharedMesh = _data.mesh;

        _aiSensor.SetSensor(_data.sensorType);
        _aiSensor.SetRadius(_data.searchRadius, _data.targetRadius);
        _aiSensor.SetUpdateRate(_data.repathRate);
    }
}

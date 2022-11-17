using UnityEditor;
using UnityEngine;

public class AISensor : MonoBehaviour
{
    public enum SensorType
    {
        NONE = 0,
        SPHERE = 1,
        BOX = 2,
        CONE = 3
    }

    private Transform _target;
    [SerializeField] private SensorType _sensorType = SensorType.NONE;
    [SerializeField] private float _searchRadius;
    [SerializeField] private float _followRadius;
    [SerializeField] private float _searchRate;
    [SerializeField] private float _followRate;
    [SerializeField] private int _angle;
    [SerializeField] private LayerMask _layer;

    private float _currentRadius;
    private float _currentSearchRate;
    private float _lastDetectionTime;

    public Transform GetTarget { get { return _target; } }
    public Transform SetTarget(Transform target) => _target = target;

    private void OnEnable()
    {
        _currentRadius = _searchRadius;
        _currentSearchRate = _searchRate;
    }

    private void FixedUpdate()
    {
        if (Time.time < _lastDetectionTime + _currentSearchRate)
            return;

        if (_sensorType != SensorType.NONE)
            Detect();
    }

    public void Detect()
    {
        _lastDetectionTime = Time.time;

        Collider[] colliders = new Collider[0];

        if (_sensorType == SensorType.BOX)
        {
            colliders = Physics.OverlapBox(transform.position, Vector3.one * _currentRadius, Quaternion.identity, _layer);
        }
        else if (_sensorType == SensorType.SPHERE || _sensorType == SensorType.CONE && _target)
        {
            colliders = Physics.OverlapSphere(transform.position, _currentRadius, _layer);
        }
        else if (_sensorType == SensorType.CONE)
        {
            Collider[] sphereCast = Physics.OverlapSphere(transform.position, _currentRadius, _layer);

            if (sphereCast.Length != 0)
            {
                Transform found = sphereCast[0].transform;
                Vector3 direction = (found.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, direction) < _angle / 2)
                    colliders = sphereCast;
            }
        }

        bool targetFound = colliders.Length > 0 ? true : false;

        _target = targetFound ? colliders[0].transform : null;
        _currentRadius = targetFound ? _followRadius : _searchRadius;
        _currentSearchRate = targetFound ? _followRate : _searchRate;
    }

    [SerializeField] private bool _showGizmos = false;
    [SerializeField] private Color _notFoundColor = Color.green;
    [SerializeField] private Color _detectedColor = Color.red;

    private void OnDrawGizmosSelected()
    {
        if (!_showGizmos)
            return;

        Handles.color = Gizmos.color = _target ? _detectedColor : _notFoundColor;

        if (_sensorType == SensorType.BOX)
        {
            Gizmos.DrawWireCube(transform.position, Vector3.one * _currentRadius);
            return;
        }

        if (_sensorType == SensorType.SPHERE || _target)
        {
            Gizmos.DrawWireSphere(transform.position, _currentRadius);
            return;
        }

        if (_sensorType == SensorType.CONE)
        {
            Vector3 leftAngle = Quaternion.AngleAxis(-_angle / 2, Vector3.up) * transform.forward;
            Vector3 rightAngle = Quaternion.AngleAxis(_angle / 2, Vector3.up) * transform.forward;

            Gizmos.DrawLine(transform.position, transform.position + leftAngle * _searchRadius);
            Gizmos.DrawLine(transform.position, transform.position + rightAngle * _searchRadius);

            Handles.DrawWireArc(transform.position, Vector3.up, leftAngle, _angle, _searchRadius);
        }
    }
}

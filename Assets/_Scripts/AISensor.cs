using UnityEditor;
using UnityEngine;

public class AISensor : MonoBehaviour
{
    public enum SensorType
    {
        HIT = 0,
        SPHERE = 1,
        BOX = 2,
        CONE = 3
    }

    private Transform _target;
    [SerializeField] private SensorType _sensorType = SensorType.HIT;
    [SerializeField] private float _searchRadius;
    [SerializeField] private float _targetRadius;
    [SerializeField] private float _updateRate;
    [SerializeField] private int _angle;
    [SerializeField] private LayerMask _layer;

    private float _currentRadius;
    private float _lastDetectionTime;

    public Transform GetTarget { get { return _target; } }

    private void OnEnable()
    {
        _currentRadius = _searchRadius;
    }

    private void FixedUpdate()
    {
        if (Time.time < _lastDetectionTime + _updateRate)
            return;

        if (_sensorType != SensorType.HIT)
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
        _currentRadius = targetFound ? _targetRadius : _searchRadius;
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

    public void SetTarget(Transform target) => _target = target;

    public void SetSensor(SensorType sensorType) => _sensorType = sensorType;

    public void SetRadius(float radius) => _searchRadius = _targetRadius = radius;

    public void SetRadius(float searchRadius, float targetRadius)
    {
        _searchRadius = searchRadius;
        _targetRadius = targetRadius;
    }

    public void SetUpdateRate(float rate) => _updateRate = rate;

    public void SetAngle(int angle) => _angle = angle;
}
using Unity.VisualScripting;
using UnityEngine;

public class TargetLocator : MonoBehaviour
{
    [SerializeField] Transform cannonBase;
    [SerializeField] Transform cannonBarrel;
    [SerializeField] ParticleSystem projectileParticles;
    [SerializeField] float range = 15f;
    Transform target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = FindFirstObjectByType<EnemyController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        AimWeapon();
        FindClosestTarget();
    }

    void AimWeapon()
    {
        float targetDistance = Vector3.Distance(target.position, transform.position);
        Vector3 baseDirection = target.position - cannonBase.position;
        baseDirection.y = 0f;

        if (baseDirection != Vector3.zero)
        {
            cannonBase.rotation = Quaternion.LookRotation(baseDirection, Vector3.up); //onlyroates y
        }

        Vector3 dir = target.position - cannonBarrel.position;

        if (dir != Vector3.zero)
        {
            float xAngle = Quaternion.LookRotation(dir).eulerAngles.x;
            cannonBarrel.localEulerAngles = new Vector3(xAngle, 0f, 0f);
        }

        if (targetDistance < range)
        {
            Attack(true);
        }
        else
        {
            Attack(false);
        }
    }

    void Attack(bool isActive)
    {
        var emissionModule = projectileParticles.emission;
        emissionModule.enabled = isActive;
    }

    void FindClosestTarget()
    {
        EnemyController[] enemies = FindObjectsByType<EnemyController>(FindObjectsSortMode.None);

        Transform closestTarget = null;

        float maxDistance = Mathf.Infinity;

        foreach (EnemyController enemy in enemies)
        {
            float targetDistance = Vector3.Distance(transform.position, enemy.transform.position);

            if (targetDistance < maxDistance)
            {
                closestTarget = enemy.transform;
                maxDistance = targetDistance;
            }
        }

        target = closestTarget;
    }


    void PracticeWeponTuring()
    {

    }
}

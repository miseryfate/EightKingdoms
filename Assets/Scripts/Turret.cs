using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {
    [Header("Basic variables")]
    public GameObject target;
    public Transform pointToRotate;
    public Transform[] firePoints;
    public GameObject[] muzzleFlash;
    private float closestRange;

    [Header("Combat Stats")]
    public float attackRange;
    public float turnSpeed;
    public float attackSpeed;
    private float attackTimer;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //Перезарядка выстрела башни
        if (attackTimer > 0) {
            attackTimer -= Time.deltaTime;
        }

        //Если у башни нет цели, то с помощью сферы с радиусом attackRange выбираем всех соприкоснувшиеся объекты на слое 10: Enemy
        if (target == null) 
        {
            //Минимальная дальность атаки приравнивается attackRange для поиска ближайшей цели
            closestRange = attackRange;
            int layerMask = 1 << 10;
            Collider[] possibleTargets = Physics.OverlapSphere(transform.position, attackRange, layerMask);

            foreach (Collider trg in possibleTargets)
            {
                //Если дистанция до цели от башни меньше, чем минимальная дальность атаки, то выбираем эту цель
                if (Vector3.Distance(trg.transform.position, transform.position) <= closestRange) {
                    target = trg.gameObject;
                    closestRange = Vector3.Distance(trg.transform.position, transform.position);
                }
            }
        }
        else 
        {
            //Захват цели башней
            LockOnTarget();

            //Если таймер атаки обнулился, то производим выстрел функцией Shoot()
            if (attackTimer <= 0f) 
            {
                Shoot();
                attackTimer = attackSpeed;
            }
        }
	}

    public void LockOnTarget() 
    {
        if (Vector3.Distance(target.transform.position, transform.position) <= attackRange)
        {
            //Высчитываем вектор поворота башни, чтобы она смотрела на цель, учитывая скорость поворота самой башни
            Vector3 direction = target.transform.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            Vector3 rotation = Quaternion.Lerp(pointToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
            pointToRotate.localRotation = Quaternion.Euler(0f, rotation.y, 0f);
        }
        else {
            //Если цель отошла дальше, чем attackRange, то цель обнуляем для башни
            target = null;
        }
    }

    void Shoot() {
        //Если наша цель еще существует, то производим выстрел
        if (target != null) {

            //Если спецэффекты выстрела установлены, то воспроизводим их
            if (muzzleFlash != null) 
            {
                foreach(GameObject flash in muzzleFlash)
                {
                    flash.SetActive(true);
                    flash.GetComponent<FadeIn>().timer = 0.1f;
                }
            }
        }
    }

	private void OnDrawGizmos()
	{
        //Только для редактора: рисуем вайр-сфера с радиусом атаки attackRange
        Gizmos.DrawWireSphere(transform.position, attackRange);
	}
}

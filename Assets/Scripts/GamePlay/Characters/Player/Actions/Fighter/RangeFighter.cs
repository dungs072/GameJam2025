using UnityEngine;

public class RangeFighter : MonoBehaviour, IFighter
{
    [SerializeField] private GameObject projectilePrefab;


    public void Attack()
    {
        Debug.Log("Range Fighter Attacking");

    }

    public void Defend()
    {
    }
}

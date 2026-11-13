using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] int maxHealth = 2;
    [SerializeField] Image changeimage;
    int health;
    public event Action Damaged;
    public event Action Died;

    void Awake()
    {
        maxHealth = maxHealth > 0 ? maxHealth : 1;
        health = maxHealth;
    }

    public bool IsDead()
    {
        return health <= 0;
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public void TakeDamage(int dmg)
    {
        if (health <= 0 || dmg <= 0) return;
        health = Mathf.Max(health - dmg, 0);
        changeimage.fillAmount = (float)health / maxHealth;
        if (health > 0) Damaged?.Invoke();
        else Died?.Invoke();
    }
}

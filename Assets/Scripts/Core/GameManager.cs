using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class GameManager 
{
    public enum GameState
    {
        PREGAME,
        INWAVE,
        WAVEEND,
        COUNTDOWN,
        GAMEOVER,
        PLAYERWIN
    }
    public GameState state;

    public int countdown;
    private static GameManager theInstance;
    public static GameManager Instance {  get
        {
            if (theInstance == null)
                theInstance = new GameManager();
            return theInstance;
        }
    }

    public GameObject player;
    public float startTime;
    
    public ProjectileManager projectileManager;
    public SpellIconManager spellIconManager;
    public EnemySpriteManager enemySpriteManager;
    public PlayerSpriteManager playerSpriteManager;
    public RelicIconManager relicIconManager;

    private List<GameObject> enemies;

    public Dictionary<int, bool> AttackGroup = new Dictionary<int, bool>();

    public int enemy_count { get { return enemies.Count; } }

    public void AddEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
    }
    public void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
    }

    public GameObject GetClosestEnemy(Vector3 point)
    {
        if (enemies == null || enemies.Count == 0) return null;
        if (enemies.Count == 1) return enemies[0];
        return enemies.Aggregate((a,b) => (a.transform.position - point).sqrMagnitude < (b.transform.position - point).sqrMagnitude ? a : b);
    }

    public GameObject GetClosestOtherEnemy(GameObject self)
    {
        Vector3 point = self.transform.position;
        if (enemies == null || enemies.Count < 2) return null;
        return enemies.FindAll((a) => a != self).Aggregate((a, b) => (a.transform.position - point).sqrMagnitude < (b.transform.position - point).sqrMagnitude ? a : b);
    }
    
    public GameObject GetClosestOfType(GameObject self, string type)
    {
        Vector3 point = self.transform.position;
        if (enemies == null || enemies.Count < 2) return null;
        return enemies.FindAll((a) => a != self && a.GetComponent<EnemyController>().monster == type).Aggregate((a, b) => (a.transform.position - point).sqrMagnitude < (b.transform.position - point).sqrMagnitude ? a : b);
    }

    public GameObject GetBestHealTarget(GameObject self)
    {
        Vector3 point = self.transform.position;
        EnemyController powers = self.GetComponent<EnemyController>();
        EnemyAction healing = powers.actions["heal"];

        if (healing == null) 
        {
            Debug.Log("Agent does not have healing powers");
            return null; 
        }

        if (enemies == null || enemies.Count < 2) return null;

        List<GameObject> Candidates = enemies.FindAll((a) => a != self &&
        a.GetComponent<EnemyController>().monster != "warlock" &&
        (a.transform.position - point).magnitude < healing.range &&
        (a.GetComponent<EnemyController>().hp.max_hp - a.GetComponent<EnemyController>().hp.hp) > 5);

        if (Candidates.Count < 1) return null;
        return Candidates.Aggregate((a, b) => 
        (a.GetComponent<EnemyController>().hp.max_hp - a.GetComponent<EnemyController>().hp.hp) > 
        (b.GetComponent<EnemyController>().hp.max_hp - b.GetComponent<EnemyController>().hp.hp) ? a : b);
    }

    public GameObject GetPBuffTarget(GameObject self)
    {
        Vector3 point = self.transform.position;
        EnemyController powers = self.GetComponent<EnemyController>();
        EnemyAction buff = powers.actions["permabuff"];

        if (buff == null)
        {
            Debug.Log("Agent does not have permabuff powers");
            return null;
        }

        if (enemies == null || enemies.Count < 2) return null;

        List<GameObject> Candidates = enemies.FindAll((a) => a != self &&
        a.GetComponent<EnemyController>().monster == "skeleton" &&
        (a.transform.position - point).magnitude < buff.range);

        if (Candidates.Count < 1) return null;
        return Candidates.Aggregate((a, b) =>
        (a.GetComponent<EnemyController>().hp.hp) >
        (b.GetComponent<EnemyController>().hp.hp) ? a : b);
    }

    /*public List<GameObject> GetEnemiesInRange(Vector3 point, float distance)
    {
        if (enemies == null || enemies.Count == 0) { Debug.Log("Enemies is null or empty!"); return null; }
        Debug.Log("Enemies is not null nor empty!");
        return enemies.FindAll((a) => (a.transform.position - point).magnitude <= distance);
    }*/
    public List<GameObject> GetEnemiesInRange(Vector3 point, float distance, string ?type = null, bool rallied = false)
    {
        if (enemies == null || enemies.Count == 0){ Debug.Log("Enemies is null or empty!"); return null; }
        Debug.Log("Enemies is not null nor empty!");

        List<GameObject> valid = enemies.FindAll((a) => (a.transform.position - point).magnitude <= distance);

        if(type != null)
        {
            valid = valid.FindAll((a) => a.GetComponent<EnemyController>().monster == type);
        }

        if (rallied)
        {
            valid = valid.FindAll((a) => AttackGroup.ContainsKey(a.GetInstanceID()));
        }

        return valid;
    }

    private GameManager()
    {
        enemies = new List<GameObject>();
    }

    public float WinTime()
    {
        return 8 * 60;
    }

    public float CurrentTime()
    {
        return (Time.time - startTime);
    }
}

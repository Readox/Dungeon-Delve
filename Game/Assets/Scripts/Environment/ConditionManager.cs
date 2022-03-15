using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionManager : MonoBehaviour
{
    
    private List<Conditions> conditionsList = new List<Conditions>();
    private List<Boons> boonsList = new List<Boons>();

    Coroutine conditionDamageTickCoroutine;

    public PlayerMovement playerMovement_script;
    public PlayerStats playerStats_script;
    public Transform storageGameObject;
    
    public GameObject poisonEffectObjectInst;
    private GameObject poisonEffectObject;
    public GameObject bleedingEffectObjectInst;
    private GameObject bleedingEffectObject;
    public GameObject burningEffectObjectInst;
    private GameObject burningEffectObject;
    
    public GameObject aegisEffectObjectInst;
    private GameObject aegisEffectObject;

    void Awake()
    {
        conditionsList.Clear();
        boonsList.Clear();
        InvokeRepeating("EffectTick", 1f, 1f); // For Conditions and Boons
    }

    void EffectTick()
    {
        int totalEffectStacks = 0; // For damage
        for (int i = 0; i < conditionsList.Count; i++) // need to do a for loop instead of foreach because of exceptions being thrown, same below
        {
            Conditions c = conditionsList[i];
            c.DoEffect();
            if (c.duration == 0f)
            {
                conditionsList.Remove(c);
                c.OnDurationExpire();
            }
            if (c.effectName.Equals("Bleeding") || c.effectName.Equals("Poison"))
            {
                totalEffectStacks += c.effectStacks;
            }
            if (c.effectName.Equals("Burning"))
            {
                if (Input.GetAxisRaw("Vertical") == 0 && Input.GetAxisRaw("Horizontal") == 0)
                {
                    totalEffectStacks = c.effectStacks * 2;
                }
                else
                {
                    totalEffectStacks += c.effectStacks;
                }
            }
        }
        for (int o = 0; o < boonsList.Count; o++)
        {
            Boons b = boonsList[o];
            b.DoEffect();
            if (b.duration == 0f)
            {
                boonsList.Remove(b);
                b.OnDurationExpire();
            }
        }

        playerStats_script.DealConditionDamage(totalEffectStacks); // TODO do animations, do proper damage, take in effect name for anim, take in effectstacks for damage
    }

    void CreateConditionEffectAnimationPrefab(Conditions c)
    {
        if (c.effectName.Equals("Poison") && poisonEffectObject == null)
        {
            poisonEffectObject = Instantiate(poisonEffectObjectInst, playerMovement_script.gameObject.transform.position, Quaternion.identity);
            poisonEffectObject.transform.SetParent(storageGameObject);
        }
        else if (c.effectName.Equals("Bleeding") && bleedingEffectObject == null)
        {
            bleedingEffectObject = Instantiate(bleedingEffectObjectInst, playerMovement_script.gameObject.transform.position, Quaternion.identity);
            bleedingEffectObject.transform.SetParent(storageGameObject);
        }
        else if (c.effectName.Equals("Burning") && burningEffectObject == null)
        {
            burningEffectObject = Instantiate(burningEffectObjectInst, playerMovement_script.gameObject.transform.position, Quaternion.identity);
            burningEffectObject.transform.SetParent(storageGameObject);
        }
    }

    void CreateBoonEffectAnimationPrefab(Boons b)
    {
        if (b.effectName.Equals("Aegis") && aegisEffectObject == null)
        {
            aegisEffectObject = Instantiate(aegisEffectObjectInst, playerMovement_script.gameObject.transform.position, Quaternion.identity);
            aegisEffectObject.transform.SetParent(playerMovement_script.gameObject.transform);
        }
        /*
        else if (c.effectName.Equals("Bleeding") && bleedingEffectObject == null)
        {
            bleedingEffectObject = Instantiate(bleedingEffectObjectInst, playerMovement_script.gameObject.transform.position, Quaternion.identity);
            bleedingEffectObject.transform.SetParent(storageGameObject);
        }
        */
    }

    public void RemoveEffectAnimation(string type)
    {
        if (type.Equals("Poison"))
        {
            Destroy(poisonEffectObject);
        }
        else if (type.Equals("Bleeding"))
        {
            Destroy(bleedingEffectObject);
        }
        else if (type.Equals("Burning"))
        {
            Destroy(burningEffectObject);
        }
        else if (type.Equals("Aegis"))
        {
            Destroy(aegisEffectObject);
        }
    }

    public void AddCondition(Conditions c)
    {
        if (c.effectName.Equals("Slowness") || c.effectName.Equals("Immobility"))
        {
            bool inList = false;
            for(int i = 0; i < conditionsList.Count; i++)
            {
                if (conditionsList[i].effectName.Equals(c.effectName))
                {
                    inList = true;
                    conditionsList[i].duration += c.duration;
                    break;
                }
            }
            if (inList == false)
            {
                conditionsList.Add(c);
                CreateConditionEffectAnimationPrefab(c);
                conditionsList[conditionsList.Count - 1].OnStart();
            }
        }
        else
        {
            conditionsList.Add(c);
            CreateConditionEffectAnimationPrefab(c);
            conditionsList[conditionsList.Count - 1].OnStart();
        }
    }

    public void AddBoon(Boons b)
    {
        if (b.effectName.Equals("Aegis"))
        {
            bool inList = false;
            for(int i = 0; i < boonsList.Count; i++)
            {
                if (boonsList[i].effectName.Equals(b.effectName))
                {
                    inList = true;
                    boonsList[i].duration += b.duration;
                    break;
                }
            }
            if (inList == false)
            {
                boonsList.Add(b);
                CreateBoonEffectAnimationPrefab(b);
                boonsList[boonsList.Count - 1].OnStart();
            }
        }
        else
        {
            boonsList.Add(b);
            CreateBoonEffectAnimationPrefab(b);
            boonsList[boonsList.Count - 1].OnStart();
        }
    }


    public void DoSlowness()
    {
        playerMovement_script.DoSlowness();
    }
    public void DoImmobility()
    {
        playerMovement_script.DoImmobility();
    }
    public void ResetSpeed()
    {
        playerMovement_script.ResetSpeed();
    }
    public void StartHealthRegen()
    {
        playerStats_script.StartHealthRegen();
    }
    public void StopHealthRegen()
    {
        playerStats_script.StopHealthRegen();
    }
    public void ActivateAegis()
    {
        playerStats_script.invulnerable = true;
    } 
    public void RemoveAegis()
    {
        playerStats_script.invulnerable = false;
        RemoveEffectAnimation("Aegis");
        int i = GetIndexInBoonsList("Aegis");
        if (i != -1)
        {
            boonsList.RemoveAt(i);
        }
        
    }


    public bool CheckForInstanceOfCondition(string type)
    {
        for (int i = 0; i < conditionsList.Count; i++)
        {
            Conditions c = conditionsList[i];
            if (c.effectName.Equals(type))
            {
                return true;
            }
        }
        return false;
    }

    public int GetIndexInBoonsList(string type)
    {
        for (int i = 0; i < boonsList.Count; i++)
        {
            Boons b = boonsList[i];
            if (b.effectName.Equals(type))
            {
                return i;
            }
        }
        return -1;
    }



}

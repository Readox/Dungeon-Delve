using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillType : MonoBehaviour
{

    public string skillType;
    public int skillLevel;
    public int skillTreeTier;


    public SkillType(string skillType, int skillLevel, int skillTreeTier)
    {
        this.skillType = skillType;
        this.skillLevel = skillLevel;
        this.skillTreeTier = skillTreeTier;
    }

    public SkillType()
    {

    }

    public string GetSkillType()
    {
        return skillType;
    }
    public int GetSkillLevel()
    {
        return skillLevel;
    }
    public int GetSkillTier()
    {
        return skillTreeTier;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

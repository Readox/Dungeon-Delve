using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyGFX : MonoBehaviour
{
    public AIPath aiPath;
    Animator anim;
    WanderingDestinationSetter wanderScript;

    private SpriteRenderer spriteRenderer;
    public bool updateSprite;
    public Sprite[] spriteSheet;
    //[SerializeField] private SpriteAtlas spriteSheet;

    void Awake()
    {
        anim = GetComponent<Animator>();
        if (gameObject.transform.parent != null)
        {
            wanderScript = gameObject.transform.parent.gameObject.GetComponent<WanderingDestinationSetter>();
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        //spriteSheet = Resources.LoadAll<Sprite>("sprite");
    }

    //LateUpdate changes the shown sprite if a spritesheet is used.
    //The spritesheet (sprite Atlas) must have an sprite with the same name.
    private void LateUpdate()
    {
        if(updateSprite && spriteRenderer.sprite.name != null)
        {
            string spriteName = spriteRenderer.sprite.name;
            foreach(Sprite s in spriteSheet) // Credit to racr0x here: https://forum.unity.com/threads/mini-tutorial-on-changing-sprite-on-runtime.212619/
            {
                if (s.name.Equals(spriteName))
                {
                    spriteRenderer.sprite = s;
                    break;
                }
            }
            //spriteRenderer.sprite = spriteSheet.GetSprite(spriteName);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // These are for setting orientation
        if (aiPath.desiredVelocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(-1f,1f,1f);
        }
        else if (aiPath.desiredVelocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(1f,1f,1f);
        }
    }

    void FixedUpdate()
    {
        // These are for setting animation states
        if (aiPath.maxSpeed == wanderScript.wanderSpeed)
        {
            anim.SetBool("Walk", true);
        }
    }
}

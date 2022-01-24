using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSwapper : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public bool updateSprite;
    public Sprite[] spriteSheet;
    //[SerializeField] private SpriteAtlas spriteSheet;

  
    private void Awake()
    {
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
}

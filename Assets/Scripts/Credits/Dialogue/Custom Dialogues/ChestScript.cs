using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : HiddenRewardsDialogue
{

    public static bool hasOpenedChest = false;

    [SerializeField] private AudioClip chestUnlock;
    [SerializeField] private Sprite unlockedTexture;

    [SerializeField] private string[] requiredItems;

    private SpriteRenderer sprite;

    // Start is called before the first frame update
    protected override void Start()
    {
        sprite= GetComponent<SpriteRenderer>();
        AudioManager.instance.addSFX(chestUnlock, gameObject, 8, false);
        base.Start();   
    }

    public override bool willGiveRewards()
    {
        bool willUnlock = true;

        foreach(var s in requiredItems)
        {
            if (!FauxInventoryScript.instance.isItemObtained(s))
                willUnlock = false;
        }

        return willUnlock && !hasClaimedRewards;
    }

    public override void InitiateDialogue()
    {
        if (willGiveRewards())
        {
            AudioManager.instance.PlaySFX(AudioManager.getName(chestUnlock, gameObject));
            sprite.sprite = unlockedTexture;
            hasOpenedChest = true;
        }
        base.InitiateDialogue();


    }
}

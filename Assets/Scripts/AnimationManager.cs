using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    // VARIABLES
    
    // Animators
    private Animator charAnimator;
    private Animator charSpriteAnimator;
    private Animator choiceAnimator;
    private Animator mbAnimator;
    private Animator itemsAnimator;
    private Animator objectivesAnimator;
    private Animator kingAnimator;
    private Animator kingSpriteAnimator;
    private Animator nightAnimator;

    // Objects that have the animators
    [SerializeField] private GameObject character;
    private GameObject characterSprite;
    [SerializeField] private GameObject choiceUI;
    [SerializeField] private GameObject messageBox;
    [SerializeField] private GameObject itemsUI;
    [SerializeField] private GameObject objectivesUI;
    [SerializeField] private GameObject king;
    [SerializeField] private GameObject kingSprite;
    [SerializeField] private GameObject night;

    // Start is called before the first frame update
    void Start()
    {
        // Get the animator from the game objects
        this.charAnimator = this.character.gameObject.GetComponent<Animator>();
        this.choiceAnimator = this.choiceUI.gameObject.GetComponent<Animator>();
        this.mbAnimator = this.messageBox.gameObject.GetComponent<Animator>();
        this.itemsAnimator = this.itemsUI.gameObject.GetComponent<Animator>();
        this.objectivesAnimator = this.objectivesUI.gameObject.GetComponent<Animator>();
        this.kingAnimator = this.king.gameObject.GetComponent<Animator>();
        this.kingSpriteAnimator = this.kingSprite.gameObject.GetComponent<Animator>();
        this.nightAnimator = this.night.gameObject.GetComponent<Animator>();

        // so helpful - discussions.unity.com/t/animation-wont-play-please-help/134809/2
        this.charAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        this.choiceAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        this.mbAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        this.itemsAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        this.objectivesAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        this.kingAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        this.kingSpriteAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        this.nightAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }



    // CHOICE UI ANIMATIONS

    /// <summary>
    /// Play animation for moving the choice UI down
    /// </summary>
    public void RunChoiceAnimDown()
    {
        this.choiceAnimator.SetBool("MoveDown", true);
    }

    /// <summary>
    /// Play animation for moving the choice UI up
    /// </summary>
    public void RunChoiceAnimUp()
    {
        this.choiceAnimator.SetBool("MoveDown", false);
    }



    // MESSAGE BOX UI ANIMATIONS

    /// <summary>
    /// Play animation for moving the message box UI up
    /// </summary>
    public void RunMessageAnimUp()
    {
        this.mbAnimator.SetBool("MoveUp", true);
    }
    
    /// <summary>
    /// Play animation for moving the message box UI down
    /// </summary>
    public void RunMessageAnimDown()
    {
        this.mbAnimator.SetBool("MoveUp", false);
    }



    // ITEMS UI ANIMATIONS

    /// <summary>
    /// Play animation for moving the items UI down
    /// </summary>
    public void RunItemsAnimDown()
    {
        this.itemsAnimator.SetBool("MoveDown", true);
    }

    /// <summary>
    /// Play animation for moving the items UI up
    /// </summary>
    public void RunItemsAnimUp()
    {
        this.itemsAnimator.SetBool("MoveDown", false);
    }



    // OBJECTIVES UI ANIMATIONS

    /// <summary>
    /// Play animation for moving the objectives UI down
    /// </summary>
    public void RunObjectivesAnimDown()
    {
        this.objectivesAnimator.SetBool("MoveDown", true);
    }

    /// <summary>
    /// Play animation for moving the objectives UI up
    /// </summary>
    public void RunObjectivesAnimUp()
    {
        this.objectivesAnimator.SetBool("MoveDown", false);
    }


    
    /// <summary>
    /// Stop walking animation after the given time.
    /// Originally used the clip length but it carried on too long
    /// </summary>
    /// <param name="time">The length of time to stop the walking animation after</param>
    /// <param name="spriteAnimator">The animator to stop the walking animation of</param>
    /// <returns></returns>
    private IEnumerator WaitUntilAnimDone(float time, Animator spriteAnimator)
    {
        yield return new WaitForSeconds(time);
        spriteAnimator.SetBool("IsWalking", false);
    }



    // KING ANIMATIONS

    /// <summary>
    /// Play animation for moving the king in
    /// </summary>
    public void RunKingAnimIn()
    {
        // flip the sprite so it is facing to the right
        if (this.kingSprite.GetComponent<SpriteRenderer>().flipX)
        {
            this.kingSprite.GetComponent<SpriteRenderer>().flipX = false;
        }

        this.kingAnimator.SetBool("MoveIn", true);
        this.kingSpriteAnimator.SetBool("IsWalking", true);

        StartCoroutine(WaitUntilAnimDone(4.5f, this.kingSpriteAnimator));
    }

    /// <summary>
    /// Play animation for moving the king out
    /// </summary>
    public void RunKingAnimOut()
    {
        // flip the sprite so it is facing to the left
        this.kingSprite.GetComponent<SpriteRenderer>().flipX = true;

        this.kingAnimator.SetBool("MoveIn", false);
        this.kingSpriteAnimator.SetBool("IsWalking", true);

        StartCoroutine(WaitUntilAnimDone(4.5f, this.kingSpriteAnimator));
    }



    // CHARACTER ANIMATIONS

    /// <summary>
    /// Play animation for moving the character in
    /// </summary>
    public void RunCharAnimIn()
    {
        // get instantiated character and get the animator
        this.characterSprite = this.character.transform.GetChild(0).gameObject;
        this.charSpriteAnimator = this.characterSprite.gameObject.GetComponent<Animator>();
        this.charSpriteAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;

        this.charAnimator.SetBool("MoveIn", true);
        this.charSpriteAnimator.SetBool("IsWalking", true);

        StartCoroutine(WaitUntilAnimDone(5, this.charSpriteAnimator));
    }

    /// <summary>
    /// Play animation for moving the character out
    /// </summary>
    public void RunCharAnimOut()
    {
        // Originally, the character would walk back the way they came in.
        // However, flipping the sprite didn't work too well, so they just walk forward.
        // every child of the body
        //GameObject body = this.characterSprite.transform.GetChild(0).gameObject;
        //body.GetComponent<SpriteRenderer>().flipX = true;
        //for (var i = 0; i < body.transform.childCount; i++)
        //{
        //    GameObject limb = body.transform.GetChild(i).gameObject;
        //    limb.GetComponent<SpriteRenderer>().flipX = true;
        //    // for every child of that child
        //    for (var j = 0; j < limb.transform.childCount; j++)
        //    {
        //        limb.transform.GetChild(j).GetComponent<SpriteRenderer>().flipY = true;
        //    }
        //}

        this.charAnimator.SetBool("MoveIn", false);
        this.charSpriteAnimator.SetBool("IsWalking", true);

        StartCoroutine(WaitUntilAnimDone(5, this.charSpriteAnimator));
    }



    // NIGHT

    /// <summary>
    /// Play animation for screen going dark
    /// </summary>
    public void RunNightAnimDark()
    {
        this.nightAnimator.SetBool("IsNight", true);
    }

    /// <summary>
    /// Play animation for screen going light (returning to normal)
    /// </summary>
    public void RunNightAnimLight()
    {
        this.nightAnimator.SetBool("IsNight", false);
    }
}

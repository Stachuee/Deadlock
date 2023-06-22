using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


[System.Serializable]
public class PlayerInfo
{
    public float hp;
    public float maxHp;

    public float speed;
    public float throwStrength;

    public float kickArmorShred;
    public float kickCooldown;

    public float meleeDamage;

    public float deathTimer;
    public float healthRecivedAfterRevive;

    public float bonusAmmo;

    public float armor;
}

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour, ITakeDamage
{
    public static PlayerController scientist;
    public static PlayerController solider;

    [SerializeField][Header("Player info")]
    public PlayerInfo playerInfo;

    [Header("Movment")]
    [Range(0, .3f)][SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
    Vector3 m_Velocity = Vector3.zero;
    [SerializeField] bool topPlayer;

    Vector2 moveDirection = Vector2.zero;
    public Vector2 currentAimDirection { get; private set; }
    public Vector2 currentMoveDirection { get; private set; }
    public Vector2 currentWeaponDirection { get; private set; }
    Vector2 desiredAimDirection = Vector2.zero;
    public bool keyboard;

    bool jumping;

    Rigidbody2D myBody;
    PlayerInput myinput;
    [Header("Controllers")]
    public GunController gunController;
    public EquipmentController equipmentController;
    public UiController uiController;
    public CameraFollowScript cameraController;
    public InventorySelector inventorySelector;

    [Header("Mele")]
    [SerializeField] float meleeDelay = 0.2f;
    float meleeTimer = 0f;
    [SerializeField] float meleeOffset;
    [SerializeField] float meleeRadius;

    bool isAttacking = false;
    bool isStimulated = false;
    bool isHealing = false;

    [Header("Death")]
    float reviveTimer;
    bool dead;

    [Header("Invincibility")]
    [SerializeField] float invincibilityAfterHitDuration;
    float invincibilityEnd;

    [Header("Camera")]
    [SerializeField]
    Camera cam;
    bool debugStart = true;
    public bool useMovement;

    [Header("Inventory")]

    List<IInteractable> inRange = new List<IInteractable>();
    IInteractable closestInRange;

    [SerializeField]
    int maxItemsHeld = 1;
    List<ItemSO> heldItems = new List<ItemSO>();

    [SerializeField]
    GameObject UsePrompt;

    [Header("Upgrades")]

    Dictionary<int, bool> upgrades = new Dictionary<int, bool>();

    [Header("SFX")]
    [SerializeField] AudioSource footstepSFX;
    [SerializeField] AudioSource jumpSFX;
    [SerializeField] AudioSource changeBulletSFX;
    [SerializeField] AudioSource medicineSFX;
    [SerializeField] AudioSource stimulatorSFX;
    [SerializeField] AudioSource inventorySFX;
    [SerializeField] AudioSource throwSFX;
    
    [SerializeField] AudioSource pickupSFX;

    [Header("Appearance")]
    [SerializeField] GameObject playerSpriteObject;
    Animator playerAnimator;
    SpriteRenderer playerSpriteRenderer;

    [SerializeField] Sprite soldierSprite;
    [SerializeField] Sprite scientistSprite;

    public bool isScientist {get; private set;}

    public bool LockInAnimation
    {
        get
        {
            return lockedInAnimation;
        }
        set
        {
            lockedInAnimation = value;
        }
    }
    UnityAction callbackWhenUnlocking;
    bool lockedInAnimation;

    public void SetUpPlayer(string controllScheme, int index, bool _scientist, bool glassesMode)
    {
        debugStart = false;
        keyboard = controllScheme == "Keyboard";
        isScientist = _scientist;
        if (isScientist) scientist = this;
        else solider = this;

        GameController.gameController.AddPlayer(this);
        cameraController.SetSplitScreenPosition(index, glassesMode);
    }

    private void Awake()
    {
        myBody = transform.GetComponent<Rigidbody2D>();
        myinput = transform.GetComponent<PlayerInput>();
        gunController = transform.GetComponent<GunController>();
        equipmentController = transform.GetComponent<EquipmentController>();

        playerAnimator = playerSpriteObject.GetComponent<Animator>();
        playerSpriteRenderer = playerSpriteObject.GetComponent<SpriteRenderer>();
        //playerInfo = new PlayerInfo() { hp = 100, maxHp = 100, speed = 5, throwStrength = 150};
    }

    private void Start()
    {
        if(debugStart)
        {
            keyboard = GetComponent<PlayerInput>().currentControlScheme == "Keyboard";
            GameController.gameController.AddPlayer(this);
        }
        EffectManager.instance.AddCameraToEffects(this);

        if (scientist)
        {
            playerAnimator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("scientist/scientist_animator");
            //playerSpriteRenderer.sprite = scientistSprite;
        }
        else
        {
            playerAnimator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("soldier/Soldier");
            //playerSpriteRenderer.sprite = soldierSprite;
        }
    }

    private void Update() 
    {

        if (!LockInAnimation && !dead) myBody.velocity = Vector3.SmoothDamp(myBody.velocity, new Vector2(moveDirection.x * playerInfo.speed, myBody.velocity.y), ref m_Velocity, m_MovementSmoothing);
        else
        {
            myBody.velocity = new Vector2(0, myBody.velocity.y);
        }

        if (keyboard) 
        {
            currentAimDirection = cameraController.ViewAngle();
            SendAimControll(currentAimDirection, currentAimDirection);
            SendMoveControll(currentAimDirection, currentAimDirection);
        }
        else
        {
            Vector2 vel = Vector2.zero, moveVel = Vector2.zero;
            if(desiredAimDirection.magnitude > 0.05f)
            {
                currentAimDirection = Vector2.SmoothDamp(currentAimDirection, desiredAimDirection, ref vel, 0.03f);
            }
            else if(moveDirection.magnitude > 0.05f && !LockInAnimation)
            {
                currentAimDirection = Vector2.SmoothDamp(currentAimDirection, moveDirection, ref vel, 0.03f);
            }
            else if(currentAimDirection.magnitude > 0.05f)
            {
                currentAimDirection = Vector2.SmoothDamp(currentAimDirection, desiredAimDirection, ref vel, 0.03f);
            }
            currentMoveDirection = Vector2.SmoothDamp(currentMoveDirection, moveDirection, ref vel, 0.03f);
            SendAimControll(desiredAimDirection, currentAimDirection);
            SendMoveControll(moveDirection, currentMoveDirection); 
        }

        if (dead)
        {
            if (reviveTimer < Time.time)
            {
                dead = false;
                invincibilityEnd = Time.time + invincibilityAfterHitDuration;
            }
            else playerInfo.hp = Mathf.Min(playerInfo.maxHp, playerInfo.hp + (playerInfo.healthRecivedAfterRevive / playerInfo.deathTimer) * Time.deltaTime);
            return;
        }


        float closestDistance = Mathf.Infinity;
        IInteractable closest = null;
        bool isItem = false;

        inRange.ForEach(x =>
        {
            float distance = Vector2.Distance(transform.position, x.GetPosition());
            bool item = x is Item;
            if ((closestDistance > distance && isItem == item) || (isItem == false && item))
            {
                isItem = item;
                closestDistance = distance;
                closest = x;
            }
        });


        if(closest != closestInRange)
        {
            if (closestInRange != null && closestInRange.IsProximity())
            {
                closestInRange.UnHighlight();
                uiController.ToHighlight = null;
            }
            if (closest != null && closest.IsProximity())
            {
                closest.Highlight();
                uiController.ToHighlight = closest;
                UsePrompt.SetActive(closest.IsUsable(this));
            }
            if(closest == null || !closest.IsProximity())
            {
                UsePrompt.SetActive(false);
            }
        }
        closestInRange = closest;

        if (isAttacking)
        {
            meleeTimer += Time.deltaTime;

            if (meleeTimer >= meleeDelay)
            {
                meleeTimer = 0;
                isAttacking = false;
            }
        }

        if(moveDirection != Vector2.zero && !footstepSFX.isPlaying && !dead && !lockedInAnimation)
        {
            playerAnimator.SetBool("isRunning", true);
            footstepSFX.volume = Random.Range(0.8f, 1);
            footstepSFX.pitch = Random.Range(0.9f, 1.01f);
            footstepSFX.Play();
        }else if(moveDirection == Vector2.zero)
            playerAnimator.SetBool("isRunning", false);


        if (currentAimDirection.x > 0)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        //playerSpriteRenderer.flipX = false;
        else if (currentAimDirection.x < 0)
            transform.rotation = Quaternion.Euler(0, 180, 0);
        //playerSpriteRenderer.flipX = true;
    }


    [SerializeField]
    GameObject itemPrefab;

    void DropHolding()
    {
        if (heldItems.Count == 0) return;
        Item itemDropped = Instantiate(itemPrefab, transform.position, Quaternion.identity).GetComponentInChildren<Item>();
        itemDropped.Innit(heldItems[heldItems.Count - 1], this);
        heldItems.RemoveAt(heldItems.Count - 1);

        itemDropped.GetComponentInParent<Rigidbody2D>().AddForce(currentAimDirection.normalized * playerInfo.throwStrength);

        throwSFX.Play();
    }

    public bool PickUp(ItemSO item)
    {
        if (heldItems.Count >= maxItemsHeld) return false;
        else
        {
            pickupSFX.Play();
            heldItems.Add(item);
            return true;
        }
    }

    public void Heal(float hpRestore, float time)
    {
        StartCoroutine(Healing(hpRestore, time));
        medicineSFX.Play();
    }

    public void Stimulate(float effectDuration)
    {
        StartCoroutine(SpeedIncrease(effectDuration));
        stimulatorSFX.Play();
    }

    readonly float HEALING_TICK = 0.2f;

    [SerializeField] ParticleSystem tempHealing;
    [SerializeField] ParticleSystem tempStim;
    IEnumerator Healing(float hpRestore, float time)
    {
        isHealing = true;
        tempHealing.Play();
        for (float i = 0; i <= time; i += HEALING_TICK)
        {
            yield return new WaitForSeconds(HEALING_TICK);

            Heal(hpRestore/time * HEALING_TICK);
            //playerInfo.hp = Mathf.Clamp(playerInfo.hp +  * HEALING_TICK, 0, playerInfo.maxHp);
        }
        tempHealing.Stop();
        isHealing = false;
    }

    IEnumerator SpeedIncrease(float time)
    {
        isStimulated = true;
        tempStim.Play();

        float tmpSpeed = playerInfo.speed;
        float tmpArmor = playerInfo.armor;

        playerInfo.speed *= 2;

        playerInfo.armor += 0.5f;

        yield return new WaitForSeconds(time);
        isStimulated = false;
        tempStim.Stop();
        playerInfo.speed = tmpSpeed;
        playerInfo.armor = tmpArmor;
    }

    public bool GetIsStimulated()
    {
        return isStimulated;
    }
    public bool GetIsHealing()
    {
        return isHealing;
    }

    public Vector2 GetMovementDirection()
    {
        return moveDirection;
    }

    

    

    #region InputRegion
    public void OnMove(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }

    public void OnDrop(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (dead) return;
            if (lockedInAnimation)
            {

            }
            else
            {
                DropHolding();
            }
        }
    }

    public void OnUse(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (dead) return;
            SendUseControll();
            if (closestInRange != null && !LockInAnimation)
            {
                closestInRange.Use(this, UseType.Hand);
            }
        } // use one
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        jumping = context.ReadValueAsButton();
        //myBody.AddForce(new Vector2(0, 200));
        jumpSFX.Play();
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (dead) return;
        SendShootControll(context.ReadValueAsButton());
        if (lockedInAnimation) return;
        gunController.ShootGun(context.ReadValueAsButton());
    }



    public void OnAttack(InputAction.CallbackContext context)
    {
        if (dead) return;
        if (context.ReadValueAsButton() && !isAttacking)
        {
            isAttacking = true;
            Collider2D[] hits = Physics2D.OverlapCircleAll((Vector2)transform.position + new Vector2((currentAimDirection.x >= 0 ? 1 : -1) * meleeOffset, 0), meleeRadius);
            for (int i = 0; i < hits.Length; i++)
            {
                if(hits[i].transform.tag == "Enemy")
                {
                    ITakeDamage hit = hits[i].GetComponent<ITakeDamage>();
                    hit.TakeDamage(playerInfo.meleeDamage, DamageSource.Turret);
                    hit.TakeArmorDamage(playerInfo.kickArmorShred);
                }
            }
        }
    }

    public void OnChangeWeapon(InputAction.CallbackContext context)
    {
        if (dead) return;
        if (!context.started) return;
    }

    public void onChangeBullet(InputAction.CallbackContext context)
    {
        if (dead) return;
        gunController.ChangeBullet(context.performed);
        //changeBulletSFX.Play();
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        desiredAimDirection = context.ReadValue<Vector2>();
        float magnitude = desiredAimDirection.magnitude;
        desiredAimDirection = magnitude > 1 ? desiredAimDirection.normalized : desiredAimDirection;
    }

    public void OnThrow(InputAction.CallbackContext context)
    {
        if (dead) return;
        if (context.performed)
        {
            equipmentController.UseEquipment();
        }
    }
    public void OnReload(InputAction.CallbackContext context)
    {
        if (dead) return;
        if (context.performed) gunController.Reload();
    }

    public void OnMenuBack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (dead) return;
            if (lockedInAnimation)
            {
                callbackWhenUnlocking.Invoke();
                //callbackWhenUnlocking = null;
                //LockInAnimation = false;
            }
            else
            {
                SendbackControll();
            }
        }
    }



    public void OnOpenInventory(InputAction.CallbackContext context)
    {
        //if (!context.started) return;
        //if (dead) return;
        if (context.ReadValueAsButton())
        {
            inventorySelector.OpenInventory();
            inventorySFX.Play();
        }
        else
        {
            inventorySelector.ChangePlayerSlot();
        }
        }
        #endregion

        #region UseRegion
        private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Interactable")
        {
            inRange.Add(collision.transform.GetComponent<IInteractable>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Interactable")
        {
            inRange.Remove(collision.transform.GetComponent<IInteractable>());
        }
    }


    #endregion

    #region interfaces
    public float TakeDamage(float damage, DamageSource source, DamageEffetcts effects = DamageEffetcts.None)
    {
        if (invincibilityEnd > Time.time || dead) return 0;
        playerInfo.hp -= damage;
        invincibilityEnd = Time.time + invincibilityAfterHitDuration;
        if (playerInfo.hp <= 0)
        {
            reviveTimer = playerInfo.deathTimer + Time.time;
            playerInfo.hp = 0;
            dead = true;
            gunController.ShootGun(false);
        }
        return damage;
    }
    public void ApplyStatus(Status toApply)
    {
        
    }

    public void TakeArmorDamage(float damage)
    {
        throw new System.NotImplementedException();
    }
    public bool IsArmored()
    {
        return false;
    }

    public bool IsImmune()
    {
        return dead;
    }

    public float Heal(float ammount)
    {
        playerInfo.hp = Mathf.Min(ammount + playerInfo.hp, playerInfo.maxHp);
        return ammount;
    }
    public Transform GetTransform()
    {
        return transform;
    }

    #endregion

    #region ForwardControll

    List<IControllSubscriberAim> aimSubscribers = new List<IControllSubscriberAim>();
    public void AddAimSubscriber(IControllSubscriberAim subscriberMovment)
    {
        aimSubscribers.Add(subscriberMovment);
    }
    public void RemoveAimSubscriber(IControllSubscriberAim subscriberMovment)
    {
        aimSubscribers.Remove(subscriberMovment);
    }
    void SendAimControll(Vector2 context, Vector2 smooth)
    {
        aimSubscribers.ForEach(x => x.ForwardCommandAim(context, smooth));
    }


    List<IControllSubscriberMove> moveSubscribers = new List<IControllSubscriberMove>();
    public void AddMoveSubscriber(IControllSubscriberMove subscriberMovment)
    {
        moveSubscribers.Add(subscriberMovment);
    }
    public void RemoveMoveSubscriber(IControllSubscriberMove subscriberMovment)
    {
        moveSubscribers.Remove(subscriberMovment);
    }
    void SendMoveControll(Vector2 context, Vector2 smooth)
    {
        moveSubscribers.ForEach(x => x.ForwardCommandMove(context, smooth));
    }


    List<IControllSubscriberUse> iControllSubscriberUse = new List<IControllSubscriberUse>();
    public void AddUseSubscriber(IControllSubscriberUse subscriberMovment)
    {
        iControllSubscriberUse.Add(subscriberMovment);
    }
    public void RemoveUseSubscriber(IControllSubscriberUse subscriberMovment)
    {
        iControllSubscriberUse.Remove(subscriberMovment);
    }
    void SendUseControll()
    {
        iControllSubscriberUse.ForEach(x => x.ForwardCommandUse());
    }

    List<IControllSubscriberBack> iControllSubscriberBack = new List<IControllSubscriberBack>();
    public void AddBackSubscriber(IControllSubscriberBack subscriberMovment)
    {
        iControllSubscriberBack.Add(subscriberMovment);
    }
    public void RemoveBackSubscriber(IControllSubscriberBack subscriberMovment)
    {
        iControllSubscriberBack.Remove(subscriberMovment);
    }
    void SendbackControll()
    {
        iControllSubscriberBack.ForEach(x => x.ForwardCommandBack());
    }

    List<IControllSubscriberShoot> controllSubscriberShoots = new List<IControllSubscriberShoot>();
    public void AddShootSubscriber(IControllSubscriberShoot subscriberShoot)
    {
        controllSubscriberShoots.Add(subscriberShoot);
    }
    public void RemoveShootSubscriber(IControllSubscriberShoot subscriberShoot)
    {
        controllSubscriberShoots.Remove(subscriberShoot);
    }
    void SendShootControll(bool isShooting)
    {
        controllSubscriberShoots.ForEach(x => x.ForwardCommandShoot(isShooting));
    }



    #endregion

    #region ExternalControll

    public void UpdateHighlight()
    {
        uiController.ToHighlight = closestInRange;
    }

    public ItemSO DepositIngredient()
    {
        if (heldItems.Count > 0)
        {
            ItemSO temp = heldItems[heldItems.Count - 1];
            heldItems.Remove(temp);
            return temp;
        }
        else return null;
    }

    public ItemSO CheckIfHoldingAnyAndDeposit(List<ItemSO> itemsAccepted)
    {
        ItemSO toReturn = null;
        itemsAccepted.ForEach(x =>
        {
            if(toReturn == null && heldItems.Contains(x))
            {
                toReturn = x;
            }
        });
        heldItems.Remove(toReturn);
        return toReturn;
    }

    public ItemSO CheckIfHoldingAnyAndDeposit(ItemSO itemAccepted)
    {
        ItemSO toReturn = null;
        
        if (heldItems.Contains(itemAccepted))
        {
            toReturn = itemAccepted;
        }
        heldItems.Remove(toReturn);
        return toReturn;
    }

    public bool CheckIfHoldingAny(ItemSO itemAccepted)
    {
        return heldItems.Contains(itemAccepted);
    }

    public bool CheckIfHoldingAny()
    {
        return heldItems.Count > 0;
    }
    
    public ItemSO CheckIfHoldingAnyAndDeposit<T>()
    {
        ItemSO toReturn = null;
        heldItems.ForEach(x =>
        {
            if(x is T) toReturn = x;
        });
        heldItems.Remove(toReturn);
        return toReturn;
    }
    public bool CheckIfHoldingAny<T>()
    {
        ItemSO toReturn = null;
        heldItems.ForEach(x =>
        {
            if (x is T) toReturn = x;
        });
        return toReturn != null;
    }

    public void RefreshPrompt()
    {
        if (closestInRange != null) UsePrompt.SetActive(closestInRange.IsUsable(this));
    }



    public void LockInAction(UnityAction callback)
    {
        if (callbackWhenUnlocking != null) callbackWhenUnlocking.Invoke();
        callbackWhenUnlocking = callback;
        LockInAnimation = true;
    }

    public void UnlockInAnimation()
    {
        LockInAnimation = false;
        callbackWhenUnlocking = null;
    }


    public bool HasUpgrade(int id)
    {
        if(upgrades.ContainsKey(id))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void GetUpgrade(int id)
    {
        upgrades.Add(id, true);
    }



    #endregion


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere((Vector2)transform.position + currentAimDirection.normalized * meleeOffset, meleeRadius);
    }
}

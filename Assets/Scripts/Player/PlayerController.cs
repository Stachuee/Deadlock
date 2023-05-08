using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


[System.Serializable]
public struct PlayerInfo
{
    public float hp;
    public float maxHp;

    public float speed;
    public float throwStrength;


    public DamageTypeResistance damageResistance;
}

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour, ITakeDamage
{
    [SerializeField][Header("Player info")]
    public PlayerInfo playerInfo;

    [Header("Movment")]
    [Range(0, .3f)][SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
    Vector3 m_Velocity = Vector3.zero;
    [SerializeField] bool topPlayer;

    Vector2 moveDirection = Vector2.zero;
    public Vector2 currentAimDirection { get; private set; }
    Vector2 desiredAimDirection = Vector2.zero;
    public bool keyboard;

    float jumping;

    Rigidbody2D myBody;
    PlayerInput myinput;
    public GunController gunController;
    public EquipmentController equipmentController;
    public UiController uiController;
    public CameraFollowScript cameraController;

    [SerializeField] GameObject attackArea;
    [SerializeField] float attackDelay = 0.2f;
    float attackTimer = 0f;
    bool isAttacking = false;



    [SerializeField]
    Camera cam;
    bool debugStart = true;

    [SerializeField]
    GameObject inventoryPanel;

    List<IInteractable> inRange = new List<IInteractable>();
    IInteractable closestInRange;

    [SerializeField]
    int maxItemsHeld = 1;
    List<ItemSO> heldItems = new List<ItemSO>();

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

    public void SetUpPlayer(string controllScheme, int index, bool scientist, bool glassesMode)
    {
        debugStart = false;
        keyboard = controllScheme == "Keyboard";
        GameController.gameController.AddPlayer(this);
        cameraController.SetSplitScreenPosition(index, glassesMode);
        isScientist = scientist;
    }

    private void Awake()
    {
        myBody = transform.GetComponent<Rigidbody2D>();
        myinput = transform.GetComponent<PlayerInput>();
        gunController = transform.GetComponent<GunController>();
        equipmentController = transform.GetComponent<EquipmentController>();
        playerInfo = new PlayerInfo() { hp = 100, maxHp = 100, speed = 5, throwStrength = 500};
    }

    private void Start()
    {
        if(debugStart)
        {
            keyboard = GetComponent<PlayerInput>().currentControlScheme == "Keyboard";
            GameController.gameController.AddPlayer(this);
        }
        EffectManager.instance.AddCameraToEffects(this);
    }

    private void Update() 
    {
        if (!LockInAnimation) myBody.velocity = Vector3.SmoothDamp(myBody.velocity, new Vector2(moveDirection.x * playerInfo.speed, myBody.velocity.y), ref m_Velocity, m_MovementSmoothing);
        else myBody.velocity = new Vector2(0, myBody.velocity.y);

        if (keyboard) 
        {
            currentAimDirection = cameraController.ViewAngle();
            SendMovmentControll(currentAimDirection);
        }
        else
        {
            Vector2 vel = Vector2.zero;
            currentAimDirection = Vector2.SmoothDamp(currentAimDirection, desiredAimDirection, ref vel, 0.03f);
            SendMovmentControll(desiredAimDirection);
        }

        float closestDistance = Mathf.Infinity;
        IInteractable closest = null;
        inRange.ForEach(x =>
        {
            float distance = Vector2.Distance(transform.position, x.GetPosition());
            if (closestDistance > distance)
            {
                closestDistance = distance;
                closest = x;
            }
        });


        if(closest != closestInRange)
        {
            if (closestInRange != null)
            {
                closestInRange.UnHighlight();
                uiController.ToHighlight = null;
            }
            if (closest != null)
            {
                closest.Highlight();
                uiController.ToHighlight = closest;
            }
        }
        closestInRange = closest;

        if (isAttacking)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer >= attackDelay)
            {
                attackTimer = 0;
                isAttacking = false;
                attackArea.SetActive(isAttacking);
            }
        }
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
    }

    public bool PickUp(ItemSO item)
    {
        if (heldItems.Count >= maxItemsHeld) return false;
        else
        {
            heldItems.Add(item);
            return true;
        }
    }

    public void Heal()
    {
        playerInfo.hp = playerInfo.maxHp;
    }

    public void Stimulate(float effectDuration)
    {
        StartCoroutine(SpeedIncrease(effectDuration));
    }


    IEnumerator SpeedIncrease(float time)
    {
        float tmpSpeed = playerInfo.speed;
        float tmpBulletRes = playerInfo.damageResistance.bulletResistance;
        float tmpFireRes = playerInfo.damageResistance.fireResistance;
        float tmpIceRes = playerInfo.damageResistance.iceResistance;
        float tmpPoisonRes = playerInfo.damageResistance.poisonResistance;
        float tmpMeleRes = playerInfo.damageResistance.meleResistance;

        playerInfo.speed *= 2;

        playerInfo.damageResistance.bulletResistance += 0.5f;
        playerInfo.damageResistance.fireResistance += 0.5f;
        playerInfo.damageResistance.iceResistance += 0.5f;
        playerInfo.damageResistance.poisonResistance += 0.5f;
        playerInfo.damageResistance.meleResistance += 0.5f;

        yield return new WaitForSeconds(time);
        playerInfo.speed = tmpSpeed;
        playerInfo.damageResistance.bulletResistance = tmpBulletRes;
        playerInfo.damageResistance.fireResistance = tmpFireRes;
        playerInfo.damageResistance.iceResistance = tmpIceRes;
        playerInfo.damageResistance.poisonResistance = tmpPoisonRes;
        playerInfo.damageResistance.meleResistance = tmpMeleRes;
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
        if (!context.started) return;
        if (lockedInAnimation)
        {
            callbackWhenUnlocking.Invoke();
            //callbackWhenUnlocking = null;
            //LockInAnimation = false;
        }
        else if (context.ReadValue<float>() > 0.9f)
        {
            SendbackControll();
            DropHolding();
        }
    }

    public void OnUse(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (context.ReadValue<float>() > 0.9f)
        {
            SendUseControll();
            if (closestInRange != null && !LockInAnimation)
            {
                closestInRange.Use(this);
            }
                
        } // use one
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        jumping = context.ReadValue<float>();
        //myBody.AddForce(new Vector2(0, 200));
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        float isShooting = context.ReadValue<float>();
        //if (!context.started) return;
        SendShootControll(isShooting > 0.8f);
        if (lockedInAnimation) return;
        gunController.ShootGun(isShooting);
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        Debug.Log(context.ReadValue<float>());
        if (context.ReadValue<float>() > 0.9f)
        {
            isAttacking = true;
            attackArea.SetActive(isAttacking);
        }
    }

    public void OnChangeWeapon(InputAction.CallbackContext context)
    {
        if (!context.started) return;
    }

    public void onChangeBullet(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        gunController.ChangeBullet(context.ReadValue<float>());
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        desiredAimDirection = context.ReadValue<Vector2>();
        desiredAimDirection = desiredAimDirection.magnitude > 1 ? desiredAimDirection.normalized : desiredAimDirection;
    }

    public void OnThrow(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (context.ReadValue<float>() > 0.9f) equipmentController.UseEquipment();
    }

    public void OnOpenInventory(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (context.ReadValue<float>() > 0.9f)
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
            gunController.SetSelectedSlot();
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
    public float TakeDamage(float damage, DamageType type)
    {
        throw new System.NotImplementedException();
    }
    public void TakeArmorDamage(DamageType type, float damage)
    {
        throw new System.NotImplementedException();
    }

    public bool IsImmune()
    {
        throw new System.NotImplementedException();
    }

    #endregion

    #region ForwardControll

    List<IControllSubscriberMovment> movmentSubscribers = new List<IControllSubscriberMovment>();
    public void AddMovmentSubscriber(IControllSubscriberMovment subscriberMovment)
    {
        movmentSubscribers.Add(subscriberMovment);
    }
    public void RemoveMovmentSubscriber(IControllSubscriberMovment subscriberMovment)
    {
        movmentSubscribers.Remove(subscriberMovment);
    }
    void SendMovmentControll(Vector2 context)
    {
        movmentSubscribers.ForEach(x => x.ForwardCommandMovment(context));
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



    #endregion

}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class Health : MonoBehaviour
{
    [SerializeField]
    private Entity entity = default;
    [SerializeField]
    private bool isInvulnerable = false;
    [SerializeField]
    private FloatReference currentHealth = new FloatReference(100);
    [SerializeField]
    private FloatReference startHealth = null;
    [SerializeField]
    private bool destroyBelowZero = true;
    [SerializeField]
    private GameObject destroyTarget = null;
    [SerializeField]
    private Renderer[] renderers = null;
    [SerializeField]
    private Color takeDamageColor = Color.red;
    [SerializeField]
    private Color healColor = Color.green;

    [Space()]
    
    [SerializeField]
    private UnityEvent onDamagedEvent = default;
    [SerializeField]
    private UnityEvent onHit = default;
    [SerializeField]
    private UnityEvent onHealedEvent = default;
    [SerializeField]
    private UnityEvent onDeathEvent = default;

    [SerializeField, HideInInspector]
    private HealthPostProcessor postProcessor;

    public float CurrentHealth => IsInvulnerable ? StartHealth : currentHealth.Value;
    public bool IsAlive => currentHealth.Value > 0;
    public bool DestroyBelowZero { get => destroyBelowZero; set => destroyBelowZero = value; }
    public float StartHealth => GetStartHealth();
    public bool IsFullHealth => currentHealth.Value >= StartHealth;
    public bool IsInvulnerable
    {
        get
        {
            if (isInvulnerable)
                return true;

            if (entity != null)
                return entity.ContainsModifier(typeof(InvincibleModifier));

            return false;
        }
    }        
    
    protected bool IsDying { get; set; }
    protected float InvincibleStart { get; set; }
    protected float InvincibleDuration { get; set; }
    protected float TimeRendererColored { get; set; }
    protected float DamageMultiplier { get; set; } = 1;
    protected MaterialData[] CurrentColorSchemes { get; set; }

    protected const float IndicatorTime = 0.1f;

    protected virtual void Awake()
    {
        entity.AddModifierListener(UpdateDamageReduction, typeof(DamageReceivedMultiplierModifier));
    }
    protected virtual void Start()
    {
        currentHealth.Value = StartHealth;
    }
    protected virtual void Update()
    {
        PollRenderer();
    }

    public void AddOnHitListener(UnityAction action)
    {
        onHit.AddListener(action);
    }
    public void RemoveHitDamageListener(UnityAction action)
    {
        onHit.RemoveListener(action);
    }
    public void AddOnDamagedListener(UnityAction action)
    {
        onDamagedEvent.AddListener(action);
    }
    public void RemoveOnDamagedListener(UnityAction action)
    {
        onDamagedEvent.RemoveListener(action);
    }
    public void AddOnHealedListener(UnityAction action)
    {
        onHealedEvent.AddListener(action);
    }
    public void RemoveOnHealedListener(UnityAction action)
    {
        onHealedEvent.RemoveListener(action);
    }
    public void AddOnDeathListener(UnityAction action)
    {
        onDeathEvent.AddListener(action);
    }
    public void RemoveOnDeathListener(UnityAction action)
    {
        onDeathEvent.AddListener(action);
    }
    public virtual void Heal(float healAmount)
    {
        currentHealth.Value = Mathf.Clamp(currentHealth.Value + healAmount, 0, StartHealth);

        onHealedEvent.Invoke();

        ColorRenderer(healColor);
    }
    public virtual void TakeDamage(float damageAmount)
    {
        onHit.Invoke();

        if (IsInvulnerable || !IsAlive)
            return;

        float damage = damageAmount * DamageMultiplier;
        
        if(damage > 0)
        {
            currentHealth.Value = Mathf.Clamp(currentHealth.Value - damage, 0, StartHealth);

            onDamagedEvent.Invoke();
            ColorRenderer(takeDamageColor);
        }        

        if (currentHealth.Value <= 0)
            Die();
    }
    protected virtual void PollRenderer()
    {
        if (CurrentColorSchemes != null)
        {
            if (Time.time - TimeRendererColored > IndicatorTime)
            {
                RevertColorSchemes();
            }
        }
    }
    protected virtual void RevertColorSchemes()
    {
        QueryColorSchemes(x => x.Revert());
        CurrentColorSchemes = null;
    }
    protected virtual void ColorRenderer(Color color)
    {
        if (renderers == null)
            return;

        TimeRendererColored = Time.time;
        
        if(CurrentColorSchemes != null)
            QueryColorSchemes(x => x.Revert());

        CurrentColorSchemes = new MaterialData[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            MaterialData materialData = new MaterialData(renderers[i]);
            materialData.ChangeColor(color);

            CurrentColorSchemes[i] = materialData;
        }
    }
    public virtual void Die()
    {
        if (IsDying)
            return;

        IsDying = true;

        onDeathEvent.Invoke();

        if(DestroyBelowZero)
            Destroy(destroyTarget);
    }
    protected virtual void OnValidate()
    {
        if (destroyTarget == null)
            destroyTarget = transform.parent == null ? gameObject : transform.parent.gameObject;

        if (postProcessor == null)
            postProcessor = GetComponentInChildren<HealthPostProcessor>();

        if(entity == null)
        {
            if(gameObject.TryGetEntity(out Entity entity))
            {
                this.entity = entity;
            }
        }
    }
    protected virtual void QueryColorSchemes(System.Action<MaterialData> action)
    {
        if (CurrentColorSchemes == null)
            return;

        for (int i = 0; i < CurrentColorSchemes.Length; i++)
            action(CurrentColorSchemes[i]);
    }
    private void UpdateDamageReduction()
    {
        DamageMultiplier = 1;

        for (int i = 0; i < entity.GetModifierCount(typeof(DamageReceivedMultiplierModifier)); i++)
        {
            DamageMultiplier = Mathf.Min(DamageMultiplier, entity.GetModifier<DamageReceivedMultiplierModifier>(i).Multiplier);
        }

        DamageMultiplier = Mathf.Clamp01(DamageMultiplier);
    }
    protected virtual float GetStartHealth()
    {
        return postProcessor == null ? startHealth.Value : postProcessor.ProcessMaxHealth(startHealth.Value);
    }
    protected virtual void OnDestroy()
    {
        RevertColorSchemes();
    }

    protected class MaterialData
    {
        public MaterialData(Renderer renderer)
        {
            originalColors = new Dictionary<Material, Color>();
            originalRenderer = renderer;

            foreach (Material material in renderer.materials)
            {
                originalColors.Add(material, material.color);
            }
        }
        
        private readonly Dictionary<Material, Color> originalColors;
        private readonly Renderer originalRenderer;

        public void ChangeColor(Color color)
        {
            foreach (Material material in originalColors.Keys)
            {
                material.color = color;
            }
        }
        public void Revert()
        {
            foreach (KeyValuePair<Material, Color> pair in originalColors)
            {
                pair.Key.color = pair.Value;
            }
        }
    }
}
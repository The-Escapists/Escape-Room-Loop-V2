#if UNITY_EDITOR
using Sirenix.OdinInspector;
using TheEscapists.ActionsAndInteractions.Actions;
using TheEscapists.ActionsAndInteractions.Interactions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PrefabChecker : ScriptableObject
{
    private void OnEnable()
    {
        this.name = "PrefabChecker";
    }

    private void OnValidate()
    {
        Check();
    }

    #region Actor Settings
    bool hasNoCollider;
    bool hasNoColliderToggler;
    [BoxGroup("Actor Settings"), InfoBox("The Prefab has no Collider 2D Component", InfoMessageType = InfoMessageType.Warning, VisibleIf = "hasNoCollider")]
    [BoxGroup("Actor Settings"), InfoBox("The Prefab has no Action Collider Visibility Component", InfoMessageType = InfoMessageType.Warning, VisibleIf = "hasNoColliderToggler")]
    [BoxGroup("Actor Settings"), ShowIf("isPartOfInteractionSystem"), OnValueChanged("Check")]
    public bool shouldCollide;

    bool hasNoSpriteRenderer;
    bool hasNoSpriteSwitcher;
    [BoxGroup("Actor Settings"), InfoBox("The Prefab has no Sprite Renderer Component", InfoMessageType = InfoMessageType.Warning, VisibleIf = "hasNoSpriteRenderer")]
    [BoxGroup("Actor Settings"), InfoBox("The Prefab has no Action Switch Sprite Component", InfoMessageType = InfoMessageType.Warning, VisibleIf = "hasNoSpriteSwitcher")]
    [BoxGroup("Actor Settings"), ShowIf("isPartOfInteractionSystem"), OnValueChanged("Check")]
    public bool shouldChangeSprite;

    bool hasNoAudioSource;
    bool hasNoAudioPlayer;
    [BoxGroup("Actor Settings"), InfoBox("The Prefab has no Audio Source Component", InfoMessageType = InfoMessageType.Warning, VisibleIf = "hasNoAudioSource")]
    [BoxGroup("Actor Settings"), InfoBox("The Prefab has no Action Play Audio Component", InfoMessageType = InfoMessageType.Warning, VisibleIf = "hasNoAudioPlayer")]
    [BoxGroup("Actor Settings"), ShowIf("isPartOfInteractionSystem"), OnValueChanged("Check")]
    public bool shouldPlaySound;

    bool hasNoLight2D;
    bool hasNoLightToggle;
    [BoxGroup("Actor Settings"), InfoBox("The Prefab has no Light 2D Component", InfoMessageType = InfoMessageType.Warning, VisibleIf = "hasNoLight2D")]
    [BoxGroup("Actor Settings"), InfoBox("The Prefab has no Action Set Light Intensity Component", InfoMessageType = InfoMessageType.Warning, VisibleIf = "hasNoLightToggle")]
    [BoxGroup("Actor Settings"), ShowIf("isPartOfInteractionSystem"), OnValueChanged("Check")]
    public bool shouldChangeLight;
    #endregion

    #region Interactor Settings
    bool hasNoInteractionInput;
    bool hasNoInteractionTrigger;
    bool hasNoTrigger;
    [BoxGroup("Interactor Settings"), InfoBox("The Prefab has no Collider 2D Component Setup as Trigger", InfoMessageType = InfoMessageType.Warning, VisibleIf = "@hasNoTrigger && shouldTriggerOnInput")]
    [BoxGroup("Interactor Settings"), InfoBox("The Prefab has no Interaction Input Component", InfoMessageType = InfoMessageType.Warning, VisibleIf = "@hasNoInteractionInput && shouldTriggerOnInput")]
    [BoxGroup("Interactor Settings"), ShowIf("@isPartOfInteractionSystem && isInteractor"), OnValueChanged("Check")]
    public bool shouldTriggerOnInput;
    [BoxGroup("Interactor Settings"), InfoBox("The Prefab has no Collider 2D Component Setup as Trigger", InfoMessageType = InfoMessageType.Warning, VisibleIf = "@hasNoTrigger && shouldTriggerOnCollide")]
    [BoxGroup("Interactor Settings"), InfoBox("The Prefab has no Interaction Trigger Component", InfoMessageType = InfoMessageType.Warning, VisibleIf = "@hasNoInteractionTrigger && shouldTriggerOnCollide")]
    [BoxGroup("Interactor Settings"), ShowIf("@isPartOfInteractionSystem && isInteractor"), OnValueChanged("Check")]
    public bool shouldTriggerOnCollide;
    #endregion

    PrefabDescription description;
    public bool hasDescription { get { return description != null; } }

    public void Init(PrefabDescription prefabDescription)
    {
        description = prefabDescription;
        shouldCollide = false;
        shouldChangeSprite = false;
        shouldPlaySound = false;
        shouldChangeLight = false;
    }

    bool isPartOfInteractionSystem { get { return description != null && description.interactionSystemDescription != InteractionSystemDescription.None; } }

    bool isInteractor { get { return description != null && description.interactionSystemDescription == InteractionSystemDescription.Interactor; } }

    public void Check()
    {
        if (isPartOfInteractionSystem)
        {
            hasNoCollider = (description.prefab.GetComponentInChildren<Collider2D>() == null) && shouldCollide;
            hasNoColliderToggler = (description.prefab.GetComponentInChildren<ActionColliderVisibility>() == null) && shouldCollide;

            hasNoSpriteRenderer = (description.prefab.GetComponentInChildren<SpriteRenderer>() == null) && shouldChangeSprite;
            hasNoSpriteSwitcher = (description.prefab.GetComponentInChildren<ActionSwitchSprite>() == null) && shouldChangeSprite;

            hasNoAudioSource = (description.prefab.GetComponentInChildren<AudioSource>() == null) && shouldPlaySound;
            hasNoAudioPlayer = (description.prefab.GetComponentInChildren<ActionPlayAudio>() == null) && shouldPlaySound;

            hasNoLight2D = (description.prefab.GetComponentInChildren<Light2D>() == null) && shouldChangeLight;
            hasNoLightToggle = (description.prefab.GetComponentInChildren<ActionSetLightIntensity>() == null) && shouldChangeLight;

            if (isInteractor)
            {
                hasNoTrigger = ((description.prefab.GetComponentInChildren<Collider2D>() == null) || (description.prefab.GetComponentInChildren<Collider2D>() == true && description.prefab.GetComponentInChildren<Collider2D>().isTrigger == false)) && (shouldTriggerOnCollide || shouldTriggerOnInput);
                hasNoInteractionInput = (description.prefab.GetComponentInChildren<InteractionInput>() == null) && shouldTriggerOnInput;
                hasNoInteractionTrigger = (description.prefab.GetComponentInChildren<InteractionTrigger>() == null) && shouldTriggerOnCollide;
            }
            else
            {
                hasNoTrigger = false;
                hasNoInteractionInput = false;
                hasNoInteractionTrigger = false;
            }
        }
        else
        {
            hasNoCollider = false;
            hasNoColliderToggler = false;
            hasNoSpriteRenderer = false;
            hasNoSpriteSwitcher = false;
            hasNoAudioSource = false;
            hasNoAudioPlayer = false;
            hasNoLight2D = false;
            hasNoLightToggle = false;
            hasNoTrigger = false;
            hasNoInteractionInput = false;
            hasNoInteractionTrigger = false;
        }
    }

    bool isMissingComponents
    {
        get
        {
            bool answer = false;
            if (isPartOfInteractionSystem)
            {
                answer = ((hasNoCollider || hasNoColliderToggler) && shouldCollide)
                       || ((hasNoSpriteRenderer || hasNoSpriteSwitcher) && shouldChangeSprite)
                       || ((hasNoAudioSource || hasNoAudioPlayer) && shouldPlaySound)
                       || ((hasNoLight2D || hasNoLightToggle) && shouldChangeLight);

                if (isInteractor && (shouldTriggerOnCollide || shouldTriggerOnInput))
                {
                    answer = answer
                        || hasNoTrigger
                        || hasNoInteractionInput
                        || hasNoInteractionTrigger;
                }
            }
            return answer;
        }
    }

    [Button, InfoBox("You can Add the Components with this Button, but you need to set them manually up"), ShowIf("isMissingComponents")]
    public void AddMissingComponents()
    {
        if (isPartOfInteractionSystem)
        {
            if (shouldCollide)
            {
                if (hasNoCollider)
                {
                    description.prefab.AddComponent<BoxCollider2D>();
                }
                if (hasNoColliderToggler)
                {
                    description.prefab.AddComponent<ActionColliderVisibility>();
                }
            }

            if (shouldChangeSprite)
            {
                if (hasNoSpriteRenderer)
                {
                    description.prefab.AddComponent<SpriteRenderer>();
                }
                if (hasNoSpriteSwitcher)
                {
                    description.prefab.AddComponent<ActionSwitchSprite>();
                }
            }

            if (shouldPlaySound)
            {
                if (hasNoAudioSource)
                {
                    description.prefab.AddComponent<AudioSource>();
                }
                if (hasNoAudioPlayer)
                {
                    description.prefab.AddComponent<ActionPlayAudio>();
                }
            }

            if (shouldChangeLight)
            {
                if (hasNoLight2D)
                {
                    description.prefab.AddComponent<Light2D>();
                }
                if (hasNoLightToggle)
                {
                    description.prefab.AddComponent<ActionSetLightIntensity>();
                }
            }
        }
        if (isInteractor && (shouldTriggerOnInput || shouldTriggerOnCollide))
        {
            if (hasNoTrigger)
            {
                Collider2D col;
                if (!description.prefab.GetComponentInChildren<Collider2D>())
                {
                    col = description.prefab.AddComponent<BoxCollider2D>();
                }
                else
                {
                    col = description.prefab.GetComponentInChildren<Collider2D>();

                }

                col.isTrigger = true;
            }

            if (hasNoInteractionInput)
            {
                description.prefab.AddComponent<InteractionInput>();
            }
            if (hasNoInteractionTrigger)
            {
                description.prefab.AddComponent<InteractionTrigger>();
            }
        }

        Check();
    }

    [Button]
    public void ShowPrefab()
    {
       // AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        AssetDatabase.OpenAsset(description.prefab);
    }
}
#endif
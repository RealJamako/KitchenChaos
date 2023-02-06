using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

[DisallowMultipleComponent]
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioRefSO audioSounds;

    private bool muteVolume;

    public bool MuteVolume { get { return muteVolume; } set { muteVolume = value; } }

    private void Awake()
    {
        InstanceCheck();
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeComplete += Instance_OnRecipeComplete;
        DeliveryManager.Instance.OnRecipeFailed += Instance_OnRecipeFailed;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        PlayerController.Instance.OnPlayerPickUp += Instance_OnPlayerPickUp;
        BaseCounter.OnAnyObjectPlaced += BaseCounter_OnAnyObjectPlaced;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }

    private void OnDisable()
    {
        DeliveryManager.Instance.OnRecipeComplete -= Instance_OnRecipeComplete;
        DeliveryManager.Instance.OnRecipeFailed -= Instance_OnRecipeFailed;
        CuttingCounter.OnAnyCut -= CuttingCounter_OnAnyCut;
        PlayerController.Instance.OnPlayerPickUp -= Instance_OnPlayerPickUp;
        BaseCounter.OnAnyObjectPlaced -= BaseCounter_OnAnyObjectPlaced;
        TrashCounter.OnAnyObjectTrashed -= TrashCounter_OnAnyObjectTrashed;
    }

    private void Instance_OnRecipeFailed()
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioSounds.DeliveryFail, deliveryCounter.transform.position);
    }

    private void Instance_OnRecipeComplete()
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioSounds.DeliverySuccess, deliveryCounter.transform.position);
    }

    private void Instance_OnPlayerPickUp(object sender, EventArgs e)
    {
        var playerController = sender as PlayerController;
        PlaySound(audioSounds.ObjectPickUp, playerController.transform.position);
    }

    private void BaseCounter_OnAnyObjectPlaced(object sender, EventArgs e)
    {
        var baseCounter = sender as BaseCounter;
        PlaySound(audioSounds.ObjectDrop, baseCounter.transform.position);
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, EventArgs e)
    {
        var trashCounter = sender as TrashCounter;
        PlaySound(audioSounds.Trash, trashCounter.transform.position);
    }

    private void CuttingCounter_OnAnyCut(object sender, EventArgs e)
    {
        var cuttingCounter = sender as CuttingCounter;
        PlaySound(audioSounds.ChopSounds, cuttingCounter.transform.position);
    }

    public void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }

    public void PlaySound(AudioClip[] arrayClips, Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(PlayRandom(arrayClips), position, muteVolume ? 0 : volume);
    }

    private AudioClip PlayRandom(AudioClip[] array)
    {
        if (array.Length < 1) { return array[0]; }
        return array[UnityRandom.Range(0, array.Length)];
    }

    public void PlayFoostepSounds(Vector3 position, float volume)
    {
        PlaySound(audioSounds.Footsteps, position, volume);
    }

    private void InstanceCheck()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one player instance");
            return;
        }
        else
        {
            Instance = this;
        }
    }
}
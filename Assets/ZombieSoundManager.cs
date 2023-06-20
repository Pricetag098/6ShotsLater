using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSoundManager : MonoBehaviour
{
    [SerializeField] SoundPlayer footstepSounds;
    [SerializeField] SoundPlayer voiceMovementSounds;
    [SerializeField] SoundPlayer voiceDeathSounds;


    public void PlayFootstep()
    {
        footstepSounds.Play();
    }

    public void PlayMovementVoice()
    {
        voiceMovementSounds.Play();
    }

    public void PlayDeathVoice()
    {
        voiceDeathSounds.Play();
    }







}


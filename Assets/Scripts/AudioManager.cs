using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // fanfare - freesound.org/people/CGEffex/sounds/99961/
    // results popup - freesound.org/people/MLaudio/sounds/615099/
    // end of day (owls) - freesound.org/people/ivolipa/sounds/353173/
    // button press - freesound.org/people/Christopherderp/sounds/342200/

    // music 1 - freesound.org/people/cormi/sounds/101981/
    // music 2 - freesound.org/people/TheoJT/sounds/510806/
    // music 3 - freesound.org/people/ZHR%C3%98/sounds/623427/

    // VARIABLES

    [SerializeField] private AudioSource[] musicSources;
    [SerializeField] AudioSource fanfare;
    [SerializeField] AudioSource result;
    [SerializeField] AudioSource owls;
    [SerializeField] AudioSource buttonPress;

    // the current music to play
    private int musicIndex = 0;
    // whether the music was stopped by the code
    private bool stopped = false;

    /// <summary>
    /// Play the fanfare sound.
    /// Also stops the day end sound
    /// </summary>
    public void PlayFanfare()
    {
        this.owls.Stop();
        this.fanfare.Play();
    }

    /// <summary>
    /// Play the results sound
    /// </summary>
    public void PlayResults()
    {
        this.result.Play();
    }

    /// <summary>
    /// Play the day end sound
    /// </summary>
    public void PlayDayEnd()
    {
        this.owls.Play();
    }

    /// <summary>
    /// Play the button press sound
    /// </summary>
    public void PlayButtonPress()
    {
        this.buttonPress.Play();
    }

    /// <summary>
    /// Play the music
    /// </summary>
    public void PlayMusic()
    {
        this.musicSources[this.musicIndex].Play();
        StartCoroutine(WaitUntilMusicDone());
    }

    /// <summary>
    /// Wait until the music clip has ended and then play the next clip
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitUntilMusicDone()
    {
        yield return new WaitForSeconds(this.musicSources[this.musicIndex].clip.length);
        this.musicIndex += 1;
        // if the music was not stopped by the code then continue playing with the next track
        if (!this.stopped)
        {
            if (this.musicIndex >= this.musicSources.Length) { this.musicIndex = 0; }
            PlayMusic();
        }
    }

    /// <summary>
    /// Stop the music
    /// </summary>
    public void StopMusic()
    {
        this.stopped = true;
        // a good way to stop all music - discussions.unity.com/t/how-to-stop-all-audio/32919/4
        for (var i = 0; i < this.musicSources.Length; i++)
        {
            this.musicSources[i].Stop();
        }
    }
}

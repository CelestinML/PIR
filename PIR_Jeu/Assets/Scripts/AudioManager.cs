using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] playlist;
    public AudioSource audioSource;
    private int played;
    // Start is called before the first frame update
    void Start()
    {
        int music = Random.Range(0, playlist.Length);
        audioSource.clip = playlist[music];
        audioSource.Play();
        played = music;
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayAnotherSong(played);
        }
    }

    public void PlayAnotherSong(int previous)
    {
        int music;
        do {
            music = Random.Range(0, playlist.Length);
        } while (music == previous);
        audioSource.clip = playlist[music];
        audioSource.Play();
        played = music;
    }
}

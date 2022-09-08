using NAudio.Wave;

namespace MediaPlayer;
class MusicPlayer
{
  public string CurrentSong
  {get; set;}

  public double CurrentTime
  {get; set;}

  public MusicPlayer()
  {
    this.CurrentSong = "";
    this.CurrentTime = 0;
  }

  public void Play(string mediafile)
  {
    using(var audioFile = new AudioFileReader(mediafile))
    using(var waveOut = new WaveOutEvent())
    {
        waveOut.Init(audioFile);
        waveOut.Play();
        while (waveOut.PlaybackState == PlaybackState.Playing)
        {
            Thread.Sleep(1000);
        }
    }
  }
}
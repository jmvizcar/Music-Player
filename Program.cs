using System;

namespace MediaPlayer;
class MusicPlayer
{
  // Music Player variable
  WMPLib.WindowsMediaPlayer MPlayer;

  public MusicPlayer()
  {
    MPlayer = new WMPLib.WindowsMediaPlayer();
  }

  private void PlayFile(string url)
  {

  }
  
}


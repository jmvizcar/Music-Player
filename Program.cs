using System;
using NAudio.Wave;

namespace MediaPlayer;
class Program
{
  static void Main(string[] args)
  {
    MusicPlayer mp = new MusicPlayer();
    mp.Play(@"C:\Users\Jesus\Music\FFXIV\ENDWALKER 7-inch Vinyl Single\2_Endwalker - Footfalls.mp3");
  }
}

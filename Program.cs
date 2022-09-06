using System;

namespace MediaPlayer;
class Program
{
  static void Main(string[] args)
  {
    MusicPlayer mp = new MusicPlayer();
    Console.WriteLine($"The Music Player is type {mp.GetType()}");
  }
}

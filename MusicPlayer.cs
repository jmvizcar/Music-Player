using NAudio.Wave;

namespace MusicPlayer;
public partial class MusicPlayer : Form
{
    private WaveOutEvent outputDevice;
    private AudioFileReader audioFile;

    public MusicPlayer()
    {
        InitializeComponent();
    }

    
}

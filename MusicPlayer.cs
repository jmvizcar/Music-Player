using NAudio.Wave;

namespace MusicPlayer;
public partial class MusicPlayer : Form
{
    private WaveOutEvent outputDevice;
    private AudioFileReader audioFile;

    public MusicPlayer()
    {
        outputDevice = null!;
        audioFile = null!;
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        var flowPanel = new FlowLayoutPanel();
        flowPanel.FlowDirection = FlowDirection.LeftToRight;
        flowPanel.Margin = new Padding(10);

        var buttonPlay = new Button();
        buttonPlay.Text = "Play";
        buttonPlay.Click += OnButtonPlayClick;
        flowPanel.Controls.Add(buttonPlay);

        var buttonStop = new Button();
        buttonStop.Text = "Stop";
        buttonStop.Click += OnButtonStopClick;
        flowPanel.Controls.Add(buttonStop);

        this.Controls.Add(flowPanel);
    }

    private void OnButtonPlayClick(object? sender, EventArgs args)
    {
        if (outputDevice == null)
        {
            outputDevice = new WaveOutEvent();
            outputDevice.PlaybackStopped += OnPlaybackStopped;
        }
        if (audioFile == null)
        {
            audioFile = new AudioFileReader(@"C:\Users\Jesus\Music\FFXIV\ENDWALKER 7-inch Vinyl Single\2_Endwalker - Footfalls.mp3");
            outputDevice.Init(audioFile);

        }
        outputDevice.Play();
    }
    private void OnButtonStopClick(object? sender, EventArgs args)
    {
        outputDevice?.Stop();
    }

    private void OnPlaybackStopped(object? sender, StoppedEventArgs args)
    {
        outputDevice.Dispose();
        outputDevice = null!;
        audioFile.Dispose();
        audioFile = null!;
    }

}

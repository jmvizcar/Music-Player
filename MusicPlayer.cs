using NAudio.Wave;
using System.Collections.Generic;

namespace MusicPlayer;
public partial class MusicPlayer : Form
{
    private WaveOutEvent outputDevice;
    private AudioFileReader audioFile;
    private bool closing;
    private string path;
    // Variable to hold the full list of albums currently in the Music directory.
    private List<string> musicDirect;
    public List<string> currentPlaylist;
    public string CurrentSong
    {get; set;}
    public double CurrentTime
    {get; set;}

    public MusicPlayer()
    {
        outputDevice = null!;
        audioFile = null!;
        closing = false;
        CurrentSong = "";
        CurrentTime = 0;
        currentPlaylist = new List<string>();
        path = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
        // Adds all mp3 files to the directory list
        musicDirect = new List<string>(Directory.GetFileSystemEntries(path, "*.mp3", SearchOption.AllDirectories));
        // Adds all m4a files to the directory list
        musicDirect.AddRange(Directory.GetFileSystemEntries(path, "*.m4a", SearchOption.AllDirectories));
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        var flowPanel = new FlowLayoutPanel();
        flowPanel.FlowDirection = FlowDirection.LeftToRight;
        flowPanel.Margin = new Padding(10);
        flowPanel.Width = 300;

        var buttonPlay = new Button();
        buttonPlay.Text = "Play";
        buttonPlay.Click += OnButtonPlayClick;
        flowPanel.Controls.Add(buttonPlay);

        var buttonPause = new Button();
        buttonPause.Text = "Pause";
        buttonPause.Click += OnButtonPauseClick;
        flowPanel.Controls.Add(buttonPause);

        var buttonStop = new Button();
        buttonStop.Text = "Stop";
        buttonStop.Click += OnButtonStopClick;
        flowPanel.Controls.Add(buttonStop);

        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(800, 450);
        this.Text = "Music Player";
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
            audioFile = new AudioFileReader(path + @"\FFXIV\ENDWALKER 7-inch Vinyl Single\2_Endwalker - Footfalls.mp3");
            outputDevice.Init(audioFile);

        }
        outputDevice.Play();
        MessageBox.Show(musicDirect[musicDirect.Count - 1]);
    }
    private void OnButtonPauseClick(object? sender, EventArgs args)
    {
        outputDevice?.Stop();
    }
    private void OnButtonStopClick(object? sender, EventArgs args)
    {
        outputDevice?.Stop();
        audioFile.Position = 0;
    }

    private void OnPlaybackStopped(object? sender, StoppedEventArgs args)
    {
        if(closing)
        {
            outputDevice.Dispose();
            outputDevice = null!;
            audioFile.Dispose();
            audioFile = null!;
        }
    }

}

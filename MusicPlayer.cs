using NAudio.Wave;
using System.Collections.Generic;

namespace MusicPlayer;

public static class WaveStreamExtensions
{
    // Set position of WaveStream to nearest block to supplied position
    public static void SetPosition(this WaveStream strm, long position)
    {
        // Distance from block boundary
        long adjust = position % strm.WaveFormat.BlockAlign;
        // Clamps the range
        long newPos = Math.Max(0, Math.Min(strm.Length, position - adjust));
        strm.Position = newPos;
    }

    // Set position of WaveStream by seconds
    public static void SetPosition(this WaveStream strm, double seconds)
    {
        strm.SetPosition((long)(seconds * strm.WaveFormat.AverageBytesPerSecond));
    }

    // Set position of WaveStream by TimeSpan
    public static void SetPosition(this WaveStream strm, TimeSpan time)
    {
        strm.SetPosition(time.TotalSeconds);
    }

    // Set position of WaveStream relative to current position
    public static void Seek(this WaveStream strm, double offset){
        strm.SetPosition(strm.Position + (long)(offset * strm.WaveFormat.AverageBytesPerSecond));
    }
}

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
        var allSongs = new TextBox();
        allSongs.Multiline = true;
        allSongs.ReadOnly = true;
        allSongs.SelectionStart = allSongs.Text.Length;
        allSongs.BackColor = Form.DefaultBackColor;
        allSongs.BorderStyle = BorderStyle.None;
        allSongs.Height = 300;
        allSongs.Width = 800;
        allSongs.ScrollToCaret();
        
        musicDirect.ForEach(song => allSongs.AppendText(song));

        var flowPanel = new FlowLayoutPanel();
        flowPanel.FlowDirection = FlowDirection.LeftToRight;
        flowPanel.Margin = new Padding(10);
        flowPanel.Width = 300;
        flowPanel.Top = allSongs.Bottom;

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

        var volumeBar = new TrackBar() { Minimum = 0, Maximum = 100, Value = 100, Location = new Point(700, 400),
            TickFrequency = 10, Orientation = Orientation.Horizontal, Height = 100};
            
        volumeBar.Scroll += (s, a) => outputDevice.Volume = volumeBar.Value / 100f;

        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(800, 450);
        this.Text = "Music Player";
        this.Controls.Add(allSongs);
        this.Controls.Add(flowPanel);
        this.Controls.Add(volumeBar);
    }

    private void OnButtonPlayClick(object? sender, EventArgs args)
    {
        // Test string used to hold "Endwalker - Footfalls.mp3"
        string song = musicDirect.Find((title) => title.Contains("Footfalls.mp3"))!;
        if (outputDevice == null)
        {
            outputDevice = new WaveOutEvent();
            outputDevice.PlaybackStopped += OnPlaybackStopped;
        }
        if (audioFile == null)
        {
            audioFile = new AudioFileReader(song);
            CurrentSong = song.Substring(song.IndexOf("Endwalker"));
            outputDevice.Init(audioFile);

        }
        outputDevice.Play();
        MessageBox.Show($"Now Playing {CurrentSong}\nWhich is {audioFile.TotalTime} long.");
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

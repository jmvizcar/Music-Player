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

    private int playlistPos;
    public List<string> currentPlaylist;
    public string CurrentSong
    {get; set;}
    public double CurrentTime
    {get; set;}

    public bool Shuffle
    {get; set;}

    public MusicPlayer()
    {
        outputDevice = null!;
        audioFile = null!;
        closing = false;
        CurrentSong = "";
        CurrentTime = 0;
        Shuffle = false;
        playlistPos = 0;
        currentPlaylist = new List<string>();
        path = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
        // Adds all mp3 files to the directory list
        musicDirect = new List<string>(Directory.GetFileSystemEntries(path, "*.mp3", SearchOption.AllDirectories));
        // Adds all m4a files to the directory list
        musicDirect.AddRange(Directory.GetFileSystemEntries(path, "*.m4a", SearchOption.AllDirectories));
        currentPlaylist = musicDirect;
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

        var buttonNext = new Button();
        buttonNext.Text = "Next";
        buttonNext.Click += OnButtonNextClick;
        buttonNext.Click += OnButtonPlayClick;
        flowPanel.Controls.Add(buttonNext);

        var buttonPrev = new Button();
        buttonPrev.Text = "Prev";
        buttonPrev.Click += OnButtonPrevClick;
        buttonPrev.Click += OnButtonPlayClick;
        flowPanel.Controls.Add(buttonPrev);

        var buttonShuf = new Button();
        buttonShuf.Text = "Shuffle";
        buttonShuf.Click += OnClickToggleShuffle;
        flowPanel.Controls.Add(buttonShuf);


        var volumeBar = new TrackBar() { Minimum = 0, Maximum = 100, Value = 100, Location = new Point(700, 400),
            TickFrequency = 10, Orientation = Orientation.Horizontal, Height = 100};
            
        volumeBar.Scroll += (s, a) => outputDevice.Volume = volumeBar.Value / 100f;

        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(800, 450);
        this.Text = "Music Player";
        this.Controls.Add(flowPanel);
        this.Controls.Add(volumeBar);
    }

    private void OnButtonPlayClick(object? sender, EventArgs args)
    {
        // Song string used to hold the first song in the music directory.
        if(Shuffle)
        {   
            Random rng = new Random();
            currentPlaylist = musicDirect.OrderBy(i => rng.Next()).ToList();
        }
        else 
        {
            currentPlaylist = musicDirect;
        }
        string song = currentPlaylist.ToArray()[playlistPos];
        if (outputDevice == null)
        {
            outputDevice = new WaveOutEvent();
            outputDevice.PlaybackStopped += OnPlaybackStopped;
        }
        if (audioFile == null)
        {
            audioFile = new AudioFileReader(song);
            CurrentSong = song;
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

    /*
     * This will advance to the next song in the playlist.
     * We set audioFile to null to force a new play from OnButtonPlayClick
     */
    private void OnButtonNextClick(object? sender, EventArgs args)
    {
        outputDevice?.Stop();
        audioFile = null!;
        playlistPos++;
    }
    /* This will return to the previous song in the playlist similar to OnButtonNextClick */
    private void OnButtonPrevClick(object? sender, EventArgs args)
    {
        outputDevice?.Stop();
        audioFile = null!;
        // Check to see if at the beginning of the playlist.
        if( playlistPos > 0 ) playlistPos--;
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

    private void OnClickToggleShuffle(object? sender, EventArgs args)
    {
        Shuffle = !Shuffle;
    }

}

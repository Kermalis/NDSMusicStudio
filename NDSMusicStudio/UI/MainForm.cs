using Kermalis.NDSMusicStudio.Core;
using Kermalis.NDSMusicStudio.Core.FileSystem;
using Kermalis.NDSMusicStudio.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Kermalis.NDSMusicStudio.UI
{
    [DesignerCategory("")]
    class MainForm : ThemedForm
    {
        public static MainForm Instance { get; } = new MainForm();

        bool stopUI = false;
        readonly List<byte> pianoNotes = new List<byte>();
        public readonly bool[] PianoTracks = new bool[0x10];

        const int iWidth = 528, iHeight = 800 + 25; // +25 for menustrip (24) and splitcontainer separator (1)
        const float sfWidth = 2.35f; // Song combobox and volumebar width
        const float spfHeight = 5.5f; // Split panel 1 height

        #region Controls

        IContainer components;
        MenuStrip mainMenu;
        ToolStripMenuItem fileToolStripMenuItem, openSDATToolStripMenuItem, configToolStripMenuItem;
        Timer timer;
        ThemedNumeric songNumerical;
        ThemedButton playButton, pauseButton, stopButton;
        SplitContainer splitContainer;
        PianoControl piano;
        ColorSlider volumeBar;
        TrackInfoControl trackInfo;
        ComboBox songsComboBox;

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        private MainForm()
        {
            for (int i = 0; i < 0x10; i++)
            {
                PianoTracks[i] = true;
            }

            components = new Container();

            // Main Menu
            openSDATToolStripMenuItem = new ToolStripMenuItem { Text = "Open SDAT", ShortcutKeys = Keys.Control | Keys.O };
            openSDATToolStripMenuItem.Click += OpenSDAT;

            configToolStripMenuItem = new ToolStripMenuItem { Text = "Refresh Config", ShortcutKeys = Keys.Control | Keys.R };
            configToolStripMenuItem.Click += ReloadConfig;

            fileToolStripMenuItem = new ToolStripMenuItem { Text = "File" };
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openSDATToolStripMenuItem, configToolStripMenuItem });


            mainMenu = new MenuStrip { Size = new Size(iWidth, 24) };
            mainMenu.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });

            // Buttons
            playButton = new ThemedButton { ForeColor = Color.MediumSpringGreen, Location = new Point(5, 3), Text = "Play" };
            playButton.Click += (o, e) => Play();
            pauseButton = new ThemedButton { ForeColor = Color.DeepSkyBlue, Location = new Point(85, 3), Text = "Pause" };
            pauseButton.Click += (o, e) => Pause();
            stopButton = new ThemedButton { ForeColor = Color.MediumVioletRed, Location = new Point(166, 3), Text = "Stop" };
            stopButton.Click += (o, e) => Stop();

            playButton.Enabled = pauseButton.Enabled = stopButton.Enabled = false;
            playButton.Size = stopButton.Size = new Size(75, 23);
            pauseButton.Size = new Size(76, 23);

            // Numericals
            songNumerical = new ThemedNumeric { Enabled = false, Location = new Point(246, 4), Minimum = ushort.MinValue };

            songNumerical.Size = new Size(45, 23);
            songNumerical.ValueChanged += (o, e) => LoadSong();

            // Timer
            timer = new Timer(components);
            timer.Tick += UpdateUI;

            // Piano
            piano = new PianoControl { Anchor = AnchorStyles.Bottom, Location = new Point(0, 125 - 50 - 1), Size = new Size(iWidth, 50) };

            // Volume bar
            int sWidth = (int)(iWidth / sfWidth);
            int sX = iWidth - sWidth - 4;
            volumeBar = new ColorSlider
            {
                Enabled = false,
                LargeChange = 20,
                Location = new Point(83, 45),
                Maximum = 100,
                Size = new Size(155, 27),
                SmallChange = 5
            };
            volumeBar.ValueChanged += VolumeBar_ValueChanged;

            // Playlist box
            songsComboBox = new ComboBox
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Enabled = false,
                Location = new Point(sX, 4),
                Size = new Size(sWidth, 23)
            };
            songsComboBox.SelectedIndexChanged += SongsComboBox_SelectedIndexChanged;

            // Track info
            trackInfo = new TrackInfoControl
            {
                Dock = DockStyle.Fill,
                Size = new Size(iWidth, 690)
            };

            // Split container
            splitContainer = new SplitContainer
            {
                BackColor = Theme.TitleBar,
                Dock = DockStyle.Fill,
                FixedPanel = FixedPanel.Panel1,
                IsSplitterFixed = true,
                Orientation = Orientation.Horizontal,
                Size = new Size(iWidth, iHeight),
                SplitterDistance = 125,
                SplitterWidth = 1
            };
            splitContainer.Panel1.Controls.AddRange(new Control[] { playButton, pauseButton, stopButton, songNumerical, songsComboBox, piano, volumeBar });
            splitContainer.Panel2.Controls.Add(trackInfo);

            // MainForm
            AutoScaleDimensions = new SizeF(6, 13);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(iWidth, iHeight);
            Controls.AddRange(new Control[] { splitContainer, mainMenu });
            MainMenuStrip = mainMenu;
            MinimumSize = new Size(8 + iWidth + 8, 30 + iHeight + 8); // Borders
            SongPlayer.Instance.SongEnded += SongEnded;
            Resize += OnResize;
            Text = "NDS Music Studio";
        }

        SDAT sdat;

        void VolumeBar_ValueChanged(object sender, EventArgs e)
        {
            SoundMixer.Instance.SetVolume(volumeBar.Value / (float)volumeBar.Maximum);
        }
        public void SetVolumeBarValue(float volume)
        {
            volumeBar.ValueChanged -= VolumeBar_ValueChanged;
            volumeBar.Value = (int)(volume * volumeBar.Maximum);
            volumeBar.ValueChanged += VolumeBar_ValueChanged;
        }

        string GetLabelForSong(int index)
        {
            if (sdat.SYMBBlock == null)
            {
                return index.ToString();
            }
            else
            {
                return sdat.SYMBBlock.SequenceSymbols.Entries[index];
            }
        }
        void LoadSong()
        {
            songsComboBox.SelectedIndexChanged -= SongsComboBox_SelectedIndexChanged;

            var index = (int)songNumerical.Value;
            SDAT.INFO.SequenceInfo song = sdat.INFOBlock.SequenceInfos.Entries[index];
            if (song != null)
            {
                string label = GetLabelForSong(index);
                Text = "NDS Music Studio - " + label;
                songsComboBox.SelectedIndex = songsComboBox.Items.IndexOf(label);
            }
            else
            {
                Text = "NDS Music Studio";
                songsComboBox.SelectedIndex = -1;
            }
            bool playing = SongPlayer.Instance.State == PlayerState.Playing; // Play new song if one is already playing
            bool paused = SongPlayer.Instance.State == PlayerState.Paused;
            Stop();
            try
            {
                if (!paused)
                {
                    SongPlayer.Instance.Pause();
                }
                SongPlayer.Instance.SetSong(sdat, index);
                if (!paused)
                {
                    SongPlayer.Instance.Stop();
                }
                trackInfo.DeleteData();
                if (playing)
                {
                    Play();
                }
                else
                {
                    pauseButton.Text = "Pause";
                }
            }
            catch (Exception ex)
            {
                FlexibleMessageBox.Show(ex.Message, string.Format("Error Loading Song {0}", songNumerical.Value));
            }

            songsComboBox.SelectedIndexChanged += SongsComboBox_SelectedIndexChanged;
        }
        void SongsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = (string)songsComboBox.SelectedItem;
            if (sdat.SYMBBlock == null)
            {
                songNumerical.Value = int.Parse(item);
            }
            else
            {
                songNumerical.Value = Array.IndexOf(sdat.SYMBBlock.SequenceSymbols.Entries, item);
            }
        }
        // Allow MainForm's thread to do the next work in UpdateUI()
        void SongEnded()
        {
            stopUI = true;
        }

        void OpenSDAT(object sender, EventArgs e)
        {
            var d = new OpenFileDialog { Title = "Open SDAT", Filter = "SDAT Files|*.sdat" };
            if (d.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            Stop();

            try
            {
                sdat = new SDAT(File.ReadAllBytes(d.FileName));
                songsComboBox.Items.Clear();
                songsComboBox.Items.AddRange(Enumerable.Range(0, sdat.INFOBlock.SequenceInfos.NumEntries).Where(i => sdat.INFOBlock.SequenceInfos.Entries[i] != null).Select(i => GetLabelForSong(i)).Cast<object>().ToArray());

                songsComboBox.SelectedIndex = 0;
                SongsComboBox_SelectedIndexChanged(null, null); // Why doesn't it work on its own??
                LoadSong();
                songNumerical.Maximum = sdat.INFOBlock.SequenceInfos.NumEntries - 1;
                songsComboBox.Enabled = songNumerical.Enabled = playButton.Enabled = volumeBar.Enabled = true;
            }
            catch (Exception ex)
            {
                FlexibleMessageBox.Show(ex.Message, "Error Loading SDAT");
            }
        }
        void ReloadConfig(object sender, EventArgs e)
        {
            Config.Instance.Load();
        }

        void Play()
        {
            SongPlayer.Instance.Play();
            pauseButton.Enabled = stopButton.Enabled = true;
            pauseButton.Text = "Pause";
            timer.Interval = (int)(1000f / Config.Instance.RefreshRate);
            timer.Start();
        }
        void Pause()
        {
            SongPlayer.Instance.Pause();
            if (SongPlayer.Instance.State != PlayerState.Paused)
            {
                stopButton.Enabled = true;
                pauseButton.Text = "Pause";
                timer.Start();
            }
            else
            {
                stopButton.Enabled = false;
                pauseButton.Text = "Unpause";
                timer.Stop();
                System.Threading.Monitor.Enter(timer);
                ClearPianoNotes();
            }
        }
        void Stop()
        {
            SongPlayer.Instance.Stop();
            pauseButton.Enabled = stopButton.Enabled = false;
            timer.Stop();
            System.Threading.Monitor.Enter(timer);
            ClearPianoNotes();
            trackInfo.DeleteData();
        }

        void ClearPianoNotes()
        {
            foreach (byte n in pianoNotes)
            {
                if (n >= piano.LowNoteID && n <= piano.HighNoteID)
                {
                    piano[n - piano.LowNoteID].NoteOnColor = Color.DeepSkyBlue;
                    piano.ReleasePianoKey(n);
                }
            }
            pianoNotes.Clear();
        }
        void UpdateUI(object sender, EventArgs e)
        {
            if (!System.Threading.Monitor.TryEnter(timer))
            {
                return;
            }
            try
            {
                // Song ended in SongPlayer
                if (stopUI)
                {
                    stopUI = false;
                    Stop();
                }
                // Draw
                else
                {
                    // Draw piano notes
                    ClearPianoNotes();
                    TrackInfo info = trackInfo.Info;
                    SongPlayer.Instance.GetSongState(info);
                    for (int i = 0xF; i >= 0; i--)
                    {
                        if (!PianoTracks[i])
                        {
                            continue;
                        }

                        byte[] notes = info.Notes[i];
                        pianoNotes.AddRange(notes);
                        foreach (byte n in notes)
                        {
                            if (n >= piano.LowNoteID && n <= piano.HighNoteID)
                            {
                                piano[n - piano.LowNoteID].NoteOnColor = Config.Instance.Colors[info.Voices[i]];
                                piano.PressPianoKey(n);
                            }
                        }
                    }
                    // Draw trackinfo
                    trackInfo.Invalidate();
                }
            }
            finally
            {
                System.Threading.Monitor.Exit(timer);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Stop();
            SongPlayer.Instance.ShutDown();
            base.OnFormClosing(e);
        }
        void OnResize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                return;
            }

            // Song combobox
            int sWidth = (int)(splitContainer.Width / sfWidth);
            int sX = splitContainer.Width - sWidth - 4;
            songsComboBox.Location = new Point(sX, 4);
            songsComboBox.Size = new Size(sWidth, 23);

            splitContainer.SplitterDistance = (int)((Height - 38) / spfHeight) - 24 - 1;

            // Piano
            piano.Size = new Size(splitContainer.Width, (int)(splitContainer.Panel1.Height / 2.5f)); // Force it to initialize piano keys again
            int targetWhites = piano.Width / 10; // Minimum width of a white key is 10 pixels
            int targetAmount = (targetWhites / 7 * 12).Clamp(1, 128); // 7 white keys per octave
            int offset = targetAmount / 2 - ((targetWhites / 7) % 2);
            piano.LowNoteID = Math.Max(0, 60 - offset);
            piano.HighNoteID = (60 + offset - 1) >= 120 ? 127 : (60 + offset - 1);

            int wWidth = piano[0].Width; // White key width
            int dif = splitContainer.Width - wWidth * piano.WhiteKeyCount;
            piano.Location = new Point(dif / 2, splitContainer.Panel1.Height - piano.Height - 1);
            piano.Invalidate(true);
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (playButton.Enabled && !songsComboBox.Focused && keyData == Keys.Space)
            {
                if (SongPlayer.Instance.State == PlayerState.Stopped)
                {
                    Play();
                }
                else
                {
                    Pause();
                }
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}

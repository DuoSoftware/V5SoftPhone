using DuoSoftware.DuoTools.DuoLogger;
using System;
using System.Windows.Forms;

namespace DuoSoftware.DuoSoftPhone.UI
{
    /// <summary>
    /// Audio setting
    /// </summary>
    public struct SoundSetting
    {
        public ComboBox ComboBoxMicrophones;
        public ComboBox ComboBoxSpeakers;
        public int MicVolume;
        public int SpeakerVolume;
        public int MicIndex;
        public int SepkIndex;
        public bool MicMute;
        public bool SpkMute;

        
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            var c = (SoundSetting)obj;
            return ((ComboBoxMicrophones == c.ComboBoxMicrophones) && (ComboBoxSpeakers == c.ComboBoxSpeakers) && (ComboBoxSpeakers == c.ComboBoxSpeakers)&& (MicVolume == c.MicVolume)&& (SpeakerVolume == c.SpeakerVolume) && (MicIndex == c.MicIndex) && (SepkIndex == c.SepkIndex));
        }
        public override int GetHashCode()
        {
            return 0;
        }
        public static bool operator ==(SoundSetting left, SoundSetting right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(SoundSetting left, SoundSetting right)
        {
            return !left.Equals(right);
        }

    }

    /// <summary>
    /// AudioWiz
    /// </summary>
    public partial class AudioWiz : Form
    {
        /// <summary>
        /// AudioPlayLoopbackTest
        /// </summary>
        /// <param name="value"></param>
        public delegate void AudioPlayLoopbackTest(bool value);

        /// <summary>
        /// OnAudioPlayTest
        /// </summary>
        public event AudioPlayLoopbackTest OnAudioPlayTest;

        /// <summary>
        /// Volume
        /// </summary>
        /// <param name="value"></param>
        public delegate void Volume(int value);

        /// <summary>
        /// OnSpeakerVolumeChanged
        /// </summary>
        public event Volume OnSpeakerChanged;

        /// <summary>
        /// OnMicVolumeChanged
        /// </summary>
        public event Volume OnMicChanged;

        /// <summary>
        /// Mute
        /// </summary>
        /// <param name="value"></param>
        public delegate void Mute(bool value);

        /// <summary>
        /// OnSpeakerMute
        /// </summary>
        public event Mute OnSpeaker;

        /// <summary>
        /// OnMicMute
        /// </summary>
        public event Mute OnMic;

        /// <summary>
        /// AudioWiz
        /// </summary>
        /// <param name="soundSetting"></param>
        public AudioWiz(SoundSetting soundSetting)
        {
            InitializeComponent();
            var control = gbControls.Controls;
            control.Add(soundSetting.ComboBoxMicrophones);
            control.Add(soundSetting.ComboBoxSpeakers);
            soundSetting.ComboBoxMicrophones.BringToFront();
            soundSetting.ComboBoxSpeakers.BringToFront();
            if (soundSetting.ComboBoxMicrophones.Items.Count >= soundSetting.MicIndex)
            {
                soundSetting.ComboBoxMicrophones.SelectedIndex = soundSetting.MicIndex;
            }

            if (soundSetting.ComboBoxSpeakers.Items.Count >= soundSetting.SepkIndex)
            {
                soundSetting.ComboBoxSpeakers.SelectedIndex = soundSetting.SepkIndex;
            }

            //TrackBarSpeaker. SetRange(0, 255);
            TrackBarSpeaker.Value = soundSetting.SpeakerVolume;

            //TrackBarMicrophone.SetRange(0, 255);
            TrackBarMicrophone.Value = soundSetting.MicVolume;

            chkboxMuteSpeaker.Checked = soundSetting.SpkMute;
            CheckBoxMute.Checked = soundSetting.MicMute;
        }

        /// <summary>
        /// AudioWiz_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AudioWiz_Load(object sender, EventArgs e)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "AudioWiz Open", Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "AudioWiz_Load", exception, Logger.LogLevel.Error);
            }
        }

        /// <summary>
        /// ButtonTestAudio_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonTestAudio_Click(object sender, EventArgs e)
        {
            try
            {
                if (ButtonTestAudio.Text == "Test Audio")
                {
                    if (OnAudioPlayTest == null) return;
                    ButtonTestAudio.Text = @"Stop Test";
                    OnAudioPlayTest(true);
                }
                else
                {
                    if (OnAudioPlayTest == null) return;
                    ButtonTestAudio.Text = @"Test Audio";
                    OnAudioPlayTest(false);
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "ButtonTestAudio_Click", exception, Logger.LogLevel.Error);
            }
        }

        /// <summary>
        /// TrackBarSpeaker_ValueChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TrackBarSpeaker_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (OnSpeakerChanged == null) return;
                chkboxMuteSpeaker.Checked = false;
                OnSpeakerChanged(TrackBarSpeaker.Value);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "ButtonTestAudio_Click", exception, Logger.LogLevel.Error);
            }
        }

        /// <summary>
        /// TrackBarMicrophone_ValueChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TrackBarMicrophone_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (OnMicChanged == null) return;
                CheckBoxMute.Checked = false;
                OnMicChanged(TrackBarMicrophone.Value);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "TrackBarMicrophone_ValueChanged", exception, Logger.LogLevel.Error);
            }
        }

        /// <summary>
        /// CheckBoxMute_CheckedChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBoxMute_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (OnMic != null)
                {
                    OnMic(CheckBoxMute.Checked);
                }

                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "AudioWiz MuteMicrophone - " + CheckBoxMute.Checked, Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "CheckBoxMute_CheckedChanged", exception, Logger.LogLevel.Error);
            }
        }

        /// <summary>
        /// chkboxMuteSpeaker_CheckedChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChkboxMuteSpeaker_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (OnSpeaker != null)
                {
                    OnSpeaker(chkboxMuteSpeaker.Checked);
                }

                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "AudioWiz MuteSpeaker - " + chkboxMuteSpeaker.Checked, Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "chkboxMuteSpeaker_CheckedChanged", exception, Logger.LogLevel.Error);
            }
        }

        /// <summary>
        /// AudioWiz_FormClosed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AudioWiz_FormClosed(object sender, FormClosedEventArgs e)
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "AudioWiz Close", Logger.LogLevel.Info);
        }

        /// <summary>
        /// gbControls_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gbControls_Click(object sender, EventArgs e)
        {
        }
    }
}
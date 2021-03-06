﻿using Infragistics.Win;
using Infragistics.Win.Misc;

//
using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DuoSoftware.DuoSoftPhone.Controllers
{
    #region ScrollingMarqueeLabelDrawFilter

    /// <summary>
    /// Normally an element's appearance can be easily controlled by setting various properties on Appearance
    /// objects exposed by Presentation Layer Framework (PLF) based controls.  However, occasionally a requirement
    /// comes along that isn't supported by the control. This is where a draw filter is invaluable.
    ///
    /// All controls that are based on the PLF expose a very flexible, fine-grained drawing extensibility mechanism.
    /// To customize the drawing of one or more UIElements of a control you need to implement the
    /// IUIElementDrawFilter interface on an object and set the DrawFilter property of the control to that
    /// object at runtime.
    ///
    /// This Draw Filter turns an UltraLabel into a scrolling marquee.  It does not modify the Text property of the
    /// UltraLabel, so you can reliably use the label's Text property when using this filter.
    /// </summary>
    public class ScrollingMarqueeLabelDrawFilter : IUIElementDrawFilter
    {
        #region Data

        private readonly UltraLabel _label;
        private bool _scrollLeft;
        private StringBuilder _text;
        private readonly Timer _timer = new Timer();
        private bool _hasScrolled;

        #endregion Data

        #region IUIElementDrawFilter Members

        #region GetPhasesToFilter

        /// <summary>
        /// This method is passed a UIElementDrawParams structure and returns a bit-flag enumeration called DrawPhase.
        /// The passed in structure exposes a property that returns the element to be rendered as well as properties
        /// and methods to support rendering operations like Graphics, BackBrush, DrawBorders etc. The returned
        /// DrawPhase bit-flags specify which phase(s) of the drawing operation to filter for this element
        /// (the DrawElement method below will be called for each bit returned).
        /// The DrawPhase enumeration allows you to filter the drawing before or after each drawing operation of
        /// an element (e.g. theme, backcolor, image background, borders, foreground, image and/or child elements).
        /// </summary>
        public DrawPhase GetPhasesToFilter(ref UIElementDrawParams drawParams)
        {
            
            // Indicates that we want this filter's DrawElement method to be invoked prior to when the
            // foreground (text) of the control is drawn.
            return drawParams.Element is TextUIElementBase ? DrawPhase.BeforeDrawForeground : DrawPhase.None;
        }

        #endregion GetPhasesToFilter

        #region DrawElement

        /// <summary>
        /// This method is passed the same UIElementDrawParams structure as GetPhasesToFilter() and a bit flag indicating
        /// which single draw phase is being performed. The method returns a boolean. If false is returned then the default
        /// drawing for that phase will be performed. If true is returned for a 'Before' phase then the default drawing
        /// for that phase will be skipped. Note: returning true for the BeforeDrawElement phase will cause all the other
        /// phases to be skipped (even if bits for those phases were returned by the call to GetPhasesToFilter).
        /// Also, if themes are active, returning true for the BeforeDrawTheme phase will skip all phases up to but not
        /// including the BeforeDrawChildElements phase. The BeforeDrawFocus phase is only called if the control has focus
        /// (or the forceDrawAsFocused parameter was set to true on the call to the Draw method) and the element's virtual
        /// DrawsFocusRect property returns true.
        /// </summary>
        public bool DrawElement(DrawPhase drawPhase, ref UIElementDrawParams drawParams)
        {
            
            // The TextHAlign set on the label must be honored, so we setup a StringFormat
            // to use when drawing the text.
            HAlign align = drawParams.AppearanceData.TextHAlign;
            StringFormat frmt = new StringFormat();
            if (align == HAlign.Left)
                frmt.Alignment = StringAlignment.Near;
            else
                if (align == HAlign.Center)
                    frmt.Alignment = StringAlignment.Center;
                else
                    if (align == HAlign.Right)
                        frmt.Alignment = StringAlignment.Far;

            // Draw the text.
            drawParams.Graphics.DrawString(
                _text.ToString(),
                drawParams.Font,
                drawParams.TextBrush,
                drawParams.Element.RectInsideBorders,
                frmt
                );

            if (!_hasScrolled)
            {
                // Rearrange the characters in our private copy of the label's Text so that the next
                // time we draw the string it will be "scrolled" by one character.
                UpdateText();

                // Since we only want to scroll the text once per tick of the timer,
                // set this flag now to prevent this block of code from executing until the next tick.
                _hasScrolled = true;
            }

            // Returning true prevents the text we just drew from being drawn over.
            return true;
        }

        #endregion DrawElement

        #endregion IUIElementDrawFilter Members

        #region Public Interface

        #region Constructors

        public ScrollingMarqueeLabelDrawFilter(UltraLabel label)
        {
            if(label==null) return;
            _label = label;
            _label.TextChanged += Label_TextChanged;
            _text = new StringBuilder(label.Text);
            _timer.Interval = 250;
            _timer.Tick += Timer_Tick;
            _scrollLeft = true;
        }

        public ScrollingMarqueeLabelDrawFilter(UltraLabel label, bool scrollLeft)
            : this(label)
        {
            _scrollLeft = scrollLeft;
        }

        public ScrollingMarqueeLabelDrawFilter(UltraLabel label, bool scrollLeft, int pauseTime)
            : this(label, scrollLeft)
        {
            if (_timer != null) _timer.Interval = pauseTime;
        }

        #endregion Constructors

        #region PauseTime

        /// <summary>
        /// Gets and sets the number of milliseconds that elapse between every time the text scrolls.
        /// </summary>
        public int PauseTime
        {
            get
            {
                return _timer.Interval;
            }
            set
            {
                if (value > 0)
                    _timer.Interval = value;
            }
        }

        #endregion PauseTime

        #region StartScrolling

        /// <summary>
        /// Causes the text to begin scrolling.
        /// </summary>
        public void StartScrolling()
        {
            if (_timer != null) _timer.Start();
        }

        #endregion StartScrolling

        #region StopScrolling

        /// <summary>
        /// Causes the text to stop scrolling.
        /// </summary>
        public void StopScrolling()
        {
            if (_timer != null) _timer.Stop();
        }

        #endregion StopScrolling

        #region ScrollLeft

        /// <summary>
        /// Gets and sets a bool which indicates if the text should scroll to the left or to the right.
        /// </summary>
        public bool ScrollLeft
        {
            get
            {
                return _scrollLeft;
            }
            set
            {
                _scrollLeft = value;
            }
        }

        #endregion ScrollLeft

        #endregion Public Interface

        #region Private Helpers

        #region UpdateText

        // This method moves the characters of the display text so that it appears that the text is scrolling.
        // Note, the Text property of the UltraLabel will not be changed by this Draw Filter.  This filter
        // keeps a local copy of the label's Text and only changes the local copy.
        private void UpdateText()
        {
            if(_text==null)return;
            if (_text.Length == 0)
                return;

            char ch;
            if (_scrollLeft)
            {
                ch = _text[0];
                _text.Remove(0, 1);
                _text.Append(ch);
            }
            else
            {
                ch = _text[_text.Length - 1];
                _text.Remove(_text.Length - 1, 1);
                _text.Insert(0, ch);
            }
        }

        #endregion UpdateText

        #region timer_Tick

        private void Timer_Tick(object sender, EventArgs e)
        {
            // When the timer ticks we want the control to be redrawn.
            // This will repaint the background but we will intercept the rendering
            // of the foreground (text) and draw it ourself.
            _hasScrolled = false;
            if (_label != null) _label.Refresh();
        }

        #endregion timer_Tick

        #region Label_TextChanged

        private void Label_TextChanged(object sender, EventArgs e)
        {
            // If the label's text changes, we need to store a local copy of the new text.
            _text = new StringBuilder(_label.Text);
        }

        #endregion Label_TextChanged

        #endregion Private Helpers
    }

    #endregion ScrollingMarqueeLabelDrawFilter
}
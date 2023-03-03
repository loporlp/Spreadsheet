using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GUI.MainPage;

namespace GUI;

/// <summary>
///   Author: H. James de St. Germain
///   Date:   Spring 2023
///   
///   This code shows:
///   1) How to augment a Maui Widget (i.e., Entry) to add more information
///       a) using StyleId
///       b) using fields
///   2) How to provide a method that matches an event handler (used to clear all cells)
///   3) How to attach a method to an event handler (i.e., Completed)
/// </summary>
public class MyEntry : Entry
{
    //JIM: better?--> int row = 0;

    /// <summary>
    ///   Function provided by "outside world" to be called whenever
    ///   this entry is modified
    /// </summary>
    private ActionOnCompleted onChange;

    private ActionOnFocus focused;

    private char column;
    private int row;
    private bool textChanged;

    /// <summary>
    ///   build an Entry element with the row "remembered"
    /// </summary>
    /// <param name="row"> unique identifier for this item </param>
    /// <param name="changeAction"> outside action that should be invoked after this cell is modified </param>
    public MyEntry(char column, int row, ActionOnCompleted changeAction, ActionOnFocus focusAction) : base()
    {

        this.row = row;
        this.column = column;
        this.textChanged = false;

        // Action to take when the user presses enter on this cell
        this.Completed += CellChangedValue;

        this.Focused += OnFocus;

        this.TextChanged += OnTextChanged;

        // "remember" outside worlds request about what to do when we change.
        onChange = changeAction;


        // run what happens when Entry is Focused on
        focused = focusAction;

        //now set to empty text
        Text = "";
        BackgroundColor = Color.FromRgb(0, 0, 0);
        HorizontalTextAlignment = TextAlignment.Center;

        this.Unfocused += CellChangedValueNoFocus;
    }

    /// <summary>
    ///   Remove focus and text from this widget
    /// </summary>
    public void ClearAndUnfocus()
    {
        this.Unfocus();
        this.Text = "";
    }

    /// <summary>
    ///   Action to take when the value of this entry widget is changed
    ///   and the Enter Key pressed.
    ///   automatically refocuses
    /// </summary>
    /// <param name="sender"> ignored, but should == this </param>
    /// <param name="e"> ignored </param>
    private void CellChangedValue(object sender, EventArgs e)
    {
        Unfocus();


        // Inform the outside world that we have changed
        onChange(column, row, this.Text, true, textChanged);

        textChanged = false;
    }

    /// <summary>
    ///   Action to take when the value of this entry widget is changed
    ///   and the Enter Key pressed.
    ///   Does not automatically refocus
    /// </summary>
    /// <param name="sender"> ignored, but should == this </param>
    /// <param name="e"> ignored </param>
    private void CellChangedValueNoFocus(object sender, EventArgs e)
    {
        Unfocus();
        // Inform the outside world that we have changed
        onChange(column, row, this.Text, false, textChanged);

        textChanged = false;
    }

    /// <summary>
    ///     Action to take when the entry is focused on
    /// </summary>
    /// <param name="sender">ignored but should == this</param>
    /// <param name="e"> ignored </param>
    private void OnFocus(object sender, EventArgs e)
    {
        textChanged = false;
        focused(column, row);
    }

    /// <summary>
    ///  Action to take when the entry text is modified
    /// </summary>
    /// <param name="sender"> ignored but should == this </param>
    /// <param name="e"> ignored </param>
    private void OnTextChanged(object sender, EventArgs e)
    {
        textChanged = true;
    }

}

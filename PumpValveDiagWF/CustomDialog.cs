using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PumpValveDiagWF
{
    public class CustomDialog
    {
        System.Windows.Forms.TextBox inputBox;
        public string ShowDialog( string text, string caption )
        {
            Form prompt = new Form();
            prompt.Width = 500;
            prompt.Height = 350;
            prompt.Text = caption;
            Label textLabel = new Label() { Left = 50, Top = 20, Width = 400, Text = text };
            inputBox = new System.Windows.Forms.TextBox() { Left = 50, Top = 50, Width = 400, Height = 200 };
            // Set the Multiline property to true.
            inputBox.Multiline = true;
            // Add vertical scroll bars to the TextBox control.
            inputBox.ScrollBars = ScrollBars.Vertical;
            // Allow the RETURN key to be entered in the TextBox control.
            inputBox.AcceptsReturn = true;
            // Allow the TAB key to be entered in the TextBox control.
            inputBox.AcceptsTab = true;
            // Set WordWrap to true to allow text to wrap to the next line.
            inputBox.WordWrap = true;
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 270 };
            confirmation.Click += ( sender, e ) => { prompt.Close(); };
            prompt.Controls.Add( confirmation );
            prompt.Controls.Add( textLabel );
            prompt.Controls.Add( inputBox );
            prompt.ShowDialog();
            return inputBox.Text;
        }
    }
}

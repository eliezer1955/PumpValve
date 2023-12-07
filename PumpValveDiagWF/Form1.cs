using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace PumpValveDiagWF
{
    public partial class Form1 : Form
    {
        public int valve;
        public int position;
        FluidicsController fluidicsController;
        public string CurrentMacro = "stepper.tst.txt";
        public string[] CmdLineArgs; 

        public Form1( string[] args)
        {
            //System.Diagnostics.Debugger.Launch();   
            InitializeComponent();
            CmdLineArgs = args;
            fluidicsController = new FluidicsController(CurrentMacro, this);
            button3.Text = fluidicsController.CurrentMacro;
            if (CmdLineArgs.Length > 0)
            {
                Thread runner = new Thread( () => fluidicsController.SocketMode( CmdLineArgs ) );
                runner.Start();
            }
        }

        private void Form1_Load( object sender, EventArgs e )
        {

        }

        private void progressBar1_Click( object sender, EventArgs e )
        {

        }

        private void button1_Click( object sender, EventArgs e )
        {
            Control[] macro = this.Controls.Find( "button3", true );
            string CurrentMacro = macro[0].Text;
            MacroRunner macroRunner = new MacroRunner( fluidicsController,null, CurrentMacro );
            macroRunner.RunMacro();
        }

        private void label3_Click( object sender, EventArgs e )
        {
            button1_Click( sender, e );
        }

        private void Pos12_CheckedChanged( object sender, EventArgs e )
        {
            fluidicsController.valve = fluidicsController.LeftRightChoice == 0 ? 2 : 3;
            fluidicsController.valvepos = 0;
            fluidicsController.MoveValve( fluidicsController.valve, fluidicsController.valvepos );

        }

        private void radioButton43_CheckedChanged( object sender, EventArgs e )
        {
            fluidicsController.LeftRightChoice = 0;
            Control[] found = this.Controls.Find( "groupBox1", true );
            if (found.Length > 0)
                found[0].Visible = false;
        }

        private void radioButton44_CheckedChanged( object sender, EventArgs e )
        {
            fluidicsController.LeftRightChoice = 1;
            Control[] found = this.Controls.Find( "groupBox1", true );
            if (found.Length > 0)
                found[0].Visible = true;
        }

        private void Pos11_CheckedChanged( object sender, EventArgs e )
        {
            fluidicsController.valve = fluidicsController.LeftRightChoice == 0 ? 2 : 3;
            fluidicsController.valvepos = 1;
            fluidicsController.MoveValve( fluidicsController.valve, fluidicsController.valvepos );
        }

        private void Pos10_CheckedChanged( object sender, EventArgs e )
        {
            fluidicsController.valve = fluidicsController.LeftRightChoice == 0 ? 2 : 3;
            fluidicsController.valvepos = 2;
            fluidicsController.MoveValve( fluidicsController.valve, fluidicsController.valvepos );
        }

        private void Pos9_CheckedChanged( object sender, EventArgs e )
        {
            fluidicsController.valve = fluidicsController.LeftRightChoice == 0 ? 2 : 3;
            fluidicsController.valvepos = 3;
            fluidicsController.MoveValve( fluidicsController.valve, fluidicsController.valvepos );
        }

        private void Pos8_CheckedChanged( object sender, EventArgs e )
        {
            fluidicsController.valve = fluidicsController.LeftRightChoice == 0 ? 2 : 3;
            fluidicsController.valvepos = 4;
            fluidicsController.MoveValve( fluidicsController.valve, fluidicsController.valvepos );
        }

        private void Pos7_CheckedChanged( object sender, EventArgs e )
        {
            fluidicsController.valve = fluidicsController.LeftRightChoice == 0 ? 2 : 3;
            fluidicsController.valvepos = 5;
            fluidicsController.MoveValve( fluidicsController.valve, fluidicsController.valvepos );
        }

        private void Pos1_CheckedChanged( object sender, EventArgs e )
        {
            fluidicsController.valve = 1;
            fluidicsController.valvepos = 0;
            fluidicsController.MoveValve( fluidicsController.valve, fluidicsController.valvepos );
        }

        private void Pos2_CheckedChanged( object sender, EventArgs e )
        {
            fluidicsController.valve = 1;
            fluidicsController.valvepos = 1;
            fluidicsController.MoveValve( fluidicsController.valve, fluidicsController.valvepos );
        }

        private void Pos3_CheckedChanged( object sender, EventArgs e )
        {
            fluidicsController.valve = 1;
            fluidicsController.valvepos = 2;
            fluidicsController.MoveValve( fluidicsController.valve, fluidicsController.valvepos );
        }

        private void Pos4_CheckedChanged( object sender, EventArgs e )
        {
            fluidicsController.valve = 1;
            fluidicsController.valvepos = 3;
            fluidicsController.MoveValve( fluidicsController.valve, fluidicsController.valvepos );
        }

        private void Pos5_CheckedChanged( object sender, EventArgs e )
        {
            fluidicsController.valve = fluidicsController.LeftRightChoice == 0 ? 2 : 3;
            fluidicsController.valvepos = 0;
            fluidicsController.MoveValve( fluidicsController.valve, fluidicsController.valvepos );
        }

        private void Pos6_CheckedChanged( object sender, EventArgs e )
        {
            fluidicsController.valve = fluidicsController.LeftRightChoice == 0 ? 2 : 3;
            fluidicsController.valvepos = 0;
            fluidicsController.MoveValve( fluidicsController.valve, fluidicsController.valvepos );
        }

        private void Pos18_CheckedChanged( object sender, EventArgs e )
        {
            fluidicsController.valve = fluidicsController.LeftRightChoice == 0 ? 5 : 1;
            fluidicsController.valvepos = 0;
            fluidicsController.MoveValve( fluidicsController.valve, fluidicsController.valvepos );
        }

        private void Pos17_CheckedChanged( object sender, EventArgs e )
        {
            fluidicsController.valve = fluidicsController.LeftRightChoice == 0 ? 5 : 1;
            fluidicsController.valvepos = 1;
            fluidicsController.MoveValve( fluidicsController.valve, fluidicsController.valvepos );
        }

        private void Pos16_CheckedChanged( object sender, EventArgs e )
        {
            fluidicsController.valve = fluidicsController.LeftRightChoice == 0 ? 5 : 1;
            fluidicsController.valvepos = 2;
            fluidicsController.MoveValve( fluidicsController.valve, fluidicsController.valvepos );
        }

        private void Pos15_CheckedChanged( object sender, EventArgs e )
        {
            fluidicsController.valve = fluidicsController.LeftRightChoice == 0 ? 5 : 1;
            fluidicsController.valvepos = 3;
            fluidicsController.MoveValve( fluidicsController.valve, fluidicsController.valvepos );
        }

        private void Pos14_CheckedChanged( object sender, EventArgs e )
        {
            fluidicsController.valve = fluidicsController.LeftRightChoice == 0 ? 5 : 1;
            fluidicsController.valvepos = 4;
            fluidicsController.MoveValve( fluidicsController.valve, fluidicsController.valvepos );
        }

        private void Pos13_CheckedChanged( object sender, EventArgs e )
        {
            fluidicsController.valve = fluidicsController.LeftRightChoice == 0 ? 5 : 1;
            fluidicsController.valvepos = 5;
            fluidicsController.MoveValve( fluidicsController.valve, fluidicsController.valvepos );
        }

        private void button2_Click( object sender, EventArgs e )
        {
            fluidicsController.valve = fluidicsController.LeftRightChoice == 0 ? 5 : 1;
            fluidicsController.valvepos = 6;
            fluidicsController.MoveValve( fluidicsController.valve, fluidicsController.valvepos );
        }

        private void button3_Click( object sender, EventArgs e )
        {
            var picker = new OpenFileDialog();
            picker.FileName = fluidicsController.CurrentMacro;
            picker.DefaultExt = "txt";
            picker.InitialDirectory= Environment.CurrentDirectory;
            picker.Filter = "txt files (*.txt)|*.txt";
            if (picker.ShowDialog() == DialogResult.OK)
            {
                fluidicsController.CurrentMacro = picker.FileName;
                button3.Text = fluidicsController.CurrentMacro;
            }
        }
       
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PumpValveDiagWF
{
    public class state
    {
        public state() { args = Environment.GetCommandLineArgs(); }
        public string[] args;
    }
    internal static class Program
    {
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        
        
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault( false );           
            state mystate= new state();
            Application.Run( new Form1( mystate.args ) );
        }
    }
}

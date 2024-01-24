using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Forms;

using System.Collections.ObjectModel;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Drawing;
using System.Net.Sockets;
using System.Threading.Tasks;
using AForge.Video.DirectShow;
using AForge.Video;
namespace PumpValveDiagWF
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public class FluidicsController : Object
    {
        private readonly log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(FluidicsController));
        
        public struct CommandStructure
        {
            public string name;
            public string description;
            public string parameters;
            public string returns;
            public int timeout;
            public int response;
        }
        public int ValveBot, ValveMid, ValveTop, Pump;
        public string comport = "COM3";
        public int LeftRightChoice;
        public SerialPort fluidicsPort;
        string localFolder;
        public string CurrentMacro = "calibrate camera.txt";
//        public string CurrentMacro = "E2E.tst.txt";
        VideoCapture capture1, capture2;
        public int valve;
        public int valvepos;

        public struct CCStatsOp
        {
            public Rectangle Rectangle;
            public int Area;
        }
        private Mat myErode( Mat src, int val )
        {
            int erosion_size = val;
            var dest = new Mat();
            CvInvoke.Erode( src, dest, null, new System.Drawing.Point( -1, -1 ), val, BorderType.Default, CvInvoke.MorphologyDefaultBorderValue );
            var dest1 = new Mat();
            CvInvoke.Dilate( dest, dest1, null, new System.Drawing.Point( -1, -1 ), val, BorderType.Default, CvInvoke.MorphologyDefaultBorderValue );
            return dest1;
        }
        /*Errors
        busy ready error
        @ '  no error
        A a  syringe failed to initialize
        B b invalid commad
        C c invalid argument
        D d communication error
        E e invalid “R” command
        F f supply voltage too low
        G g device not initialized
        H h program in progress
        I i syringe overload
        J j not used
        K k syringe move not allowed
        L l cannot move against limit
        M m expanded NVM failed
        O o command buffer overflow
        P p not used
        Q p 1loops nested too deep
        R r program label not found
        S s end of program not found
        T t HOME not set
        V v too many program calls
        W w program not found
        X x not used
        Y y syringe position corrupted
        Z z syringe may go past home
        */

        /// <summary>
        /// Establish socket connection
        /// 
        /// </summary>
        /// <param name="server">address to connect to</param>
        /// <param name="message">message to send</param>
        private void serialSetup()
        {
            fluidicsPort = new SerialPort();
            try
            {
                //get name of comport associated to PumpValve (as obtained by Listports.py)
                ComPortMap map = new ComPortMap();
                comport = map.GetComPort( "FLUIDICS" );
                fluidicsPort.PortName = comport;
                fluidicsPort.BaudRate = 9600;
                fluidicsPort.DataBits = 8;
                fluidicsPort.StopBits = StopBits.One;
                fluidicsPort.Parity = Parity.None;
                fluidicsPort.ReadTimeout = 500;
                fluidicsPort.WriteTimeout = 500;
                fluidicsPort.Handshake = Handshake.None;
                fluidicsPort.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show( comport + " Exception: " + ex.Message );
            }

        }


        public FluidicsController( string runthis, Form1 parentIn )
        {
            localFolder = Directory.GetCurrentDirectory();
            serialSetup();
            // Find all cameras
            List<int> videocams = new List<int>();
            var allCameras = new AForge.Video.DirectShow.FilterInfoCollection( FilterCategory.VideoInputDevice );
            for (int i = 0 ; i < allCameras.Count ; i++)
            {
                string id = allCameras[i].MonikerString;

                if (id.Contains( "pid_9422" )) //Producer ID for video cameras
                {
                    var videoSource = new VideoCaptureDevice( allCameras[i].MonikerString );
                    videocams.Add( i );
                }
            }
            //Create opencv Video Capture objects
            if (videocams.Count >= 2)
            {
                capture1 = new VideoCapture( videocams[0] );
                capture2 = new VideoCapture( videocams[1] );
            }
            CurrentMacro = runthis;

        }

        public void MoveValve( int rs485Device, int pos )
        {
            fluidicsPort.WriteLine( string.Format( "/{0}I{1}", rs485Device, pos ) );
        }

        private void InitializeSyringe()
        {
            // initialize syringe
            fluidicsPort.WriteLine( LeftRightChoice == 0 ? "/5ZR" : "/1ZR" );
        }
        private void SelectMacro()
        {
            var picker = new OpenFileDialog();
            if (picker.ShowDialog() == DialogResult.OK)
            {
                CurrentMacro = picker.FileName;
            }
        }
        private void RunMacro()
        {

            // Open the Macro File and read it back.
            using (StreamReader fs = new StreamReader( localFolder + "\\" + CurrentMacro ))
            {
                byte[] b = new byte[1024];
                System.Text.UTF8Encoding temp = new System.Text.UTF8Encoding( true );
                string line;
                string response = "";
                while ((line = fs.ReadLine()) != null)
                {
                    if (line.StartsWith( "SLEEP" ))
                    {
                        int delay = 0;
                        string[] line1 = line.Split( '#' ); //Disregard comments
                        string[] parsedLine = line1[0].Split( ',' );
                        if (string.IsNullOrWhiteSpace( parsedLine[0] )) //Disregard blanks lines
                            continue;
                        if (parsedLine[1] != null)
                            delay = Int32.Parse( parsedLine[1] );
                        Thread.Sleep( delay );
                        continue;
                    }
                    if (line.StartsWith( "WAIT" ))
                    {
                        string[] line1 = line.Split( '#' ); //Disregard comments
                        string[] parsedLine = line1[0].Split( ',' );
                        if (string.IsNullOrWhiteSpace( parsedLine[0] )) //Disregard blanks lines
                            continue;
                        if (parsedLine[1] != null)
                        {
                            bool motionDone = false;
                            do
                            {
                                Int32.Parse( parsedLine[1] );
                                fluidicsPort.WriteLine( "/Q" + parsedLine[1] + "R" );
                                Thread.Sleep( 100 );
                                byte c1;
                                do
                                {
                                    c1 = (byte)fluidicsPort.ReadByte();
                                    response += c1;
                                } while (c1 != '\n');
                                if ((response.TrimEnd( '\r', '\n' )[2] & 0x40) != 0) continue; //isolate status byte, busy bit
                                motionDone = true;
                            } while (!motionDone);

                        }
                        continue;
                    }

                    if (line.StartsWith( "ALERT" ))
                    {
                        string[] line1 = line.Split( '#' ); //Disregard comments
                        string[] parsedLine = line1[0].Split( ',' );
                        if (string.IsNullOrWhiteSpace( parsedLine[0] )) //Disregard blanks lines
                            continue;
                        if (parsedLine[1] != null)
                            continue;
                    }

                    //Actual command
                    string[] lin1 = line.Split( '#' );
                    if (!string.IsNullOrWhiteSpace( lin1[0] ))
                    {
                        fluidicsPort.WriteLine( lin1[0] );
                        response = "";
                        do
                        {
                            byte RxBuffer = (byte)fluidicsPort.ReadByte();
                            response += RxBuffer;
                            if (response.Contains( "\n" )) break;
                        } while (true);
                    }
                }
            }

        }


        public Image<Rgb, byte> AcquireFrame( int camera )
        {
            Image<Rgb, float> accum = new Image<Rgb, float>( 640, 480 );
            Image<Rgb, byte> img = new Image<Rgb, byte>( 640, 480 );
            for (int i = 0 ; i < 30 ; i++)
            {
                if (camera == 1)
                    capture1.Read( img.Mat );
                else
                    capture2.Read( img.Mat );
                accum += img.Convert<Rgb, float>();
            }
            accum /= 30;
            img = accum.Convert<Rgb, byte>();
            accum.Dispose();
            return img;
        }
        public double MeniscusFrom2Img( Image<Rgb, byte> img1, Image<Rgb, byte> img2 )
        {
            double delta = int.MaxValue;
            Image<Gray, byte> gray1;
            Image<Gray, byte> gray2;

            gray1 = new Image<Gray, byte>( img1.Rows, img1.Cols );
            CvInvoke.CvtColor( img1, gray1, Emgu.CV.CvEnum.ColorConversion.Rgb2Gray );
            gray2 = new Image<Gray, byte>( img2.Rows, img2.Cols );
            CvInvoke.CvtColor( img2, gray2, Emgu.CV.CvEnum.ColorConversion.Rgb2Gray );
            gray1 = gray1.AbsDiff( gray2 );
            gray2 = gray1.ThresholdBinary( new Gray( 3 ), new Gray( 255 ) ).Erode( 5 ).Dilate( 5 );

            //CvInvoke.AdaptiveThreshold( gray1, gray2, 255,
            //    AdaptiveThresholdType.MeanC, ThresholdType.Binary, 5, 0.0 );
            CvInvoke.Imshow( "Before", gray1 );
            CvInvoke.Imshow( "After", gray2 );
            CvInvoke.WaitKey( 30 );


            CvInvoke.Imshow( "Subtracted", gray2 );
            CvInvoke.WaitKey( -1 );

            Mat imgLabel = new Mat();
            Mat stats = new Mat();
            Mat centroids = new Mat();

            int nLabel = CvInvoke.ConnectedComponentsWithStats( gray2, imgLabel, stats, centroids );
            CCStatsOp[] statsOp = new CCStatsOp[stats.Rows];
            stats.CopyTo( statsOp );
            // Find the largest non background component.
            // Note: range() starts from 1 since 0 is the background label.
            int maxval = -1;
            int maxLabel = -1;
            Rectangle rect1 = new Rectangle( 0, 0, 0, 0 );
            for (int i = 1 ; i < nLabel ; i++)
            {
                int temp = statsOp[i].Area;
                if (temp > maxval)
                {
                    maxval = temp;
                    maxLabel = i;
                    rect1 = statsOp[i].Rectangle;
                }
            }

            gray2.Draw( rect1, new Gray( 64 ) );
            CvInvoke.Imshow( "Rect", gray2 );
            CvInvoke.WaitKey( -1 );
            if (rect1.Top != 0)
            {
                delta = rect1.Top - rect1.Bottom;
                System.Console.WriteLine( rect1.Top.ToString( "G" ) + rect1.Bottom.ToString( "G" ) + delta.ToString( "G" ) );
            }
            return delta;
        }
        async public Task SocketMode( string[] CmdLineArgs )
        {
            PipeClient pipeClient = new PipeClient();
            var mr = new MacroRunner( this, pipeClient, null );
            //Thread macroThread = new Thread( new ThreadStart( mr.RunMacro ) );
            mr.RunMacro();
        }
    }
}
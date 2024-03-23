using Emgu.CV.Structure;
using Emgu.CV;
using Emgu.CV.UI;
using System;
using System.IO;
using System.IO.Ports;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV.Aruco;
using System.Collections.Generic;
using System.Data;
using MeniscusTracking;
using AForge.Imaging.Filters;
using System.Collections;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;



namespace PumpValveDiagWF
{
    public class MacroRunner
    {

        public void ShowImage( string name, Image<Rgb, Byte> img )
        {
            try
            {
                CvInvoke.Imshow( name, img );
                CvInvoke.WaitKey( 0 );
                return;
                ImageBox imgBox = new ImageBox();
                imgBox.Image = img;
            }
            catch (Exception ex)
            {
            }
        }


        public class MyRef<T>
        {
            public T Ref { get; set; }
        }


        public void refreshGUI()
        {
            this.controller.parent.Invalidate();
            this.controller.parent.Update();
            this.controller.parent.Refresh();
            System.Windows.Forms.Application.DoEvents();
        }



        public void AddVar( string key, object v ) //storing the ref to a string
        {
            if (null == v)
            {
                v = new MyRef<string> { Ref = " " };
            }
            variables.Add( key, v );
        }



        public void changeVar( string key, object newValue ) //changing any of them
        {
            var ref2 = variables[key] as MyRef<string>;
            if (ref2 == null)
            {
                ref2 = new MyRef<string> { Ref = " " };
            }
            ref2.Ref = newValue.ToString();
        }
        private readonly log4net.ILog _logger = log4net.LogManager.GetLogger( typeof( MacroRunner ) );


        public MeniscusAnalysis FlowDetector = null;
        public string CurrentMacro;
        public SerialPort fluidicsPort;
        StreamReader fs = null;
        FluidicsController controller = null;
        bool socketMode = false;
        public PipeClient pipeClient = null;
        public Image<Rgb, byte> referenceImg1 = null;
        public Image<Rgb, byte> targetImg1 = null;
        public Image<Rgb, byte> referenceImg2 = null;
        public Image<Rgb, byte> targetImg2 = null;
        private string[] Macro;
        private int currentline = 0;
        private System.Collections.Generic.Dictionary<string, int> label = new System.Collections.Generic.Dictionary<string, int>();
        private String response;
        private Dictionary<string, object> variables = new Dictionary<string, object>();
        public String fluidMeasurement = " ";

        private string ExpandVariables( string instring )
        {
            StringBuilder sb = new StringBuilder();
            int start = 0;
            int i;

            var val = new MyRef<string> { Ref = "" };
            for (i = start ; i < instring.Length ; i++)
                if (instring[i] == '%')
                    for (int j = 1 ; j < instring.Length - i ; j++)
                        if (instring[i + j] == '%')
                        {
                            sb.Append( instring.Substring( start, i - start ) );
                            string key = instring.Substring( i + 1, j - 1 );
                            if (variables.ContainsKey( key ))
                            {
                                val = (MyRef<string>)variables[key];
                                sb.Append( val.Ref );
                                start = i = i + j + 1;
                            }
                            else _logger.Error( "Unknown variable:" + val );
                            continue;
                        }
            if ((i - start > 0) && (start < instring.Length))
                sb.Append( instring.Substring( start, i - start ) );
            return sb.ToString();
        }

        private string Evaluate( string instring )
        {
            try
            {
                instring = ExpandVariables( instring );
                DataTable dt = new DataTable();
                var v = dt.Compute( instring, "" );
                return v.ToString();
            }
            catch (Exception ex)
            {
                return instring;
            }
        }
        public MacroRunner( FluidicsController sc, PipeClient pipeClientin, string filename = null )
        {
            fluidicsPort = sc.fluidicsPort;
            CurrentMacro = filename;
            pipeClient = pipeClientin;
            controller = sc;
            socketMode = (CurrentMacro == null);
            int currentline = 0;
            if (CurrentMacro != null)
            {
                //Load full macro into memory as array of strings
                Macro = System.IO.File.ReadAllLines( CurrentMacro );
                //Scan macro array for labels, record their line number in Dictionary
                currentline = 0;
                foreach (string line in Macro)
                {
                    string[] line1 = line.Split( '#' ); //Disregard comments
                    if (line1[0].StartsWith( ":" ))
                        label.Add( line1[0].Substring( 1 ).TrimEnd( '\r', '\n', ' ', '\t' ), currentline + 1 );
                    ++currentline;
                }
            }
        }


        public async Task<string> readLine()
        {
            //System.Diagnostics.Debugger.Launch();
            string s;
            if (socketMode)
            {
                await pipeClient.receive(); //block until string is received
                s = pipeClient.lastReceive; //retrieve string received
                lock (pipeClient._writerSemaphore)
                {
                    pipeClient.lastReceive = null; //reset lastreceive for next read
                }
            }
            else
            {
                s = currentline >= Macro.Length ? null : Macro[currentline++];
            }
            return s;
        }
        public float GetWeight()
        {

            float weight = -99999;
            string response = "";
            try
            {
                fluidicsPort.Write( "/9JR\r\n" );
                fluidicsPort.ReadTimeout = 1000;
                byte c1;
                do //swallow echo
                {
                    c1 = (byte)fluidicsPort.ReadByte();
                    response += (char)(c1);
                } while (c1 != '\n');
                response = "";
                do
                {
                    c1 = (byte)fluidicsPort.ReadByte();
                    response += (char)(c1);
                } while (c1 != '\n');
                string ret = response;
                var index = ret.IndexOf( (char)3 );

                var success = float.TryParse( ret.Substring( 3, index - 3 ), out weight );
                this.controller.SetControlPropertyThreadSafe( controller.parent.textBox1, "Text", weight.ToString() );
                refreshGUI();
            }
            catch { }

            return weight;

        }

        public long MonitorWeight( long period, long logperiod = -1 )
        {
            long startTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            long currentTime = startTime;
            this.controller.SetControlPropertyThreadSafe( controller.parent.button4, "Visible", true );
            refreshGUI();
            if (logperiod > 0)
                _logger.Info( "Starting Weight monitoring " );
            long lastlog = currentTime; ;
            while (currentTime - startTime < period)
            {
                currentTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                float tempa = GetWeight();
                if (logperiod > 0) //log to logfile
                {
                    if (currentTime - lastlog > logperiod)
                    {
                        lastlog = currentTime;
                        _logger.Info( "Weight= " + tempa.ToString() );
                    };
                }
                if (controller.StopMonitoring)
                {
                    controller.StopMonitoring = false;
                    break;
                }
            }
            this.controller.SetControlPropertyThreadSafe( controller.parent.button4, "Visible", false );
            refreshGUI();
            if (logperiod > 0)
                _logger.Info( "Weight monitoring completed" );
            return period;
        }


        public async void RunMacro()
        {

            if (!socketMode)
            {

            }
            //  Read in macro stream

            byte[] b = new byte[1024];
            string[] lastCommand;

            System.Text.UTF8Encoding temp = new System.Text.UTF8Encoding( true );
            string line;
            string response = "";
            while (true)
            {
                line = await readLine();

                if (line == null) break;
                if (line.StartsWith( "\0" )) continue;
                if (line.StartsWith( ":" )) continue;
                if (line.StartsWith( "#" )) continue;
                if (string.IsNullOrEmpty( line )) continue;
                if (string.IsNullOrWhiteSpace( line )) continue;
                if (line.StartsWith( "END" )) //Terminate program
                {
                    string expr = "";
                    string[] line1 = line.Split( '#' ); //Disregard comments
                    string[] parsedLine = line1[0].Split( ',' );
                    if (string.IsNullOrWhiteSpace( parsedLine[0] )) //Disregard blanks lines
                        continue;
                    if (parsedLine[1] != null)
                        expr = parsedLine[1]; //isolate expression
                    Environment.Exit( Int32.Parse( Evaluate( expr ) ) );
                }
                if (line.StartsWith( "IFRETURNISNOT" )) //conditional execution based on last return
                {
                    string value = "";
                    string expr = "";
                    string[] line1 = line.Split( '#' ); //Disregard comments
                    string[] parsedLine = line1[0].Split( ',' );
                    if (string.IsNullOrWhiteSpace( parsedLine[0] )) //Disregard blanks lines
                        continue;
                    if (parsedLine[1] != null)
                        expr = parsedLine[1]; //isolate expression
                    if (parsedLine[2] != null)
                        value = parsedLine[2]; //isolate target value
                    value = Evaluate( value );
                    expr = Evaluate( expr );

                    if (value == expr) //last return matches value
                        continue; //do nothing, go to read next command
                                  //value is not equal to last response, execute conditional command
                    line = ""; //reassemble rest of conditional command
                    for (int i = 3 ; i < parsedLine.Length ; i++)
                    {
                        line += parsedLine[i];
                        if (i < parsedLine.Length - 1) line += ",";
                    }
                    //continue execution as if it was non-conditional
                }
                if (line.StartsWith( "IFRETURNIS" )) //conditional execution based on last return
                {
                    string value = "";
                    string expr = "";
                    string[] line1 = line.Split( '#' ); //Disregard comments
                    string[] parsedLine = line1[0].Split( ',' );
                    if (string.IsNullOrWhiteSpace( parsedLine[0] )) //Disregard blanks lines
                        continue;
                    if (parsedLine[1] != null)
                        expr = parsedLine[1]; //isolate expression
                    if (parsedLine[2] != null)
                        value = parsedLine[2]; //isolate target value
                    value = Evaluate( value );
                    expr = Evaluate( expr );
                    if (value != Evaluate( expr )) //last return does not match value
                        continue; //do nothing, go to read next command
                                  //value is equal to last response
                    line = ""; //reassemble rest of command
                    for (int i = 3 ; i < parsedLine.Length ; i++)
                    {
                        line += parsedLine[i];
                        if (i < parsedLine.Length - 1) line += ",";
                    }
                    //continue execution as if it was non-conditional
                }
                if (line.StartsWith( "EVALUATE" )) //Set response to evaluation of expression
                {
                    string value = "";
                    string[] line1 = line.Split( '#' ); //Disregard comments
                    string[] parsedLine = line1[0].Split( ',' );
                    if (string.IsNullOrWhiteSpace( parsedLine[0] )) //Disregard blanks lines
                        continue;
                    if (parsedLine[1] != null)
                        value = parsedLine[1]; //isolate target value

                    response = Evaluate( parsedLine[1] );
                    changeVar( "response", response );
                    continue;

                }
                if (line.StartsWith( "SET" )) //set value of global var; create it if needed
                {
                    string variable = "";
                    string value = "";
                    string[] line1 = line.Split( '#' ); //Disregard comments
                    string[] parsedLine = line1[0].Split( ',' );
                    if (string.IsNullOrWhiteSpace( parsedLine[0] )) //Disregard blanks lines
                        continue;
                    if (parsedLine[1] != null)
                        variable = parsedLine[1];
                    if (parsedLine[2] != null)
                        value = Evaluate( parsedLine[2] );
                    if (!variables.ContainsKey( variable ))
                        AddVar( variable, null );
                    changeVar( variable, value );
                    continue;
                }
                if (line.StartsWith( "EXIT" )) //stop macro
                {
                    break;
                }
                if (line.StartsWith( "EXECUTE" )) //stop macro
                {
                    string value = "";

                    string[] line1 = line.Split( '#' ); //Disregard comments
                    string[] parsedLine = line1[0].Split( ',' );
                    if (string.IsNullOrWhiteSpace( parsedLine[0] )) //Disregard blanks lines
                        continue;
                    if (parsedLine[1] != null)
                        value = parsedLine[1];
                    var proc = System.Diagnostics.Process.Start( "CMD.exe", "/C " + value );
                    if (parsedLine.Length > 2 && parsedLine[2] == "parallel") continue;
                    //force serial execution
                    proc?.WaitForExit();
                    response = proc?.ExitCode.ToString();
                    continue;
                }

                if (line.StartsWith( "LOGERROR" )) //write log entry
                {
                    string value = "";
                    string[] line1 = line.Split( '#' ); //Disregard comments
                    string[] parsedLine = line1[0].Split( ',' );
                    if (string.IsNullOrWhiteSpace( parsedLine[0] )) //Disregard blanks lines
                        continue;
                    if (parsedLine[1] != null)
                        value = ExpandVariables( parsedLine[1] );

                    _logger.Error( value );
                    continue;
                }
                if (line.StartsWith( "GOTO" ))
                {
                    string value = "";
                    string[] line1 = line.Split( '#' ); //Disregard comments
                    string[] parsedLine = line1[0].Split( ',' );
                    if (string.IsNullOrWhiteSpace( parsedLine[0] )) //Disregard blanks lines
                        continue;
                    if (parsedLine[1] != null)
                        value = parsedLine[1].TrimEnd( '\r', '\n', ' ', '\t' );
                    if (!label.ContainsKey( value ))
                        _logger.Error( "Unknown label " + value );
                    else
                    {

                        currentline = label[value];
                        continue;
                    }

                }

                // "Nested" macro calling
                if (line.StartsWith( "@" ))
                {
                    MacroRunner macroRunner = new MacroRunner( controller, pipeClient, line.Substring( 1 ) );
                    macroRunner.RunMacro();
                    continue;
                }
                // acquire reference image
                if (line.StartsWith( "SNAPREFERENCE" ))
                {
                    int camera = 0;
                    string[] line1 = line.Split( '#' ); //Disregard comments
                    string[] parsedLine = line1[0].Split( ',' );
                    if (string.IsNullOrWhiteSpace( parsedLine[0] )) //Disregard blanks lines
                        continue;
                    if (parsedLine[1] != null)
                        camera = Int32.Parse( parsedLine[1] );
                    if (camera == 1)
                    {
                        referenceImg1 = controller.AcquireFrame( camera );
                        ShowImage( "Before", referenceImg1 );
                    }
                    else
                    {
                        referenceImg2 = controller.AcquireFrame( camera );
                        ShowImage( "Before", referenceImg2 );
                    }
                    continue;
                }
                // acquire  image, compare to reference
                if (line.StartsWith( "SNAPMEASURE" ))
                {
                    int camera = 0;
                    string[] line1 = line.Split( '#' ); //Disregard comments
                    string[] parsedLine = line1[0].Split( ',' );
                    if (string.IsNullOrWhiteSpace( parsedLine[0] )) //Disregard blanks lines
                        continue;
                    if (parsedLine[1] != null)
                        camera = Int32.Parse( parsedLine[1] );

                    double measurement = -999;
                    int rotorsteps = 68000;

                    Action<MeniscusAnalysis> pf = MeniscusTracker.ProcessByHorizontalPeakFinder;

                    if (camera == 1)
                    {
                        targetImg1 = controller.AcquireFrame( camera );
                        ShowImage( "After", targetImg1 );
                        FlowDetector = new MeniscusAnalysis( referenceImg1,
                                                             targetImg1, pf, rotorsteps );
                    }
                    else
                    {
                        targetImg2 = controller.AcquireFrame( camera );
                        ShowImage( "After", targetImg2 );
                        FlowDetector = new MeniscusAnalysis( referenceImg2,
                                                             targetImg2, pf, rotorsteps );
                    }
                    MeniscusTracker.MeniscusFrom2Img( FlowDetector );

                    if (FlowDetector.Meniscii.Count > 0)
                    {
                        measurement = FlowDetector.Meniscii[0].BrightestIndex;
                        fluidMeasurement = ((int)measurement).ToString();
                        changeVar( "fluidMeasurement", fluidMeasurement );
                    }

                    _logger.Error( "Fluid measurement=" + measurement.ToString() );
                    response = measurement.ToString();

                    continue;
                }


                // Wait for fixed time
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
                // Wait until status is idle
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
                            StringBuilder status = new StringBuilder();
                            fluidicsPort.Write( "/" + parsedLine[1] + "QR" + "\r\n" );
                            fluidicsPort.BaseStream.Flush();
                            Thread.Sleep( 100 );
                            byte c1;
                            try
                            {
                                do
                                {
                                    c1 = (byte)fluidicsPort.ReadByte();
                                    status.Append( c1 );
                                } while (c1 != '\n');
                            }
                            catch (Exception ex) { };


                            String s = status.ToString();
                            s = s.TrimEnd( '\r', '\n' );
                            if (s.Length < 3)
                                continue;
                            if ((s[2] & 0x40) != 0)
                                continue; //isolate status byte, busy bit
                            motionDone = true;
                            Thread.Sleep( 100 );
                        } while (!motionDone);

                    }
                    continue;
                }
                // Pop up MessageBox
                if (line.StartsWith( "ALERT" ))
                {
                    string[] line1 = line.Split( '#' ); //Disregard comments
                    string[] parsedLine = line1[0].Split( ',' );
                    if (string.IsNullOrWhiteSpace( parsedLine[0] )) //Disregard blanks lines
                        continue;
                    if (parsedLine[1] != null)
                    {
                        parsedLine[1] = ExpandVariables( parsedLine[1] );
                        MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                        DialogResult result;
                        result = MessageBox.Show( parsedLine[1], "Fluidics Alert!", buttons );
                        response = result.ToString();
                        if (response == "No")
                        {
                            var observations = new CustomDialog();
                            _logger.Error( observations.ShowDialog( parsedLine[1], "Enter observations" ) );
                        }
                        continue;
                    }
                }


                if (line.StartsWith( "REPORT" ))
                {

                    string[] line1 = line.Split( '#' ); //Disregard comments
                    string[] parsedLine = line1[0].Split( ',' );
                    if (string.IsNullOrWhiteSpace( parsedLine[0] )) //Disregard blank lines
                        continue;

                    if (parsedLine[1] != null)
                    {
                        var i = line.IndexOf( ',' );
                        if (i > -1)
                        {
                            await pipeClient.client.Send( "PumpValve:" + line.Substring( i + 1 ) );
                            continue;
                        }
                    }
                }
                if (line.StartsWith( "MONITORWEIGHT" ))
                {

                    string[] line1 = line.Split( '#' ); //Disregard comments
                    string[] parsedLine = line1[0].Split( ',' );
                    if (string.IsNullOrWhiteSpace( parsedLine[0] )) //Disregard blank lines
                        continue;

                    if (parsedLine[1] != null)
                    {
                        long logperiod = -1;
                        var period = int.Parse( parsedLine[1] );
                        if (parsedLine.Length > 2 && parsedLine[2] != null)
                            logperiod = long.Parse( parsedLine[2] );
                        MonitorWeight( period, logperiod );
                    }
                }


                //Actual command
                string[] lin1 = line.Split( '#' );
                lin1[0] = lin1[0].TrimEnd( new char[] { ' ', '\r', '\n', '\t' } );
                if (!string.IsNullOrWhiteSpace( lin1[0] ))
                {
                    lastCommand = lin1;

                    fluidicsPort.DiscardOutBuffer();
                    fluidicsPort.DiscardInBuffer();

                    fluidicsPort.Write( lin1[0] + "\r\n" );
                    fluidicsPort.BaseStream.Flush();
                    Thread.Sleep( 10 );

                    StringBuilder response1 = new StringBuilder();
                    try
                    {
                        do
                        {
                            fluidicsPort.ReadTimeout = 500;
                            int RxBuffer = fluidicsPort.ReadByte();
                            response1.Append( (char)RxBuffer );
                            if (RxBuffer == '\n') break;
                        } while (true);
                    }
                    catch (Exception ex)
                    { }
                    response = response1.ToString();
                    fluidicsPort.DiscardOutBuffer();
                    fluidicsPort.DiscardInBuffer();

                }
            }
        }

    }
}
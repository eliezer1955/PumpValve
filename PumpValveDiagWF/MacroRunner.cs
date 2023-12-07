using System;
using System.IO;
using System.IO.Ports;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace PumpValveDiagWF
{
    public class MacroRunner
    {
        public string CurrentMacro;
        public SerialPort fluidicsPort;
        StreamReader fs = null;
        FluidicsController controller = null;
        bool socketMode = false;
        public PipeClient pipeClient = null;

        public MacroRunner( FluidicsController sc, PipeClient pipeClientin, string filename = null )
        {
            fluidicsPort = sc.fluidicsPort;
            CurrentMacro = filename;
            pipeClient = pipeClientin;
            controller = sc;
            socketMode = (CurrentMacro == null);
            if (CurrentMacro != null)
                fs = new StreamReader( CurrentMacro );
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
                s = fs.ReadLine();
            }
            return s;
        }


        public async void RunMacro()
        {

            //  Read in macro stream

            byte[] b = new byte[1024];
            string[] lastCommand;
            string lastCommandReturnTypes;
            System.Text.UTF8Encoding temp = new System.Text.UTF8Encoding( true );
            string line;
            string response = "";
            while (true)
            {
                line = await readLine();

                if (line == null) break;
                if (line.StartsWith( "\0" )) continue;
                if (line.StartsWith( "#" )) continue;
                if (string.IsNullOrEmpty( line )) continue;
                if (string.IsNullOrWhiteSpace( line )) continue;
                // "Nested" macro calling
                if (line.StartsWith( "@" ))
                {
                    MacroRunner macroRunner = new MacroRunner( controller, pipeClient, line.Substring( 1 ) );
                    macroRunner.RunMacro();
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
                // Pop up MessageBox
                if (line.StartsWith( "ALERT" ))
                {
                    string[] line1 = line.Split( '#' ); //Disregard comments
                    string[] parsedLine = line1[0].Split( ',' );
                    if (string.IsNullOrWhiteSpace( parsedLine[0] )) //Disregard blanks lines
                        continue;
                    if (parsedLine[1] != null)
                        continue;
                }


                if (line.StartsWith( "REPORT" ))
                {

                    string[] line1 = line.Split( '#' ); //Disregard comments
                    string[] parsedLine = line1[0].Split( ',' );
                    if (string.IsNullOrWhiteSpace( parsedLine[0] )) //Disregard blanks lines
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


                //Actual command
                string[] lin1 = line.Split( '#' );
                if (!string.IsNullOrWhiteSpace( lin1[0] ))
                {
                    lastCommand = lin1;



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
}
using System;
using System.IO;
using System.IO.Ports;
using System.Net.Sockets;
using System.Text;
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

        public MacroRunner(FluidicsController sc, PipeClient pipeClientin, string filename = null)
        {
            fluidicsPort = sc.fluidicsPort;
            CurrentMacro = filename;
            pipeClient = pipeClientin;
            controller = sc;
            socketMode = (CurrentMacro == null);
            if (CurrentMacro != null)
                fs = new StreamReader(CurrentMacro);
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
            System.Text.UTF8Encoding temp = new System.Text.UTF8Encoding(true);
            string line;
            string response = "";
            while (true)
            {
                line = await readLine();

                if (line == null) break;
                if (line.StartsWith("\0")) continue;
                if (line.StartsWith("#")) continue;
                if (string.IsNullOrEmpty(line)) continue;
                if (string.IsNullOrWhiteSpace(line)) continue;
                // "Nested" macro calling
                if (line.StartsWith("@"))
                {
                    MacroRunner macroRunner = new MacroRunner(controller, pipeClient, line.Substring(1));
                    macroRunner.RunMacro();
                    continue;
                }
                // Wait for fixed time
                if (line.StartsWith("SLEEP"))
                {
                    int delay = 0;
                    string[] line1 = line.Split('#'); //Disregard comments
                    string[] parsedLine = line1[0].Split(',');
                    if (string.IsNullOrWhiteSpace(parsedLine[0])) //Disregard blanks lines
                        continue;
                    if (parsedLine[1] != null)
                        delay = Int32.Parse(parsedLine[1]);
                    Thread.Sleep(delay);
                    continue;
                }
                // Wait until status is idle
                if (line.StartsWith("WAIT"))
                {
                    string[] line1 = line.Split('#'); //Disregard comments
                    string[] parsedLine = line1[0].Split(',');
                    if (string.IsNullOrWhiteSpace(parsedLine[0])) //Disregard blanks lines
                        continue;
                    if (parsedLine[1] != null)
                    {
                        bool motionDone = false;
                        do
                        {
                            Int32.Parse(parsedLine[1]);
                            fluidicsPort.Write("/" + parsedLine[1] + "QR" + "\r\n");
                            fluidicsPort.BaseStream.Flush();
                            Thread.Sleep(100);
                            byte c1;
                            try
                            {
                                do
                                {
                                    c1 = (byte)fluidicsPort.ReadByte();
                                    response += c1;
                                } while (c1 != '\n');
                            }
                            catch (Exception ex) { };
                            if (response.Length < 3) continue;
                            if ((response.TrimEnd('\r', '\n')[2] & 0x40) != 0) continue; //isolate status byte, busy bit
                            motionDone = true;
                            Thread.Sleep(100);
                        } while (!motionDone);

                    }
                    continue;
                }
                // Pop up MessageBox
                if (line.StartsWith("ALERT"))
                {
                    string[] line1 = line.Split('#'); //Disregard comments
                    string[] parsedLine = line1[0].Split(',');
                    if (string.IsNullOrWhiteSpace(parsedLine[0])) //Disregard blanks lines
                        continue;
                    if (parsedLine[1] != null)
                    {
                        MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                        DialogResult result;
                        result = MessageBox.Show(parsedLine[1], "Stepper Alert!", buttons);
                        continue;
                    }
                }


                if (line.StartsWith("REPORT"))
                {

                    string[] line1 = line.Split('#'); //Disregard comments
                    string[] parsedLine = line1[0].Split(',');
                    if (string.IsNullOrWhiteSpace(parsedLine[0])) //Disregard blanks lines
                        continue;

                    if (parsedLine[1] != null)
                    {
                        var i = line.IndexOf(',');
                        if (i > -1)
                        {
                            await pipeClient.client.Send("PumpValve:" + line.Substring(i + 1));
                            continue;
                        }
                    }
                }


                //Actual command
                string[] lin1 = line.Split('#');
                if (!string.IsNullOrWhiteSpace(lin1[0]))
                {
                    lastCommand = lin1;

                    fluidicsPort.DiscardOutBuffer();
                    fluidicsPort.DiscardInBuffer();

                    fluidicsPort.Write(lin1[0] + "\r\n");
                    fluidicsPort.BaseStream.Flush();
                    Thread.Sleep(10);

                    StringBuilder response1 = new StringBuilder();
                    try
                    {
                        do
                        {
                            fluidicsPort.ReadTimeout = 500;
                            int RxBuffer = fluidicsPort.ReadByte();
                            response1.Append((char)RxBuffer);
                            if (RxBuffer == '\n') break;
                        } while (true);
                    }
                    catch (Exception ex)
                    { }

                    fluidicsPort.DiscardOutBuffer();
                    fluidicsPort.DiscardInBuffer();

                }
            }
        }

    }
}
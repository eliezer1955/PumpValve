using System;
using System.Net.Sockets;

namespace PumpValveDiagWF
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        public string[] clArguments = Environment.GetCommandLineArgs();





        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Pos1 = new System.Windows.Forms.RadioButton();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.Pos2 = new System.Windows.Forms.RadioButton();
            this.Pos3 = new System.Windows.Forms.RadioButton();
            this.Pos4 = new System.Windows.Forms.RadioButton();
            this.Pos5 = new System.Windows.Forms.RadioButton();
            this.Pos6 = new System.Windows.Forms.RadioButton();
            this.Pos7 = new System.Windows.Forms.RadioButton();
            this.Pos8 = new System.Windows.Forms.RadioButton();
            this.Pos9 = new System.Windows.Forms.RadioButton();
            this.Pos10 = new System.Windows.Forms.RadioButton();
            this.Pos11 = new System.Windows.Forms.RadioButton();
            this.Pos12 = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.Pos13 = new System.Windows.Forms.RadioButton();
            this.Pos14 = new System.Windows.Forms.RadioButton();
            this.Pos15 = new System.Windows.Forms.RadioButton();
            this.Pos16 = new System.Windows.Forms.RadioButton();
            this.Pos17 = new System.Windows.Forms.RadioButton();
            this.Pos18 = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.radioButton19 = new System.Windows.Forms.RadioButton();
            this.radioButton20 = new System.Windows.Forms.RadioButton();
            this.radioButton21 = new System.Windows.Forms.RadioButton();
            this.radioButton22 = new System.Windows.Forms.RadioButton();
            this.radioButton23 = new System.Windows.Forms.RadioButton();
            this.radioButton24 = new System.Windows.Forms.RadioButton();
            this.radioButton25 = new System.Windows.Forms.RadioButton();
            this.radioButton26 = new System.Windows.Forms.RadioButton();
            this.radioButton27 = new System.Windows.Forms.RadioButton();
            this.radioButton28 = new System.Windows.Forms.RadioButton();
            this.radioButton29 = new System.Windows.Forms.RadioButton();
            this.radioButton30 = new System.Windows.Forms.RadioButton();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.radioButton31 = new System.Windows.Forms.RadioButton();
            this.radioButton32 = new System.Windows.Forms.RadioButton();
            this.radioButton33 = new System.Windows.Forms.RadioButton();
            this.radioButton34 = new System.Windows.Forms.RadioButton();
            this.radioButton35 = new System.Windows.Forms.RadioButton();
            this.radioButton36 = new System.Windows.Forms.RadioButton();
            this.radioButton37 = new System.Windows.Forms.RadioButton();
            this.radioButton38 = new System.Windows.Forms.RadioButton();
            this.radioButton39 = new System.Windows.Forms.RadioButton();
            this.radioButton40 = new System.Windows.Forms.RadioButton();
            this.radioButton41 = new System.Windows.Forms.RadioButton();
            this.radioButton42 = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.radioButton7 = new System.Windows.Forms.RadioButton();
            this.radioButton8 = new System.Windows.Forms.RadioButton();
            this.radioButton9 = new System.Windows.Forms.RadioButton();
            this.radioButton10 = new System.Windows.Forms.RadioButton();
            this.radioButton11 = new System.Windows.Forms.RadioButton();
            this.radioButton12 = new System.Windows.Forms.RadioButton();
            this.radioButton13 = new System.Windows.Forms.RadioButton();
            this.radioButton14 = new System.Windows.Forms.RadioButton();
            this.radioButton15 = new System.Windows.Forms.RadioButton();
            this.radioButton16 = new System.Windows.Forms.RadioButton();
            this.radioButton17 = new System.Windows.Forms.RadioButton();
            this.radioButton18 = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.radioButton6 = new System.Windows.Forms.RadioButton();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.radioButton44 = new System.Windows.Forms.RadioButton();
            this.radioButton43 = new System.Windows.Forms.RadioButton();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.groupBox13.SuspendLayout();
            this.SuspendLayout();
            // 
            // Pos1
            // 
            this.Pos1.AutoSize = true;
            this.Pos1.Location = new System.Drawing.Point(18, 17);
            this.Pos1.Margin = new System.Windows.Forms.Padding(2);
            this.Pos1.Name = "Pos1";
            this.Pos1.Size = new System.Drawing.Size(52, 17);
            this.Pos1.TabIndex = 0;
            this.Pos1.TabStop = true;
            this.Pos1.Text = "Pos 1";
            this.Pos1.UseVisualStyleBackColor = true;
            this.Pos1.CheckedChanged += new System.EventHandler(this.Pos1_CheckedChanged);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(5, 34);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(2);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(432, 19);
            this.progressBar1.TabIndex = 1;
            this.progressBar1.Click += new System.EventHandler(this.progressBar1_Click);
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(5, 91);
            this.progressBar2.Margin = new System.Windows.Forms.Padding(2);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(432, 19);
            this.progressBar2.TabIndex = 2;
            // 
            // Pos2
            // 
            this.Pos2.AutoSize = true;
            this.Pos2.Location = new System.Drawing.Point(94, 17);
            this.Pos2.Margin = new System.Windows.Forms.Padding(2);
            this.Pos2.Name = "Pos2";
            this.Pos2.Size = new System.Drawing.Size(52, 17);
            this.Pos2.TabIndex = 3;
            this.Pos2.TabStop = true;
            this.Pos2.Text = "Pos 2";
            this.Pos2.UseVisualStyleBackColor = true;
            this.Pos2.CheckedChanged += new System.EventHandler(this.Pos2_CheckedChanged);
            // 
            // Pos3
            // 
            this.Pos3.AutoSize = true;
            this.Pos3.Location = new System.Drawing.Point(188, 17);
            this.Pos3.Margin = new System.Windows.Forms.Padding(2);
            this.Pos3.Name = "Pos3";
            this.Pos3.Size = new System.Drawing.Size(49, 17);
            this.Pos3.TabIndex = 4;
            this.Pos3.TabStop = true;
            this.Pos3.Text = "Pos3";
            this.Pos3.UseVisualStyleBackColor = true;
            this.Pos3.CheckedChanged += new System.EventHandler(this.Pos3_CheckedChanged);
            // 
            // Pos4
            // 
            this.Pos4.AutoSize = true;
            this.Pos4.Location = new System.Drawing.Point(279, 17);
            this.Pos4.Margin = new System.Windows.Forms.Padding(2);
            this.Pos4.Name = "Pos4";
            this.Pos4.Size = new System.Drawing.Size(49, 17);
            this.Pos4.TabIndex = 5;
            this.Pos4.TabStop = true;
            this.Pos4.Text = "Pos4";
            this.Pos4.UseVisualStyleBackColor = true;
            this.Pos4.CheckedChanged += new System.EventHandler(this.Pos4_CheckedChanged);
            // 
            // Pos5
            // 
            this.Pos5.AutoSize = true;
            this.Pos5.Location = new System.Drawing.Point(361, 17);
            this.Pos5.Margin = new System.Windows.Forms.Padding(2);
            this.Pos5.Name = "Pos5";
            this.Pos5.Size = new System.Drawing.Size(49, 17);
            this.Pos5.TabIndex = 6;
            this.Pos5.TabStop = true;
            this.Pos5.Text = "Pos5";
            this.Pos5.UseVisualStyleBackColor = true;
            this.Pos5.CheckedChanged += new System.EventHandler(this.Pos5_CheckedChanged);
            // 
            // Pos6
            // 
            this.Pos6.AutoSize = true;
            this.Pos6.Location = new System.Drawing.Point(451, 17);
            this.Pos6.Margin = new System.Windows.Forms.Padding(2);
            this.Pos6.Name = "Pos6";
            this.Pos6.Size = new System.Drawing.Size(49, 17);
            this.Pos6.TabIndex = 7;
            this.Pos6.TabStop = true;
            this.Pos6.Text = "Pos6";
            this.Pos6.UseVisualStyleBackColor = true;
            this.Pos6.CheckedChanged += new System.EventHandler(this.Pos6_CheckedChanged);
            // 
            // Pos7
            // 
            this.Pos7.AutoSize = true;
            this.Pos7.Location = new System.Drawing.Point(445, 11);
            this.Pos7.Margin = new System.Windows.Forms.Padding(2);
            this.Pos7.Name = "Pos7";
            this.Pos7.Size = new System.Drawing.Size(52, 17);
            this.Pos7.TabIndex = 13;
            this.Pos7.TabStop = true;
            this.Pos7.Text = "Pos 6";
            this.Pos7.UseVisualStyleBackColor = true;
            this.Pos7.CheckedChanged += new System.EventHandler(this.Pos7_CheckedChanged);
            // 
            // Pos8
            // 
            this.Pos8.AutoSize = true;
            this.Pos8.Location = new System.Drawing.Point(355, 11);
            this.Pos8.Margin = new System.Windows.Forms.Padding(2);
            this.Pos8.Name = "Pos8";
            this.Pos8.Size = new System.Drawing.Size(52, 17);
            this.Pos8.TabIndex = 12;
            this.Pos8.TabStop = true;
            this.Pos8.Text = "Pos 5";
            this.Pos8.UseVisualStyleBackColor = true;
            this.Pos8.CheckedChanged += new System.EventHandler(this.Pos8_CheckedChanged);
            // 
            // Pos9
            // 
            this.Pos9.AutoSize = true;
            this.Pos9.Location = new System.Drawing.Point(273, 11);
            this.Pos9.Margin = new System.Windows.Forms.Padding(2);
            this.Pos9.Name = "Pos9";
            this.Pos9.Size = new System.Drawing.Size(52, 17);
            this.Pos9.TabIndex = 11;
            this.Pos9.TabStop = true;
            this.Pos9.Text = "Pos 4";
            this.Pos9.UseVisualStyleBackColor = true;
            this.Pos9.CheckedChanged += new System.EventHandler(this.Pos9_CheckedChanged);
            // 
            // Pos10
            // 
            this.Pos10.AutoSize = true;
            this.Pos10.Location = new System.Drawing.Point(182, 11);
            this.Pos10.Margin = new System.Windows.Forms.Padding(2);
            this.Pos10.Name = "Pos10";
            this.Pos10.Size = new System.Drawing.Size(52, 17);
            this.Pos10.TabIndex = 10;
            this.Pos10.TabStop = true;
            this.Pos10.Text = "Pos 3";
            this.Pos10.UseVisualStyleBackColor = true;
            this.Pos10.CheckedChanged += new System.EventHandler(this.Pos10_CheckedChanged);
            // 
            // Pos11
            // 
            this.Pos11.AutoSize = true;
            this.Pos11.Location = new System.Drawing.Point(88, 11);
            this.Pos11.Margin = new System.Windows.Forms.Padding(2);
            this.Pos11.Name = "Pos11";
            this.Pos11.Size = new System.Drawing.Size(52, 17);
            this.Pos11.TabIndex = 9;
            this.Pos11.TabStop = true;
            this.Pos11.Text = "Pos 2";
            this.Pos11.UseVisualStyleBackColor = true;
            this.Pos11.CheckedChanged += new System.EventHandler(this.Pos11_CheckedChanged);
            // 
            // Pos12
            // 
            this.Pos12.AutoSize = true;
            this.Pos12.Location = new System.Drawing.Point(15, 11);
            this.Pos12.Margin = new System.Windows.Forms.Padding(2);
            this.Pos12.Name = "Pos12";
            this.Pos12.Size = new System.Drawing.Size(52, 17);
            this.Pos12.TabIndex = 8;
            this.Pos12.TabStop = true;
            this.Pos12.Text = "Pos 1";
            this.Pos12.UseVisualStyleBackColor = true;
            this.Pos12.CheckedChanged += new System.EventHandler(this.Pos12_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(94, 150);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(67, 19);
            this.button1.TabIndex = 14;
            this.button1.Text = "Run Macro";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Pos13
            // 
            this.Pos13.AutoSize = true;
            this.Pos13.Location = new System.Drawing.Point(435, 17);
            this.Pos13.Margin = new System.Windows.Forms.Padding(2);
            this.Pos13.Name = "Pos13";
            this.Pos13.Size = new System.Drawing.Size(52, 17);
            this.Pos13.TabIndex = 21;
            this.Pos13.TabStop = true;
            this.Pos13.Text = "Pos 6";
            this.Pos13.UseVisualStyleBackColor = true;
            this.Pos13.CheckedChanged += new System.EventHandler(this.Pos13_CheckedChanged);
            // 
            // Pos14
            // 
            this.Pos14.AutoSize = true;
            this.Pos14.Location = new System.Drawing.Point(339, 17);
            this.Pos14.Margin = new System.Windows.Forms.Padding(2);
            this.Pos14.Name = "Pos14";
            this.Pos14.Size = new System.Drawing.Size(52, 17);
            this.Pos14.TabIndex = 20;
            this.Pos14.TabStop = true;
            this.Pos14.Text = "Pos 5";
            this.Pos14.UseVisualStyleBackColor = true;
            this.Pos14.CheckedChanged += new System.EventHandler(this.Pos14_CheckedChanged);
            // 
            // Pos15
            // 
            this.Pos15.AutoSize = true;
            this.Pos15.Location = new System.Drawing.Point(269, 17);
            this.Pos15.Margin = new System.Windows.Forms.Padding(2);
            this.Pos15.Name = "Pos15";
            this.Pos15.Size = new System.Drawing.Size(52, 17);
            this.Pos15.TabIndex = 19;
            this.Pos15.TabStop = true;
            this.Pos15.Text = "Pos 4";
            this.Pos15.UseVisualStyleBackColor = true;
            this.Pos15.CheckedChanged += new System.EventHandler(this.Pos15_CheckedChanged);
            // 
            // Pos16
            // 
            this.Pos16.AutoSize = true;
            this.Pos16.Location = new System.Drawing.Point(172, 17);
            this.Pos16.Margin = new System.Windows.Forms.Padding(2);
            this.Pos16.Name = "Pos16";
            this.Pos16.Size = new System.Drawing.Size(52, 17);
            this.Pos16.TabIndex = 18;
            this.Pos16.TabStop = true;
            this.Pos16.Text = "Pos 3";
            this.Pos16.UseVisualStyleBackColor = true;
            this.Pos16.CheckedChanged += new System.EventHandler(this.Pos16_CheckedChanged);
            // 
            // Pos17
            // 
            this.Pos17.AutoSize = true;
            this.Pos17.Location = new System.Drawing.Point(84, 17);
            this.Pos17.Margin = new System.Windows.Forms.Padding(2);
            this.Pos17.Name = "Pos17";
            this.Pos17.Size = new System.Drawing.Size(52, 17);
            this.Pos17.TabIndex = 17;
            this.Pos17.TabStop = true;
            this.Pos17.Text = "Pos 2";
            this.Pos17.UseVisualStyleBackColor = true;
            this.Pos17.CheckedChanged += new System.EventHandler(this.Pos17_CheckedChanged);
            // 
            // Pos18
            // 
            this.Pos18.AutoSize = true;
            this.Pos18.Location = new System.Drawing.Point(8, 17);
            this.Pos18.Margin = new System.Windows.Forms.Padding(2);
            this.Pos18.Name = "Pos18";
            this.Pos18.Size = new System.Drawing.Size(52, 17);
            this.Pos18.TabIndex = 16;
            this.Pos18.TabStop = true;
            this.Pos18.Text = "Pos 1";
            this.Pos18.UseVisualStyleBackColor = true;
            this.Pos18.CheckedChanged += new System.EventHandler(this.Pos18_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox5);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.Pos1);
            this.groupBox1.Controls.Add(this.Pos2);
            this.groupBox1.Controls.Add(this.Pos3);
            this.groupBox1.Controls.Add(this.Pos4);
            this.groupBox1.Controls.Add(this.Pos5);
            this.groupBox1.Controls.Add(this.Pos6);
            this.groupBox1.Location = new System.Drawing.Point(0, 86);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(580, 45);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Middle valve";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.groupBox6);
            this.groupBox5.Controls.Add(this.groupBox8);
            this.groupBox5.Controls.Add(this.radioButton37);
            this.groupBox5.Controls.Add(this.radioButton38);
            this.groupBox5.Controls.Add(this.radioButton39);
            this.groupBox5.Controls.Add(this.radioButton40);
            this.groupBox5.Controls.Add(this.radioButton41);
            this.groupBox5.Controls.Add(this.radioButton42);
            this.groupBox5.Location = new System.Drawing.Point(6, -47);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox5.Size = new System.Drawing.Size(580, 32);
            this.groupBox5.TabIndex = 25;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "groupBox5";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.groupBox7);
            this.groupBox6.Controls.Add(this.radioButton25);
            this.groupBox6.Controls.Add(this.radioButton26);
            this.groupBox6.Controls.Add(this.radioButton27);
            this.groupBox6.Controls.Add(this.radioButton28);
            this.groupBox6.Controls.Add(this.radioButton29);
            this.groupBox6.Controls.Add(this.radioButton30);
            this.groupBox6.Location = new System.Drawing.Point(6, 345);
            this.groupBox6.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox6.Size = new System.Drawing.Size(580, 56);
            this.groupBox6.TabIndex = 24;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "groupBox6";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.radioButton19);
            this.groupBox7.Controls.Add(this.radioButton20);
            this.groupBox7.Controls.Add(this.radioButton21);
            this.groupBox7.Controls.Add(this.radioButton22);
            this.groupBox7.Controls.Add(this.radioButton23);
            this.groupBox7.Controls.Add(this.radioButton24);
            this.groupBox7.Location = new System.Drawing.Point(6, 345);
            this.groupBox7.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox7.Size = new System.Drawing.Size(580, 45);
            this.groupBox7.TabIndex = 23;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "groupBox7";
            // 
            // radioButton19
            // 
            this.radioButton19.AutoSize = true;
            this.radioButton19.Location = new System.Drawing.Point(18, 17);
            this.radioButton19.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton19.Name = "radioButton19";
            this.radioButton19.Size = new System.Drawing.Size(52, 17);
            this.radioButton19.TabIndex = 0;
            this.radioButton19.TabStop = true;
            this.radioButton19.Text = "Pos 1";
            this.radioButton19.UseVisualStyleBackColor = true;
            // 
            // radioButton20
            // 
            this.radioButton20.AutoSize = true;
            this.radioButton20.Location = new System.Drawing.Point(94, 17);
            this.radioButton20.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton20.Name = "radioButton20";
            this.radioButton20.Size = new System.Drawing.Size(52, 17);
            this.radioButton20.TabIndex = 3;
            this.radioButton20.TabStop = true;
            this.radioButton20.Text = "Pos 2";
            this.radioButton20.UseVisualStyleBackColor = true;
            // 
            // radioButton21
            // 
            this.radioButton21.AutoSize = true;
            this.radioButton21.Location = new System.Drawing.Point(188, 17);
            this.radioButton21.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton21.Name = "radioButton21";
            this.radioButton21.Size = new System.Drawing.Size(91, 17);
            this.radioButton21.TabIndex = 4;
            this.radioButton21.TabStop = true;
            this.radioButton21.Text = "radioButton21";
            this.radioButton21.UseVisualStyleBackColor = true;
            // 
            // radioButton22
            // 
            this.radioButton22.AutoSize = true;
            this.radioButton22.Location = new System.Drawing.Point(279, 17);
            this.radioButton22.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton22.Name = "radioButton22";
            this.radioButton22.Size = new System.Drawing.Size(91, 17);
            this.radioButton22.TabIndex = 5;
            this.radioButton22.TabStop = true;
            this.radioButton22.Text = "radioButton22";
            this.radioButton22.UseVisualStyleBackColor = true;
            // 
            // radioButton23
            // 
            this.radioButton23.AutoSize = true;
            this.radioButton23.Location = new System.Drawing.Point(361, 17);
            this.radioButton23.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton23.Name = "radioButton23";
            this.radioButton23.Size = new System.Drawing.Size(91, 17);
            this.radioButton23.TabIndex = 6;
            this.radioButton23.TabStop = true;
            this.radioButton23.Text = "radioButton23";
            this.radioButton23.UseVisualStyleBackColor = true;
            // 
            // radioButton24
            // 
            this.radioButton24.AutoSize = true;
            this.radioButton24.Location = new System.Drawing.Point(451, 17);
            this.radioButton24.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton24.Name = "radioButton24";
            this.radioButton24.Size = new System.Drawing.Size(91, 17);
            this.radioButton24.TabIndex = 7;
            this.radioButton24.TabStop = true;
            this.radioButton24.Text = "radioButton24";
            this.radioButton24.UseVisualStyleBackColor = true;
            // 
            // radioButton25
            // 
            this.radioButton25.AutoSize = true;
            this.radioButton25.Location = new System.Drawing.Point(18, 17);
            this.radioButton25.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton25.Name = "radioButton25";
            this.radioButton25.Size = new System.Drawing.Size(52, 17);
            this.radioButton25.TabIndex = 0;
            this.radioButton25.TabStop = true;
            this.radioButton25.Text = "Pos 1";
            this.radioButton25.UseVisualStyleBackColor = true;
            // 
            // radioButton26
            // 
            this.radioButton26.AutoSize = true;
            this.radioButton26.Location = new System.Drawing.Point(94, 17);
            this.radioButton26.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton26.Name = "radioButton26";
            this.radioButton26.Size = new System.Drawing.Size(52, 17);
            this.radioButton26.TabIndex = 3;
            this.radioButton26.TabStop = true;
            this.radioButton26.Text = "Pos 2";
            this.radioButton26.UseVisualStyleBackColor = true;
            // 
            // radioButton27
            // 
            this.radioButton27.AutoSize = true;
            this.radioButton27.Location = new System.Drawing.Point(188, 17);
            this.radioButton27.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton27.Name = "radioButton27";
            this.radioButton27.Size = new System.Drawing.Size(91, 17);
            this.radioButton27.TabIndex = 4;
            this.radioButton27.TabStop = true;
            this.radioButton27.Text = "radioButton27";
            this.radioButton27.UseVisualStyleBackColor = true;
            // 
            // radioButton28
            // 
            this.radioButton28.AutoSize = true;
            this.radioButton28.Location = new System.Drawing.Point(279, 17);
            this.radioButton28.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton28.Name = "radioButton28";
            this.radioButton28.Size = new System.Drawing.Size(91, 17);
            this.radioButton28.TabIndex = 5;
            this.radioButton28.TabStop = true;
            this.radioButton28.Text = "radioButton28";
            this.radioButton28.UseVisualStyleBackColor = true;
            // 
            // radioButton29
            // 
            this.radioButton29.AutoSize = true;
            this.radioButton29.Location = new System.Drawing.Point(361, 17);
            this.radioButton29.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton29.Name = "radioButton29";
            this.radioButton29.Size = new System.Drawing.Size(91, 17);
            this.radioButton29.TabIndex = 6;
            this.radioButton29.TabStop = true;
            this.radioButton29.Text = "radioButton29";
            this.radioButton29.UseVisualStyleBackColor = true;
            // 
            // radioButton30
            // 
            this.radioButton30.AutoSize = true;
            this.radioButton30.Location = new System.Drawing.Point(451, 17);
            this.radioButton30.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton30.Name = "radioButton30";
            this.radioButton30.Size = new System.Drawing.Size(91, 17);
            this.radioButton30.TabIndex = 7;
            this.radioButton30.TabStop = true;
            this.radioButton30.Text = "radioButton30";
            this.radioButton30.UseVisualStyleBackColor = true;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.radioButton31);
            this.groupBox8.Controls.Add(this.radioButton32);
            this.groupBox8.Controls.Add(this.radioButton33);
            this.groupBox8.Controls.Add(this.radioButton34);
            this.groupBox8.Controls.Add(this.radioButton35);
            this.groupBox8.Controls.Add(this.radioButton36);
            this.groupBox8.Location = new System.Drawing.Point(6, 345);
            this.groupBox8.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox8.Size = new System.Drawing.Size(580, 45);
            this.groupBox8.TabIndex = 23;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "groupBox8";
            // 
            // radioButton31
            // 
            this.radioButton31.AutoSize = true;
            this.radioButton31.Location = new System.Drawing.Point(18, 17);
            this.radioButton31.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton31.Name = "radioButton31";
            this.radioButton31.Size = new System.Drawing.Size(52, 17);
            this.radioButton31.TabIndex = 0;
            this.radioButton31.TabStop = true;
            this.radioButton31.Text = "Pos 1";
            this.radioButton31.UseVisualStyleBackColor = true;
            // 
            // radioButton32
            // 
            this.radioButton32.AutoSize = true;
            this.radioButton32.Location = new System.Drawing.Point(94, 17);
            this.radioButton32.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton32.Name = "radioButton32";
            this.radioButton32.Size = new System.Drawing.Size(52, 17);
            this.radioButton32.TabIndex = 3;
            this.radioButton32.TabStop = true;
            this.radioButton32.Text = "Pos 2";
            this.radioButton32.UseVisualStyleBackColor = true;
            // 
            // radioButton33
            // 
            this.radioButton33.AutoSize = true;
            this.radioButton33.Location = new System.Drawing.Point(188, 17);
            this.radioButton33.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton33.Name = "radioButton33";
            this.radioButton33.Size = new System.Drawing.Size(91, 17);
            this.radioButton33.TabIndex = 4;
            this.radioButton33.TabStop = true;
            this.radioButton33.Text = "radioButton33";
            this.radioButton33.UseVisualStyleBackColor = true;
            // 
            // radioButton34
            // 
            this.radioButton34.AutoSize = true;
            this.radioButton34.Location = new System.Drawing.Point(279, 17);
            this.radioButton34.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton34.Name = "radioButton34";
            this.radioButton34.Size = new System.Drawing.Size(91, 17);
            this.radioButton34.TabIndex = 5;
            this.radioButton34.TabStop = true;
            this.radioButton34.Text = "radioButton34";
            this.radioButton34.UseVisualStyleBackColor = true;
            // 
            // radioButton35
            // 
            this.radioButton35.AutoSize = true;
            this.radioButton35.Location = new System.Drawing.Point(361, 17);
            this.radioButton35.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton35.Name = "radioButton35";
            this.radioButton35.Size = new System.Drawing.Size(91, 17);
            this.radioButton35.TabIndex = 6;
            this.radioButton35.TabStop = true;
            this.radioButton35.Text = "radioButton35";
            this.radioButton35.UseVisualStyleBackColor = true;
            // 
            // radioButton36
            // 
            this.radioButton36.AutoSize = true;
            this.radioButton36.Location = new System.Drawing.Point(451, 17);
            this.radioButton36.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton36.Name = "radioButton36";
            this.radioButton36.Size = new System.Drawing.Size(91, 17);
            this.radioButton36.TabIndex = 7;
            this.radioButton36.TabStop = true;
            this.radioButton36.Text = "radioButton36";
            this.radioButton36.UseVisualStyleBackColor = true;
            // 
            // radioButton37
            // 
            this.radioButton37.AutoSize = true;
            this.radioButton37.Location = new System.Drawing.Point(18, 17);
            this.radioButton37.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton37.Name = "radioButton37";
            this.radioButton37.Size = new System.Drawing.Size(52, 17);
            this.radioButton37.TabIndex = 0;
            this.radioButton37.TabStop = true;
            this.radioButton37.Text = "Pos 1";
            this.radioButton37.UseVisualStyleBackColor = true;
            // 
            // radioButton38
            // 
            this.radioButton38.AutoSize = true;
            this.radioButton38.Location = new System.Drawing.Point(94, 17);
            this.radioButton38.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton38.Name = "radioButton38";
            this.radioButton38.Size = new System.Drawing.Size(52, 17);
            this.radioButton38.TabIndex = 3;
            this.radioButton38.TabStop = true;
            this.radioButton38.Text = "Pos 2";
            this.radioButton38.UseVisualStyleBackColor = true;
            // 
            // radioButton39
            // 
            this.radioButton39.AutoSize = true;
            this.radioButton39.Location = new System.Drawing.Point(188, 17);
            this.radioButton39.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton39.Name = "radioButton39";
            this.radioButton39.Size = new System.Drawing.Size(91, 17);
            this.radioButton39.TabIndex = 4;
            this.radioButton39.TabStop = true;
            this.radioButton39.Text = "radioButton39";
            this.radioButton39.UseVisualStyleBackColor = true;
            // 
            // radioButton40
            // 
            this.radioButton40.AutoSize = true;
            this.radioButton40.Location = new System.Drawing.Point(279, 17);
            this.radioButton40.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton40.Name = "radioButton40";
            this.radioButton40.Size = new System.Drawing.Size(91, 17);
            this.radioButton40.TabIndex = 5;
            this.radioButton40.TabStop = true;
            this.radioButton40.Text = "radioButton40";
            this.radioButton40.UseVisualStyleBackColor = true;
            // 
            // radioButton41
            // 
            this.radioButton41.AutoSize = true;
            this.radioButton41.Location = new System.Drawing.Point(361, 17);
            this.radioButton41.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton41.Name = "radioButton41";
            this.radioButton41.Size = new System.Drawing.Size(91, 17);
            this.radioButton41.TabIndex = 6;
            this.radioButton41.TabStop = true;
            this.radioButton41.Text = "radioButton41";
            this.radioButton41.UseVisualStyleBackColor = true;
            // 
            // radioButton42
            // 
            this.radioButton42.AutoSize = true;
            this.radioButton42.Location = new System.Drawing.Point(451, 17);
            this.radioButton42.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton42.Name = "radioButton42";
            this.radioButton42.Size = new System.Drawing.Size(91, 17);
            this.radioButton42.TabIndex = 7;
            this.radioButton42.TabStop = true;
            this.radioButton42.Text = "radioButton42";
            this.radioButton42.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.groupBox4);
            this.groupBox3.Controls.Add(this.radioButton13);
            this.groupBox3.Controls.Add(this.radioButton14);
            this.groupBox3.Controls.Add(this.radioButton15);
            this.groupBox3.Controls.Add(this.radioButton16);
            this.groupBox3.Controls.Add(this.radioButton17);
            this.groupBox3.Controls.Add(this.radioButton18);
            this.groupBox3.Location = new System.Drawing.Point(6, 345);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox3.Size = new System.Drawing.Size(580, 56);
            this.groupBox3.TabIndex = 24;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "groupBox3";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.radioButton7);
            this.groupBox4.Controls.Add(this.radioButton8);
            this.groupBox4.Controls.Add(this.radioButton9);
            this.groupBox4.Controls.Add(this.radioButton10);
            this.groupBox4.Controls.Add(this.radioButton11);
            this.groupBox4.Controls.Add(this.radioButton12);
            this.groupBox4.Location = new System.Drawing.Point(6, 345);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox4.Size = new System.Drawing.Size(580, 45);
            this.groupBox4.TabIndex = 23;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "groupBox4";
            // 
            // radioButton7
            // 
            this.radioButton7.AutoSize = true;
            this.radioButton7.Location = new System.Drawing.Point(18, 17);
            this.radioButton7.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton7.Name = "radioButton7";
            this.radioButton7.Size = new System.Drawing.Size(52, 17);
            this.radioButton7.TabIndex = 0;
            this.radioButton7.TabStop = true;
            this.radioButton7.Text = "Pos 1";
            this.radioButton7.UseVisualStyleBackColor = true;
            // 
            // radioButton8
            // 
            this.radioButton8.AutoSize = true;
            this.radioButton8.Location = new System.Drawing.Point(94, 17);
            this.radioButton8.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton8.Name = "radioButton8";
            this.radioButton8.Size = new System.Drawing.Size(52, 17);
            this.radioButton8.TabIndex = 3;
            this.radioButton8.TabStop = true;
            this.radioButton8.Text = "Pos 2";
            this.radioButton8.UseVisualStyleBackColor = true;
            // 
            // radioButton9
            // 
            this.radioButton9.AutoSize = true;
            this.radioButton9.Location = new System.Drawing.Point(188, 17);
            this.radioButton9.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton9.Name = "radioButton9";
            this.radioButton9.Size = new System.Drawing.Size(85, 17);
            this.radioButton9.TabIndex = 4;
            this.radioButton9.TabStop = true;
            this.radioButton9.Text = "radioButton9";
            this.radioButton9.UseVisualStyleBackColor = true;
            // 
            // radioButton10
            // 
            this.radioButton10.AutoSize = true;
            this.radioButton10.Location = new System.Drawing.Point(279, 17);
            this.radioButton10.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton10.Name = "radioButton10";
            this.radioButton10.Size = new System.Drawing.Size(91, 17);
            this.radioButton10.TabIndex = 5;
            this.radioButton10.TabStop = true;
            this.radioButton10.Text = "radioButton10";
            this.radioButton10.UseVisualStyleBackColor = true;
            // 
            // radioButton11
            // 
            this.radioButton11.AutoSize = true;
            this.radioButton11.Location = new System.Drawing.Point(361, 17);
            this.radioButton11.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton11.Name = "radioButton11";
            this.radioButton11.Size = new System.Drawing.Size(91, 17);
            this.radioButton11.TabIndex = 6;
            this.radioButton11.TabStop = true;
            this.radioButton11.Text = "radioButton11";
            this.radioButton11.UseVisualStyleBackColor = true;
            // 
            // radioButton12
            // 
            this.radioButton12.AutoSize = true;
            this.radioButton12.Location = new System.Drawing.Point(451, 17);
            this.radioButton12.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton12.Name = "radioButton12";
            this.radioButton12.Size = new System.Drawing.Size(91, 17);
            this.radioButton12.TabIndex = 7;
            this.radioButton12.TabStop = true;
            this.radioButton12.Text = "radioButton12";
            this.radioButton12.UseVisualStyleBackColor = true;
            // 
            // radioButton13
            // 
            this.radioButton13.AutoSize = true;
            this.radioButton13.Location = new System.Drawing.Point(18, 17);
            this.radioButton13.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton13.Name = "radioButton13";
            this.radioButton13.Size = new System.Drawing.Size(52, 17);
            this.radioButton13.TabIndex = 0;
            this.radioButton13.TabStop = true;
            this.radioButton13.Text = "Pos 1";
            this.radioButton13.UseVisualStyleBackColor = true;
            // 
            // radioButton14
            // 
            this.radioButton14.AutoSize = true;
            this.radioButton14.Location = new System.Drawing.Point(94, 17);
            this.radioButton14.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton14.Name = "radioButton14";
            this.radioButton14.Size = new System.Drawing.Size(52, 17);
            this.radioButton14.TabIndex = 3;
            this.radioButton14.TabStop = true;
            this.radioButton14.Text = "Pos 2";
            this.radioButton14.UseVisualStyleBackColor = true;
            // 
            // radioButton15
            // 
            this.radioButton15.AutoSize = true;
            this.radioButton15.Location = new System.Drawing.Point(188, 17);
            this.radioButton15.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton15.Name = "radioButton15";
            this.radioButton15.Size = new System.Drawing.Size(91, 17);
            this.radioButton15.TabIndex = 4;
            this.radioButton15.TabStop = true;
            this.radioButton15.Text = "radioButton15";
            this.radioButton15.UseVisualStyleBackColor = true;
            // 
            // radioButton16
            // 
            this.radioButton16.AutoSize = true;
            this.radioButton16.Location = new System.Drawing.Point(279, 17);
            this.radioButton16.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton16.Name = "radioButton16";
            this.radioButton16.Size = new System.Drawing.Size(91, 17);
            this.radioButton16.TabIndex = 5;
            this.radioButton16.TabStop = true;
            this.radioButton16.Text = "radioButton16";
            this.radioButton16.UseVisualStyleBackColor = true;
            // 
            // radioButton17
            // 
            this.radioButton17.AutoSize = true;
            this.radioButton17.Location = new System.Drawing.Point(361, 17);
            this.radioButton17.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton17.Name = "radioButton17";
            this.radioButton17.Size = new System.Drawing.Size(91, 17);
            this.radioButton17.TabIndex = 6;
            this.radioButton17.TabStop = true;
            this.radioButton17.Text = "radioButton17";
            this.radioButton17.UseVisualStyleBackColor = true;
            // 
            // radioButton18
            // 
            this.radioButton18.AutoSize = true;
            this.radioButton18.Location = new System.Drawing.Point(451, 17);
            this.radioButton18.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton18.Name = "radioButton18";
            this.radioButton18.Size = new System.Drawing.Size(91, 17);
            this.radioButton18.TabIndex = 7;
            this.radioButton18.TabStop = true;
            this.radioButton18.Text = "radioButton18";
            this.radioButton18.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButton1);
            this.groupBox2.Controls.Add(this.radioButton2);
            this.groupBox2.Controls.Add(this.radioButton3);
            this.groupBox2.Controls.Add(this.radioButton4);
            this.groupBox2.Controls.Add(this.radioButton5);
            this.groupBox2.Controls.Add(this.radioButton6);
            this.groupBox2.Location = new System.Drawing.Point(6, 345);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(580, 45);
            this.groupBox2.TabIndex = 23;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "groupBox2";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(18, 17);
            this.radioButton1.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(52, 17);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Pos 1";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(94, 17);
            this.radioButton2.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(52, 17);
            this.radioButton2.TabIndex = 3;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Pos 2";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(188, 17);
            this.radioButton3.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(85, 17);
            this.radioButton3.TabIndex = 4;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "radioButton3";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(279, 17);
            this.radioButton4.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(85, 17);
            this.radioButton4.TabIndex = 5;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "radioButton4";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.Location = new System.Drawing.Point(361, 17);
            this.radioButton5.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(85, 17);
            this.radioButton5.TabIndex = 6;
            this.radioButton5.TabStop = true;
            this.radioButton5.Text = "radioButton5";
            this.radioButton5.UseVisualStyleBackColor = true;
            // 
            // radioButton6
            // 
            this.radioButton6.AutoSize = true;
            this.radioButton6.Location = new System.Drawing.Point(451, 17);
            this.radioButton6.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton6.Name = "radioButton6";
            this.radioButton6.Size = new System.Drawing.Size(85, 17);
            this.radioButton6.TabIndex = 7;
            this.radioButton6.TabStop = true;
            this.radioButton6.Text = "radioButton6";
            this.radioButton6.UseVisualStyleBackColor = true;
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.Pos18);
            this.groupBox9.Controls.Add(this.Pos17);
            this.groupBox9.Controls.Add(this.Pos13);
            this.groupBox9.Controls.Add(this.Pos16);
            this.groupBox9.Controls.Add(this.Pos14);
            this.groupBox9.Controls.Add(this.Pos15);
            this.groupBox9.Location = new System.Drawing.Point(28, 351);
            this.groupBox9.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox9.Size = new System.Drawing.Size(506, 46);
            this.groupBox9.TabIndex = 23;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Bottom Valve";
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.Pos12);
            this.groupBox10.Controls.Add(this.Pos11);
            this.groupBox10.Controls.Add(this.Pos10);
            this.groupBox10.Controls.Add(this.Pos9);
            this.groupBox10.Controls.Add(this.Pos8);
            this.groupBox10.Controls.Add(this.Pos7);
            this.groupBox10.Location = new System.Drawing.Point(6, 46);
            this.groupBox10.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox10.Size = new System.Drawing.Size(573, 35);
            this.groupBox10.TabIndex = 24;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Top valve";
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.radioButton44);
            this.groupBox11.Controls.Add(this.radioButton43);
            this.groupBox11.Location = new System.Drawing.Point(158, 1);
            this.groupBox11.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox11.Size = new System.Drawing.Size(188, 41);
            this.groupBox11.TabIndex = 25;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "Active Side";
            // 
            // radioButton44
            // 
            this.radioButton44.AutoSize = true;
            this.radioButton44.Location = new System.Drawing.Point(121, 17);
            this.radioButton44.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton44.Name = "radioButton44";
            this.radioButton44.Size = new System.Drawing.Size(50, 17);
            this.radioButton44.TabIndex = 1;
            this.radioButton44.TabStop = true;
            this.radioButton44.Text = "Right";
            this.radioButton44.UseVisualStyleBackColor = true;
            this.radioButton44.CheckedChanged += new System.EventHandler(this.radioButton44_CheckedChanged);
            // 
            // radioButton43
            // 
            this.radioButton43.AutoSize = true;
            this.radioButton43.Location = new System.Drawing.Point(11, 17);
            this.radioButton43.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton43.Name = "radioButton43";
            this.radioButton43.Size = new System.Drawing.Size(43, 17);
            this.radioButton43.TabIndex = 0;
            this.radioButton43.TabStop = true;
            this.radioButton43.Text = "Left";
            this.radioButton43.UseVisualStyleBackColor = true;
            this.radioButton43.CheckedChanged += new System.EventHandler(this.radioButton43_CheckedChanged);
            // 
            // groupBox13
            // 
            this.groupBox13.Controls.Add(this.label2);
            this.groupBox13.Controls.Add(this.label1);
            this.groupBox13.Controls.Add(this.progressBar2);
            this.groupBox13.Controls.Add(this.progressBar1);
            this.groupBox13.Location = new System.Drawing.Point(12, 209);
            this.groupBox13.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox13.Size = new System.Drawing.Size(586, 138);
            this.groupBox13.TabIndex = 27;
            this.groupBox13.TabStop = false;
            this.groupBox13.Text = "Syringe Pump";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(454, 91);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Actual";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(454, 37);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Desired";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(314, 178);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Macro Name";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(411, 18);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(134, 24);
            this.button2.TabIndex = 28;
            this.button2.Text = "Initialize";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(211, 153);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(322, 25);
            this.button3.TabIndex = 29;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 419);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox13);
            this.Controls.Add(this.groupBox11);
            this.Controls.Add(this.groupBox10);
            this.Controls.Add(this.groupBox9);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Fluidics Controller";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.groupBox13.ResumeLayout(false);
            this.groupBox13.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton Pos1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ProgressBar progressBar2;
        private System.Windows.Forms.RadioButton Pos2;
        private System.Windows.Forms.RadioButton Pos3;
        private System.Windows.Forms.RadioButton Pos4;
        private System.Windows.Forms.RadioButton Pos5;
        private System.Windows.Forms.RadioButton Pos6;
        private System.Windows.Forms.RadioButton Pos7;
        private System.Windows.Forms.RadioButton Pos8;
        private System.Windows.Forms.RadioButton Pos9;
        private System.Windows.Forms.RadioButton Pos10;
        private System.Windows.Forms.RadioButton Pos11;
        private System.Windows.Forms.RadioButton Pos12;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RadioButton Pos13;
        private System.Windows.Forms.RadioButton Pos14;
        private System.Windows.Forms.RadioButton Pos15;
        private System.Windows.Forms.RadioButton Pos16;
        private System.Windows.Forms.RadioButton Pos17;
        private System.Windows.Forms.RadioButton Pos18;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton radioButton5;
        private System.Windows.Forms.RadioButton radioButton6;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton radioButton7;
        private System.Windows.Forms.RadioButton radioButton8;
        private System.Windows.Forms.RadioButton radioButton9;
        private System.Windows.Forms.RadioButton radioButton10;
        private System.Windows.Forms.RadioButton radioButton11;
        private System.Windows.Forms.RadioButton radioButton12;
        private System.Windows.Forms.RadioButton radioButton13;
        private System.Windows.Forms.RadioButton radioButton14;
        private System.Windows.Forms.RadioButton radioButton15;
        private System.Windows.Forms.RadioButton radioButton16;
        private System.Windows.Forms.RadioButton radioButton17;
        private System.Windows.Forms.RadioButton radioButton18;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.RadioButton radioButton19;
        private System.Windows.Forms.RadioButton radioButton20;
        private System.Windows.Forms.RadioButton radioButton21;
        private System.Windows.Forms.RadioButton radioButton22;
        private System.Windows.Forms.RadioButton radioButton23;
        private System.Windows.Forms.RadioButton radioButton24;
        private System.Windows.Forms.RadioButton radioButton25;
        private System.Windows.Forms.RadioButton radioButton26;
        private System.Windows.Forms.RadioButton radioButton27;
        private System.Windows.Forms.RadioButton radioButton28;
        private System.Windows.Forms.RadioButton radioButton29;
        private System.Windows.Forms.RadioButton radioButton30;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.RadioButton radioButton31;
        private System.Windows.Forms.RadioButton radioButton32;
        private System.Windows.Forms.RadioButton radioButton33;
        private System.Windows.Forms.RadioButton radioButton34;
        private System.Windows.Forms.RadioButton radioButton35;
        private System.Windows.Forms.RadioButton radioButton36;
        private System.Windows.Forms.RadioButton radioButton37;
        private System.Windows.Forms.RadioButton radioButton38;
        private System.Windows.Forms.RadioButton radioButton39;
        private System.Windows.Forms.RadioButton radioButton40;
        private System.Windows.Forms.RadioButton radioButton41;
        private System.Windows.Forms.RadioButton radioButton42;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.RadioButton radioButton44;
        private System.Windows.Forms.RadioButton radioButton43;
        private System.Windows.Forms.GroupBox groupBox13;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
    }
}


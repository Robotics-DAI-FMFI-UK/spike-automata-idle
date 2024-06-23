using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace State_Automata_IDLE
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newAutomataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAutomataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.blankStateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.activeStateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.automataStateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.finiteStateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.variToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stringToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.globalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.localToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.intToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.globalToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.localToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.booleanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.globalToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.localToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.portToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.motorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.largeMotorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mediumMotorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colorSenzorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.distanceSenzorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tuchSenzorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.driveBaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openAppToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendFSAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.soundsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.menuToolStripMenuItem, this.stateToolStripMenuItem, this.variToolStripMenuItem, this.openAppToolStripMenuItem, this.sendFSAToolStripMenuItem });
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuToolStripMenuItem
            // 
            this.menuToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { this.newAutomataToolStripMenuItem, this.saveAutomataToolStripMenuItem, this.loadToolStripMenuItem, this.exportToolStripMenuItem, this.exitToolStripMenuItem });
            this.menuToolStripMenuItem.Name = "menuToolStripMenuItem";
            this.menuToolStripMenuItem.Size = new System.Drawing.Size(58, 24);
            this.menuToolStripMenuItem.Text = "Menu";
            // 
            // newAutomataToolStripMenuItem
            // 
            this.newAutomataToolStripMenuItem.Name = "newAutomataToolStripMenuItem";
            this.newAutomataToolStripMenuItem.Size = new System.Drawing.Size(121, 24);
            this.newAutomataToolStripMenuItem.Text = "New";
            this.newAutomataToolStripMenuItem.Click += new System.EventHandler(this.newAutomataToolStripMenuItem_Click);
            // 
            // saveAutomataToolStripMenuItem
            // 
            this.saveAutomataToolStripMenuItem.Name = "saveAutomataToolStripMenuItem";
            this.saveAutomataToolStripMenuItem.Size = new System.Drawing.Size(121, 24);
            this.saveAutomataToolStripMenuItem.Text = "Save";
            this.saveAutomataToolStripMenuItem.Click += new System.EventHandler(this.saveAutomataToolStripMenuItem_Click);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(121, 24);
            this.loadToolStripMenuItem.Text = "Load";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(121, 24);
            this.exportToolStripMenuItem.Text = "Export";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(121, 24);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // stateToolStripMenuItem
            // 
            this.stateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { this.blankStateToolStripMenuItem, this.activeStateToolStripMenuItem, this.automataStateToolStripMenuItem, this.finiteStateToolStripMenuItem });
            this.stateToolStripMenuItem.Name = "stateToolStripMenuItem";
            this.stateToolStripMenuItem.Size = new System.Drawing.Size(87, 24);
            this.stateToolStripMenuItem.Text = "New state";
            // 
            // blankStateToolStripMenuItem
            // 
            this.blankStateToolStripMenuItem.Name = "blankStateToolStripMenuItem";
            this.blankStateToolStripMenuItem.Size = new System.Drawing.Size(205, 24);
            this.blankStateToolStripMenuItem.Text = "Blank state";
            this.blankStateToolStripMenuItem.Click += new System.EventHandler(this.blankStateToolStripMenuItem_Click);
            // 
            // activeStateToolStripMenuItem
            // 
            this.activeStateToolStripMenuItem.Name = "activeStateToolStripMenuItem";
            this.activeStateToolStripMenuItem.Size = new System.Drawing.Size(205, 24);
            this.activeStateToolStripMenuItem.Text = "Active state";
            this.activeStateToolStripMenuItem.Click += new System.EventHandler(this.activeStateToolStripMenuItem_Click);
            // 
            // automataStateToolStripMenuItem
            // 
            this.automataStateToolStripMenuItem.Name = "automataStateToolStripMenuItem";
            this.automataStateToolStripMenuItem.Size = new System.Drawing.Size(205, 24);
            this.automataStateToolStripMenuItem.Text = "SubAutomata state";
            this.automataStateToolStripMenuItem.Click += new System.EventHandler(this.automataStateToolStripMenuItem_Click);
            // 
            // finiteStateToolStripMenuItem
            // 
            this.finiteStateToolStripMenuItem.Name = "finiteStateToolStripMenuItem";
            this.finiteStateToolStripMenuItem.Size = new System.Drawing.Size(205, 24);
            this.finiteStateToolStripMenuItem.Text = "Finite state";
            this.finiteStateToolStripMenuItem.Click += new System.EventHandler(this.finiteStateToolStripMenuItem_Click);
            // 
            // variToolStripMenuItem
            // 
            this.variToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { this.stringToolStripMenuItem, this.intToolStripMenuItem, this.booleanToolStripMenuItem, this.portToolStripMenuItem });
            this.variToolStripMenuItem.Name = "variToolStripMenuItem";
            this.variToolStripMenuItem.Size = new System.Drawing.Size(75, 24);
            this.variToolStripMenuItem.Text = "Variable";
            // 
            // stringToolStripMenuItem
            // 
            this.stringToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { this.globalToolStripMenuItem, this.localToolStripMenuItem });
            this.stringToolStripMenuItem.Name = "stringToolStripMenuItem";
            this.stringToolStripMenuItem.Size = new System.Drawing.Size(133, 24);
            this.stringToolStripMenuItem.Text = "String";
            // 
            // globalToolStripMenuItem
            // 
            this.globalToolStripMenuItem.Name = "globalToolStripMenuItem";
            this.globalToolStripMenuItem.Size = new System.Drawing.Size(122, 24);
            this.globalToolStripMenuItem.Text = "Global";
            this.globalToolStripMenuItem.Click += new System.EventHandler(this.GlobalToolStripMenuItem_Click);
            // 
            // localToolStripMenuItem
            // 
            this.localToolStripMenuItem.Name = "localToolStripMenuItem";
            this.localToolStripMenuItem.Size = new System.Drawing.Size(122, 24);
            this.localToolStripMenuItem.Text = "Local";
            this.localToolStripMenuItem.Click += new System.EventHandler(this.LocalToolStripMenuItem_Click);
            // 
            // intToolStripMenuItem
            // 
            this.intToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { this.globalToolStripMenuItem1, this.localToolStripMenuItem1 });
            this.intToolStripMenuItem.Name = "intToolStripMenuItem";
            this.intToolStripMenuItem.Size = new System.Drawing.Size(133, 24);
            this.intToolStripMenuItem.Text = "Int";
            // 
            // globalToolStripMenuItem1
            // 
            this.globalToolStripMenuItem1.Name = "globalToolStripMenuItem1";
            this.globalToolStripMenuItem1.Size = new System.Drawing.Size(122, 24);
            this.globalToolStripMenuItem1.Text = "Global";
            this.globalToolStripMenuItem1.Click += new System.EventHandler(this.GlobalToolStripMenuItem1_Click);
            // 
            // localToolStripMenuItem1
            // 
            this.localToolStripMenuItem1.Name = "localToolStripMenuItem1";
            this.localToolStripMenuItem1.Size = new System.Drawing.Size(122, 24);
            this.localToolStripMenuItem1.Text = "Local";
            this.localToolStripMenuItem1.Click += new System.EventHandler(this.LocalToolStripMenuItem1_Click);
            // 
            // booleanToolStripMenuItem
            // 
            this.booleanToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { this.globalToolStripMenuItem2, this.localToolStripMenuItem2 });
            this.booleanToolStripMenuItem.Name = "booleanToolStripMenuItem";
            this.booleanToolStripMenuItem.Size = new System.Drawing.Size(133, 24);
            this.booleanToolStripMenuItem.Text = "Boolean";
            // 
            // globalToolStripMenuItem2
            // 
            this.globalToolStripMenuItem2.Name = "globalToolStripMenuItem2";
            this.globalToolStripMenuItem2.Size = new System.Drawing.Size(122, 24);
            this.globalToolStripMenuItem2.Text = "Global";
            this.globalToolStripMenuItem2.Click += new System.EventHandler(this.GlobalToolStripMenuItem2_Click);
            // 
            // localToolStripMenuItem2
            // 
            this.localToolStripMenuItem2.Name = "localToolStripMenuItem2";
            this.localToolStripMenuItem2.Size = new System.Drawing.Size(122, 24);
            this.localToolStripMenuItem2.Text = "Local";
            this.localToolStripMenuItem2.Click += new System.EventHandler(this.LocalToolStripMenuItem2_Click);
            // 
            // portToolStripMenuItem
            // 
            this.portToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { this.motorToolStripMenuItem, this.colorSenzorToolStripMenuItem, this.distanceSenzorToolStripMenuItem, this.tuchSenzorToolStripMenuItem, this.driveBaseToolStripMenuItem });
            this.portToolStripMenuItem.Name = "portToolStripMenuItem";
            this.portToolStripMenuItem.Size = new System.Drawing.Size(133, 24);
            this.portToolStripMenuItem.Text = "Port";
            // 
            // motorToolStripMenuItem
            // 
            this.motorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { this.largeMotorToolStripMenuItem, this.mediumMotorToolStripMenuItem });
            this.motorToolStripMenuItem.Name = "motorToolStripMenuItem";
            this.motorToolStripMenuItem.Size = new System.Drawing.Size(181, 24);
            this.motorToolStripMenuItem.Text = "Motor";
            // 
            // largeMotorToolStripMenuItem
            // 
            this.largeMotorToolStripMenuItem.Name = "largeMotorToolStripMenuItem";
            this.largeMotorToolStripMenuItem.Size = new System.Drawing.Size(174, 24);
            this.largeMotorToolStripMenuItem.Text = "LargeMotor";
            this.largeMotorToolStripMenuItem.Click += new System.EventHandler(this.largeMotorToolStripMenuItem_Click);
            // 
            // mediumMotorToolStripMenuItem
            // 
            this.mediumMotorToolStripMenuItem.Name = "mediumMotorToolStripMenuItem";
            this.mediumMotorToolStripMenuItem.Size = new System.Drawing.Size(174, 24);
            this.mediumMotorToolStripMenuItem.Text = "MediumMotor";
            this.mediumMotorToolStripMenuItem.Click += new System.EventHandler(this.mediumMotorToolStripMenuItem_Click);
            // 
            // colorSenzorToolStripMenuItem
            // 
            this.colorSenzorToolStripMenuItem.Name = "colorSenzorToolStripMenuItem";
            this.colorSenzorToolStripMenuItem.Size = new System.Drawing.Size(181, 24);
            this.colorSenzorToolStripMenuItem.Text = "Color sensor";
            this.colorSenzorToolStripMenuItem.Click += new System.EventHandler(this.ColorSensorToolStripMenuItem_Click);
            // 
            // distanceSenzorToolStripMenuItem
            // 
            this.distanceSenzorToolStripMenuItem.Name = "distanceSenzorToolStripMenuItem";
            this.distanceSenzorToolStripMenuItem.Size = new System.Drawing.Size(181, 24);
            this.distanceSenzorToolStripMenuItem.Text = "Distance sensor";
            this.distanceSenzorToolStripMenuItem.Click += new System.EventHandler(this.DistanceSensorToolStripMenuItem_Click);
            // 
            // tuchSenzorToolStripMenuItem
            // 
            this.tuchSenzorToolStripMenuItem.Name = "tuchSenzorToolStripMenuItem";
            this.tuchSenzorToolStripMenuItem.Size = new System.Drawing.Size(181, 24);
            this.tuchSenzorToolStripMenuItem.Text = "Force sensor";
            this.tuchSenzorToolStripMenuItem.Click += new System.EventHandler(this.ForceSensorToolStripMenuItem_Click);
            // 
            // driveBaseToolStripMenuItem
            // 
            this.driveBaseToolStripMenuItem.Name = "driveBaseToolStripMenuItem";
            this.driveBaseToolStripMenuItem.Size = new System.Drawing.Size(181, 24);
            this.driveBaseToolStripMenuItem.Text = "Drive base";
            this.driveBaseToolStripMenuItem.Click += new System.EventHandler(this.DriveBaseToolStripMenuItem_Click);
            // 
            // openAppToolStripMenuItem
            // 
            this.openAppToolStripMenuItem.Name = "openAppToolStripMenuItem";
            this.openAppToolStripMenuItem.Size = new System.Drawing.Size(89, 24);
            this.openAppToolStripMenuItem.Text = "Open App";
            this.openAppToolStripMenuItem.Click += new System.EventHandler(this.openAppToolStripMenuItem_Click);
            // 
            // sendFSAToolStripMenuItem
            // 
            this.sendFSAToolStripMenuItem.Name = "sendFSAToolStripMenuItem";
            this.sendFSAToolStripMenuItem.Size = new System.Drawing.Size(83, 24);
            this.sendFSAToolStripMenuItem.Text = "Send FSA";
            this.sendFSAToolStripMenuItem.Click += new System.EventHandler(this.sendFSAToolStripMenuItem_Click);
            // 
            // soundsToolStripMenuItem
            // 
            this.soundsToolStripMenuItem.Name = "soundsToolStripMenuItem";
            this.soundsToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(0, 28);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(200, 200);
            this.treeView1.TabIndex = 1;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.Click += new System.EventHandler(this.treeView1_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { this.columnHeader1 });
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 234);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(200, 204);
            this.listView1.TabIndex = 3;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.List;
            this.listView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ListView1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(594, 78);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(50, 54);
            this.textBox1.TabIndex = 4;
            this.textBox1.Text = "something not working";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.Location = new System.Drawing.Point(15, 15);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Click += new System.EventHandler(this.MainForm_Click);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1_KeyPress);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.ToolStripMenuItem openAppToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sendFSAToolStripMenuItem;

        private System.Windows.Forms.ToolStripMenuItem largeMotorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mediumMotorToolStripMenuItem;

        private System.Windows.Forms.TextBox textBox1;

        private System.Windows.Forms.ToolStripMenuItem driveBaseToolStripMenuItem;

        private System.Windows.Forms.ToolStripMenuItem finiteStateToolStripMenuItem;

        private System.Windows.Forms.ColumnHeader columnHeader1;

        public System.Windows.Forms.ListView listView1;

        private System.Windows.Forms.ToolStripMenuItem soundsToolStripMenuItem;

        private System.Windows.Forms.ToolStripMenuItem distanceSenzorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tuchSenzorToolStripMenuItem;

        private System.Windows.Forms.ToolStripMenuItem variToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stringToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem globalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem localToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem intToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem globalToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem localToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem booleanToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem globalToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem localToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem portToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem motorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem colorSenzorToolStripMenuItem;

        public System.Windows.Forms.TreeView treeView1;

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newAutomataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAutomataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem blankStateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem activeStateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem automataStateToolStripMenuItem;

        #endregion
    }
}
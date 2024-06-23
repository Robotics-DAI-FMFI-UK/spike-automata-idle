using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using State_Automata_IDLE.States;
using State_Automata_IDLE.Memory;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using Newtonsoft.Json;
using Application = System.Windows.Forms.Application;
using Clipboard = System.Windows.Clipboard;
using MessageBox = System.Windows.Forms.MessageBox;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace State_Automata_IDLE
{
    public class FiniteStateMachine
    {
        // [name, Automata]
        public Dictionary<string, Automata> AutomataDict { get; set; } = new Dictionary<string, Automata>();

        // [actualAutomata:name, State]
        public Dictionary<string, State> StateDict { get; set; } = new Dictionary<string, State>();

        // [actualAutomata:stateName, List<Transition>]
        public Dictionary<string, List<Transition>> TransitionDict { get; set; } =
            new Dictionary<string, List<Transition>>();

        public readonly List<string> ParallelAutomata = new List<string>();
        public Data GlobalData { get; set; } = new Data();
        public List<string> NamesOfVariables { get; set; } = new List<string>();
        public int Counter { get; set; }

        // ReSharper disable once InconsistentNaming
        public int CounterID { get; set; }
        public List<string> NamesOfStates { get; set; } = new List<string>();
    }

    public partial class MainForm : Form
    {
        // ReSharper disable once InconsistentNaming
        public FiniteStateMachine FSM { get; set; } = new FiniteStateMachine();
        public List<string> AllAutomataList;

        // ReSharper disable once InconsistentNaming
        public Automata ActualAutomata;
        public State EditState;
        public Transition EditTransition;
        public bool DrawLine;
        public int FontSize = 5;
        public Point MousePoint;
        public Point StartPoint;
        public Point EndPoint;
        public Keys Key = Keys.None;
        public TextBox NameTextBox;
        public TextBox ValueTextBox;
        public Label HintBoxLabel;
        public Label NameTextBoxLabel;
        public Label ValueTextBoxLabel;
        private Process _process;

        public static readonly Dictionary<string, (int procedureId, int numArgs)> ProcedureIdMap =
            new Dictionary<string, (int, int)>
            {
                { "print_message", (0, 1) },
                { "print_value", (1, 2) },
                { "matrix_clear", (2, 0) },
                { "matrix_orientation", (3, 1) },
                { "matrix_set_pixel", (4, 3) },
                { "matrix_set_image", (5, 1) },
                { "matrix_letter", (6, 1) },
                { "matrix_digit", (7, 1) },
                { "play_tone", (8, 2) },
                { "stop_play", (9, 0) },
                { "set_local_number", (10, 2) },
                { "set_local_string", (11, 2) },
                { "set_local_boolean", (12, 2) },
                { "set_global_number", (13, 2) },
                { "set_global_string", (14, 2) },
                { "set_global_boolean", (15, 2) },
                { "motor_on", (16, 2) },
                { "motor_on_for", (17, 3) },
                { "motor_turn", (18, 3) },
                { "motor_off", (19, 1) },
                { "base_speed", (20, 2) },
                { "base_fwd", (21, 2) },
                { "base_on", (22, 1) },
                { "base_off", (23, 1) },
                { "base_turn", (24, 3) },
                { "base_tank", (25, 3) },
                { "base_tank_for", (26, 4) },
                { "base_tank_turn", (27, 3) },
                { "delay", (28, 1) },
                { "while", (29, 1) },
                { "wend", (30, 0) },
                { "if", (31, 1) },
                { "else", (32, 0) },
                { "endif", (33, 0) },
                { "lock", (34, 0) },
                { "unlock", (35, 0) }
            };

        public static readonly Dictionary<string, (int functionId, int numArgs)> FunctionIdMap =
            new Dictionary<string, (int, int)>
            {
                { "read", (100, 0) },
                { "read_line", (101, 0) },
                { "angle", (102, 1) },
                { "motor_angle", (103, 1) },
                { "motor_speed", (104, 1) },
                { "left_button", (105, 0) },
                { "right_button", (106, 0) },
                { "bt_button", (107, 0) },
                { "distance", (108, 1) },
                { "force", (109, 1) },
                { "color", (110, 1) },
                { "reflect", (111, 1) },
                { "ambient", (112, 1) },
                { "concat", (113, 2) },
                { "substr", (114, 3) },
                { "eqstr", (115, 2) },
                { "setchar", (116, 3) },
                { "ascii", (117, 1) },
                { "chr", (118, 1) },
                { "eq", (119, 2) },
                { "less", (120, 2) },
                { "more", (121, 2) },
                { "between", (122, 3) },
                { "plus", (123, 2) },
                { "minus", (124, 2) },
                { "times", (125, 2) },
                { "divide", (126, 2) },
                { "mod", (127, 2) },
                { "sqrt", (128, 1) },
                { "abs", (129, 1) },
                { "sgn", (130, 1) },
                { "sin", (131, 1) },
                { "cos", (132, 1) },
                { "atan2", (133, 2) },
                { "ceil", (134, 1) },
                { "floor", (135, 1) },
                { "and", (136, 2) },
                { "or", (137, 2) },
                { "xor", (138, 2) },
                { "not", (139, 1) }
            };


        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            treeView1.Location = new Point(0, 24);
            treeView1.Size = new Size(Convert.ToInt32(Screen.PrimaryScreen.Bounds.Width * 0.1),
                Convert.ToInt32(Screen.PrimaryScreen.Bounds.Height * 0.472));
            listView1.Location = new Point(0, Convert.ToInt32(Screen.PrimaryScreen.Bounds.Height * 0.5));
            listView1.View = View.Details;
            listView1.Size = new Size(Convert.ToInt32(Screen.PrimaryScreen.Bounds.Width * 0.1),
                Convert.ToInt32(Screen.PrimaryScreen.Bounds.Height * 0.5));
            textBox1.Location = new Point(Convert.ToInt32(Screen.PrimaryScreen.Bounds.Width * 0.82), 24);
            textBox1.Size = new Size(Convert.ToInt32(Screen.PrimaryScreen.Bounds.Width * 0.18),
                Convert.ToInt32(Screen.PrimaryScreen.Bounds.Height * 0.98));
            textBox1.ScrollBars = ScrollBars.Both;
            string helpText = LoadHelpText();
            textBox1.Text = helpText != "" ? helpText : "Something went wrong..";
            textBox1.WordWrap = false;
            treeView1.Font = new Font("", FontSize * 2);
            listView1.Font = new Font("", FontSize * 2);
            textBox1.Font = new Font("", FontSize * 2);
            Start();
        }

        private string LoadHelpText()
        {
            try
            {
                using (var sr = new StreamReader("Function.txt"))
                    return sr.ReadToEnd();
            }
            catch (IOException e)
            {
                MessageBox.Show("The file could not be read:" + e.Message);
            }

            return "";
        }

        private void Clear()
        {
            FSM.NamesOfStates.Clear();
            FSM.AutomataDict = new Dictionary<string, Automata>();
            ActualAutomata = null;
            EditState = null;
            EditTransition = null;
            DrawLine = false;
            StartPoint = EndPoint = new Point();
            FSM.GlobalData.Clear();
            listView1.Clear();
            treeView1.Nodes.Clear();
        }

        private void Start()
        {
            Clear();
            while (true)
            {
                SetUpDialog("Unnamed", "");
                NameTextBoxLabel.Text = "Name of automata:";
                HintBoxLabel.Text =
                    "Write new name for your automata.\nOnly letters and numbers are supported.\nMaximum length is 20 letters.";
                HintBoxLabel.Size = new Size(300, 60);
                ValueTextBox.Visible = false;
                if (Dialog(NameTextBox.Text) && IsCorrect(NameTextBox.Text))
                    break;
            }

            string newName = NameTextBox.Text;
            ActualAutomata = new Automata(this, treeView1, newName);
            treeView1.SelectedNode = treeView1.Nodes[0];
            FSM.NamesOfStates.Add(newName);
            FSM.AutomataDict.Add(newName, ActualAutomata);
            FSM.ParallelAutomata.Add(newName);
            WriteListView();
            ActualAutomata.Enable();
            Invalidate();
        }

        private void Form_Paint(object sender, PaintEventArgs e)
        {
            if (ActualAutomata == null)
                return;

            foreach (string state in ActualAutomata.StatesList)
            {
                FSM.StateDict[ActualAutomata.Name + ":" + state].Draw(e.Graphics);
                if (!FSM.TransitionDict.Keys.Contains(ActualAutomata.Name + ":" + state))
                    continue;
                foreach (Transition transition in FSM.TransitionDict[ActualAutomata.Name + ":" + state])
                    transition.Draw(e.Graphics);
            }

            if (DrawLine)
                e.Graphics.DrawLine(Pens.Black, StartPoint, EndPoint);
            e.Graphics.DrawString(ActualAutomata.Name, new Font("", FontSize * 6), Brushes.Black,
                Convert.ToInt32(Screen.PrimaryScreen.Bounds.Width * 0.1), 20);
            e.Graphics.DrawString("Help: F1/H", new Font("", FontSize * 3), Brushes.Black,
                Convert.ToInt32(Screen.PrimaryScreen.Bounds.Width * 0.7), 20);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_process != null)
            {
                Process[] process = Process.GetProcessesByName("msedge");
                foreach (Process proc in process)
                {
                    Console.WriteLine(proc.MainWindowTitle);
                    if (proc.MainWindowTitle.StartsWith("Pybricks Code"))
                    {
                        proc.Kill();
                        break;
                    }
                }

                Application.Exit();
            }
        }

        private void blankStateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetUpDialog("Unnamed", "");
            NameTextBoxLabel.Text = "Name of state:";
            HintBoxLabel.Text =
                "Write new name for created state.\nOnly letters and numbers are supported.\nMaximum length is 20 letters.";
            HintBoxLabel.Size = new Size(300, 60);
            ValueTextBox.Visible = false;
            if (Dialog(NameTextBox.Text) && IsCorrectNameOfState(NameTextBox.Text))
                CreateState(new BlankState(this, ActualAutomata.Name, 400, 100, NameTextBox.Text));
        }

        private void activeStateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetUpDialog("Unnamed", "");
            NameTextBoxLabel.Text = "Name of state:";
            HintBoxLabel.Text =
                "Write new name for created state.\nOnly letters and numbers are supported.\nMaximum length is 20 letters.";
            HintBoxLabel.Size = new Size(300, 60);
            ValueTextBox.Visible = false;
            if (Dialog(NameTextBox.Text) && IsCorrectNameOfState(NameTextBox.Text))
                CreateState(new ActiveState(this, ActualAutomata.Name, 400, 100, NameTextBox.Text));
        }

        public void CreateState(State state)
        {
            ActualAutomata.StatesList.Add(state.Name);
            FSM.NamesOfStates.Add(state.Name);
            FSM.StateDict.Add(ActualAutomata.Name + ":" + state.Name, state);
            Invalidate();
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            MousePoint = new Point(e.X, e.Y);
            if (DrawLine)
                EndPoint = MousePoint;

            if (EditTransition?.To != null)
                EditTransition.Relocate(e.X, e.Y);

            EditState?.Relocate(e.X, e.Y);
            ActualAutomata.RelocateTransition(EditState);

            Invalidate();
        }

        private void newAutomataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetUpDialog("Unnamed", "");
            NameTextBoxLabel.Text = "Name of automata:";
            HintBoxLabel.Text =
                "Write new name for created automata.\nOnly letters and numbers are supported.\nMaximum length is 20 letters.";
            HintBoxLabel.Size = new Size(300, 60);
            ValueTextBox.Visible = false;
            if (Dialog(NameTextBox.Text) && IsCorrectNameOfState(NameTextBox.Text))
            {
                string newName = NameTextBox.Text;
                ActualAutomata.Disable();
                ActualAutomata = new Automata(this, treeView1, newName);
                FSM.AutomataDict.Add(ActualAutomata.Name, ActualAutomata);
                FSM.ParallelAutomata.Add(ActualAutomata.Name);
                FSM.NamesOfStates.Add(newName);
                EditState = null;
                EditTransition = null;
                treeView1.SelectedNode = treeView1.Nodes.Find(newName, false)[0];
                WriteListView();
                ActualAutomata.Enable();
            }
        }

        public void finiteStateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetUpDialog("Unnamed finite state", "");
            NameTextBoxLabel.Text = "Name of finite state:";
            HintBoxLabel.Text =
                "Write new name for created state.\nOnly letters and numbers are supported.\nMaximum length is 20 letters.";
            HintBoxLabel.Size = new Size(300, 60);
            ValueTextBox.Visible = false;
            if (Dialog(NameTextBox.Text) && IsCorrectNameOfState(NameTextBox.Text))
            {
                State state = new BlankState(this, ActualAutomata.Name, 400, 100, NameTextBox.Text);
                CreateState(state);
                FSM.TransitionDict.Add(ActualAutomata.Name + ":" + state.Name, new List<Transition>
                    { new Transition(this, state, 1), new Transition(this, state, 2) });
            }
        }

        private void automataStateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetUpDialog("Unnamed", "");
            NameTextBoxLabel.Text = "Name of state:";
            HintBoxLabel.Text =
                "Write new name for created state.\nOnly letters and numbers are supported.\nMaximum length is 20 letters.";
            HintBoxLabel.Size = new Size(300, 60);
            ValueTextBox.Visible = false;
            if (Dialog(NameTextBox.Text) && IsCorrectNameOfState(NameTextBox.Text))
            {
                string newName = NameTextBox.Text;
                Automata automata = new Automata(this, ActualAutomata.TreeNode, newName);
                CreateState(new AutomataState(this, 400, 100, ActualAutomata, automata, newName));
                FSM.AutomataDict.Add(automata.Name, automata);
                ActualAutomata.TreeNode.Expand();
                automata.Disable();
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ActualAutomata.Disable();
            foreach (Automata automata in FSM.AutomataDict.Values)
            {
                ActualAutomata = automata.FindAutomata(e.Node.Text);
                if (ActualAutomata != null)
                    break;
            }

            if (ActualAutomata == null)
                ActualAutomata = FSM.AutomataDict[FSM.ParallelAutomata[0]];
            ActualAutomata.Enable();
            WriteListView();
            EditState = null;
            EditTransition = null;
            Invalidate();
        }

        private void RenameState(State state)
        {
            SetUpDialog("Unnamed", "");
            NameTextBoxLabel.Text = "New name for state:";
            HintBoxLabel.Text =
                "Write new name for selected state.\nOnly letters and numbers are supported.\nMaximum length is 20 letters.";
            HintBoxLabel.Size = new Size(300, 60);
            ValueTextBox.Visible = false;
            if (Dialog(NameTextBox.Text) && IsCorrectNameOfState(NameTextBox.Text))
                state.Rename(NameTextBox.Text);
        }

        private void ChangeLabel(Transition transition)
        {
            SetUpDialog("", "");
            NameTextBoxLabel.Text = "New label for transition:";
            HintBoxLabel.Text =
                "Write new label for selected transition.\nOnly letters and numbers are supported.\nMaximum length is 20 letters.";
            HintBoxLabel.Size = new Size(300, 60);
            ValueTextBox.Visible = false;
            if (Dialog(NameTextBox.Text))
                transition.Label = NameTextBox.Text;
        }

        private void treeView1_Click(object sender, EventArgs e)
        {
            MouseEventArgs mouseButtons = (MouseEventArgs)e;
            if (treeView1.SelectedNode == null)
            {
                return;
            }

            Rectangle rectangle = treeView1.SelectedNode.Bounds;
            if (mouseButtons.X < rectangle.Location.X || mouseButtons.X > rectangle.Location.X + rectangle.Size.Width ||
                mouseButtons.Y < rectangle.Location.Y || mouseButtons.Y > rectangle.Location.Y + rectangle.Size.Height)
            {
                return;
            }

            if (mouseButtons.Button == MouseButtons.Right)
            {
                ActualAutomata.Disable();
                Automata owner = FindAutomataWithState(ActualAutomata.Name);
                if (owner != null)
                    owner.DeleteState(owner.FindState(ActualAutomata.Name));
                else
                    ActualAutomata.RecursiveDelete();

                if (treeView1.Nodes.Count == 0)
                    Start();
                ActualAutomata.Enable();
            }

            if (mouseButtons.Button == MouseButtons.Left)
            {
                SetUpDialog("Unnamed", "");
                NameTextBoxLabel.Text = "New name for selected automata:";
                HintBoxLabel.Text =
                    "Write new name for selected automata.\nOnly letters and numbers are supported.\nMaximum length is 20 letters.";
                HintBoxLabel.Size = new Size(300, 60);
                ValueTextBox.Visible = false;
                if (Dialog(NameTextBox.Text) && IsCorrectNameOfState(NameTextBox.Text))
                {
                    string newName = NameTextBox.Text;
                    string oldName = treeView1.SelectedNode.Text;
                    FSM.NamesOfStates.Remove(oldName);
                    FSM.NamesOfStates.Add(newName);
                    if (FSM.ParallelAutomata.Contains(treeView1.SelectedNode.Text))
                    {
                        FSM.ParallelAutomata.Remove(oldName);
                        FSM.ParallelAutomata.Add(newName);
                        ActualAutomata.Rename(newName);
                    }
                    else
                    {
                        State state = FindAutomataWithState(treeView1.SelectedNode.Text)
                            .FindState(treeView1.SelectedNode.Text);
                        state.Rename(newName);
                    }
                }
            }

            Invalidate();
        }

        private Automata FindAutomataWithState(string name)
        {
            foreach (Automata automata in FSM.AutomataDict.Values)
            {
                Automata outAutomata = automata.FindAutomataWithState(name);
                if (outAutomata != null)
                    return outAutomata;
            }

            return null;
        }

        private static bool IsCorrect(string newName)
        {
            if (newName.ToLower() == "init" || newName == "Unnamed" || newName.Length == 0 || newName.Length > 20)
                return false;

            foreach (char letter in newName)
            {
                if (Between(letter, 'a', 'z') || Between(letter, 'A', 'Z') ||
                    Between(letter, '0', '9') || letter == '-' || letter == '_')
                    continue;
                return false;
            }

            return Between(newName[0], 'a', 'z') || Between(newName[0], 'A', 'Z');
        }

        private bool IsCorrectNameOfState(string newName)
        {
            return IsCorrect(newName) && !FSM.NamesOfStates.Contains(newName);
        }

        private bool IsCorrectNameOfVariable(string newName)
        {
            return IsCorrect(newName) && !FSM.NamesOfVariables.Contains(newName);
        }

        private static bool Between(char letter, int start, int end)
        {
            return start <= letter && letter <= end;
        }

        private static void ShowHelp()
        {
            try
            {
                using (var sr = new StreamReader("HelpText.txt"))
                    MessageBox.Show(sr.ReadToEnd(), "Help");
            }
            catch (IOException e)
            {
                MessageBox.Show("The file could not be read:" + e.Message);
            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (ActiveControl.GetType() == new TextBox().GetType())
            {
                return;
            }

            e.Handled = true;
        }

        private void MainForm_Click(object sender, EventArgs e)
        {
            EditState = null;
            EditTransition = null;
            DrawLine = false;
            Key = Keys.None;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (ActiveControl.GetType() == new TextBox().GetType())
                return;

            e.Handled = true;
            if (e.KeyCode == Keys.Menu)
            {
                MainForm_Click(null, null);
                return;
            }

            if (Key != Keys.None)
                return;

            switch (e.KeyCode)
            {
                case Keys.H:
                case Keys.F1:
                    ShowHelp();
                    return;
                case Keys.B:
                    blankStateToolStripMenuItem_Click(sender, e);
                    return;
                case Keys.A:
                    activeStateToolStripMenuItem_Click(sender, e);
                    return;
                case Keys.S:
                    automataStateToolStripMenuItem_Click(sender, e);
                    return;
                case Keys.F:
                    finiteStateToolStripMenuItem_Click(sender, e);
                    return;
                case Keys.Add:
                    if (FontSize < 10)
                        FontSize++;
                    treeView1.Font = new Font("", FontSize * 2);
                    listView1.Font = new Font("", FontSize * 2);
                    textBox1.Font = new Font("", FontSize * 2);
                    Invalidate();
                    return;
                case Keys.Subtract:
                    if (FontSize > 1)
                        FontSize--;
                    treeView1.Font = new Font("", FontSize * 2);
                    listView1.Font = new Font("", FontSize * 2);
                    textBox1.Font = new Font("", FontSize * 2);
                    Invalidate();
                    return;
            }

            Key = e.KeyCode;

            State state = ActualAutomata.FindClickedState(MousePoint.X, MousePoint.Y);
            Transition transition = ActualAutomata.FindClickedTransition(MousePoint.X, MousePoint.Y);

            switch (Key)
            {
                case Keys.M when state != null:
                    EditState = state;
                    Invalidate();
                    return;
                case Keys.M when transition != null:
                    EditTransition = transition;
                    Invalidate();
                    return;
                case Keys.T when state != null:
                    EditTransition = new Transition(this, state);
                    DrawLine = true;
                    StartPoint = new Point(state.PositionX, state.PositionY);
                    EndPoint = new Point(state.PositionX, state.PositionY);
                    Invalidate();
                    return;
                case Keys.C when transition != null:
                    EditTransition = transition;
                    EditTransition.AddPoint();
                    Invalidate();
                    return;
                case Keys.D when state != null:
                    ActualAutomata.DeleteState(state);
                    Invalidate();
                    return;
                case Keys.D when transition != null:
                    DeletePointOfTransition(transition);
                    Invalidate();
                    return;
                case Keys.R when state != null:
                {
                    if (state.Name != "Init")
                        RenameState(state);
                    Key = Keys.None;
                    Invalidate();
                    return;
                }
                case Keys.R when transition != null:
                {
                    if (!(transition.IsInit() || transition.IsFinite()))
                        ChangeLabel(transition);
                    Key = Keys.None;
                    Invalidate();
                    return;
                }
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (ActiveControl.GetType() == new TextBox().GetType())
                return;

            e.Handled = true;
            if (Key != e.KeyCode)
                return;
            Key = Keys.None;

            State state = ActualAutomata.FindClickedState(MousePoint.X, MousePoint.Y);

            EditState = null;

            if (EditTransition != null)
            {
                DrawLine = false;
                if (EditTransition.To == null)
                    EditTransition.End(state);
                else
                    EditTransition.RemoveReference();
                EditTransition = null;
            }

            Invalidate();
        }

        private void DeletePointOfTransition(Transition transition)
        {
            if (transition.DeletePoint())
                ActualAutomata.DeleteTransition(transition);
            else
                transition.RelocateScripts();
            EditTransition = null;
        }

        private void CreateNewGlobalData(string name, Variables variable)
        {
            FSM.GlobalData.Add(name, variable);
            FSM.NamesOfVariables.Add(name);
            WriteListView();
        }

        private void CreateNewLocalData(string name, Variables variable)
        {
            ActualAutomata.LocalData.Add(name, variable);
            FSM.NamesOfVariables.Add(name);
            WriteListView();
        }

        private bool Dialog(string dummyName)
        {
            DialogResult result = ShowCreateDialog();

            if (result != DialogResult.OK)
                return false;
            if (NameTextBox.Text == dummyName)
                return false;
            return true;
        }

        private void SetUpDialog(string firstBox, string secondBox)
        {
            NameTextBox = new TextBox();
            NameTextBox.Location = new Point(20, 35);
            NameTextBox.Width = 250;
            NameTextBox.Text = firstBox;

            ValueTextBox = new TextBox();
            ValueTextBox.Location = new Point(20, 85);
            ValueTextBox.Width = 250;
            ValueTextBox.Text = secondBox;

            NameTextBoxLabel = new Label();
            NameTextBoxLabel.Location = new Point(20, 10);
            NameTextBoxLabel.Width = 250;

            ValueTextBoxLabel = new Label();
            ValueTextBoxLabel.Location = new Point(20, 60);
            ValueTextBoxLabel.Width = 250;

            HintBoxLabel = new Label();
            HintBoxLabel.Location = new Point(20, 110);
            HintBoxLabel.Width = 250;
        }

        private DialogResult ShowCreateDialog()
        {
            Form dialog = new Form();
            dialog.Text = "Input from user...";
            dialog.Width = 300;
            dialog.Height = 250;

            Button buttonOk = new Button();
            buttonOk.Text = "OK";
            buttonOk.DialogResult = DialogResult.OK;
            buttonOk.Location = new Point(50, 180);

            Button buttonCancel = new Button();
            buttonCancel.Text = "Cancel";
            buttonCancel.DialogResult = DialogResult.Cancel;
            buttonCancel.Location = new Point(180, 180);

            dialog.Controls.Add(NameTextBoxLabel);
            dialog.Controls.Add(ValueTextBoxLabel);
            dialog.Controls.Add(HintBoxLabel);
            dialog.Controls.Add(NameTextBox);
            dialog.Controls.Add(ValueTextBox);
            dialog.Controls.Add(buttonOk);
            dialog.Controls.Add(buttonCancel);

            dialog.AcceptButton = buttonOk;
            dialog.CancelButton = buttonCancel;
            dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
            dialog.StartPosition = FormStartPosition.CenterParent;


            DialogResult result = dialog.ShowDialog();
            dialog.Dispose();
            return result;
        }

        private void GlobalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetUpDialog("Unnamed global string", "Empty string");
            NameTextBoxLabel.Text = "Name of global string:";
            ValueTextBoxLabel.Text = "Value of global string:";
            if (Dialog(NameTextBox.Text) && IsCorrectNameOfVariable(NameTextBox.Text) && ValueTextBox.Text.Length <= 30)
                CreateNewGlobalData(NameTextBox.Text, new Variables("string", ValueTextBox.Text));
        }

        private void LocalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetUpDialog("Unnamed local String", "Empty string");
            NameTextBoxLabel.Text = "Name of local string:";
            ValueTextBoxLabel.Text = "Value of local string:";
            if (Dialog(NameTextBox.Text) && IsCorrectNameOfVariable(NameTextBox.Text) && ValueTextBox.Text.Length <= 30)
                CreateNewLocalData(NameTextBox.Text, new Variables("string", ValueTextBox.Text));
        }

        private void GlobalToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SetUpDialog("Unnamed global Int", "0");
            NameTextBoxLabel.Text = "Name of global integer:";
            ValueTextBoxLabel.Text = "Value of global integer:";
            if (Dialog(NameTextBox.Text) && IsCorrectNameOfVariable(NameTextBox.Text) &&
                FSM.GlobalData.Value(ValueTextBox.Text))
                CreateNewGlobalData(NameTextBox.Text, new Variables("int", ValueTextBox.Text));
        }


        private void LocalToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SetUpDialog("Unnamed local Int", "0");
            NameTextBoxLabel.Text = "Name of local integer:";
            ValueTextBoxLabel.Text = "Value of local integer:";
            if (Dialog(NameTextBox.Text) && IsCorrectNameOfVariable(NameTextBox.Text) &&
                FSM.GlobalData.Value(ValueTextBox.Text))
                CreateNewLocalData(NameTextBox.Text, new Variables("int", ValueTextBox.Text));
        }

        private void GlobalToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            SetUpDialog("Unnamed global Bool", "false");
            NameTextBoxLabel.Text = "Name of global boolean:";
            ValueTextBoxLabel.Text = "Value of global boolean:";
            if (Dialog(NameTextBox.Text) && IsCorrectNameOfVariable(NameTextBox.Text) &&
                FSM.GlobalData.Bool(ValueTextBox.Text))
                CreateNewGlobalData(NameTextBox.Text, new Variables("bool", ValueTextBox.Text));
        }

        private void LocalToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            SetUpDialog("Unnamed local Bool", "false");
            NameTextBoxLabel.Text = "Name of local boolean:";
            ValueTextBoxLabel.Text = "Value of local boolean:";
            if (Dialog(NameTextBox.Text) && IsCorrectNameOfVariable(NameTextBox.Text) &&
                FSM.GlobalData.Bool(ValueTextBox.Text))
                CreateNewLocalData(NameTextBox.Text, new Variables("bool", ValueTextBox.Text));
        }

        private void ColorSensorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetUpDialog("Unnamed color sensor", "A");
            NameTextBoxLabel.Text = "Name of color sensor instance:";
            ValueTextBoxLabel.Text = "Name of port:";
            if (Dialog(NameTextBox.Text) && IsCorrectNameOfVariable(NameTextBox.Text) &&
                FSM.GlobalData.Port(ValueTextBox.Text))
                CreateNewGlobalData(NameTextBox.Text, new Variables("ColorSensor", ValueTextBox.Text));
        }

        private void DistanceSensorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetUpDialog("Unnamed distance sensor", "A");
            NameTextBoxLabel.Text = "Name of distance sensor instance:";
            ValueTextBoxLabel.Text = "Name of port:";
            if (Dialog(NameTextBox.Text) && IsCorrectNameOfVariable(NameTextBox.Text) &&
                FSM.GlobalData.Port(ValueTextBox.Text))
                CreateNewGlobalData(NameTextBox.Text, new Variables("DistanceSensor", ValueTextBox.Text));
        }

        private void ForceSensorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetUpDialog("Unnamed force sensor", "A");
            NameTextBoxLabel.Text = "Name of force sensor instance:";
            ValueTextBoxLabel.Text = "Name of port:";
            if (Dialog(NameTextBox.Text) && IsCorrectNameOfVariable(NameTextBox.Text) &&
                FSM.GlobalData.Port(ValueTextBox.Text))
                CreateNewGlobalData(NameTextBox.Text, new Variables("ForceSensor", ValueTextBox.Text));
        }

        private void DriveBaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetUpDialog("Unnamed drive base", "A+B");
            NameTextBoxLabel.Text = "Name of drive base instance:";
            ValueTextBoxLabel.Text = "Name of ports:";
            if (Dialog(NameTextBox.Text) && IsCorrectNameOfVariable(NameTextBox.Text) &&
                FSM.GlobalData.Ports(ValueTextBox.Text))
                CreateNewGlobalData(NameTextBox.Text, new Variables("DriveBase", ValueTextBox.Text));
        }

        private void ListView1_Click(object sender, EventArgs e)
        {
            MouseEventArgs mouseEventArgs = (MouseEventArgs)e;
            switch (mouseEventArgs.Button)
            {
                case MouseButtons.Right:
                    DeleteVariable(listView1.SelectedItems[0].Text);
                    break;
                case MouseButtons.Left:
                    ChangeValueVariable(listView1.SelectedItems[0].Text, GetVariable(listView1.SelectedItems[0].Text));
                    RenameVariable(listView1.SelectedItems[0].Text);
                    break;
                case MouseButtons.None:
                case MouseButtons.XButton1:
                case MouseButtons.XButton2:
                default:
                    return;
            }

            WriteListView();
        }

        private Variables GetVariable(string text)
        {
            return FSM.GlobalData.Elements.TryGetValue(text, out Variables element)
                ? element
                : ActualAutomata.LocalData.Elements[text];
        }

        private void ChangeValueVariable(string text, Variables variable)
        {
            SetUpDialog(variable.GetInitValue(), "");
            NameTextBoxLabel.Text = "New value for \"" + text + "\":";
            ValueTextBox.Visible = false;
            if (Dialog(NameTextBox.Text))
            {
                string newValue = NameTextBox.Text;
                if (FSM.GlobalData.Elements.ContainsKey(text))
                    FSM.GlobalData.ChangeVariable(text, newValue);
                else
                    ActualAutomata.LocalData.ChangeVariable(text, newValue);
            }
        }

        private void RenameVariable(string text)
        {
            SetUpDialog(text, "");
            NameTextBoxLabel.Text = "New name for \"" + text + "\" variable:";
            HintBoxLabel.Text =
                "Write new name for selected variable.\nOnly letters and numbers are supported.\nMaximum length is 20 letters.";
            HintBoxLabel.Size = new Size(300, 60);
            ValueTextBox.Visible = false;
            if (Dialog(NameTextBox.Text) && IsCorrectNameOfVariable(NameTextBox.Text))
            {
                string newName = NameTextBox.Text;
                FSM.NamesOfVariables.Remove(text);
                if (!IsCorrectNameOfVariable(newName))
                    return;
                if (FSM.GlobalData.Elements.ContainsKey(text))
                    FSM.GlobalData.Rename(text, newName);
                else
                    ActualAutomata.LocalData.Rename(text, newName);
            }
        }

        private void DeleteVariable(string text)
        {
            if (FSM.GlobalData.Elements.ContainsKey(text))
                FSM.GlobalData.Delete(text);
            else
                ActualAutomata.LocalData.Delete(text);
            FSM.NamesOfVariables.Remove(text);
        }


        private void WriteListView()
        {
            listView1.Clear();
            listView1.Columns.Add("Name", 70);
            listView1.Columns.Add("Value", 70);
            foreach (var data in FSM.GlobalData.Elements)
            {
                ListViewItem item = new ListViewItem(data.Key);
                item.SubItems.Add(data.Value.GetInitValue());
                if (data.Value.GetVariableType().Contains("Motor"))
                    item.BackColor = Color.Yellow;
                else if (data.Value.GetVariableType().Contains("Sensor"))
                    item.BackColor = Color.Aquamarine;
                else if (data.Value.GetVariableType().Contains("DriveBase"))
                    item.BackColor = Color.Fuchsia;
                else if (data.Value.GetVariableType() == "string" || data.Value.GetVariableType() == "int" ||
                         data.Value.GetVariableType() == "bool")
                    item.BackColor = Color.Gray;
                else
                    item.BackColor = Color.Lime;
                listView1.Items.Add(item);
            }

            foreach (var data in ActualAutomata.LocalData.Elements)
            {
                ListViewItem item = new ListViewItem(data.Key);
                item.SubItems.Add(data.Value.GetInitValue());
                listView1.Items.Add(item);
            }
        }

        public void DeleteNameFromVariables(string dataKey)
        {
            FSM.NamesOfVariables.Remove(dataKey);
        }

        private void DisableUserInput()
        {
            foreach (Control control in Application.OpenForms[0].Controls)
            {
                control.Enabled = false;
            }

            Key = Keys.OemQuotes;
        }

        private void EnableUserInput()
        {
            foreach (Control control in Application.OpenForms[0].Controls)
            {
                control.Enabled = true;
            }

            Key = Keys.None;
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // <program> ::= <program-header> <global-variables> *<automaton> ; <automaton> occurs <automata-count> times

            // <program-header> ::= <automata-count> <LF> <top-level-automata-count> <LF> ; two integers
            string export = "7777777777" + FSM.AutomataDict.Count + "\n" + FSM.ParallelAutomata.Count + "\n";

            // <global-variables> ::= <global-variables-count> <LF> *<variable> ; <variable> occurs <global-variables-count> times
            try
            {
                export += FSM.GlobalData.Export();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Global data\n" + exception);
                WindowState = FormWindowState.Maximized;
                return;
            }

            AllAutomataList = FSM.ParallelAutomata;
            foreach (string key in FSM.AutomataDict.Keys)
            {
                if (AllAutomataList.Contains(key))
                    continue;
                AllAutomataList.Add(key);
            }


            foreach (string automaton in AllAutomataList)
            {
                // <automaton> ::= <automaton-header> <local-variables> *<state> *<event>    ; <state> occurs <number-of-states> times, <event> occurs <number-of-events> times
                try
                {
                    export += FSM.AutomataDict[automaton].ExportHeader();
                    export += FSM.AutomataDict[automaton].ExportLocalData();
                    export += FSM.AutomataDict[automaton].ExportStates();
                    export += FSM.AutomataDict[automaton].ExportEvents();
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Automaton: " + automaton + "\n" + exception);
                    WindowState = FormWindowState.Maximized;
                    return;
                }
            }

            Clipboard.SetText(export);
            MessageBox.Show("Interpreted code saved to clipboard.");
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = CheckPath("Saves", "");
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Path.GetDirectoryName(path);
            openFileDialog.Filter = "JSON Files (*.json)|*.json";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            DialogResult result = openFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                try
                {
                    string filePath = openFileDialog.FileName;
                    string fileContent = File.ReadAllText(filePath);
                    ActualAutomata.Disable();
                    Clear();
                    FSM = JsonConvert.DeserializeObject<FiniteStateMachine>(fileContent);
                    ActualAutomata = FSM.AutomataDict[FSM.ParallelAutomata[0]];
                    ReInitialize();

                    ActualAutomata.Enable();
                    WriteListView();
                    Invalidate();
                    MessageBox.Show("Load successful.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error occured: " + ex);
                    Console.WriteLine(ex);
                }
            }
        }

        private void saveAutomataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // file name
            SetUpDialog("Unnamed file", "");
            NameTextBoxLabel.Text = "Write name for saved file:";
            HintBoxLabel.Text =
                "Write new name for selected automata.\nOnly letters and numbers are supported.\nMaximum length is 20 letters.";
            HintBoxLabel.Size = new Size(300, 60);
            ValueTextBox.Visible = false;
            if (!(Dialog(NameTextBox.Text) && IsCorrect(NameTextBox.Text)))
            {
                MessageBox.Show("File not created", "Error");
                return;
            }

            string fileName = NameTextBox.Text + ".json";

            // checking Saves folder
            var path = CheckPath("Saves", fileName);

            // serializing
            //FSM.Save();
            DisableUserInput();
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented
            };

            string outputJson = JsonConvert.SerializeObject(FSM, settings);
            File.WriteAllText(path, outputJson);
            EnableUserInput();
            MessageBox.Show("Saves successful. You can find it in working directory in Saves folder.");
        }

        private void ReInitialize()
        {
            foreach (string automataName in FSM.AutomataDict.Keys)
            {
                Automata automata = FSM.AutomataDict[automataName];
                automata.MainForm = this;
                foreach (string stateName in automata.StatesList)
                {
                    State state = FSM.StateDict[automataName + ":" + stateName];
                    state.MainForm = this;
                    if (FSM.TransitionDict.ContainsKey(automataName + ":" + stateName))
                    {
                        foreach (Transition transition in FSM.TransitionDict[automataName + ":" + stateName])
                        {
                            transition.MainForm = this;
                            if (transition.Conditions != null) transition.Conditions.MainForm = this;
                            if (transition.Tasks != null) transition.Tasks.MainForm = this;
                        }
                    }

                    if (state.Text != null && state.Tasks != null)
                    {
                        FSM.StateDict.Remove(automataName + ":" + stateName);
                        state = new ActiveState(state);
                        state.Tasks.MainForm = this;
                        FSM.StateDict.Add(automataName + ":" + stateName, state);
                    }
                    else if (state.SubAutomata != null)
                    {
                        FSM.StateDict.Remove(automataName + ":" + stateName);
                        state = new AutomataState(state);
                        FSM.StateDict.Add(automataName + ":" + stateName, state);
                    }
                    else
                    {
                        FSM.StateDict.Remove(automataName + ":" + stateName);
                        state = new BlankState(state);
                        FSM.StateDict.Add(automataName + ":" + stateName, state);
                    }
                }
            }

            foreach (string parallelAutomata in FSM.ParallelAutomata)
            {
                Automata parallel = FSM.AutomataDict[parallelAutomata];
                parallel.TreeNode = treeView1.Nodes.Add(parallelAutomata, parallelAutomata);
                RecursiveThroughSubAutomata(parallelAutomata, parallel.TreeNode);
            }
        }

        private void RecursiveThroughSubAutomata(string automataName, TreeNode automataTreeNode)
        {
            foreach (string nameOfState in FSM.AutomataDict[automataName].StatesList)
            {
                State state = FSM.StateDict[automataName + ":" + nameOfState];
                if (state.SubAutomata != null)
                {
                    Automata automata = FSM.AutomataDict[state.SubAutomata];
                    automata.TreeNode = automataTreeNode.Nodes.Add(nameOfState, nameOfState);
                    RecursiveThroughSubAutomata(automata.Name, automata.TreeNode);
                }
            }

            automataTreeNode.Expand();
        }

        private static string CheckPath(string folder, string fileName)
        {
            string path = Assembly.GetExecutingAssembly().Location;
            path = path.Substring(0, path.LastIndexOf("\\", StringComparison.Ordinal));
            string subFolder = @"\" + folder + @"\";
            if (!Directory.Exists(path + subFolder))
                Directory.CreateDirectory(path + subFolder);
            path += subFolder + fileName;
            return path;
        }

        public Control.ControlCollection GetControl()
        {
            return Controls;
        }

        public TreeView GetTreeView()
        {
            return treeView1;
        }

        public void DeleteAllTransitionWithState(State state)
        {
            foreach (Automata automata in FSM.AutomataDict.Values)
                automata.DeleteAllTransitionWithState(state);
        }

        private void largeMotorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetUpDialog("Unnamed large motor", "A");
            NameTextBoxLabel.Text = "Name of large motor instance:";
            ValueTextBoxLabel.Text = "Name of port:";
            if (Dialog(NameTextBox.Text) && IsCorrectNameOfVariable(NameTextBox.Text) &&
                FSM.GlobalData.Port(ValueTextBox.Text))
                CreateNewGlobalData(NameTextBox.Text, new Variables("LargeMotor", ValueTextBox.Text));
        }

        private void mediumMotorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetUpDialog("Unnamed medium motor", "A");
            NameTextBoxLabel.Text = "Name of medium motor instance:";
            ValueTextBoxLabel.Text = "Name of port:";
            if (Dialog(NameTextBox.Text) && IsCorrectNameOfVariable(NameTextBox.Text) &&
                FSM.GlobalData.Port(ValueTextBox.Text))
                CreateNewGlobalData(NameTextBox.Text, new Variables("MediumMotor", ValueTextBox.Text));
        }

        private void openAppToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _process = new Process();
            _process.StartInfo.FileName = "C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge_proxy.exe";
            _process.StartInfo.Arguments =
                "--profile-directory=Default --app-id=lddnggebiebgbmmhckmjlkegadpkofda --app-url=https://code.pybricks.com/ --app-launch-source=4";
            _process.Start();
        }

        private void sendFSAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_process == null)
            {
                MessageBox.Show("First open the application!");
                return;
            }

            IntPtr hWnd = new IntPtr();
            bool found = false;
            Process[] process = Process.GetProcessesByName("msedge");
            foreach (Process proc in process)
            {
                Console.WriteLine(proc.MainWindowTitle);
                if (proc.MainWindowTitle.StartsWith("Pybricks Code"))
                {
                    hWnd = proc.MainWindowHandle;
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                MessageBox.Show("Please start the pybricks application!");
                return;
            }

            SetForegroundWindow(hWnd);

            string clipboard = Clipboard.GetText();
            int i = 0;
            for (; i < clipboard.Length / 500; i++)
            {
                Thread.Sleep(500);
                Clipboard.SetText(clipboard.Substring(i * 500, 500));
                SendKeys.Send("^v");
            }

            Thread.Sleep(500);
            Clipboard.SetText(clipboard.Substring(i * 500));
            SendKeys.Send("^v");
        }

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
    }
}
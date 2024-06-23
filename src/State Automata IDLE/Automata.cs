using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using State_Automata_IDLE.Memory;
using State_Automata_IDLE.States;

namespace State_Automata_IDLE
{
    public class Automata
    {
        public string Name { get; set; }

        public List<string> StatesList { get; set; } = new List<string>();
        public Data LocalData { get; set; } = new Data();
        [field: NonSerialized] public TreeNode TreeNode;
        [field: NonSerialized] public MainForm MainForm;

        [JsonConstructor]
        public Automata(string name, List<string> statesList, Data localData)
        {
            Name = name;
            StatesList = statesList;
            LocalData = localData;
        }

        public Automata(MainForm mainForm, TreeNode treeNode, string name = "Unnamed")
        {
            Name = name;
            MainForm = mainForm;
            TreeNode = treeNode.Nodes.Add(Name, name);
            CreateStartUpStates();
        }

        public Automata(MainForm mainForm, TreeView treeView, string name = "Unnamed")
        {
            Name = name;
            MainForm = mainForm;
            TreeNode treeNode = treeView.Nodes.Add(Name, name);
            treeNode.Text = name;
            TreeNode = treeNode;
            CreateStartUpStates();
        }

        private void CreateStartUpStates()
        {
            State init = new BlankState(MainForm, Name, 300, 150, "Init");
            StatesList.Add(init.Name);
            string name = "Finite" + MainForm.FSM.Counter++;
            State finite = new BlankState(MainForm, Name, 500, 350, name);
            StatesList.Add(finite.Name);

            MainForm.FSM.NamesOfStates.Add(name);
            MainForm.FSM.StateDict.Add(Name + ":" + init.Name, init);
            MainForm.FSM.StateDict.Add(Name + ":" + finite.Name, finite);

            Transition initTransition = new Transition(MainForm, init, 0);
            MainForm.FSM.TransitionDict.Add(Name + ":" + init.Name, new List<Transition> { initTransition });

            Transition finiteTransition = new Transition(MainForm, finite, 1);
            Transition finite2Transition = new Transition(MainForm, finite, 2);
            MainForm.FSM.TransitionDict.Add(Name + ":" + finite.Name,
                new List<Transition> { finiteTransition, finite2Transition });
        }

        public bool ContainsTransition(Transition transition)
        {
            foreach (string state in StatesList)
                if (MainForm.FSM.TransitionDict[Name + ":" + state].Contains(transition))
                    return true;
            return false;
        }

        public void RemoveTransition(Transition transition)
        {
            foreach (string state in StatesList)
                if (MainForm.FSM.TransitionDict[Name + ":" + state].Contains(transition))
                {
                    MainForm.FSM.TransitionDict[Name + ":" + state].Remove(transition);
                    return;
                }
        }

        public void RelocateTransition(State inputState)
        {
            if (inputState == null)
                return;
            foreach (string state in StatesList)
            {
                if (!MainForm.FSM.TransitionDict.ContainsKey(Name + ":" + state))
                    continue;
                foreach (Transition transition in MainForm.FSM.TransitionDict[Name + ":" + state])
                    if (transition.From == inputState.Name || transition.To == inputState.Name)
                    {
                        transition.Relocate(inputState.PositionX, inputState.PositionY);
                    }
            }
        }

        public void Disable()
        {
            foreach (string state in StatesList)
            {
                if (!MainForm.FSM.StateDict.ContainsKey(Name + ":" + state))
                    continue;
                MainForm.FSM.StateDict[Name + ":" + state].Disable();
            }
        }

        public void Enable()
        {
            foreach (string state in StatesList)
            {
                if (!MainForm.FSM.StateDict.ContainsKey(Name + ":" + state))
                    continue;
                MainForm.FSM.StateDict[Name + ":" + state].Enable();
            }
        }

        public State FindState(string name)
        {
            foreach (string nameOfState in StatesList)
            {
                State state = MainForm.FSM.StateDict[Name + ":" + nameOfState];
                if (state.Name == name)
                    return state;
                if (state.SubAutomata != null)
                {
                    State output = MainForm.FSM.AutomataDict[state.SubAutomata].FindState(name);
                    if (output != null)
                        return output;
                }
            }

            return null;
        }

        public Automata FindAutomata(string name)
        {
            if (Name == name)
                return this;

            foreach (string nameOfState in StatesList)
            {
                if (!MainForm.FSM.StateDict.ContainsKey(Name + ":" + nameOfState))
                    continue;
                State state = MainForm.FSM.StateDict[Name + ":" + nameOfState];
                if (state.SubAutomata != null)
                {
                    Automata output = MainForm.FSM.AutomataDict[state.SubAutomata].FindAutomata(name);
                    if (output != null)
                        return output;
                }
            }

            return null;
        }

        public State FindClickedState(int x, int y)
        {
            foreach (string state in StatesList)
            {
                State found = MainForm.FSM.StateDict[Name + ":" + state].SelectClicked(x, y);
                if (found != null)
                    return found;
            }

            return null;
        }

        public Transition FindClickedTransition(int x, int y)
        {
            foreach (string state in StatesList)
            {
                if (!MainForm.FSM.TransitionDict.ContainsKey(Name + ":" + state))
                    continue;
                foreach (Transition transition in MainForm.FSM.TransitionDict[Name + ":" + state])
                {
                    Transition found = transition.FindClickedTransition(x, y);
                    if (found != null && transition.Points.Count > 2)
                        return found;
                }
            }

            return null;
        }

        public void DeleteState(State state, bool recursive = false)
        {
            if (recursive)
                DeleteAllTransition();
            if (StatesList[0] == state.Name && !recursive)
                return;
            state.Delete();
            StatesList.Remove(state.Name);
            MainForm.FSM.NamesOfStates.Remove(state.Name);
            MainForm.FSM.StateDict.Remove(state.Name);
            if (state.SubAutomata != null)
                MainForm.FSM.AutomataDict[state.SubAutomata].RecursiveDelete();
            while (CountFiniteState() == 0 && recursive == false)
            {
                MainForm.Invalidate();
                MainForm.finiteStateToolStripMenuItem_Click(null, null);
                MainForm.Key = Keys.None;
            }
        }

        public void DeleteAllTransition()
        {
            foreach (string stateName in StatesList)
            {
                MainForm.FSM.TransitionDict.Remove(Name + ":" + stateName);
            }
        }

        private int CountFiniteState()
        {
            int count = 0;
            foreach (string nameOfState in StatesList)
                if (MainForm.FSM.StateDict[Name + ":" + nameOfState].IsFinite())
                    count++;
            return count;
        }

        public void RecursiveDelete()
        {
            while (StatesList.Count > 0)
                DeleteState(MainForm.FSM.StateDict[Name + ":" + StatesList[0]], true);
            foreach (var data in LocalData.Elements)
                MainForm.DeleteNameFromVariables(data.Key);
            LocalData.Clear();
            LocalData.Clear();
            TreeNode.Remove();
            MainForm.FSM.AutomataDict.Remove(Name);
            MainForm.FSM.ParallelAutomata.Remove(Name);
            MainForm.FSM.NamesOfStates.Remove(Name);
        }

        public bool ContainsStateWithName(string name)
        {
            return StatesList.Contains(name);
        }

        public Automata FindAutomataWithState(string name)
        {
            if (ContainsStateWithName(name))
                return this;
            foreach (string nameOfState in StatesList)
            {
                if (!MainForm.FSM.StateDict.ContainsKey(Name + ":" + nameOfState))
                    continue;
                State state = MainForm.FSM.StateDict[Name + ":" + nameOfState];
                if (state.SubAutomata != null)
                {
                    Automata outAutomata = MainForm.FSM.AutomataDict[state.SubAutomata].FindAutomataWithState(name);
                    if (outAutomata != null)
                        return outAutomata;
                }
            }

            return null;
        }

        public void DeleteTransition(Transition transition)
        {
            foreach (string state in StatesList)
            {
                if (MainForm.FSM.TransitionDict[Name + ":" + state].Contains(transition))
                {
                    MainForm.FSM.TransitionDict[Name + ":" + state].Remove(transition);
                    transition.DeleteTransition();
                }
            }
        }

        public void DeleteAllTransitionWithState(State inputState)
        {
            foreach (string nameOfState in StatesList)
            {
                if (!MainForm.FSM.StateDict.ContainsKey(Name + ":" + nameOfState))
                    continue;
                State state = MainForm.FSM.StateDict[Name + ":" + nameOfState];
                if (state.SubAutomata != null)
                {
                    MainForm.FSM.AutomataDict[state.SubAutomata].DeleteAllTransitionWithState(inputState);
                }

                state.DeleteAllTransitionWithState(inputState);
            }
        }

        public void Rename(string newName)
        {
            MainForm.FSM.AutomataDict.Add(newName, MainForm.FSM.AutomataDict[Name]);
            MainForm.FSM.AutomataDict.Remove(Name);
            foreach (string nameOfState in StatesList)
            {
                MainForm.FSM.StateDict[Name + ":" + nameOfState].ActualAutomata = newName;
                MainForm.FSM.StateDict.Add(newName + ":" + nameOfState,
                    MainForm.FSM.StateDict[Name + ":" + nameOfState]);
                MainForm.FSM.StateDict.Remove(Name + ":" + nameOfState);
                MainForm.FSM.TransitionDict.Add(newName + ":" + nameOfState, new List<Transition>());
                if (MainForm.FSM.TransitionDict.ContainsKey(Name + ":" + nameOfState))
                {
                    foreach (Transition transition in MainForm.FSM.TransitionDict[Name + ":" + nameOfState])
                    {
                        MainForm.FSM.TransitionDict[newName + ":" + nameOfState].Add(transition);
                        transition.AutomataName = newName;
                    }
                }

                MainForm.FSM.TransitionDict.Remove(Name + ":" + nameOfState);
            }

            Name = newName;
            TreeNode.Text = newName;
        }

        private int CountFinalStates()
        {
            int count = 0;
            foreach (string name in StatesList)
                if (MainForm.FSM.StateDict[Name + ":" + name].IsFinite())
                    count++;
            return count;
        }

        private int CountFinalTransitions()
        {
            int count = 0;
            foreach (string name in StatesList)
            {
                foreach (Transition transition in MainForm.FSM.TransitionDict[Name + ":" + name])
                {
                    // do not count empty or just write if(false) inside them 
                    if (transition.IsFinite() && transition.Conditions != null &&
                        transition.Conditions.Text.Trim() != "")
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        public string ExportHeader()
        {
            // <automaton-header> ::= <automata-name> <LF> <number-of-states> <LF> <number-of-final-states> *(<LF> <final-state-number>) <LF> <number-of-events> <LF>   ; <final-state_number> occurs <number-of-final-states>-times (events - global conditions to final states)
            string export = Name + "\n" + StatesList.Count + "\n" + CountFinalStates();
            foreach (string name in StatesList)
            {
                if (MainForm.FSM.StateDict[Name + ":" + name].IsFinite())
                {
                    export += "\n" + MainForm.FSM.AutomataDict[Name].StatesList.IndexOf(name);
                }
            }

            export += "\n" + CountFinalTransitions() + "\n";
            return export;
        }

        public string ExportLocalData()
        {
            return LocalData.Export();
        }

        public string ExportStates()
        {
            /*
              <state> ::= <state-name> <LF> <number-of-script-lines> <LF> <script-launch-period-in-ms> <LF> <subautomaton-id> <LF> <number-of-transitions> <LF> *<transition> *<script-line>
              ; only active state type has nonzero script lines
              ; only subautomaton type has subautomaton-id != -1
              ;   therefore state type can be inferred
              ; <transition> occurs <number-of-transitions> times
              ; <script-line> occurs <number-of-script-lines> times
              ; <script-launch-period-in-ms> == -1 means to run the script once only, otherwise repeat forever
             */
            string export = "";
            int count = 0;
            foreach (string name in StatesList)
            {
                export += name + "\n";
                State state = MainForm.FSM.StateDict[Name + ":" + name];
                string tasks = state.Text;

                // if init or finite, then init transition or exit transition are like tasks in active states, but they are started only once
                if (state.IsInit())
                {
                    List<Transition> transitions = MainForm.FSM.TransitionDict[Name + ":" + name];
                    foreach (Transition transition in transitions)
                    {
                        if (transition.IsInit())
                        {
                            tasks = transition.Tasks.Text;
                            break;
                        }
                    }
                }

                if (state.IsFinite())
                {
                    List<Transition> transitions = MainForm.FSM.TransitionDict[Name + ":" + name];
                    foreach (Transition transition in transitions)
                    {
                        if (transition.IsFinite() && transition.Conditions == null)
                        {
                            tasks = transition.Tasks.Text;
                            break;
                        }
                    }
                }
                
                string[] taskLines = {};
                if (tasks != null)
                {
                    taskLines = tasks.Trim().Split('\n');
                    foreach (string procedure in taskLines)
                        if (procedure.Trim() != "")
                            count++;
                    int delay;
                    if (state.IsInit() || state.IsFinite())
                        delay = -1;
                    else
                        try
                        {
                            delay = Convert.ToInt32(taskLines);
                        }
                        catch (Exception)
                        {
                            throw new Exception("Wrong delay for task in state: " + name);
                        }


                    export += count + "\n" + delay + "\n";
                }
                else
                {
                    export += "0\n-1\n";
                }

                if (state.SubAutomata != null)
                    export += MainForm.AllAutomataList.IndexOf(state.SubAutomata);
                else
                    export += "-1";
                int countOfTransitions = 0;
                if (MainForm.FSM.TransitionDict.ContainsKey(Name + ":" + name))
                    countOfTransitions = MainForm.FSM.TransitionDict[Name + ":" + name].Count;

                // init transition in Init state and exit transition and super/event transition in Finite state dont count
                if (state.IsInit())
                    countOfTransitions--;
                if (state.IsFinite())
                    countOfTransitions -= 2;
                export += "\n" + countOfTransitions + "\n";

                if (countOfTransitions > 0)
                {
                    try
                    {
                        export += ExportTransitions(state);
                    }
                    catch (Exception exception)
                    {
                        throw new Exception("State: " + state.Name + "\n" + exception);
                    }
                }
                try
                {
                    foreach (string line in taskLines)
                        if (line.Trim() != "")
                            export += ExportTask(line) + "\n";
                }
                catch (Exception exception)
                {
                    throw new Exception("Error with parsing state task of state: " + state.Name + '\n' + exception);
                }
            }

            return export;
        }

        private string ExportTransitions(State state)
        {
            /*
                <transition> ::= <destination-state-ID> <LF> <specification-of-condition> <LF> <number-of-transition-script-lines> <LF> *<script-line>  ; <script-line> occurs <number-of-transition-script-lines> times
                <destination-state-ID> ::= 1*<DIGIT>   ; index in the list of states starting with 0
                <specification-of-condition> ::= <value-expression>   ; types 0, 1 will lead to runtime-error as well as 3, 4, 5 that do not return boolean
             */
            string export = "";
            List<Transition> transitions = MainForm.FSM.TransitionDict[Name + ":" + state.Name];
            foreach (Transition transition in transitions)
            {
                if (transition.IsInit() || transition.IsFinite())
                    continue;
                export += MainForm.FSM.AutomataDict[Name].StatesList.IndexOf(transition.To) + "\n";
                try
                {
                    export += ExportValueExpression(transition.Conditions.Text.Trim()) + "\n";
                }
                catch (Exception exception)
                {
                    throw new Exception(
                        "Transition From:" + transition.From + " To:" + transition.To + "\n" + exception);
                }

                export += transition.Tasks.Text.Trim().Split('\n').Length + "\n";
                try
                {
                    foreach (string row in transition.Tasks.Text.Trim().Split('\n'))
                        if (row.Trim() != "")
                            export += ExportTask(row) + "\n";
                }
                catch (Exception exception)
                {
                    throw new Exception("Error while parsing task in state " + state.Name + '\n' + exception);
                }
            }

            return export;
        }

        public string ExportValueExpression(string condition)
        {
            if (condition == "")
            {
                condition = "true";
            }

            string export = "";
            bool number = true;
            try
            {
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                Convert.ToDouble(condition);
            }
            catch (Exception)
            {
                number = false;
            }

            if (condition[0] == '"')
                export += "0 " + condition;
            if (number)
                export += "1 " + Convert.ToDouble(condition);
            else if (condition.ToLower() == "true" || condition.ToLower() == "false")
                export += "2 " + condition;
            else if (MainForm.FSM.GlobalData.Elements.Keys.Contains(condition))
                export += "3 " + MainForm.FSM.GlobalData.Elements.Keys.ToList().IndexOf(condition);
            else if (LocalData.Elements.Keys.Contains(condition))
                export += "4 " + LocalData.Elements.Keys.ToList().IndexOf(condition);
            else
            {
                try
                {
                    export += "5 " + ExportCondition(condition);
                }
                catch (Exception exception)
                {
                    throw new Exception("Invalid argument in condition or task of state:\n" + exception);
                }
            }


            return export;
        }

        public string ExportCondition(string condition)
        {
            // <function-call> ::= <function-id> *(" " <value-expression>)
            string export = "";
            if (condition.IndexOf('(') == -1)
            {
                throw new Exception("Missing '(' in condition");
            }

            ValidateParentheses(condition);

            string function = condition.Substring(0, condition.IndexOf('('));
            int index;
            try
            {
                index = MainForm.FunctionIdMap[function].functionId;
            }
            catch (Exception)
            {
                throw new Exception("Invalid function name: " + function);
            }

            export += index;

            string args =
                condition.Substring(condition.IndexOf('(') + 1, condition.Length - condition.IndexOf('(') - 2);
            string[] argArray = SplitArguments(args);

            if (MainForm.FunctionIdMap[function].numArgs != argArray.Length)
                throw new Exception("Invalid number of arguments in function: " + function);

            try
            {
                foreach (string arg in argArray)
                    export += " " + ExportValueExpression(arg);
                if (argArray.Contains(""))
                    throw new Exception("Empty argument in function: " + function);
            }
            catch (Exception exception)
            {
                throw new Exception("Error while parsing function " + function + "\n" + exception);
            }

            return export;
        }

        public string ExportTask(string task)
        {
            // <script-line> ::= <statement> <LF>

            // <statement> ::= <procedure-id> *(" " <value-expression>)
            string export = "";
            if (task.IndexOf('(') == -1)
            {
                throw new Exception("Missing '(' in task");
            }

            ValidateParentheses(task);

            string procedure = task.Substring(0, task.IndexOf('('));
            int index;
            try
            {
                index = MainForm.ProcedureIdMap[procedure].procedureId;
            }
            catch (Exception)
            {
                throw new Exception("Invalid procedure name: " + procedure);
            }

            export += index;

            string args = task;
            if (args.LastIndexOf('\r') == args.Length - 1)
                args = args.Substring(0, args.Length - 1);
            args =
                args.Substring(args.IndexOf('(') + 1, args.Length - args.IndexOf('(') - 2);
            string[] argArray = SplitArguments(args);

            if (MainForm.ProcedureIdMap[procedure].numArgs != argArray.Length)
                throw new Exception("Invalid number of arguments in procedure: " + procedure);
            try
            {
                foreach (string arg in argArray)
                    export += " " + ExportValueExpression(arg);
                if (argArray.Contains(""))
                    throw new Exception("Empty argument in procedure: " + procedure);
            }
            catch (Exception exception)
            {
                throw new Exception("Error while parsing procedure " + procedure + "\n" + exception);
            }

            return export;
        }

        private static string[] SplitArguments(string args)
        {
            List<string> result = new List<string>();
            int parenDepth = 0;
            bool inString = false;
            int start = 0;

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == '"' && (i == 0 || args[i - 1] != '\\'))
                {
                    inString = !inString;
                }

                if (!inString)
                {
                    if (args[i] == '(')
                    {
                        parenDepth++;
                    }
                    else if (args[i] == ')')
                    {
                        parenDepth--;
                    }
                    else if (args[i] == ',' && parenDepth == 0)
                    {
                        result.Add(args.Substring(start, i - start).Trim());
                        start = i + 1;
                    }
                }
            }

            result.Add(args.Substring(start).Trim());

            return result.ToArray();
        }

        public static void ValidateParentheses(string expression)
        {
            int parenDepth = 0;
            bool inString = false;

            for (int i = 0; i < expression.Length; i++)
            {
                if (expression[i] == '"' && (i == 0 || expression[i - 1] != '\\'))
                {
                    inString = !inString;
                }

                if (!inString)
                {
                    if (expression[i] == '(')
                    {
                        parenDepth++;
                    }
                    else if (expression[i] == ')')
                    {
                        parenDepth--;
                    }

                    if (parenDepth < 0)
                    {
                        throw new ArgumentException(
                            $"Mismatched parentheses at position {i} in expression: {expression}");
                    }
                }
            }

            if (parenDepth != 0)
            {
                throw new ArgumentException($"Mismatched parentheses in expression: {expression}");
            }
        }

        public string ExportEvents()
        {
            /*
                <transition> ::= <destination-state-ID> <LF> <specification-of-condition> <LF> <number-of-transition-script-lines> <LF> *<script-line>  ; <script-line> occurs <number-of-transition-script-lines> times
                <destination-state-ID> ::= 1*<DIGIT>   ; index in the list of states starting with 0
                <specification-of-condition> ::= <value-expression>   ; types 0, 1 will lead to runtime-error as well as 3, 4, 5 that do not return boolean
             */
            string export = "";
            foreach (string name in StatesList)
            {
                List<Transition> transitions = MainForm.FSM.TransitionDict[Name + ":" + name];
                foreach (Transition transition in transitions)
                {
                    if (!transition.IsFinite() || transition.Conditions == null || transition.Conditions.Text.Trim() == "")
                        continue;
                    export += MainForm.FSM.AutomataDict[Name].StatesList.IndexOf(transition.To) + "\n";
                    try
                    {
                        export += ExportValueExpression(transition.Conditions.Text.Trim()) + "\n";
                    }
                    catch (Exception exception)
                    {
                        throw new Exception(
                            "Transition event:" + transition.From + "\n" + exception);
                    }

                    export += transition.Tasks.Text.Trim().Split('\n').Length + "\n";
                    try
                    {
                        foreach (string row in transition.Tasks.Text.Trim().Split('\n'))
                            if (row.Trim() != "")
                                export += ExportTask(row) + "\n";
                    }
                    catch (Exception exception)
                    {
                        throw new Exception("Error with parsing event in finite state " + name + '\n' + exception);
                    }
                }
            }

            return export;
        }
    }
}
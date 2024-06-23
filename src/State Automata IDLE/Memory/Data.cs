using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace State_Automata_IDLE.Memory
{
    public class Data
    {
        public Dictionary<string, Variables> Elements { get; set; } = new Dictionary<string, Variables>();

        [field: NonSerialized] public readonly string[] PortModules =
            { "LargeMotor", "MediumMotor", "ColorSensor", "DistanceSensor", "ForceSensor", "DriveBase" };

        public void Add(string name, Variables variable)
        {
            Elements.Add(name, variable);
        }

        public void Rename(string text, string newName)
        {
            if (text == newName)
                return;
            Elements.Add(newName, Elements[text]);
            Delete(text);
        }

        public void Delete(string text)
        {
            Elements.Remove(text);
        }

        public void Clear()
        {
            Elements.Clear();
        }

        public void ChangeVariable(string text, string newValue)
        {
            Variables variable = Elements[text];
            if (variable.GetVariableType() == "string")
            {
                if (newValue.Length <= 30)
                    variable.SetNewValue(newValue);
            }
            else if (variable.GetVariableType() == "int" && Value(newValue))
                variable.SetNewValue(newValue);
            else if (variable.GetVariableType() == "bool" && Bool(newValue))
                variable.SetNewValue(newValue);
            else if (PortModules.Contains(variable.GetVariableType()) && (Port(newValue) || Ports(newValue)))
                variable.SetNewValue(newValue);
        }

        public bool Value(string text)
        {
            try
            {
                double value = Convert.ToDouble(text);
                return value.Equals(value);
            }
            catch (FormatException)
            {
            }
            catch (OverflowException)
            {
            }

            return false;
        }

        public bool Bool(string text)
        {
            return text.ToLower() == "false" || text.ToLower() == "true" || text == "0" || text == "1";
        }

        public bool Port(string text)
        {
            return text.ToUpper()[0] >= 'A' && text.ToUpper()[0] <= 'F' && text.Length == 1;
        }

        public bool Ports(string text)
        {
            return text.Length == 3 && text[1] == '+' && Port(text[0].ToString()) && Port(text[2].ToString());
        }

        public string[] GetPortModules()
        {
            return PortModules;
        }

        public string Export()
        {
            // <global-variables> ::= <global-variables-count> <LF> *<variable> ; <variable> occurs <global-variables-count> times

            // <local-variables> ::= <local-variables-count> <LF> *<variable> ; <variable> occurs <local-variables-count> times

            string text = Elements.Count + "\n";

            // <variable> ::= <variable-name> <LF> <variable-type> <LF> <variable-init-value> <LF> ; variables are indexed from 0 in the order the appear in the list
            // <variable-init-value> ::= <constant-string-value> | <constant-numeric-value> | <constant-boolean-value> ; the type of init value should match the specified variable type; port-type variable initial values are ignored (can be empty string, or 0)

            foreach (KeyValuePair<string, Variables> data in Elements)
            {
                if (!ValidName(data.Key))
                    throw new Exception("Not valid name: " + data.Key);
                try
                {
                    text += data.Key + "\n" + data.Value.GetExportType() + "\n" + data.Value.GetExportValue() + "\n";
                }
                catch (Exception exception)
                {
                    throw new Exception(data.Key + "\n" + exception);
                }
            }
            return text;
        }

        private static bool ValidName(string name)
        {
            // <variable-name> ::= <ALPHA> *19(<ALPHA> | <DIGIT> | "_" | "-")
            for (int i = 0; i < name.Length; i++)
            {
                if (i == 0)
                    if (('a' <= name[i] && name[i] <= 'z') || ('A' <= name[i] && name[i] <= 'Z'))
                        continue;
                    else
                        return false;
                if (('a' <= name[i] && name[i] <= 'z') || ('A' <= name[i] && name[i] <= 'Z') ||
                    ('0' <= name[i] && name[i] <= '9') || '-' == name[i] || name[i] == '_')
                    continue;
                return false;
            }

            return true;
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mod.ModMenu
{
    public class ModMenuItemInt
    {
        public string Title { get; set; }

        public string[] Values { get; set; }

        public int SelectedValue { get; set; }

        public string RMSName { get; set; }

        public string Description { get; set; }

        public bool isDisabled { get; set; }

        public string DisabledReason { get; set; }

        public ModMenuItemInt(string title, string[] values, string rmsName)
        {
            Title = title;
            Values = values;
            RMSName = rmsName;
            SelectedValue = 0;
            Description = null;
            isDisabled = false;
        }

        public ModMenuItemInt(string title, string[] values, string rmsName, string disabledreason) : this(title, values, rmsName)
        {
            DisabledReason = disabledreason;
        }

        public ModMenuItemInt(string title, string[] values, int selectedValue, string rmsName) : this(title, values, rmsName)
        {
            SelectedValue = selectedValue;
        }

        public ModMenuItemInt(string title, string[] values, int selectedValue, string rmsName, string disabledreason) : this(title, values, selectedValue, rmsName)
        {
            DisabledReason = disabledreason;
        }

        public ModMenuItemInt(string title, string[] values, string rmsName, bool isdisabled) : this(title, values, rmsName)
        {
            isDisabled = isdisabled;
        }

        public ModMenuItemInt(string title, string[] values, string rmsName, bool isdisabled, string disabledreason) : this(title, values, rmsName, isdisabled)
        {
            DisabledReason = disabledreason;
        }

        public ModMenuItemInt(string title, string[] values, int selectedValue, string rmsName, bool isdisabled) : this(title, values, rmsName, isdisabled)
        {
            SelectedValue = selectedValue;
        }

        public ModMenuItemInt(string title, string[] values, int selectedValue, string rmsName, bool isdisabled, string disabledreason) : this(title, values, selectedValue, rmsName, isdisabled)
        {
            DisabledReason = disabledreason;
        }



        public ModMenuItemInt(string title, string description, string rmsName)
        {
            Title = title;
            Values = null;
            SelectedValue = 0;
            RMSName = rmsName;
            Description = description;
            isDisabled = false;
        }

        public ModMenuItemInt(string title, string description, string rmsName, string disabledreason) : this(title, description, rmsName)
        {
            DisabledReason = disabledreason;
        }

        public ModMenuItemInt(string title, string description, int selectedValue, string rmsName) : this(title, description, rmsName)
        {
            SelectedValue = selectedValue;
        }

        public ModMenuItemInt(string title, string description, int selectedValue, string rmsName, string disabledreason) : this(title, description, selectedValue, rmsName)
        {
            DisabledReason = disabledreason;
        }

        public ModMenuItemInt(string title, string description, string rmsName, bool isdisabled) : this(title, description, rmsName)
        {
            isDisabled = isdisabled;
        }

        public ModMenuItemInt(string title, string description, string rmsName, bool isdisabled, string disabledreason) : this(title, description, rmsName, isdisabled)
        {
            DisabledReason = disabledreason;
        }

        public ModMenuItemInt(string title, string description, int selectedValue, string rmsName, bool isdisabled) : this(title, description, selectedValue, rmsName)
        {
            isDisabled = isdisabled;
        }

        public ModMenuItemInt(string title, string description, int selectedValue, string rmsName, bool isdisabled, string disabledreason) : this(title, description, selectedValue, rmsName, isdisabled)
        {
            DisabledReason = disabledreason;
        }

        public override bool Equals(object obj)
        {
            if (obj is ModMenuItemInt modMenuItem)
            {
                return modMenuItem.Title == Title && modMenuItem.Values == Values && modMenuItem.SelectedValue == SelectedValue && modMenuItem.RMSName == RMSName;
            }
            return false;
        }

        public string getSelectedValue()
        {
            return Values[SelectedValue];
        }

        public void SwitchSelection()
        {
            if (Values != null)
            {
                SelectedValue++;
                if (SelectedValue > Values.Length - 1) SelectedValue = 0;
            }
            ModMenuPanel.onModMenuIntsValueChanged();
        }

        public void setValue(int value)
        {
            SelectedValue = value;
            ModMenuPanel.onModMenuIntsValueChanged();
        }

        public override int GetHashCode()
        {
            int hashCode = -1820188900;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Title);
            hashCode = hashCode * -1521134295 + EqualityComparer<string[]>.Default.GetHashCode(Values);
            hashCode = hashCode * -1521134295 + SelectedValue.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(RMSName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Description);
            return hashCode;
        }
    }
}
﻿using System;
using System.Windows.Forms;

namespace GH.Database
{

    public class UpdatablePropertyAttribute : Attribute
    {
        public bool Key { get; set; } = false;
        public virtual string Group { get; set; }
        public string SubGroup { get; set; } = null;
        public string Caption { get; set; }
        public string ToolTip { get; set; }
        public EditorType EditorType { get; set; } = EditorType.Text;
        public bool ReadOnly { get; set; } = false;
        public object Default { get; set; } = null;
        public int Order { get; set; } = 0;
        public bool Required { get; set; } = false;
        public int MaxLength { get; set; } = 0;
        public CharacterCasing CharacterCasing { get; set; }
    }

    public enum EditorType
    {
        Text,
        Combo,
        Check,
        Button,
        PathSeacher
    }

}

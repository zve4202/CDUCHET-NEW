using System.Collections.Generic;
using System.Windows.Forms;

namespace GH.Configs
{
    public class IniFile : Dictionary<string, CfgCore>
    {
        public IniFile()
        {
        }

        #region методы

        public void SaveAll()
        {
            foreach (KeyValuePair<string, CfgCore> instance in this)
                instance.Value.Save();
        }

        public static Form GetMaimForm()
        {
            for (int i = 0; i < Application.OpenForms.Count; i++)
            {
                if (Application.OpenForms[i] is IMainForm)
                    return Application.OpenForms[i];

            }

            return Application.OpenForms[0];
        }

        public void AddInstance(CfgCore instanse)
        {
            if (instanse is CfgApp cfgApp)
            {
                this[nameof(CfgApp)] = cfgApp;
                cfgApp.Form = GetMaimForm();
            }
            else
            if (!ContainsValue(instanse))
            {
                this[instanse.GetName()] = instanse;
            }
        }
        #endregion
    }
}

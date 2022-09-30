using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using System;
using System.Windows.Forms;

namespace GH.Windows
{
    public static class DevExpressHelper
    {
        public static void SetupLookups(object item)
        {
            if (item == null)
                return;

            if (item is GridControl gridControl)
            {
                foreach (object repo in gridControl.RepositoryItems)
                {
                    SetupLookups(repo);
                }

            }
            else
            if (item is RepositoryItemLookUpEdit repositoryItemLook)
            {
                repositoryItemLook.ValueMember = "Key";
                repositoryItemLook.DisplayMember = "Value";
                repositoryItemLook.ShowHeader = false;
                repositoryItemLook.ShowFooter = false;
            }
            else
            if (item is LookUpEdit lookUpEdit)
            {
                lookUpEdit.Properties.ValueMember = "Key";
                lookUpEdit.Properties.DisplayMember = "Value";
                lookUpEdit.Properties.ShowHeader = false;
                lookUpEdit.Properties.ShowFooter = false;
            }
            else
            if (item is Control control)
            {
                foreach (object ctrl in control.Controls)
                {
                    SetupLookups(ctrl);
                }
            }
        }

        public static string CalculateProcessing(int processed, int total)
        {
            return string.Format("{0} из {1}", processed, total);
        }

        public static string CalculateRemaining(DateTime processStarted, int totalElements, int processedElements)
        {
            int secondsRemaining = 0;
            int totalSecond = (int)(DateTime.Now - processStarted).TotalSeconds;

            if (totalSecond > 0)
            {
                int itemsPerSecond = processedElements / totalSecond;

                if (itemsPerSecond > 0)
                    secondsRemaining = (totalElements - processedElements) / itemsPerSecond;
            }

            return new TimeSpan(0, 0, secondsRemaining).ToString(@"hh\:mm\:ss");
        }

        public static string CalculateDuration(DateTime processStarted)
        {
            return TimeSpan.FromTicks(DateTime.Now.Subtract(processStarted).Ticks).ToString(@"hh\:mm\:ss");
        }




    }
}

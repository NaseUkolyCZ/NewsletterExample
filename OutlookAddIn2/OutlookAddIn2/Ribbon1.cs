using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using System.Threading;

namespace OutlookAddIn2
{
    public partial class Ribbon1
    {
        private void Ribbon1_Load(object sender, RibbonUIEventArgs e)
        {

        }

        private void button1_Click(object sender, RibbonControlEventArgs e)
        {
            //ThisAddIn.emails.Clear();
            ThreadStart threadDelegate = new ThreadStart(ThisAddIn.instance.EnumerateStores);
            Thread newThread = new Thread(threadDelegate);
            newThread.Start(); 
        }
    }
}

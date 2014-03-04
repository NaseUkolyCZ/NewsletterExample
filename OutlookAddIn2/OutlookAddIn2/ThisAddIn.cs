using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Outlook = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace OutlookAddIn2
{
    public partial class ThisAddIn
    {
        internal static ThisAddIn instance = null;

        //http://msdn.microsoft.com/en-us/library/office/ff184648.aspx
        public void EnumerateStores()
        {
            try { 
            Outlook.Stores stores = Application.Session.Stores;
            foreach (Outlook.Store store in stores)
            {
                Outlook.Folder root =
                store.GetRootFolder() as Outlook.Folder;
                EnumerateFolders(root);
            }
            MessageBox.Show("mails exported!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:" + ex.Message + "\n[" + ex.StackTrace + "]");
            }
        }

        //http://msdn.microsoft.com/en-us/library/ff184607.aspx
        private void EnumerateFoldersInDefaultStore()
        {
            Outlook.Folder root =
                Application.Session.
                DefaultStore.GetRootFolder() as Outlook.Folder;
            EnumerateFolders(root);
        }

        // smtpAddress, name
        //public static Dictionary<string, string> emails = new Dictionary<string, string>();

        private void SaveRecipient(string smtpAddress, string RecipientName)
        {
            using (SqlConnection cnn = new SqlConnection("Server=(localdb)\\Projects;Database=Newsletter;Trusted_Connection=True;Connection Timeout=30;"))
            {
                cnn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_InsertOrUpdate @0, @1", cnn))
                {
                    cmd.Parameters.AddWithValue("@0", ( String.Empty + smtpAddress ).Trim('\'', '"') );
                    cmd.Parameters.AddWithValue("@1", ( String.Empty + RecipientName ).Trim('\'', '"') );
                    cmd.ExecuteNonQuery();
                }
            }
        }

        //http://msdn.microsoft.com/en-us/library/office/ff184647.aspx
        private void GetSMTPAddressForRecipients(Outlook.MailItem mail)
        {
            const string PR_SMTP_ADDRESS =
                "http://schemas.microsoft.com/mapi/proptag/0x39FE001E";
            Outlook.Recipients recips = mail.Recipients;
            recips.ResolveAll();
            foreach (Outlook.Recipient recip in recips)
            {
                try { 
                Outlook.PropertyAccessor pa = recip.PropertyAccessor;
                string smtpAddress =
                    pa.GetProperty(PR_SMTP_ADDRESS).ToString();
                //if (!emails.Keys.Contains(smtpAddress)) emails.Add(smtpAddress, recip.Name );
                SaveRecipient(smtpAddress, recip.Name);
                SetSenderSubject(smtpAddress, mail.Subject);
                }
                catch (Exception ex)
                {

                }
            }
        }

        private void SetSenderSubject(string smtpAddress, string p)
        {
            using (SqlConnection cnn = new SqlConnection("Server=(localdb)\\Projects;Database=Newsletter;Trusted_Connection=True;Connection Timeout=30;"))
            {
                cnn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_UpdateSampleSubject @0, @1", cnn))
                {
                    cmd.Parameters.AddWithValue("@0", (String.Empty + smtpAddress).Trim('\'', '"'));
                    cmd.Parameters.AddWithValue("@1", (String.Empty + p).Trim('\'', '"'));
                    cmd.ExecuteNonQuery();
                }
            }            
        }

        // Uses recursion to enumerate Outlook subfolders.
        private void EnumerateFolders(Outlook.Folder folder)
        {
            int count = 0;
            foreach (object folderItem in folder.Items)
            {
                System.Windows.Forms.Application.DoEvents();
                Outlook.MailItem item = folderItem as Outlook.MailItem;
                if (item != null)
                {
                    //if (!emails.Keys.Contains(item.SenderEmailAddress)) emails.Add(item.SenderEmailAddress, item.SenderName);
                    try
                    {
                        SaveRecipient(item.SenderEmailAddress, item.SenderName);
                        SetSenderSubject(item.SenderEmailAddress, item.Subject);
                        GetSMTPAddressForRecipients(item);
                        /*
                        string[] tos = item.To == null ? new string[] { } : item.To.Split(';');
                        string[] ccs = item.CC == null ? new string[] { } :  item.CC.Split(';');
                        string[] bccs = item.BCC == null ? new string[] { } : item.BCC.Split(';');
                        AddAll( new string[][] { tos, ccs, bccs } );
                         * */
                        Thread.Sleep(100);
                        count++;
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            Outlook.Folders childFolders =
                folder.Folders;
            if (childFolders.Count > 0)
            {
                foreach (Outlook.Folder childFolder in childFolders)
                {
                    // Write the folder path.
                    //System.IO.File.AppendAllText(@"C:\Users\david.podhola\AppData\Local\Temp\outlookfolders.txt", childFolder.FolderPath);
                    // Call EnumerateFolders using childFolder.
                    EnumerateFolders(childFolder);
                }
            }
        }

        /*
        private void AddAll(string[][] p)
        {
            foreach (string[] emails1 in p)
            {
                foreach (string email in emails1)
                    if (!emails.Contains(email)) emails.Add(email);
            }
        }
        */

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            instance = this;
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
        #endregion
    }
}

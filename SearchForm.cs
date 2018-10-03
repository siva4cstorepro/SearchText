using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SearchText
{
    public partial class SearchForm : Form
    {
        Search searchTemp = null;

        public SearchForm()
        {
            InitializeComponent();
            txtFolderPath.Text = @"C:\CSPData\Development\Rocket\Web\CSPWeb";
        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            bool isOpSuccessful = false;
            string strShowStatus = "";

            List<string> lstSearchText = null;
            string strSearchFolder = @"C:\Users\vishnu\Desktop\Temp";
            string strIncludedExtensions = "";
            string strExcludedExtensions = "";

            if (this.txtSearchText.Text != null && this.txtSearchText.Text != "")
            {

                try
                {
                    string[] strArrSearchText = File.ReadAllLines(txtSearchText.Text);
                    lstSearchText = new List<string>(strArrSearchText);
                }
                catch (IOException)
                {

                }

                if (txtFolderPath.Text != null && txtFolderPath.Text != "")
                {
                    strSearchFolder = txtFolderPath.Text;

                    if (includedFileTypesTextBox.Text != null && includedFileTypesTextBox.Text != "")
                    {
                        strIncludedExtensions = includedFileTypesTextBox.Text;
                    }
                    else
                    {
                        strIncludedExtensions = "*.*";
                    }

                    if (includedFileTypesTextBox.Text != null && includedFileTypesTextBox.Text != "")
                    {
                        strExcludedExtensions = includedFileTypesTextBox.Text;
                    }
                    else
                    {
                        //strExcludedExtensions = "*.*";
                    }
                }
                else
                {
                    isOpSuccessful = false;
                }
            }
            else
            {
                isOpSuccessful = false;
            }

            searchTemp = new Search(inSearchText: lstSearchText
                                    , inSearchFolder: strSearchFolder
                                    , inIncludedExtension: strIncludedExtensions
                                    , inExcludedExtension: strExcludedExtensions);
            searchTemp.OnProgressUpdate += updateStatus;
            bkgWorker.RunWorkerAsync(searchTemp);
        }


        private void btnFileBrowser_Click(object sender, EventArgs e)
        {
            if(browserDialogFolder.ShowDialog() == DialogResult.OK)
            {
                this.txtFolderPath.Text = browserDialogFolder.SelectedPath;
            }
        }

        private void bkgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            bool isOpSuccessful = false;
            string strShowStatus = "";
            float progressVal;
            Search searchTemp = e.Argument as Search;
            isOpSuccessful = searchTemp.performSearch(out strShowStatus, out progressVal);
        }

        private void updateStatus(string inStatus, float progressVal)
        {
            // Its another thread so invoke back to UI thread
            base.Invoke((Action)delegate
            {
                this.progressBar.Value = (int)progressVal;
                this.lblSearchStatus.Text = "";
                this.lblSearchStatus.Text += inStatus;
            });
        }

        private void btnFileName_Click(object sender, EventArgs e)
        {
            DialogResult result = fileDialogSearchText.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string filePath = fileDialogSearchText.InitialDirectory + fileDialogSearchText.FileName;
                this.txtSearchText.Text = filePath;
            }
        }

        
    }
}

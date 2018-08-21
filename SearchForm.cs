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
        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            bool isOpSuccessful = false;
            string strShowStatus = "";

            List<string> lstSearchText = null;
            string strSearchFolder = "";
            string strIncludedExtensions = "";
            string strExcludedExtensions = "";

            if(this.txtSearchText.Text != null && this.txtSearchText.Text != "")
            {

                try
                {
                    string[] strArrSearchText = File.ReadAllLines(this.txtSearchText.Text);
                    lstSearchText = new List<string>(strArrSearchText);
                }
                catch (IOException)
                {

                }

                if (this.txtFolderPath.Text != null && this.txtFolderPath.Text != "")
                {
                    strSearchFolder = this.txtFolderPath.Text;

                    if (this.chlboxIncludedFiles.Text != null && this.chlboxIncludedFiles.Text != "")
                    {
                        strIncludedExtensions = this.chlboxIncludedFiles.Text;
                    }
                    else
                    {
                        strIncludedExtensions = "*.*";
                    }

                    if (this.chklstBoxExcludedFileTypes.Text != null && this.chklstBoxExcludedFileTypes.Text != "")
                    {
                        strExcludedExtensions = this.chklstBoxExcludedFileTypes.Text;
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
                                    , inIncludedExtenstion: strIncludedExtensions
                                    , inExcludedExtenstion: strExcludedExtensions);
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

            Search searchTemp = e.Argument as Search;
            isOpSuccessful = searchTemp.performSearch(out strShowStatus);
        }

        private void updateStatus(string inStatus)
        {
            // Its another thread so invoke back to UI thread
            base.Invoke((Action)delegate
            {
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

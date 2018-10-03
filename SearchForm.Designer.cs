namespace SearchText
{
    partial class SearchForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtSearchText = new System.Windows.Forms.TextBox();
            this.lblSearchText = new System.Windows.Forms.Label();
            this.lblFileFolder = new System.Windows.Forms.Label();
            this.lblIncludedFileTypes = new System.Windows.Forms.Label();
            this.lblSearchStatus = new System.Windows.Forms.Label();
            this.txtFolderPath = new System.Windows.Forms.TextBox();
            this.btnFileBrowser = new System.Windows.Forms.Button();
            this.browserDialogFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.bkgWorker = new System.ComponentModel.BackgroundWorker();
            this.btnFileName = new System.Windows.Forms.Button();
            this.fileDialogSearchText = new System.Windows.Forms.OpenFileDialog();
            this.includedFileTypesTextBox = new System.Windows.Forms.TextBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.Location = new System.Drawing.Point(404, 365);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(120, 36);
            this.btnSearch.TabIndex = 0;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtSearchText
            // 
            this.txtSearchText.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchText.Location = new System.Drawing.Point(298, 45);
            this.txtSearchText.Name = "txtSearchText";
            this.txtSearchText.Size = new System.Drawing.Size(327, 23);
            this.txtSearchText.TabIndex = 2;
            // 
            // lblSearchText
            // 
            this.lblSearchText.AutoSize = true;
            this.lblSearchText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearchText.Location = new System.Drawing.Point(294, 22);
            this.lblSearchText.Name = "lblSearchText";
            this.lblSearchText.Size = new System.Drawing.Size(331, 20);
            this.lblSearchText.TabIndex = 3;
            this.lblSearchText.Text = "Search Text (Seperate by comma for multiple)";
            // 
            // lblFileFolder
            // 
            this.lblFileFolder.AutoSize = true;
            this.lblFileFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFileFolder.Location = new System.Drawing.Point(389, 86);
            this.lblFileFolder.Name = "lblFileFolder";
            this.lblFileFolder.Size = new System.Drawing.Size(135, 20);
            this.lblFileFolder.TabIndex = 5;
            this.lblFileFolder.Text = "Folders to Search";
            // 
            // lblIncludedFileTypes
            // 
            this.lblIncludedFileTypes.AutoSize = true;
            this.lblIncludedFileTypes.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIncludedFileTypes.Location = new System.Drawing.Point(392, 152);
            this.lblIncludedFileTypes.Name = "lblIncludedFileTypes";
            this.lblIncludedFileTypes.Size = new System.Drawing.Size(132, 20);
            this.lblIncludedFileTypes.TabIndex = 6;
            this.lblIncludedFileTypes.Text = "Include File types";
            // 
            // lblSearchStatus
            // 
            this.lblSearchStatus.AutoSize = true;
            this.lblSearchStatus.Location = new System.Drawing.Point(62, 436);
            this.lblSearchStatus.Name = "lblSearchStatus";
            this.lblSearchStatus.Size = new System.Drawing.Size(0, 13);
            this.lblSearchStatus.TabIndex = 15;
            // 
            // txtFolderPath
            // 
            this.txtFolderPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFolderPath.Location = new System.Drawing.Point(298, 109);
            this.txtFolderPath.Name = "txtFolderPath";
            this.txtFolderPath.Size = new System.Drawing.Size(327, 23);
            this.txtFolderPath.TabIndex = 16;
            // 
            // btnFileBrowser
            // 
            this.btnFileBrowser.Location = new System.Drawing.Point(643, 109);
            this.btnFileBrowser.Name = "btnFileBrowser";
            this.btnFileBrowser.Size = new System.Drawing.Size(29, 23);
            this.btnFileBrowser.TabIndex = 17;
            this.btnFileBrowser.Text = "...";
            this.btnFileBrowser.UseVisualStyleBackColor = true;
            this.btnFileBrowser.Click += new System.EventHandler(this.btnFileBrowser_Click);
            // 
            // bkgWorker
            // 
            this.bkgWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bkgWorker_DoWork);
            // 
            // btnFileName
            // 
            this.btnFileName.Location = new System.Drawing.Point(643, 45);
            this.btnFileName.Name = "btnFileName";
            this.btnFileName.Size = new System.Drawing.Size(29, 23);
            this.btnFileName.TabIndex = 18;
            this.btnFileName.Text = "...";
            this.btnFileName.UseVisualStyleBackColor = true;
            this.btnFileName.Click += new System.EventHandler(this.btnFileName_Click);
            // 
            // includedFileTypesTextBox
            // 
            this.includedFileTypesTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.includedFileTypesTextBox.Location = new System.Drawing.Point(298, 175);
            this.includedFileTypesTextBox.Name = "includedFileTypesTextBox";
            this.includedFileTypesTextBox.Size = new System.Drawing.Size(327, 23);
            this.includedFileTypesTextBox.TabIndex = 19;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(175, 272);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(603, 23);
            this.progressBar.TabIndex = 20;
            // 
            // SearchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(977, 493);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.includedFileTypesTextBox);
            this.Controls.Add(this.btnFileName);
            this.Controls.Add(this.btnFileBrowser);
            this.Controls.Add(this.txtFolderPath);
            this.Controls.Add(this.lblSearchStatus);
            this.Controls.Add(this.lblIncludedFileTypes);
            this.Controls.Add(this.lblFileFolder);
            this.Controls.Add(this.lblSearchText);
            this.Controls.Add(this.txtSearchText);
            this.Controls.Add(this.btnSearch);
            this.Name = "SearchForm";
            this.Text = "Search";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtSearchText;
        private System.Windows.Forms.Label lblSearchText;
        private System.Windows.Forms.Label lblFileFolder;
        private System.Windows.Forms.Label lblIncludedFileTypes;
        public System.Windows.Forms.Label lblSearchStatus;
        private System.Windows.Forms.TextBox txtFolderPath;
        private System.Windows.Forms.Button btnFileBrowser;
        private System.Windows.Forms.FolderBrowserDialog browserDialogFolder;
        private System.ComponentModel.BackgroundWorker bkgWorker;
        private System.Windows.Forms.Button btnFileName;
        private System.Windows.Forms.OpenFileDialog fileDialogSearchText;
        private System.Windows.Forms.TextBox includedFileTypesTextBox;
        private System.Windows.Forms.ProgressBar progressBar;
    }
}


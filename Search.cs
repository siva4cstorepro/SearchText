using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchText
{

    static class SearchResult
    {
         //string strSearchText;
         //string strFoundFilePath;
         //string strFoundLineNumber;
         //string strFoundLineText;


        //public SearchResult(string inSearchText, string inFoundFilePath, string inFoundLineNumber, string inFoundLineText)
        //{
        //    strSearchText = inSearchText;
        //    strFoundFilePath = inFoundFilePath;
        //    strFoundLineNumber = inFoundLineNumber;
        //    strFoundLineText = inFoundLineText;
        //}


        //public bool writeSearchResult()
        //{
        //    bool isOpSuccessful = false;


        //    return isOpSuccessful;
        //}


    }

    class Search
    {
        private List<string> lstSearchText;
        private List<string> lstFolders;
        private List<string> lstIncludedExtension; 
        private List<string> lstExcludedExtension;
        private List<string> lstAllSearchTextOccurences = new List<string>();
        private string strStatusMessage;

        public delegate void ProgressUpdate(string strShowStatus);
        public event ProgressUpdate OnProgressUpdate;

        //TODO: Do file upload search
        public Search(List<string> inSearchText, string inSearchFolder, string inIncludedExtenstion = ".cs", string inExcludedExtenstion = "*.mdf")
        {
            lstSearchText = inSearchText;
            lstFolders = inSearchFolder.Split(',').ToList();
            lstIncludedExtension = inIncludedExtenstion.Split(',').ToList();
            lstExcludedExtension = inExcludedExtenstion.Split(',').ToList();
        }

        public bool performSearch(out string outShowStatus)
        {
            bool isOpSuccessful = false;

            foreach(string strSearchText in lstSearchText) //Pick each text
            {
                foreach (string strFolderPath in lstFolders) //Go 
                {
                    string strAllIncludedExtensions = String.Join(",", lstIncludedExtension);

                    //TODO
                    //1. Parallel the search
                    //2. Check for read access for the files
                    List<string> filenames = new List<string>(Directory.GetFiles(strFolderPath, "*.*", SearchOption.AllDirectories));

                    int intTotalFilesCount = filenames.Count;
                    int intFoundInFilesCount = 0;
                    int intSearchedInFilesCount = 0;

                    if(filenames.Count > 0)
                    {
                        foreach (string file in filenames)
                        {
                            intSearchedInFilesCount++;
                            string[] fileLines = File.ReadAllLines(file);

                            int i = 0;
                            foreach (string currentLine in fileLines)
                            {
                                i++;
                                if (currentLine.Contains(strSearchText))
                                {
                                    StringBuilder sbFoundTextEntry = new StringBuilder("");
                                    sbFoundTextEntry.Append(file);
                                    sbFoundTextEntry.Append("\t");
                                    sbFoundTextEntry.Append("Line#: " + i.ToString());
                                    sbFoundTextEntry.Append("\t");
                                    sbFoundTextEntry.Append(currentLine);
                                    sbFoundTextEntry.Append("\n");

                                    lstAllSearchTextOccurences.Add(sbFoundTextEntry.ToString());
                                    intFoundInFilesCount++;
                                    isOpSuccessful = true;
                                }
                                else
                                {
                                    // do not need to mark it as false
                                }
                            }

                            //var lstSearchTextOccurences = fileLines.Where(s => s.Contains(strSearchText)).ToList();
                            //if (lstSearchTextOccurences != null)
                            //{
                            //    if(lstSearchTextOccurences.Count > 0)
                            //    {
                            //        lstAllSearchTextOccurences.AddRange(lstSearchTextOccurences);
                            //        isOpSuccessful = true;
                            //        intFoundInFilesCount++;
                            //    }
                            //    else
                            //    {

                            //    }
                            //}

                            strStatusMessage = "In the Folder " + strFolderPath + " there are " + intTotalFilesCount.ToString() + " files with extension " + strAllIncludedExtensions.ToString() + " searched in " + intSearchedInFilesCount.ToString() + " found matching files in " + intFoundInFilesCount.ToString();
                            if (OnProgressUpdate != null)
                            {
                                OnProgressUpdate(strStatusMessage);
                            }
                        }
                    }
                    else
                    {
                        strStatusMessage = "No files find with these extenstions: " + strAllIncludedExtensions + " in the folder " + strFolderPath;
                    }
                }
            }


            if(isOpSuccessful)
            {
                if(lstAllSearchTextOccurences.Count > 0)
                {
                    writeAndDownloadFile();
                    isOpSuccessful = true;
                }
                else
                {

                }

            }

            outShowStatus = strStatusMessage;
            return isOpSuccessful;
        }


        private bool writeAndDownloadFile()
        {
            bool isOpSuccessful = false;
            string path = @"C:\CSPData\Development\SandBox\SearchText\SearchText\Result\results.txt";
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                using (TextWriter tw = new StreamWriter(fs))
                {
                    try
                    {
                        lstAllSearchTextOccurences.ForEach(tw.WriteLine);
                        isOpSuccessful = true;
                    }
                    catch(Exception ex)
                    {
                        isOpSuccessful = false;
                    }

                }
            }

            return isOpSuccessful;
        }
    }
}

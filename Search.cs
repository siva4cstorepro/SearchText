using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
        private List<string> files;
        private string strStatusMessage;
        private float progress;
        private DataTable dataTable;
        public delegate void ProgressUpdate(string strShowStatus, float progressVal);
        public event ProgressUpdate OnProgressUpdate;

        //TODO: Do file upload search
        public Search(List<string> inSearchText, string inSearchFolder, string inIncludedExtension = ".cs", string inExcludedExtension = "*.mdf")
        {
            lstSearchText = inSearchText;
            lstFolders = inSearchFolder.Split(',').ToList();
            lstIncludedExtension = inIncludedExtension.Split(',').ToList();
            lstExcludedExtension = inExcludedExtension.Split(',').ToList();


            //List<string> result = File.ReadLines(@"C:\CSPData\Search Tool\Result\resultshtml.txt").ToList();
            //List<string> writeResult = new List<string>();
            //for (int i = 1; i < result.Count - 1; i++)
            //{
            //    if (result[i] == "")
            //    {
            //        result.RemoveAt(i);
            //    }
            //    if (result[i].Split('Ø').Length > 1)
            //    {
            //        writeResult.Add(string.Join("|", new string[] { result[i].Split('Ø')[0], result[i].Split('Ø')[1], result[i].Split('Ø')[2] }));
            //    }
            //}
            //File.WriteAllLines(@"C:\CSPData\Search Tool\Result\resultsCSS.txt", writeResult);

            files = new List<string>();
            //lstIncludedExtension = ".js,.css,.html,.png,.svg,.jpg,.gif,.ttf,.config,.aspx,.ashx,.cs".Split(',').ToList();
            foreach (string extension in lstIncludedExtension)
            {
                Directory.GetFiles(inSearchFolder, "*" + extension, SearchOption.AllDirectories).ToList().ForEach(item => files.Add(item));
            }

            lstIncludedExtension = ".js,.css,.html,.png,.svg,.jpg,.gif,.ttf,.config,.aspx,.ashx,.cs".Split(',').ToList();
            //foreach (string extension in lstIncludedExtension)
            //{
            //    files = new List<string>();
            //    Directory.GetFiles(inSearchFolder, "*" + extension, SearchOption.AllDirectories).ToList().ForEach(item => files.Add(item.Split('\\')[item.Split('\\').Length - 1].Split('.')[item.Split('\\')[item.Split('\\').Length - 1].Split('.').Length - 1] + "|" + item.Split('\\')[item.Split('\\').Length - 1] + "|" + item));
            //    if (".css".Contains(extension))
            //    {
            //        File.AppendAllLines(@"C:\CSPData\Search Tool\Result\css.txt", files);
            //    }
            //    if (".js".Contains(extension))
            //    {
            //        File.AppendAllLines(@"C:\CSPData\Search Tool\Result\js.txt", files);
            //    }
            //    if (".html".Contains(extension))
            //    {
            //        File.AppendAllLines(@"C:\CSPData\Search Tool\Result\html.txt", files);
            //    }
            //    if (".png,.svg,.jpg,.gif".Contains(extension))
            //    {
            //        File.AppendAllLines(@"C:\CSPData\Search Tool\Result\images.txt", files);
            //    }
            //    if (".ttf".Contains(extension))
            //    {
            //        File.AppendAllLines(@"C:\CSPData\Search Tool\Result\ttf.txt", files);
            //    }
            //    if (".config".Contains(extension))
            //    {
            //        File.AppendAllLines(@"C:\CSPData\Search Tool\Result\config.txt", files);
            //    }
            //    if (".aspx".Contains(extension))
            //    {
            //        File.AppendAllLines(@"C:\CSPData\Search Tool\Result\aspx.txt", files);
            //    }
            //    if (".ashx".Contains(extension))
            //    {
            //        File.AppendAllLines(@"C:\CSPData\Search Tool\Result\ashx.txt", files);
            //    }
            //    if (".cs".Contains(extension))
            //    {
            //        File.AppendAllLines(@"C:\CSPData\Search Tool\Result\cs.txt", files);
            //    }
            //    File.AppendAllLines(@"C:\CSPData\Search Tool\Result\MasterData.txt", files);
            //}

            //string[] delatList = File.ReadAllLines(@"C:\Users\vishnu\Desktop\Delta.txt");
            //string deltas = string.Join("", delatList);

            for (int i = 0; i < files.Count; i++)
            {
                foreach (string ext in new string[] { ".mdf", ".git", ".ldf", ".sqlite", ".lock", ".cache" })
                {
                    if (files[i].Contains(ext))
                    {
                        files.RemoveAt(i);
                    }
                    //if (deltas.Contains(files[i].Split('\\')[files[i].Split('\\').Length - 1]))
                    //{
                    //    File.Delete(files[i]);
                    //}
                }
            }

            dataTable = new DataTable("SearchResults");
            //dataTable.Columns.Add("Id", typeof(string));
            dataTable.Columns.Add("StoredProcedure", typeof(string));
            dataTable.Columns.Add("Path", typeof(string));
            dataTable.Columns.Add("LineNumber", typeof(string));
            dataTable.Columns.Add("Line", typeof(string));
        }

        public bool performSearch(out string outShowStatus, out float progressVal)
        {
            bool isOpSuccessful = false;
            foreach (string strFolderPath in lstFolders) // Foreach folder
            {
                string strAllIncludedExtensions = String.Join(",", lstIncludedExtension);

                int intTotalFilesCount = files.Count;
                float intFoundInFilesCount = 0;
                float intSearchedInFilesCount = 0;

                if (files.Count > 0)
                {
                    foreach (string file in files) // Foreach file
                    {
                        intSearchedInFilesCount++;
                        string[] fileLines = File.ReadAllLines(file);

                        int i = 0;
                        foreach (string currentLine in fileLines) // Foreach line
                        {
                            i++;
                            void helper(string strSearchText) //foreach (string strSearchText in lstSearchText)
                            {
                                if (currentLine.Contains(strSearchText))
                                {
                                    StringBuilder sbFoundTextEntry = new StringBuilder("");
                                    sbFoundTextEntry.Append(strSearchText);
                                    sbFoundTextEntry.Append("Ø");
                                    sbFoundTextEntry.Append(file);
                                    sbFoundTextEntry.Append("Ø");
                                    sbFoundTextEntry.Append(i.ToString());
                                    sbFoundTextEntry.Append("Ø");
                                    sbFoundTextEntry.Append(currentLine.TrimStart(' ', '\t'));
                                    sbFoundTextEntry.Append("\n");

                                    lstAllSearchTextOccurences.Add(sbFoundTextEntry.ToString());
                                    //dataTable.Rows.Add(sbFoundTextEntry.ToString().Split('Ø'));
                                    intFoundInFilesCount++;
                                    isOpSuccessful = true;
                                }
                            }
                            //lstSearchText.Remove(file.Split('\\')[file.Split('\\').Length - 1].Replace(".cs", ""));
                            Parallel.ForEach(lstSearchText, strSearchText => helper(strSearchText));
                            //Parallel.ForEach(lstSearchText, strSearchText => helper(strSearchText.Replace(".ashx", "")));
                            //lstSearchText.Add(file.Split('\\')[file.Split('\\').Length - 1].Replace(".cs", ""));

                            strStatusMessage = "In the Folder " + strFolderPath + " there are " + intTotalFilesCount.ToString() + " files with extension " + strAllIncludedExtensions.ToString() + " searched in " + intSearchedInFilesCount.ToString() + " found matching files in " + intFoundInFilesCount.ToString();
                            if (OnProgressUpdate != null)
                            {
                                OnProgressUpdate(strStatusMessage, progress);
                                progress = (intSearchedInFilesCount / intTotalFilesCount) * 100;
                            }
                        }

                    }
                }
                else
                {
                    strStatusMessage = "No files find with these extenstions: " + strAllIncludedExtensions + " in the folder " + strFolderPath;
                }
            }

            if (isOpSuccessful)
            {
                if (lstAllSearchTextOccurences.Count > 0)
                {
                    WriteAndDownloadFile();
                    isOpSuccessful = true;
                }
                else
                {

                }
            }

            outShowStatus = strStatusMessage;
            progressVal = (float)progress;
            return isOpSuccessful;
        }


        private void InsertDB(string tblName)
        {
            string connectionString = @"Data Source=(local)\SQL2017;Initial Catalog=SearchTextDB;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    using (var command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "IF EXISTS( SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'SearchResults') " + @"DROP TABLE [dbo].[SearchResults]";
                        command.ExecuteNonQuery();
                        command.CommandText = "IF NOT EXISTS( SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'SearchResults') " + @"CREATE TABLE [dbo].[SearchResults](
                                                                                                                                                                [StoredProcedure][NVARCHAR](MAX) NULL, 
                                                                                                                                                                [Path] [NVARCHAR](MAX) NULL,
                                                                                                                                                                [LineNumber] [NVARCHAR] (50) NULL,
	                                                                                                                                                            [Line][NVARCHAR](MAX) NULL
                                                                                                                                                           ) ON[PRIMARY] TEXTIMAGE_ON[PRIMARY]
                                                                                                                                                        ";
                        command.ExecuteNonQuery();
                    }
                    foreach (DataColumn c in dataTable.Columns)
                    {
                        bulkCopy.ColumnMappings.Add(c.ColumnName, c.ColumnName);
                    }

                    bulkCopy.DestinationTableName = dataTable.TableName;
                    try
                    {
                        bulkCopy.WriteToServer(dataTable);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        private bool WriteAndDownloadFile()
        {
            bool isOpSuccessful = false;
            string path = @"C:\CSPData\Search Tool\Result\ResultASHX.txt";

            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                using (TextWriter tw = new StreamWriter(fs))
                {
                    try
                    {
                        lstAllSearchTextOccurences.Insert(0, "FileØPathØLineNumberØLine");
                        lstAllSearchTextOccurences.ForEach(tw.WriteLine);
                        isOpSuccessful = true;
                    }
                    catch (Exception ex)
                    {
                        isOpSuccessful = false;
                    }

                }
            }

            InsertDB("SearchResults");

            return isOpSuccessful;
        }
    }
}

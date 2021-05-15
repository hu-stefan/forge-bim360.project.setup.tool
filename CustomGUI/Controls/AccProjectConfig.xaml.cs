﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AdskConstructionCloudBreakdown;
using CsvHelper;
using CsvHelper.Configuration;

namespace CustomGUI.Controls
{
    /// <summary>
    /// Interaction logic for AccProjectConfig.xaml
    /// </summary>
    public partial class AccProjectConfig : UserControl
    {
        private List<Bim360Project> projects { get; set; }
        private string csvpath { get; set; }

        private Bim360Project activeProject { set; get; }

        private Folder activeFolder { set; get; }

        public AccProjectConfig()
        {
            InitializeComponent();

            FolderPermissionComboBox.ItemsSource = Enum.GetValues(typeof(AdskConstructionCloudBreakdown.AccessPermissionEnum));

        }

        private void AccProjectConfig_OnInitialized(object? sender, EventArgs e)
        {
            try
            {
                //TOdo: add last path to csvpath to load last config
                // projects = SerializationParser.LoadBim360ProjectsFromCsv("csvpath");
            }
            catch{}

        }

        public Boolean LoadBim360Projects(string filepath)
        {
            for (int i = 0; i < 2; i++)
            {
                using (var streamReader = new StreamReader(filepath))
                {
                    //try different configs
                    CsvConfiguration csvconfig;
                    if (i == 0)
                    {
                        //CSV with current date config
                        csvconfig = new CsvConfiguration(CultureInfo.CurrentCulture)
                        {
                            HeaderValidated = null,
                            MissingFieldFound = null
                        };
                    }
                    else
                    {
                        //CSV with Invariant Config
                        csvconfig = new CsvConfiguration(CultureInfo.InvariantCulture)
                        {
                            HeaderValidated = null,
                            MissingFieldFound = null
                        };
                    }

                    //try each configuration
                    using (var csv = new CsvReader(streamReader, csvconfig))
                    {
                        //Maps the Header of the CSV Data to the class attributes
                        csv.Context.RegisterClassMap<UserDataMap>();

                        //call the import
                        var output = SerializationParser.LoadBim360ProjectsFromCsv(csv);

                        //Sort in Data if read was successful
                        if (output != null)
                        {
                            //ToDo: sort data into Frontend
                            ProjectsView.ItemsSource = output;
                            activeProject = output[0];
                            return true;
                        }
                    }

                }
            }

            return false;
        }

        public void ExportBim360Projects(string filepath,List<Bim360Project> dataset)
        {
            using (var streamWriter = new StreamWriter(filepath))
            {
                //maybe the CultureInfo needs to be changed
                using (var csv = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                {
                    //Maps the Header of the CSV Data to the class attributes
                    var tmp = SerializationParser.ExportBim360ToCSV(dataset);
                    csv.Context.RegisterClassMap<UserDataExport>();
                    csv.WriteRecords(tmp);
                }
            }

        }
        //change active project
        private void ProjectsView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //should be right i guess
            activeProject=(Bim360Project)ProjectsView.SelectedCells[0].Item;
        }
    }
}

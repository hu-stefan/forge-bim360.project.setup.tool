﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using CustomGUI;

namespace AdskConstructionCloudBreakdown
{
    public class SerializationParser
    {
        public SerializationParser()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Bim360Project> LoadBim360ProjectsFromCsv(CsvReader input)
        {
            //var declaration
            var output = new List<Bim360Project>();
            Folder activeFolder;

            //loop over all rows
            while (input.Read())
            {
                var tmp = input.GetRecord<UserData>();

                //create only new Projects if their is a new name
                if (tmp._project_name!=null)
                {
                    output.Add(new Bim360Project(tmp._project_name));
                    activeFolder = null;
                }

                //set type
                if (tmp._project_type.Equals("Office"))
                {
                    output[output.Count].ProjectType = ProjectTypeEnum.Office;
                }
                else if (tmp._project_type.Equals("Library"))
                {
                    output[output.Count].ProjectType = ProjectTypeEnum.Library;
                }

                //ToDo: Add the folder structure dynamic

            }



            return output ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Bim360Project> LoadBim360ProjectsFromJson()
        {
            throw new NotImplementedException("this is openSource. Do it yourself. ");
        }


        ///<summary>
        ///
        /// </summary>
        public static void ExportBim360toCSV()
        {
            throw new NotImplementedException("YOU SHALL NOT PASS \ndata out currently");
        }


    }
}
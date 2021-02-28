using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Sg132S_2A.App_Code;
using System.Web.UI.DataVisualization.Charting;
using System.IO;
using System.Drawing;
using Sg132S_2A.Models;

namespace Sg132S_2A.Controllers
{
    public class DriveController : Controller
    {        

        // GET: Drive        
        public ActionResult Index(Parameters drive)
        {                       
            
           if(drive.ID == 0)
            {
                drive = Parameters.DefaultEngineParams;
                drive.Ts = 11;
                drive.Jext = 0.3;
                drive.Dext = 0.001;
                drive.F0 = 0;
                drive.Fk = 50;
            }            
            return View(drive);
        }   
        

        [HttpPost]
        public ActionResult Save(Parameters drive)
        {
            drive.ID = 1;            
            return (RedirectToAction("Index", drive));            
            
        }

       
        public ActionResult DrawWaveforms(Parameters param)
        {
            //solver results
            //0   |1  |2  |3  |4  |5  |6  |7    |8   |9  |10 |11 |12 |13 |14 |15  |16
            //time|Isu|Isv|Isw|Iru|Irv|Irw|Omega|Alfa|Usu|Usv|Usw|Uru|Urv|Urv|Text|Telem
            //-------
                        
            DataTable dTableIU;
            DataTable dTableOmegaTorque;            
            DataView dViewIU;
            DataView dViewOmegaTorque;
            DataColumn column;
            DataRow row;
                                  

            
            ModelParams modelParams = new ModelParams();
            parametersPassing(param, modelParams);
            
            MathModel mathModel = new MathModel(modelParams, modelParams.TimeStart, modelParams.InitCondit);
            Solver solver = new Solver(modelParams, mathModel);
            
            

            solver.SolverStart(modelParams);            

            //currents and voltages chart
            dTableIU = new DataTable();
            column = new DataColumn();
            column.DataType = Type.GetType("System.Double");
            column.ColumnName = "Time";
            dTableIU.Columns.Add(column);
            //______________________
            column = new DataColumn();
            column.DataType = Type.GetType("System.Double");
            column.ColumnName = "Iu";
            dTableIU.Columns.Add(column);
            //_______________________
            column = new DataColumn();
            column.DataType = Type.GetType("System.Double");
            column.ColumnName = "Iv";
            dTableIU.Columns.Add(column);
            //_______________________
            column = new DataColumn();
            column.DataType = Type.GetType("System.Double");
            column.ColumnName = "Iw";
            dTableIU.Columns.Add(column);
            //________________________
            column = new DataColumn();
            column.DataType = Type.GetType("System.Double");
            column.ColumnName = "Uuv";
            dTableIU.Columns.Add(column);
            // Adding rows
            
            double Ki = 1; // or Math.Sqrt(3)
            double Ku = 1; // or 0.25
            for (int i = 0; i < modelParams.PtsResults; i++)
            {
                row = dTableIU.NewRow();
                row["Time"] = solver.Results[i, 0];
                row["Iu"] = solver.Results[i, 1] * Ki;
                row["Iv"] = solver.Results[i, 2] * Ki;
                row["Iw"] = solver.Results[i, 3] * Ki;
                row["Uuv"] = solver.Results[i, 9] * Ku;
                dTableIU.Rows.Add(row);
            }
            dViewIU = new DataView(dTableIU);

            Chart chartIU = new Chart();
            chartIU.Width = 1050;
            chartIU.Height = 700;

            chartIU.Series.Clear();
            chartIU.Titles.Clear();
            chartIU.Titles.Add("Currents and voltages waveforms");
            
            chartIU.DataBindTable(dViewIU, "Time");
            chartIU.ChartAreas.Add(new ChartArea("Current"));
            chartIU.ChartAreas.Add(new ChartArea("Voltage"));
            chartIU.ChartAreas["Voltage"].AxisY.Title = "Uuv[V]";
            chartIU.ChartAreas["Current"].AxisY.Title = "Stator currents [A]";
            chartIU.Series["Uuv"].ChartType = SeriesChartType.Line;
            chartIU.Series["Iu"].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
            chartIU.Series["Iv"].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
            chartIU.Series["Iw"].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
            chartIU.Series["Uuv"].ChartArea = "Voltage";
            chartIU.Series["Iu"].ChartArea = "Current";
            chartIU.Series["Iv"].ChartArea = "Current";
            chartIU.Series["Iw"].ChartArea = "Current";
            chartIU.ChartAreas[0].AxisX.Minimum = 0;
            chartIU.ChartAreas[1].AxisX.Minimum = 0;

            //________________________________________________________________________________________________
            
            chartIU.Titles[0].Font = new System.Drawing.Font(
                "Times New Roman", 16F, System.Drawing.FontStyle.Bold);
            chartIU.Titles[0].ForeColor = System.Drawing.Color.FromArgb(250, 0, 0);
            //***********************************************************

            //Speed and torque
            dTableOmegaTorque = new DataTable();
            column = new DataColumn();
            column.DataType = Type.GetType("System.Double");
            column.ColumnName = "Time";
            dTableOmegaTorque.Columns.Add(column);
            //___________________________
            column = new DataColumn();
            column.DataType = Type.GetType("System.Double");
            column.ColumnName = "Omega";
            dTableOmegaTorque.Columns.Add(column);
            //___________________________
            column = new DataColumn();
            column.DataType = Type.GetType("System.Double");
            column.ColumnName = "Torque";
            dTableOmegaTorque.Columns.Add(column);
            //___________________________
            double Kt = 2;
            //adding rows
            for (int i = 0; i < modelParams.PtsResults; i++)
            {
                row = dTableOmegaTorque.NewRow();
                row["Time"] = solver.Results[i, 0];
                row["Omega"] = solver.Results[i, 7];
                row["Torque"] = solver.Results[i, 16] * Kt;
                dTableOmegaTorque.Rows.Add(row);
            }

            dViewOmegaTorque = new DataView(dTableOmegaTorque);

            Chart chartOmegaTorque = new Chart();
            chartOmegaTorque.Width = 1050;
            chartOmegaTorque.Height = 700;

            dViewOmegaTorque = new DataView(dTableOmegaTorque);

            chartOmegaTorque.ChartAreas.Add("Waveforms");
            chartOmegaTorque.DataBindTable(dViewOmegaTorque, "Time");
            chartOmegaTorque.Series["Omega"].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
            chartOmegaTorque.Series["Torque"].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
            chartOmegaTorque.ChartAreas["Waveforms"].AxisX.Minimum = 0.0;
            chartOmegaTorque.ChartAreas["Waveforms"].AxisX.Maximum = (double)solver.Results[modelParams.PtsResults - 1, 0];
            chartOmegaTorque.ChartAreas["Waveforms"].AxisX.LabelStyle.Format = "{#0.000}";
            chartOmegaTorque.Width = 1050;
            chartOmegaTorque.Height = 400;
            
            // Legend and title
            chartOmegaTorque.ChartAreas["Waveforms"].AxisY.Title = "Omega and Torque";
            chartOmegaTorque.Legends.Add(new Legend("Legend"));
            chartOmegaTorque.Legends["Legend"].DockedToChartArea = "Waveforms";
            chartOmegaTorque.Series["Omega"].Legend = "Legend";
            chartOmegaTorque.Series["Omega"].IsVisibleInLegend = true;
            chartOmegaTorque.Series["Omega"].LegendText = "Omega [rad/s]";
            chartOmegaTorque.Series["Torque"].LegendText = "Torque*2 [Nm]";
            chartOmegaTorque.Legends["Legend"].Alignment = StringAlignment.Center;
            

            
            // ------------- Creating chart list ------------

            var chartList = new List<Chart> { chartIU, chartOmegaTorque };
            var imageList = ChartsToImages(chartList);            

            // ----- Final size of image and bonding charts into one .bmp file            
            var size = new Size();

            foreach(var image in imageList)
            {
                if(image.Width > size.Width)
                {
                    size.Width = image.Width;
                }
                size.Height += image.Height;
            }
            var finalImage = new Bitmap(size.Width, size.Height);
            using (var gfx = Graphics.FromImage(finalImage))
            {
                var y = 0;
                foreach(var image in imageList)
                {
                    gfx.DrawImage(image, 0, y);
                    y += image.Height;
                }
            }
            MemoryStream finalms = new MemoryStream();
            finalImage.Save(finalms, System.Drawing.Imaging.ImageFormat.Png);
            for (int i = 0; i < 8; i++)
            {
                modelParams.InitCondit[i] = 0;
            }
            
            return File(finalms.GetBuffer(), "image/png");            

        }        
        
        List<Image> ChartsToImages(List<Chart> charts)
        {
            var imageList = new List<Image>();
            foreach(var chart in charts)
            {
                using (MemoryStream imgListMS = new MemoryStream())
                {
                    chart.SaveImage(imgListMS, ChartImageFormat.Png);
                    Image img = System.Drawing.Bitmap.FromStream(imgListMS);
                    imageList.Add(img);
                }
            }
            return imageList;
        } 
        //----------------------------------------------------------------------

        //Assign params from math model 
        //Edit: Today, with more experience, I'd use automapper or something like that
        public void parametersPassing(Parameters model, ModelParams appcode)
        {            
            appcode.Fs = model.Fs;
            appcode.Uas_rms = model.Uu1;
            appcode.Ubs_rms = model.Uv1;
            appcode.Ucs_rms = model.Uw1;
            appcode.Phase = model.Phase;
            appcode.Mas_ext = model.Msu;
            appcode.Mbs_ext = model.Msv;
            appcode.Mcs_ext = model.Msw;
            appcode.Ras_ext = model.Rsu;
            appcode.Rbs_ext = model.Rsv;
            appcode.Rcs_ext = model.Rsw;
            appcode.Jrext = model.Jext;
            appcode.Drext = model.Dext;
            appcode.NoLoadTime = model.Ttl;
            appcode.Textern = model.Tn;
            appcode.TimeStart = model.Ti;
            appcode.TimeStop = model.Ts;
            appcode.pwmState = model.pwmState;
            appcode.TsoftStart = model.Tsoftstart;
            appcode.F0 = model.F0;
            appcode.Fk = model.Fk;
            appcode.SlopeRatio = model.SlopeRatio;
        }
        //---------------------------------------

        // PWM Status
        private string pwmState;
        public string PwmState
        {
            get { return pwmState; }
            set
            {
                pwmState = value;
            }
        }
        public SelectList getPwmValue()
        {
            List<String> states = new List<String>();
            states.Add("On");
            states.Add("Off");
            SelectList pwmStateSelect = new SelectList(states);
            return pwmStateSelect;
        }
        public enum pwmStatus
        {
            On,
            Off
        }
        //---------------------------------------------------
    }
}
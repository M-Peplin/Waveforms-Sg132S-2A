using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//-----
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

namespace Sg132S_2A.App_Code
{
    public class MathModel
    {
        private int size;
        //---
        public double[,] Msys { get; set; }
        public double[] Usys { get; set; }
        public double[,] Rsys { get; set; }
        public double[,] Gsys { get; set; }
        public double Telem { get; set; }
        private double[,] Gsr;
        //---
        ModelParams modParams;
        //------
        public MathModel(ModelParams modParams, double time, double[] vars)
        {
            this.modParams = modParams;
            this.size = modParams.Size;
            Msys = new double[size, size];
            Usys = new double[size];
            Rsys = new double[size, size];
            Gsys = new double[size, size];
            Gsr = new double[size - 2, size - 2];
            //---
            SetMsys(time, vars);
            SetGsys(time, vars);
            SetRsys(time, vars);
            SetUsys(time, vars);
        }
        //------
        public void SetModel(double time, double[] vars)
        {
            SetMsys(time, vars);
            SetGsys(time, vars);
            SetGsr(time, vars);
            SetRsys(time, vars);
            SetUsys(time, vars);
            //---
        }
        //------
        private void SetMsys(double time, double[] vars)
        {
            int p = modParams.PP;
            double fi = vars[size-1];
            //---
            Msys[0, 0] = modParams.Mss_int + modParams.Mss_sigma + modParams.Mas_ext;
            Msys[0, 1] = -0.5*modParams.Mss_int;
            Msys[0, 2] = -0.5*modParams.Mss_int;
            Msys[0, 3] = modParams.Msr * Math.Cos(p*fi);
            Msys[0, 4] = modParams.Msr * Math.Cos(p * fi + 2*Math.PI/3);
            Msys[0, 5] = modParams.Msr * Math.Cos(p * fi + 4 * Math.PI / 3);
            Msys[0, 6] = 0;
            Msys[0, 7] = 0;
            //---
            Msys[1, 0] = Msys[0,1];
            Msys[1, 1] = modParams.Mss_int + modParams.Mss_sigma + modParams.Mbs_ext;
            Msys[1, 2] = -0.5 * modParams.Mss_int;
            Msys[1, 3] = modParams.Msr * Math.Cos(p * fi - 2 * Math.PI / 3);
            Msys[1, 4] = modParams.Msr * Math.Cos(p * fi);
            Msys[1, 5] = modParams.Msr * Math.Cos(p * fi + 2 * Math.PI / 3);
            Msys[1, 6] = 0;
            Msys[1, 7] = 0;
            //---
            Msys[2, 0] = Msys[0,2];
            Msys[2, 1] = Msys[1,2];
            Msys[2, 2] = modParams.Mss_int + modParams.Mss_sigma + modParams.Mcs_ext;
            Msys[2, 3] = modParams.Msr * Math.Cos(p * fi - 4 * Math.PI / 3);
            Msys[2, 4] = modParams.Msr * Math.Cos(p * fi - 2 * Math.PI / 3);
            Msys[2, 5] = modParams.Msr * Math.Cos(p * fi - 0 * Math.PI / 3);
            Msys[2, 6] = 0;
            Msys[2, 7] = 0;
            //---
            Msys[3, 0] = Msys[0,3];
            Msys[3, 1] = Msys[1,3];
            Msys[3, 2] = Msys[2,3];
            Msys[3, 3] = modParams.Mrr_int + modParams.Mrr_sigma + modParams.Mar_ext;
            Msys[3, 4] = -0.5 * modParams.Mrr_int;
            Msys[3, 5] = -0.5 * modParams.Mrr_int;
            Msys[3, 6] = 0;
            Msys[3, 7] = 0;
            //---
            Msys[4, 0] = Msys[0, 4];
            Msys[4, 1] = Msys[1, 4];
            Msys[4, 2] = Msys[2, 4];
            Msys[4, 3] = Msys[3, 4];
            Msys[4, 4] = modParams.Mrr_int + modParams.Mrr_sigma + modParams.Mbr_ext;
            Msys[4, 5] = -0.5 * modParams.Mrr_int;
            Msys[4, 6] = 0;
            Msys[4, 7] = 0;
            //---
            Msys[5, 0] = Msys[0, 5];
            Msys[5, 1] = Msys[1, 5];
            Msys[5, 2] = Msys[2, 5];
            Msys[5, 3] = Msys[3, 5];
            Msys[5, 4] = Msys[4, 5];
            Msys[5, 5] = modParams.Mrr_int + modParams.Mrr_sigma + modParams.Mcr_ext;
            Msys[5, 6] = 0;
            Msys[5, 7] = 0;
            //---
            Msys[6, 0] = Msys[0, 6];
            Msys[6, 1] = Msys[1, 6];
            Msys[6, 2] = Msys[2, 6];
            Msys[6, 3] = Msys[3, 6];
            Msys[6, 4] = Msys[4, 6];
            Msys[6, 5] = Msys[5, 6];
            Msys[6, 6] = modParams.Jrint + modParams.Jrext;
            Msys[6, 7] = 0;
            //---
            Msys[7, 7] = 1;
        }
        //------
        private void SetUsys(double time, double[] vars)
        {
            SetTelem(time, vars);
            double omegas = 2 * Math.PI * modParams.Fs;
            double phase = modParams.Phase;
            //---
            Usys[0] = Math.Sqrt(2) * modParams.Uas_rms * Math.Cos(omegas * time + phase);
            Usys[1] = Math.Sqrt(2) * modParams.Ubs_rms * Math.Cos(omegas * time - 2 * Math.PI / 3 + phase);
            Usys[2] = Math.Sqrt(2) * modParams.Ucs_rms * Math.Cos(omegas * time - 4 * Math.PI / 3 + phase);
            Usys[3] = 0;
            Usys[4] = 0;
            Usys[5] = 0;
            //---
            if(time <= modParams.NoLoadTime)
            {
                Usys[6] = Telem;
            }
            else
            {
                if(time <= modParams.TimeToLoad)
                {
                    Usys[6] = modParams.Textern*(time - modParams.NoLoadTime) / (modParams.TimeToLoad - modParams.NoLoadTime) + Telem;
                }
                else
                {
                    Usys[6] = modParams.Textern + Telem;
                }
            }
            //---
            Usys[7] = vars[size - 2];
        }
        //------
        private void SetTelem(double time, double[] vars)
        {
            double[] array = {vars[0], vars[1], vars[2], vars[3], vars[4], vars[5]};
            //---
            double[] res = new double[size - 2];
            double sum = 0;
            for (int i = 0; i < size - 2; i++)
            {
                sum = 0;
                for (int j = 0; j < size - 2; j++)
                {
                    sum += Gsr[i, j] * array[j];
                }
                res[i] = sum;
            }
            //---
            sum = 0;
            for (int i = 0; i < size - 2; i++)
            {
                sum += array[i] * res[i];
            }
            Telem = 0.5*sum;
        }
        //------
        private void SetRsys(double time, double[] vars)
        {
            Rsys[0, 0] = modParams.Ras_int + modParams.Ras_ext;
            Rsys[1, 1] = modParams.Rbs_int + modParams.Rbs_ext;
            Rsys[2, 2] = modParams.Rcs_int + modParams.Rcs_ext;
            Rsys[3, 3] = modParams.Rar_int + modParams.Rar_ext;
            Rsys[4, 4] = modParams.Rbr_int + modParams.Rbr_ext;
            Rsys[5, 5] = modParams.Rcr_int + modParams.Rcr_ext;
            Rsys[6, 6] = modParams.Drint + modParams.Drext;
        }
        //------
        private void SetGsys(double time, double[] vars)
        {
            double fi = vars[size - 1];
            int p = modParams.PP;
            //---
            Gsys[0, 0] = 0;
            Gsys[0, 1] = 0;
            Gsys[0, 2] = 0;
            Gsys[0, 3] = -p*modParams.Msr * Math.Sin(p * fi);
            Gsys[0, 4] = -p*modParams.Msr * Math.Sin(p * fi + 2 * Math.PI / 3);
            Gsys[0, 5] = -p*modParams.Msr * Math.Sin(p * fi + 4 * Math.PI / 3);
            Gsys[0, 6] = 0;
            Gsys[0, 7] = 0;
            //---
            Gsys[1, 0] = 0;
            Gsys[1, 1] = 0;
            Gsys[1, 2] = 0;
            Gsys[1, 3] = -p*modParams.Msr * Math.Sin(p * fi - 2 * Math.PI / 3);
            Gsys[1, 4] = -p*modParams.Msr * Math.Sin(p * fi);
            Gsys[1, 5] = -p*modParams.Msr * Math.Sin(p * fi + 2 * Math.PI / 3);
            Gsys[1, 6] = 0;
            Gsys[1, 7] = 0;
            //---
            Gsys[2, 0] = 0;
            Gsys[2, 1] = 0;
            Gsys[2, 2] = 0;
            Gsys[2, 3] = -p*modParams.Msr * Math.Sin(p * fi - 4 * Math.PI / 3);
            Gsys[2, 4] = -p*modParams.Msr * Math.Sin(p * fi - 2 * Math.PI / 3);
            Gsys[2, 5] = -p*modParams.Msr * Math.Sin(p * fi - 0 * Math.PI / 3);
            Gsys[2, 6] = 0;
            Gsys[2, 7] = 0;
            //---
            Gsys[3, 0] = Gsys[0, 3];
            Gsys[3, 1] = Gsys[1, 3];
            Gsys[3, 2] = Gsys[2, 3];
            Gsys[3, 3] = 0;
            Gsys[3, 4] = 0;
            Gsys[3, 5] = 0;
            Gsys[3, 6] = 0;
            Gsys[3, 7] = 0;
            //---
            Gsys[4, 0] = Gsys[0, 4];
            Gsys[4, 1] = Gsys[1, 4];
            Gsys[4, 2] = Gsys[2, 4];
            Gsys[4, 3] = Gsys[3, 4];
            Gsys[4, 4] = 0;
            Gsys[4, 5] = 0;
            Gsys[4, 6] = 0;
            Gsys[4, 7] = 0;
            //---
            Gsys[5, 0] = Gsys[0, 5];
            Gsys[5, 1] = Gsys[1, 5];
            Gsys[5, 2] = Gsys[2, 5];
            Gsys[5, 3] = Gsys[3, 5];
            Gsys[5, 4] = Gsys[4, 5];
            Gsys[5, 5] = 0;
            Gsys[5, 6] = 0;
            Gsys[5, 7] = 0;
        }
        //---
        private void SetGsr(double time, double[] vars)
        {
            double fi = vars[size - 1];
            int p = modParams.PP;
            //---
            Gsr[0, 0] = Gsys[0, 0];
            Gsr[0, 1] = Gsys[0, 1];
            Gsr[0, 2] = Gsys[0, 2];
            Gsr[0, 3] = Gsys[0, 3];
            Gsr[0, 4] = Gsys[0, 4];
            Gsr[0, 5] = Gsys[0, 5];
            //---
            Gsr[1, 0] = Gsys[1, 0];
            Gsr[1, 1] = Gsys[1, 1];
            Gsr[1, 2] = Gsys[1, 2];
            Gsr[1, 3] = Gsys[1, 3];
            Gsr[1, 4] = Gsys[1, 4];
            Gsr[1, 5] = Gsys[1, 5];
            //---
            Gsr[2, 0] = Gsys[2, 0];
            Gsr[2, 1] = Gsys[2, 1];
            Gsr[2, 2] = Gsys[2, 2];
            Gsr[2, 3] = Gsys[2, 3];
            Gsr[2, 4] = Gsys[2, 4];
            Gsr[2, 5] = Gsys[2, 5];
            //---
            Gsr[3, 0] = Gsys[3, 0];
            Gsr[3, 1] = Gsys[3, 1];
            Gsr[3, 2] = Gsys[3, 2];
            Gsr[3, 3] = Gsys[3, 3];
            Gsr[3, 4] = Gsys[3, 4];
            Gsr[3, 5] = Gsys[3, 5];
            //---
            Gsr[4, 0] = Gsys[4, 0];
            Gsr[4, 1] = Gsys[4, 1];
            Gsr[4, 2] = Gsys[4, 2];
            Gsr[4, 3] = Gsys[4, 3];
            Gsr[4, 4] = Gsys[4, 4];
            Gsr[4, 5] = Gsys[4, 5];
            //---
            Gsr[5, 0] = Gsys[5, 0];
            Gsr[5, 1] = Gsys[5, 1];
            Gsr[5, 2] = Gsys[5, 2];
            Gsr[5, 3] = Gsys[5, 3];
            Gsr[5, 4] = Gsys[5, 4];
            Gsr[5, 5] = Gsys[5, 5];
        }
    }
}

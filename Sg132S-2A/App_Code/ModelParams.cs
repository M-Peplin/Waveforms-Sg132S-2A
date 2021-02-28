using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//---
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

namespace Sg132S_2A.App_Code
{
    public class ModelParams
    {
        //Parametry wewnetrzne maszyny
        public int PP { get; set; }
        public double Mss_int { get; set; }
        public double Mss_sigma { get; set; }
        public double Msr { get; set; }
        public double Mrr_int { get; set; }
        public double Mrr_sigma { get; set; }
        public double Jrint { get; set; }
        //---
        public double Ras_int { get; set; }
        public double Rbs_int { get; set; }
        public double Rcs_int { get; set; }
        public double Rar_int { get; set; }
        public double Rbr_int { get; set; }
        public double Rcr_int { get; set; }
        public double Drint { get; set; }
        //---
        //Parametry zewnetrzne obwodu elektromechnicznego
        public double Mas_ext { get; set; }
        public double Mbs_ext { get; set; }
        public double Mcs_ext { get; set; }
        public double Mar_ext { get; set; }
        public double Mbr_ext { get; set; }
        public double Mcr_ext { get; set; }
        public double Jrext { get; set; }
        //---
        public double Ras_ext { get; set; }
        public double Rbs_ext { get; set; }
        public double Rcs_ext { get; set; }
        public double Rar_ext { get; set; }
        public double Rbr_ext { get; set; }
        public double Rcr_ext { get; set; }
        public double Drext { get; set; }
        //Parametry czasu
        public double TimeStart { get; set; }
        public double TimeStop { get; set; }
        public int PtsResults { get; set; }
        public double NoLoadTime { get; set; }
        public double TimeToLoad { get; set; }
        //Parametry napiec zasilajacych i momentu zewnetrznego
        public double Fs { get; set; }
        public double Uas_rms { get; set; }
        public double Ubs_rms { get; set; }
        public double Ucs_rms { get; set; }
        public double Uar_rms { get; set; }
        public double Ubr_rms { get; set; }
        public double Ucr_rms { get; set; }
        public double Textern { get; set; }
        public double Phase { get; set; }
        //------
        private int size;
        public int Size { get { return size; } }
        public double[] InitCondit { get; set; }
        //---
        public ModelParams()
        {
            this.PP = 1;    //liczba par biegunów
            this.Fs = 50;
            //Inicjalizacja napiec i momentu zewnetrznego
            this.Phase = 0;
            this.Uas_rms = 400;
            this.Ubs_rms = 400;
            this.Ucs_rms = 400;
            this.Uar_rms = 0;
            this.Ubr_rms = 0;
            this.Ucr_rms = 0;
            this.Textern = 0;
            //Inicjalizacja parametrow wewnetrznych
            this.Mss_int = 1.04;
            this.Mss_sigma = 0.0052;
            this.Msr = 0.996*this.Mss_int;
            this.Mrr_int = Mss_int;
            this.Mrr_sigma = Mss_sigma;
            this.Jrint = 0.023;
            //---
            this.Ras_int = 2.8;
            this.Rbs_int = 2.8;
            this.Rcs_int = 2.8;
            this.Rar_int = 2.8;
            this.Rbr_int = 2.8;
            this.Rcr_int = 2.8;
            this.Drint = 0.027;
            //Inicjalizacja parametrow zewnetrznych
            this.Mas_ext = 0;
            this.Mbs_ext = 0;
            this.Mcs_ext = 0;
            this.Mar_ext = 0;
            this.Mbr_ext = 0;
            this.Mcr_ext = 0;
            this.Jrext = 0;
            this.Drext = 0;
            //Inicjalizacja parametrow czasu i liczby wynikow
            this.TimeStart = 0;
            this.TimeStop = 0.4;
            this.NoLoadTime = 0.3;
            this.TimeToLoad = 0.32;
            PtsResults = 4000;
            //Warunki poczatkowe
            this.size = 8;
            InitCondit = new double[size];
            InitCondit[0] = 0;      //ias
            InitCondit[1] = 0;      //ibs
            InitCondit[2] = 0;      //ics
            InitCondit[3] = 0;      //iar
            InitCondit[4] = 0;      //ibr
            InitCondit[5] = 0;      //icr
            InitCondit[6] = 0;      //omega
            InitCondit[7] = 0;      //angle
        }
    }
}

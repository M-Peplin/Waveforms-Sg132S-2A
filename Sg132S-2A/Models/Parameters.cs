using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sg132S_2A.App_Code;

namespace Sg132S_2A.Models
{
    public class Parameters
    {
        public int ID { get; set; }
        public double RatedPower { get; set; }
        public double RatedVoltage { get; set; }
        public double RatedCurrent { get; set; }
        public double RatedSpeed { get; set; }
        public double PowerFactor { get; set; }
        public double Frequency { get; set; }
        public string IPR { get; set; }
        // status PWM
        public string pwmState { get; set; }
        // parametry do obliczania przebiegow
        public double Fs { get; set; }
        public double Uu1 { get; set; }
        public double Uv1 { get; set; }
        public double Uw1 { get; set; }
        public double Phase { get; set; }
        public double Msu { get; set; }
        public double Msv { get; set; }
        public double Msw { get; set; }
        public double Rsu { get; set; }
        public double Rsv { get; set; }
        public double Rsw { get; set; }
        public double Jext { get; set; }
        public double Dext { get; set; }
        public double Nlt { get; set; }
        public double Ttl { get; set; }
        public double Tn { get; set; }
        public double Ti { get; set; }
        public double Ts { get; set; }
        public double Tsoftstart { get; set; }
        public double F0 { get; set; }
        public double Fk { get; set; }
        public double SlopeRatio { get; set; }

        public static Parameters DefaultEngineParams { get; }
        public static ModelParams defaultParams = new ModelParams();

        static Parameters()
        {
            DefaultEngineParams = new Parameters
            {
                ID = 0,
                RatedPower = 5500,
                RatedVoltage = 380,
                RatedCurrent = 10.9,
                RatedSpeed = 2920,
                PowerFactor = 0.88,
                Frequency = 50,
                IPR = "IP55",
                // parametry do obliczen
                Fs = defaultParams.Fs,
                Uu1 = defaultParams.Uas_rms,
                Uv1 = defaultParams.Ubs_rms,
                Uw1 = defaultParams.Ucs_rms,
                Phase = defaultParams.Phase,
                Msu = defaultParams.Mas_ext,
                Msv = defaultParams.Mbs_ext,
                Msw = defaultParams.Mcs_ext,
                Rsu = defaultParams.Ras_ext,
                Rsv = defaultParams.Rbs_ext,
                Rsw = defaultParams.Rcs_ext,
                Jext = defaultParams.Jrext,
                Dext = defaultParams.Drext,
                Nlt = defaultParams.NoLoadTime,
                Ttl = defaultParams.TimeToLoad,
                Tn = defaultParams.Textern,
                Ti = defaultParams.TimeStart,
                Ts = defaultParams.TimeStop,
                // PWM status
                pwmState = "On",
                // czas rozruchu
                Tsoftstart = 10,
                // czestotliwosc poczatkowa
                F0 = defaultParams.F0,
                Fk = defaultParams.Fk,
                SlopeRatio = defaultParams.SlopeRatio
            
            };
           
        }
    }

   
}
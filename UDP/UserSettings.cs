using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
    public class UserSettings
    {
        /*public string UserName = "";
        public string PassHashCode = "";

        public int HVProfile = 0;
        public int MicroscopyMode = 0;

        public int focus_course = 0;
        public int Focus = 0;
        public int dFocus = 0;
        
        public int Vf_WideField = 0;//Zoom
        public int Vf_Resolution = 0;//Zoom
        public int Vf_Field = 0;//Zoom
        public int Vf_Rokveld = 0;//Zoom
        public int dZoom = 0;

        public int Stig_x = 0;
        public int Stig_y = 0;
        public int dStig = 0;

        public int Gain_x = 0;
        public int Gain_y = 0;
        public int dGain = 0;

        public int ObjectCentering_x = 0;
        public int ObjectCentering_y = 0;
        public int dObjectCentering = 0;

        public int IMLCentering_x = 0;
        public int IMLCentering_y = 0;
        public int dIMLCentering = 0;
        
        public int GunShift_x = 0;
        public int GunShift_y = 0;
        public int dGunShift = 0;

        public int GunTilt_x = 0;
        public int GunTilt_y = 0;
        public int dGunTilt = 0;

        public int Heat = 0;
        public int HV = 0;

        public int PC = 0;
        public int Speed = 0;*/

        private string userName = "";
        private string passHashCode = "";

        private int hVProfile = 0;
        private int microscopyMode = 0;

        private int focus_course = 0;
        //private int focus = 0;
        private int focusfine_WideField = 0;//Focus
        private int focusfine_Resolution = 0;//Focus
        private int focusfine_Field = 0;//Focus
        private int focusfine_Rokveld = 0;//Focus
        private int dFocus = 1;

        private int vf_WideField = 0;//Zoom
        private int vf_Resolution = 0;//Zoom
        private int vf_Field = 0;//Zoom
        private int vf_Rokveld = 0;//Zoom
        private int dZoom = 1;

        private int stig_x = 0;
        private int stig_y = 0;
        private int dStig = 1;

        private int gain_x = 0;
        private int gain_y = 0;
        private int dGain = 1;

        private int objectCentering_x = 0;
        private int objectCentering_y = 0;
        private int dObjectCentering = 1;

        private int iMLCentering_x = 0;
        private int iMLCentering_y = 0;
        private int dIMLCentering = 1;

        private int gunShift_x = 0;
        private int gunShift_y = 0;
        private int dGunShift = 1;

        private int gunTilt_x = 0;
        private int gunTilt_y = 0;
        private int dGunTilt = 1;

        private int heat = 0;
        private int hV = 1;

        private int pc1 = 0;
        private int pc2 = 0;
        private double pc1coef = 2;
        private int pc2coef = 10;

        private int speed = 0;

        private double iobj = 0;
        private double iiML = 0;
        private double wd_real = 0;
        
        public string UserName { get => userName; set => userName = value; }
        public string PassHashCode { get => passHashCode; set => passHashCode = value; }
        public int HVProfile { get => hVProfile; set => hVProfile = verify(value,0,2); }
        public int MicroscopyMode { get => microscopyMode; set => microscopyMode = verify(value, 0, 2); }
        public int Focus_course { get => focus_course; set => focus_course = value; }
        //public int Focus { get => focus; set => focus = value; }
        public int FocusFine_WideField { get => focusfine_WideField; set => focusfine_WideField = value; }
        public int FocusFine_Resolution { get => focusfine_Resolution; set => focusfine_Resolution = value; }
        public int FocusFine_Field { get => focusfine_Field; set => focusfine_Field = value; }
        public int FocusFine_Rokveld { get => focusfine_Rokveld; set => focusfine_Rokveld = value; }
        public int DFocus { get => dFocus; set => dFocus = verify(value, 1, 10); }
        public int Vf_WideField { get => vf_WideField; set => vf_WideField = value; }
        public int Vf_Resolution { get => vf_Resolution; set => vf_Resolution = value; }
        public int Vf_Field { get => vf_Field; set => vf_Field = value; }
        public int Vf_Rokveld { get => vf_Rokveld; set => vf_Rokveld = value; }
        public int DZoom { get => dZoom; set => dZoom = verify(value, 1, 10); }
        public int Stig_x { get => stig_x; set => stig_x = value; }
        public int Stig_y { get => stig_y; set => stig_y = value; }
        public int DStig { get => dStig; set => dStig = verify(value, 1, 10); }
        public int Gain_x { get => gain_x; set => gain_x = value; }
        public int Gain_y { get => gain_y; set => gain_y = value; }
        public int DGain { get => dGain; set => dGain = verify(value, 1, 10); }
        public int ObjectCentering_x { get => objectCentering_x; set => objectCentering_x = value; }
        public int ObjectCentering_y { get => objectCentering_y; set => objectCentering_y = value; }
        public int DObjectCentering { get => dObjectCentering; set => dObjectCentering = verify(value, 1, 10); }
        public int IMLCentering_x { get => iMLCentering_x; set => iMLCentering_x = value; }
        public int IMLCentering_y { get => iMLCentering_y; set => iMLCentering_y = value; }
        public int DIMLCentering { get => dIMLCentering; set => dIMLCentering = verify(value, 1, 10); }
        public int GunShift_x { get => gunShift_x; set => gunShift_x = value; }
        public int GunShift_y { get => gunShift_y; set => gunShift_y = value; }
        public int DGunShift { get => dGunShift; set => dGunShift = verify(value, 1, 10); }
        public int GunTilt_x { get => gunTilt_x; set => gunTilt_x = value; }
        public int GunTilt_y { get => gunTilt_y; set => gunTilt_y = value; }
        public int DGunTilt { get => dGunTilt; set => dGunTilt = verify(value, 1, 10); }
        public int Heat { get => heat; set => heat = verify(value, 0, 100); }
        public int HV { get => hV; set => hV = verify(value, 1, 30); }
        public int PC1 { get => pc1; set => pc1 = verify(value, 0, 4095); }
        public int PC2 { get => pc2; set => pc2 = verify(value, 0, 4095); }
        public double PC1Coef { get => pc1coef; set => pc1coef = verify(value, 2, 4); }
        public int PC2Coef { get => pc2coef; set => pc2coef = verify(value, 10, 20); }
        public int Speed { get => speed; set => speed = verify(value, 0, 7); }
        public double IObj { get => iobj; set => iobj = value; }
        public double IIML { get => iiML; set => iiML = value; }
        public double WD_real { get => wd_real; set => wd_real = value; }

        private int verify(int value, int min, int max)
        {
            if (value < min) return min;
            else if (value > max) return max;
            else return value;
        }

        private double verify(double value, double min, double max)
        {
            if (value < min) return min;
            else if (value > max) return max;
            else return value;
        }
    }
}

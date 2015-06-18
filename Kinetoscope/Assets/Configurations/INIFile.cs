using System;
using System.Runtime.InteropServices;
using System.Text;

namespace INI
{
    /// <summary>
    /// Create a New INI file to store or load data
    /// </summary>
    public class IniFile
    {
        public string path;

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section,
            string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section,
                 string key, string def, StringBuilder retVal,
            int size, string filePath);

        /// <summary>
        /// INIFile Constructor.
        /// </summary>
        /// <PARAM name="INIPath"></PARAM>
        public IniFile(string INIPath)
        {
            path = INIPath;
        }
        /// <summary>
        /// Write Data to the INI File
        /// </summary>
        /// <PARAM name="Section"></PARAM>
        /// Section name
        /// <PARAM name="Key"></PARAM>
        /// Key Name
        /// <PARAM name="Value"></PARAM>
        /// Value Name
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.path);
        }

        /// <summary>
        /// Read Data Value From the Ini File
        /// </summary>
        /// <PARAM name="Section"></PARAM>
        /// <PARAM name="Key"></PARAM>
        /// <PARAM name="Path"></PARAM>
        /// <returns></returns>
        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            GetPrivateProfileString(Section, Key, "", temp,
                                            255, this.path);
            return temp.ToString();

        }
    }

	public class INIValues
	{
		private INIValues(string value) { Value = value; }
		
		public string Value { get; set; }
		
		public static INIValues ScreenWidth { get { return new INIValues("screenwidth"); } }
		public static INIValues ScreenHeight { get { return new INIValues("screenheight"); } }
		public static INIValues ScreenCenterX { get { return new INIValues("screencenterx"); } }
		public static INIValues ScreenCenterY { get { return new INIValues("screencentery"); } }
		public static INIValues ScreenCenterZ { get { return new INIValues("screencenterz"); } }
		
		//observator
		public static INIValues ObservatorX { get { return new INIValues("observatorx"); } }
		public static INIValues ObservatorY { get { return new INIValues("observatory"); } }
		public static INIValues ObservatorZ { get { return new INIValues("observatorz"); } }

		//network
		public static INIValues IsNetworkEnabled { get { return new INIValues("isnetworkenabled"); } }
		public static INIValues IpAddress { get { return new INIValues("ipaddress"); } }
		public static INIValues Port { get { return new INIValues("port"); } }
		public static INIValues IsServer { get { return new INIValues("isserver"); } }
	}
}
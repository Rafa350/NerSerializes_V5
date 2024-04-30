namespace NetSerializer.v4.Storage.Xml {

    using System.Globalization;
    using System.Text;

    public sealed class XmlStorageWriterSettings {
     
        public CultureInfo Culture { get; set; }
        public Encoding Encoding { get; set; }
        public bool UseNames { get; set; }
        public bool UseTypes { get; set; }
        public int Indentation { get; set; }
        public bool EncodedStrings { get; set; }

        public XmlStorageWriterSettings() {

            UseNames = true;
            UseTypes = false;
            Culture = CultureInfo.InvariantCulture;
            Encoding = Encoding.UTF8;
            Indentation = 4;
            EncodedStrings = true;
        }
    }
}

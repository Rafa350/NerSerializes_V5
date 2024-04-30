namespace NetSerializer.v4.Storage.Xml {

    using System;
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Xsl;

    public sealed class XmlReaderPreprocessor {

        private readonly XslCompiledTransform transform;

        public XmlReaderPreprocessor(Stream template) {

            if (template == null)
                throw new ArgumentNullException("template");

            // Carrega la transformacio
            //
            transform = new XslCompiledTransform();
            transform.Load(XmlReader.Create(template));
        }

        public void Process(Stream input, Stream output, bool closeOutput) {

            if (input == null)
                throw new ArgumentNullException("input");

            if (output == null)
                throw new ArgumentNullException("output");

            XmlReaderSettings rdSettings = new XmlReaderSettings();
            rdSettings.IgnoreWhitespace = true;
            rdSettings.IgnoreComments = true;
            rdSettings.IgnoreProcessingInstructions = true;

            using (XmlReader reader = XmlReader.Create(input, rdSettings)) {

                XmlWriterSettings wrSettings = new XmlWriterSettings();
                wrSettings.Encoding = Encoding.UTF8;
                wrSettings.IndentChars = "    ";
                wrSettings.Indent = true;
                wrSettings.ConformanceLevel = ConformanceLevel.Document;
                wrSettings.CloseOutput = closeOutput;

                using (XmlWriter writer = XmlWriter.Create(output, wrSettings)) {

                    // Realitza la transformacio
                    //
                    transform.Transform(reader, null, writer);
                }
            }
        }
    }
}

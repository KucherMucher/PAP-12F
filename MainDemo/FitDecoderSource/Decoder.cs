using System.IO;
using System.Collections.Generic;


using Dynastream.Fit;

namespace FitDecoderSource
{
    public class Decoder
    {
        public FitMessages FitMessages => _fitListener.FitMessages; // get messages from fit file

        public HashSet<string> RecordFieldNames = new HashSet<string>();
        public HashSet<string> RecordDeveloperFieldNames = new HashSet<string>();

        private FitListener _fitListener = new FitListener(); 
        private Stream inputStream; // file input
        private Dynastream.Fit.File fileType; // file type

        public Decoder(Stream stream, Dynastream.Fit.File fileType)
        {
            inputStream = stream;
            this.fileType = fileType;
        }

        public bool Decode()
        {

            Decode decoder = new Decode();

            if (!decoder.IsFIT(inputStream))
            {
                throw new FileTypeException($".fit file expected: {fileType}, not a .fit file :shrugg:");
            }

            MesgBroadcaster mesgBroadcaster = new MesgBroadcaster();

            decoder.MesgEvent += mesgBroadcaster.OnMesg;

            mesgBroadcaster.FileIdMesgEvent += OnFileIdMesg;
            mesgBroadcaster.RecordMesgEvent += OnRecordMesg;

            decoder.MesgEvent += _fitListener.OnMesg;

            try
            {
                bool readOk = decoder.Read(inputStream);

                if (readOk && FitMessages.HrMesgs.Count > 0)
                {
                    HrToRecordMesg.MergeHeartRates(FitMessages);
                    RecordFieldNames.Add("Heart Rate");
                }

                return readOk;

            }
            catch (FileTypeException ex)
            {
                throw (ex);
            }
            catch (FitException ex)
            {
                throw (ex);
            }
            catch (System.Exception ex)
            {
                throw (ex);
            }
            finally
            {
            }
        }

        public void OnFileIdMesg(object sender, MesgEventArgs e)
        {
            if ((e.mesg as FileIdMesg).GetType() != fileType)
            {
                throw new FileTypeException($".fit file expected: {fileType}, recieved File Type: {(e.mesg as FileIdMesg).GetType()}");
            }
        }

        public void OnRecordMesg(object sender, MesgEventArgs e)
        {
            foreach (Field field in e.mesg.Fields)
            {
                if (field.Name.ToLower() != "unknown")
                {
                    RecordFieldNames.Add(field.Name);
                }
            }

            foreach (DeveloperField devField in e.mesg.DeveloperFields)
            {
                RecordFieldNames.Add(devField.Name);
            }
        }
        /*
         The diference between normal message and dev message:
            normal message is a default message for the .fit file (ex: heartbeat, km, speed, etc.)
            dev message is a custom message added by the manufacturer of the .fit sender (ex: i dunno)
         */
    }
}

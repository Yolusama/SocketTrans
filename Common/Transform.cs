namespace Common
{
    public class TransObj
    {
        public int Port { get; set; }
    }

    public class MsgTransObj: TransObj 
    { 
        public string Message { get; set; }
    }

    public class FileTransObj : TransObj
    {
        public string FileName { get; set; }
        public byte[] Data { get; set; }
    }
}
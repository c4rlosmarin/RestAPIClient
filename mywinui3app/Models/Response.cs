using System.Collections.ObjectModel;

namespace mywinui3app.Models
{
    public class ResponseModel
    {
        public string Body { get; set; }
        public ObservableCollection<ResponseData> Headers { get; set; }
    }

    public class ResponseData
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}

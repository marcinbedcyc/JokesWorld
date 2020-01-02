using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace RestServiceLibrary
{
    [ServiceContract]
    public interface IRestServiceLibrary
    {
      
        [OperationContract]
        [WebInvoke(Method = "GET", 
            ResponseFormat = WebMessageFormat.Json, 
            BodyStyle = WebMessageBodyStyle.Wrapped, 
            UriTemplate = "json/{id}")]
        List<Product> GetProductDetails(string id);
    }

    public class Product
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCost { get; set; }
    }
}
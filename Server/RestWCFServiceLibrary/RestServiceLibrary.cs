using System.Collections.Generic;

namespace RestServiceLibrary
{
    public class RestServiceLibrary : IRestServiceLibrary
    {
        public List<Product> GetProductDetails(string productId)
        {
            Product objProduct = new Product();
            List<Product> objProductData = new List<Product>();
            objProduct.ProductId = productId;
            objProduct.ProductName = "Laptop";
            objProduct.ProductCost = "1000 $";
            objProductData.Add(objProduct);
            return objProductData;
        }
    }
}
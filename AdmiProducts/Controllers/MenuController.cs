using AdmiProducts.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdmiProducts.Controllers
{
    public class MenuController
    {

        private readonly ProductService _productService;

        public MenuController(ProductService service)
        {
            _productService = service;
        }



        public void Start()
        {
            try
            {
                var productos = _productService.getAllProducts();

                foreach (var product in productos)
                {
                    Console.WriteLine($"{product.ProductId} | {product.Description} | {product.EstatusId}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }



    }
}

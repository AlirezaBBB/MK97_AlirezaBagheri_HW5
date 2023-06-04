﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WarehouseOperations.Domain;
using WarehouseOperations.Interface;
using WarehouseOperations.Util;
using static System.Reflection.Metadata.BlobBuilder;

namespace WarehouseOperations.Services
{
    public class ProductRepository : IProductRepository
    {
        private List<Product> products;
        private string productRelativePath;

        public ProductRepository()
        {
            string? Directorypath = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.FullName;
            string dataBaseFolderPath = Path.Combine(Directorypath, "DataBase");
            productRelativePath = Path.Combine(dataBaseFolderPath, "ProductJson.json");

            products = ProductJsonRead();
        }

        public string AddProduct(Product product)
        {
            int newProductId = products.Count > 0 ? products.Max(u => u.ProductId) + 1 : 1;

            if (CheckProductName(product.Name) == false)
            {
                var newProduct = new Product(1, product.Name, product.Barcode);
                products.Add(newProduct);
                ProductJsonWrite(products);
                return "Product added to DB";
            }
            else
            {
                return "Couldn't add product to DB because name you entered is not match our limits";
            }
        }

        public string GetProductById(int id)
        {
            throw new NotImplementedException();
        }

        public List<Product> GetProductList()
        {
            throw new NotImplementedException();
        }

        public bool CheckProductName(string productName)
        {
            string nameCheck = "^[a-z]{1}[a-z_]{3}[0-9]{3}$";
            bool isValid = Regex.IsMatch(productName, nameCheck);
            return isValid;
        }

        private List<Product> ProductJsonRead()
        {
            string productJson = File.ReadAllText(productRelativePath);
            var products = JsonConvert.DeserializeObject<List<Product>>(productJson);
            return products ?? new List<Product>();
        }

        private void ProductJsonWrite(List<Product> products)
        {
            string jsonConvert = JsonConvert.SerializeObject(products);
            File.WriteAllText(productRelativePath, jsonConvert);
        }
    }
}

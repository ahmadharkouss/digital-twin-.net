

using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Satisfactory
{
    
    internal class Products
    {
        public List<(Product, int, int)> AllProducts {get; set;}
        public Products()
        {
            AllProducts = new List<(Product, int, int)>();
        }

        /// <summary>
        /// Adds a "Product" inside the all_products list.<br />
        /// does not check if id is unique!
        /// </summary>
        /// <param name="product">Product to add to list</param>
        /// <param name="id">numeric id of the product(not the name id)</param>
        /// <param name="quantity">numerical quantity to be produce by the assembly line</param>
        public void AddNewProductQt(Product product, int id, int quantity)
        {
            AllProducts.Add((product, id, quantity));
        }


        /// <summary>
        /// takes all_product tuple list and generate corresponding list of individual products. <br />
        /// Products are clone from the stored product and are cloned in order. <br />
        /// Exemple: <br />
        /// if you have 2 product A and 3 product B in this order in all_product, you will get: <br />
        /// AABBB in the resulting list
        /// </summary>
        /// <returns>the generated List<Product>. </returns>
        public List<Product> GenerateProductionList()
        {
            var ext = new List<Product>();
            foreach ((Product, int, int) item in AllProducts)
            {
                for (var i = 0; i < item.Item3; i++)
                {
                    ext.Add((Product)item.Item1.Clone());
                }
            }
            return ext;
        }

        /// <summary>
        /// Simple to_string() method. <br />
        /// Will iterate over each element in all_products list and for each one.<br />
        /// Print its product, its Id and its quantity with a separator between each.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("Products = {\n");
            foreach ((Product, int, int) item in AllProducts)
            {
                sb.Append($"{item.Item1.ToString()}\n");
                sb.Append($"Product number Id: {item.Item2}\n");
                sb.Append($"Product ordered quantity: {item.Item3}\n");
                sb.Append("===================================================\n");
            };
            sb.Append("}");
            return sb.ToString();

        }
    }
}

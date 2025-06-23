namespace Satisfactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

internal class Input
{

    /// <summary>
    /// Parse xml string to create a Products object.
    /// </summary>
    /// <param name="path">Path to the XML file to be parsed</param>
    /// <param name="types">List of all existing type given by the types.xml file</param>
    /// <returns>
    /// a "Products" object filled the quantities of "Product" requiered<br />
    /// to be manufactured.
    /// </returns>
    public static Products ParseInput(string path, IEnumerable<Type> types)
    {
        // check for invalid path
        if (path == null) throw new ArgumentNullException(nameof(path));

        var id = 1;
        var l_xmlDoc = XDocument.Load(path);
        Products products = new Products();


        foreach (var element in l_xmlDoc.Descendants("list").Elements())
        {
            // get element from parsed string
            string productName = element.Name.LocalName;
            int quantity = int.Parse(element.Attribute("qt").Value);
            Type type = types.FirstOrDefault(x => x.GetId().Equals(productName));

            // create product
            Product product = new Product(productName, type.GetName(), type);
            // add product to list of products
            products.AddNewProductQt(product, id, quantity);
            
            id++;
        }
        return products;
    }
}
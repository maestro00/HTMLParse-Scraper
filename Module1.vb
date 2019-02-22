Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net.Http
Imports System.Text
Imports System.Threading.Tasks
Imports System.Text.RegularExpressions

Public Module Module1
    Public Sub Main()
        GetHtmlAsync()
        Console.ReadLine()
    End Sub

    Private Async Sub GetHtmlAsync()
        Dim url = "https://www.ebay.com/sch/i.html?_nkw=xbox+one&_in_kw=1&_ex_kw=&_sacat=0&_udlo=&_udhi=&_ftrt=901&_ftrv=1&_sabdlo=&_sabdhi=&_samilow=&_samihi=&_sadis=15&_stpos=&_sargn=-1%26saslc%3D1&_salic=1&_sop=12&_dmd=1&_ipg=50&_fosrp=1"
        Dim httpClient = New HttpClient()
        Dim html = Await httpClient.GetStringAsync(url)
        Dim Hnode As HtmlAgilityPack.HtmlNode = Nothing
        Dim htmlDocument As HtmlAgilityPack.HtmlDocument = New HtmlAgilityPack.HtmlDocument()
        htmlDocument.LoadHtml(html)

        Dim ProductsHtml = htmlDocument.DocumentNode.Descendants("ul").
            Where(Function(node) node.GetAttributeValue("id", "").
            Equals("ListViewInner")).ToList()

        Dim ProductListItems = ProductsHtml(0).Descendants("li").
            Where(Function(node) node.GetAttributeValue("id", "").
            Contains("item")).ToList()

        For Each ProductListItem In ProductListItems
            'Id
            Console.WriteLine(ProductListItem.GetAttributeValue("listingid", ""))
            'ProductName
            Console.WriteLine(ProductListItem.Descendants("h3").
            Where(Function(node) node.GetAttributeValue("class", "").
            Equals("lvtitle")).FirstOrDefault().InnerText.Replace("\'", "").Replace("\""", "").Replace("\/", "/"))
            'Price
            Console.WriteLine(
                Regex.Match(
                ProductListItem.Descendants("li").
            Where(Function(node) node.GetAttributeValue("class", "").
            Equals("lvprice prc")).FirstOrDefault().InnerText.Replace("\'", "").Replace("\""", "").Replace("\/", "/"), "\d+.\d+"))

            'ListingType lvformat
            Console.WriteLine(
                ProductListItem.Descendants("li").
            Where(Function(node) node.GetAttributeValue("class", "").
            Equals("lvformat")).FirstOrDefault().InnerText.Replace("\'", "").Replace("\""", "").Replace("\/", "/"))

            'URL
            Console.WriteLine(
            ProductListItem.Descendants("a").FirstOrDefault().GetAttributeValue("href", "").Replace("\'", "").Replace("\""", "").Replace("\/", "/"))

        Next
        Console.WriteLine()
    End Sub
End Module
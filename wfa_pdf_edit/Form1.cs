using IronBarCode;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Rectangle = iTextSharp.text.Rectangle;

namespace wfa_pdf_edit
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {



            /*
            Courier-Bold
            Courier-Oblique
            Courier-BoldOblique
            Helvetica
            Helvetica-Bold
            Helvetica-Oblique
            Helvetica-BoldOblique
            Times-Roman
            Times-Bold
            Times-Italic
            Times-BoldItalic    
            *//*
            WHITE
            BLUE
            CYAN
            MAGENTA
            GREEN
            ORANGE
            YELLOW
            RED
            BLACK
            DARK_GRAY
            GRAY
            LIGHT_GRAY
            PINK
            */


            List<OrdenDeCompra> Ordenes = new List<OrdenDeCompra>();

            OrdenDeCompra OrdenOne = new OrdenDeCompra();
            OrdenOne.OrdenId = "13454355";
            OrdenOne.Company = "COMPAÑIA: NIKE DE MEXICO S. DE R.L. DE C.V.";
            OrdenOne.Address = "DIRECCION: IGNACIO ZARAGOZA No. 1385";
            OrdenOne.Colonia = "COLONIA: TEPALCATES I";
            OrdenOne.CodigoPostal = "C.P.: 9210";
            Ordenes.Add(OrdenOne);


            OrdenDeCompra OrdenTwo = new OrdenDeCompra();
            OrdenTwo.OrdenId = "13454356";
            OrdenTwo.Company = "COMPAÑIA: REEBOK DE MEXICO S.A. DE C.V.";
            OrdenTwo.Address = "DIRECCION: BOULEVARD ADOLFO LOPES MATEOS N° 2349 PISO 4";
            OrdenTwo.Colonia = "COLONIA: ATLAMAYA";
            OrdenTwo.CodigoPostal = "C.P.: 1760";
            Ordenes.Add(OrdenTwo);

            OrdenDeCompra OrdenThree = new OrdenDeCompra();
            OrdenThree.OrdenId = "13454357";//
            OrdenThree.Company = "COMPAÑIA: ADIDAS INDUSTRIAL- S.A. DE C.V.";
            OrdenThree.Address = "DIRECCION: A. RUIZ CORTINES No. 3642";
            OrdenThree.Colonia = "COLONIA: JARDINES DEL PEDREGAL";
            OrdenThree.CodigoPostal = "C.P.: 1900";
            Ordenes.Add(OrdenThree);

            foreach (OrdenDeCompra orden in Ordenes)
            {
                CreateDocument(orden);
            }

        }

        class OrdenDeCompra
        {
            public string OrdenId { get; set; }
            public string Company { get; set; }
            public string Address { get; set; }
            public string Colonia { get; set; }
            public string CodigoPostal { get; set; }
            
        }
        private void CreateDocument(OrdenDeCompra orden)
        {

            ModelPDFModified Model = new ModelPDFModified();

            Model.RootPath = @"c:\PdfDatos\";

            Model.FileNameOrigin = @"template.pdf";

            Model.FileNameModify = @"modificado_" + orden.OrdenId + ".pdf";

            Font Fuente = new Font("Courier-Bold", "BLUE", 8);

            Model.Font = Fuente;

            Model.Hojas = new List<Hoja>();


            Hoja oHojaOne = new Hoja();

            oHojaOne.Id = 1;

            oHojaOne.Descricion = "1 de 4";

            var Parametros = new List<Parametro>();

            var parametro_01 = new Parametro();
            parametro_01.Tipo = Parametro.TypeOf.TEXTO;
            parametro_01.Texto = orden.Company;
            parametro_01.Posicion = new Posicion(220, 690);
            Parametros.Add(parametro_01);

            var parametro_02 = new Parametro();
            parametro_02.Tipo = Parametro.TypeOf.TEXTO;
            parametro_02.Texto = orden.Address;
            parametro_02.Posicion = new Posicion(220, 670);
            Parametros.Add(parametro_02);

            var parametro_03 = new Parametro();
            parametro_03.Tipo = Parametro.TypeOf.TEXTO;
            parametro_03.Texto = orden.Colonia;
            parametro_03.Posicion = new Posicion(220, 650);
            Parametros.Add(parametro_03);

            var parametro_04 = new Parametro();
            parametro_04.Tipo = Parametro.TypeOf.BARCODE;
            parametro_04.Texto = orden.OrdenId;
            parametro_04.Properties.Add(new Property("URL", ModelPDFModified.ObtenerNombreBarcode(parametro_04.Texto)));
            parametro_04.Posicion = new Posicion(400, 0);
            Parametros.Add(parametro_04);

            var parametro_05 = new Parametro();
            parametro_05.Tipo = Parametro.TypeOf.TEXTO;
            parametro_05.Texto = orden.CodigoPostal;
            parametro_05.Posicion = new Posicion(220, 630);
            Parametros.Add(parametro_05);

            var parametro_06 = new Parametro();
            parametro_06.Tipo = Parametro.TypeOf.TEXTO;
            parametro_06.Texto = orden.OrdenId;
            parametro_06.Posicion = new Posicion(220, 610);
            Parametros.Add(parametro_06);

            oHojaOne.Propiedades = Parametros;

            Model.Hojas.Add(oHojaOne);


            Hoja oHojaTwo = new Hoja();

            oHojaTwo.Id = 2;

            oHojaTwo.Descricion = "2 de 4";

            oHojaTwo.Propiedades = ObtenerParametros();

            Model.Hojas.Add(oHojaTwo);


            Hoja oHojaThree = new Hoja();

            oHojaThree.Id = 3;

            oHojaThree.Descricion = "3 de 4";

            oHojaThree.Propiedades = ObtenerParametros();

            Model.Hojas.Add(oHojaThree);



            List<Parametro> ParametrosTwo = new List<Parametro>();

            ParametrosTwo = Model.Hojas.Where(h => h.Id == 1).First().Propiedades;



            CreateBarcodes(ParametrosTwo.Where(p => p.Tipo == Parametro.TypeOf.BARCODE).ToList(), Model.RootPath);

            ModifyPDF(Model);
        }
        private void CreateBarcodes(List<Parametro> Parametros, string RootPath)
        {
            foreach (Parametro parametro in Parametros)
            {
                CreateBarcode(RootPath, parametro.Texto);
            }
        }
        private void CreateBarcode(string RootPath, string Barcode)
        {
            GeneratedBarcode MyBarCode = IronBarCode.BarcodeWriter.CreateBarcode(Barcode, BarcodeWriterEncoding.Code128);
            MyBarCode.ResizeTo(300, 100); 
            MyBarCode.SaveAsPng(RootPath + ModelPDFModified.ObtenerNombreBarcode(Barcode));
        }
        private BaseColor ObtenerColor(string name)
        {
            BaseColor color = BaseColor.BLACK;
            switch (name)
            {
                case "WHITE":
                    color = BaseColor.WHITE;
                    break;
                case "BLUE":
                    color = BaseColor.BLUE;
                    break;
                case "CYAN":
                    color = BaseColor.CYAN;
                    break;
                case "MAGENTA":
                    color = BaseColor.MAGENTA;
                    break;
                case "GREEN":
                    color = BaseColor.GREEN;
                    break;
                case "ORANGE":
                    color = BaseColor.ORANGE;
                    break;
                case "YELLOW":
                    color = BaseColor.YELLOW;
                    break;
                case "RED":
                    color = BaseColor.RED;
                    break;
                case "BLACK":
                    color = BaseColor.BLACK;
                    break;
                case "DARK_GRAY":
                    color = BaseColor.DARK_GRAY;
                    break;
                case "GRAY":
                    color = BaseColor.GRAY;
                    break;
                case "LIGHT_GRAY":
                    color = BaseColor.LIGHT_GRAY;
                    break;
                case "PINK":
                    color = BaseColor.PINK;
                    break;
                default:
                    color = BaseColor.BLACK;
                    break;
            }
            
            return color;
    }
        private void ModifyPDF(ModelPDFModified Model)
        {
            //rutas de nuestros pdf
            string pathPDF = Model.RootPath + Model.FileNameOrigin;//@"C:\original.pdf";
            string pathPDF2 = Model.RootPath + Model.FileNameModify;//@"C:\modificado.pdf";

            //Objeto para leer el pdf original
            PdfReader oReader = new PdfReader(pathPDF);
            //Objeto que tiene el tamaño de nuestro documento
            Rectangle oSize = oReader.GetPageSizeWithRotation(1);
            //documento de itextsharp para realizar el trabajo asignandole el tamaño del original
            Document oDocument = new Document(oSize);
            // Creamos el objeto en el cual haremos la inserción
            FileStream oFS = new FileStream(pathPDF2, FileMode.Create, FileAccess.Write);
            PdfWriter oWriter = PdfWriter.GetInstance(oDocument, oFS);
            

            try
            {


                oDocument.Open();

                //El contenido del pdf, aqui se hace la escritura del contenido
                PdfContentByte oPDF = oWriter.DirectContent;


                foreach (Hoja oHoja in Model.Hojas)
                {
                    oDocument.NewPage();
                    //crea una nueva pagina y agrega el pdf original
                    PdfImportedPage page = oWriter.GetImportedPage(oReader, oHoja.Id);
                    oPDF.AddTemplate(page, 0, 0);

                    //Propiedades de nuestra fuente a insertar
                    BaseFont bf = BaseFont.CreateFont(Model.Font.Family, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    oPDF.SetColorFill(ObtenerColor(Model.Font.Color));
                    oPDF.SetFontAndSize(bf, Model.Font.Size);

                    //Se abre el flujo para escribir el texto
                    oPDF.BeginText();

                    //asignamos el texto

                    List<Parametro> Parametros = oHoja.Propiedades;

                    //Parametros.Where(p => p.Tipo == Parametro.TypeOf.TEXTO);

                    foreach (Parametro parametro in Parametros.Where(p => p.Tipo == Parametro.TypeOf.TEXTO).ToList())
                    {
                        oPDF.ShowTextAligned(Element.ALIGN_LEFT, parametro.Texto, parametro.Posicion.X, parametro.Posicion.Y, 0);
                    }

                    oPDF.EndText();

                    ///Parametros.Where(p => p.Tipo == Parametro.TypeOf.BARCODE);

                    List<Parametro> Barcodes = Parametros.Where(p => p.Tipo == Parametro.TypeOf.BARCODE).ToList();

                    foreach (Parametro parametro in Barcodes)
                    {
                        List<Property> Propiedades = new List<Property>();

                        Propiedades = parametro.Properties;


                        Property Propiedad = Propiedades.Where(pro => pro.Key == "URL").First();

                        string imageURL = Model.RootPath + Propiedad.Value;

                        iTextSharp.text.Image oPNG = iTextSharp.text.Image.GetInstance(imageURL);

                        //Resize image depend upon your need
                        oPNG.ScaleToFit(140f, 120f);
                        //Give space before image
                        oPNG.SpacingBefore = 10f;
                        //Give some space after the image
                        oPNG.SpacingAfter = 1f;

                        oPNG.Alignment = Element.ALIGN_LEFT;

                        oPNG.SetAbsolutePosition(parametro.Posicion.X, parametro.Posicion.X);

                        oPDF.AddImage(oPNG);
                    }
                }

                /*


                oDocument.NewPage();

                //crea una nueva pagina y agrega el pdf original
                PdfImportedPage page_two = oWriter.GetImportedPage(oReader, 2);
                oPDF.AddTemplate(page_two, 0, 0);




                //Propiedades de nuestra fuente a insertar
                BaseFont bff = BaseFont.CreateFont(Model.Font.Family, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                oPDF.SetColorFill(ObtenerColor(Model.Font.Color));
                oPDF.SetFontAndSize(bff, Model.Font.Size);

                //Se abre el flujo para escribir el texto
                oPDF.BeginText();

                foreach (Parametro parametro in Parametros.Where(p => p.Tipo == Parametro.TypeOf.TEXTO).ToList())
                {
                    oPDF.ShowTextAligned(1, parametro.Texto, parametro.Posicion.X, parametro.Posicion.Y, 0);
                }

                oPDF.EndText();

                oDocument.NewPage();

                //crea una nueva pagina y agrega el pdf original
                PdfImportedPage page_three = oWriter.GetImportedPage(oReader, 3);
                oPDF.AddTemplate(page_three, 0, 0);

                oDocument.NewPage();

                //crea una nueva pagina y agrega el pdf original
                PdfImportedPage page_four = oWriter.GetImportedPage(oReader, 4);
                oPDF.AddTemplate(page_four, 0, 0);
                */
                // Cerramos los objetos utilizados
                //oDocument.Close();
                //oFS.Close();
                //oWriter.Close();
                //oReader.Close();
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Exception: " + ex.Message);
                ModelPDFModified.Log(Model.RootPath, ex.Message);
            }
            finally
            {
                oDocument.Close();
                oFS.Close();
                oWriter.Close();
                oReader.Close();
            }
        }
        private static List<Parametro> ObtenerParametros()
        {
            var Parametros = new List<Parametro>();
            
            var parametro_01 = new Parametro();
            parametro_01.Tipo = Parametro.TypeOf.TEXTO;
            parametro_01.Texto = "First Name: Eduardo";
            parametro_01.Posicion = new Posicion(100, 690);
            Parametros.Add(parametro_01);

            var parametro_02 = new Parametro();
            parametro_02.Tipo = Parametro.TypeOf.TEXTO;
            parametro_02.Texto = "Last Name: Park";
            parametro_02.Posicion = new Posicion(100, 680);
            Parametros.Add(parametro_02);

            var parametro_03 = new Parametro();
            parametro_03.Tipo = Parametro.TypeOf.TEXTO;
            parametro_03.Texto = "Address: 1600 Amphitheatre Parkway Mountain View, California, Estados Unidos.";
            parametro_03.Posicion = new Posicion(400, 690);
            Parametros.Add(parametro_03);

            //var parametro_04 = new Parametro();
            //parametro_04.Tipo = Parametro.TypeOf.BARCODE;
            //parametro_04.Texto = "7509997006990";
            //parametro_04.Properties.Add(new Property("URL", ModelPDFModified.ObtenerNombreBarcode(parametro_04.Texto)));
            //parametro_04.Posicion = new Posicion(400, 400);
            //Parametros.Add(parametro_04);

            var parametro_05 = new Parametro();
            parametro_05.Tipo = Parametro.TypeOf.TEXTO;
            parametro_05.Texto = "Address: 1600 Amphitheatre Parkway.";
            parametro_05.Posicion = new Posicion(400, 600);
            Parametros.Add(parametro_05);

            //var parametro_06 = new Parametro();
            //parametro_06.Tipo = Parametro.TypeOf.BARCODE;
            //parametro_06.Texto = "7509997006992";
            //parametro_06.Properties.Add(new Property("URL", ModelPDFModified.ObtenerNombreBarcode(parametro_06.Texto)));
            //parametro_06.Posicion = new Posicion(300, 300);
            //Parametros.Add(parametro_06);

            return Parametros;
        }
        
    }

    class Parametro
    {
        public enum TypeOf { TEXTO, BARCODE };
        public Parametro()
        {
            Properties = new List<Property>(); 
        }
        public TypeOf Tipo { get; set; }
        public List<Property> Properties { get; set; }
        public string Texto { get; set; }
        public Posicion Posicion { get; set; }

    }
    class ModelPDFModified
    {
        public string RootPath { get; set; }
        public string FileNameOrigin { get; set; }
        public string FileNameModify { get; set; }
        public Font Font { get; set; }
        public List<Hoja> Hojas { get; set; }
        public static string ObtenerNombreBarcode(string Barcode)
        {
            return "my_barcode_" + Barcode + ".png";
        }
        public static void Log(string RootPath, string message)
        {
            File.AppendAllText(RootPath + "log.txt", message);
        }
    }
    class Hoja
    {
        public int Id { get; set; }
        public string Descricion { get; set; }
        public List<Parametro> Propiedades { get; set; }
    }
    class Property
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public Property(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
    class Font
    {
        public Font(string family, string color, int size)
        {
            Family = family;
            Color = color;
            Size = size;
        }
        public string Family { get; set; }
        public string Color { get; set; }
        public int Size { get; set; } 
    }
    class Posicion
    {
        public Posicion(float x, float y)
        {
            X = x;
            Y = y;
        }
        public float X { get; set; }
        public float Y { get; set; }
    }
}

using IronBarCode;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace wfa_barcode
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            GeneratedBarcode MyBarCode = IronBarCode.BarcodeWriter.CreateBarcode("7509997006990", BarcodeWriterEncoding.Code128);
            MyBarCode.ResizeTo(300, 100); // pixels
            //MyBarCode.AddBarcodeValueTextBelowBarcode();
            //MyBarCode.AddBarcodeValueTextAboveBarcode();
            MyBarCode.SaveAsPng("MyBarCode.png");


            /*** STYING GENERATED BARCODES  ***/

            // BarcodeWriter.CreateBarcode creates a GeneratedBarcode object which allows the barcode to be styled and annotated.
            //GeneratedBarcode MyBarCode = BarcodeWriter.CreateBarcode("Iron Software", BarcodeWriterEncoding.QRCode);

            // Any text (or commonly, the value of the barcode) can be added to the image in a default or specified font.
            // Text positions are automatically centered, above or below.  Fonts that are too large for a given image are automatically scaled down.
            //MyBarCode.AddBarcodeValueTextBelowBarcode();
            //MyBarCode.AddAnnotationTextAboveBarcode("This is My Barcode", new Font(new FontFamily("Arial"), 12, FontStyle.Regular, GraphicsUnit.Pixel), Color.DarkSlateBlue);

            // Resize, add Margins and Check final Image Dimensions
            //MyBarCode.ResizeTo(300, 300); // pixels
            //MyBarCode.SetMargins(0, 20, 0, 20);

            //int FinalWidth = MyBarCode.Width;
            //int FinalHeight = MyBarCode.Height;

            //Recolor the barcode and its background
            //MyBarCode.ChangeBackgroundColor(Color.LightGray);
            //MyBarCode.ChangeBarCodeColor(Color.DarkSlateBlue);
            //if (!MyBarCode.Verify())
            //{
                //Console.WriteLine("Color contrast should be at least 50% or a barcode may become unreadable.  Test using GeneratedBarcode.Verify()");
            //}

            // Finally save the result
            //MyBarCode.SaveAsHtmlFile("StyledBarcode.html");


            /*** STYING BARCODES IN A SINGLE LINQ STYLE EXPRESSION ***/

            // Fluent API
            //BarcodeWriter.CreateBarcode("https://ironsoftware.com", BarcodeWriterEncoding.Aztec).ResizeTo(250, 250).SetMargins(10).AddBarcodeValueTextAboveBarcode().SaveAsImage("StyledBarcode.png");
            /*** STYING QR CODES WITH LOGO IMAGES OR BRANDING ***/

            // Use the QRCodeWriter.CreateQrCodeWithLogo Method instead of BarcodeWriter.CreateBarcode
            // Logo will automatically be sized appropriately and snapped to the QR grid.

            //GeneratedBarcode QRWithLogo = QRCodeWriter.CreateQrCodeWithLogo("https://visualstudio.microsoft.com/", "visual-studio-logo.png");
            //QRWithLogo.ResizeTo(500, 500).SetMargins(10).ChangeBarCodeColor(Color.DarkGreen);
            //QRWithLogo.SaveAsPng("QRWithLogo.Png").SaveAsPdf("MyVerifiedQR.html"); // save as 2 formats 
        }
    }
}

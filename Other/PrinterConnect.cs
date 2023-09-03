using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using BuildMaterials.Models;

namespace BuildMaterials.Other
{
    public class PrinterConnect : IDisposable
    {
        private string? printObj;
        private PrintDocument printDocument;
        private bool disposedValue = false;

        public PrinterConnect()
        {
            printDocument = new PrintDocument();
        }

        public bool Print(object modelObj)
        {
            try
            {
                if (modelObj is Models.Account)
                {
                    printObj = (modelObj as Account)?.ToString();
                }
                if (modelObj is Models.Contract)
                {
                    printObj = (modelObj as Contract)?.ToString();
                }
                if (modelObj is Models.Customer)
                {
                    printObj = (modelObj as Customer)?.AsString();
                }
                if (modelObj is Models.Employee)
                {
                    printObj = (modelObj as Employee)?.ToString();
                }
                if (modelObj is Models.Material)
                {
                    printObj = (modelObj as Material)?.AsString();
                }
                if (modelObj is Models.Provider)
                {
                    printObj = (modelObj as Provider)?.AsString();
                }
                if (modelObj is Models.Trade)
                {
                    printObj = (modelObj as Trade)?.ToString();
                }
                if (modelObj is Models.TTN)
                {
                    printObj = (modelObj as TTN)?.ToString();
                }
                if (printObj == null)
                {
                    printObj = "error";
                }
                printDocument.PrintPage += PrintPageHandler;

                PrintDialog printDialog = new PrintDialog()
                {
                    Document = printDocument
                };

                if (printDialog.ShowDialog() == DialogResult.OK)
                {
                    printDialog.Document.Print();
                    return true;
                }
                return false;
            }
            finally
            {
                printDocument.PrintPage -= PrintPageHandler;
                printObj = null;
            }
        }

        private void PrintPageHandler(object sender, PrintPageEventArgs e)
        {                        
            e.Graphics?.DrawString(printObj, new Font("Arial", 14), System.Drawing.Brushes.Black, 0, 0);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    printObj = null;
                }

                printDocument.Dispose();
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
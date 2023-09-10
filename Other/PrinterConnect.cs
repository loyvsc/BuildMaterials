using BuildMaterials.Models;
using System.Drawing.Printing;

namespace BuildMaterials.Other
{
    public class PrinterConnect : IDisposable
    {
        private string? printObj;
        private readonly PrintDocument printDocument;
        private bool disposedValue = false;

        public PrinterConnect()
        {
            printDocument = new PrintDocument();
        }

        public bool Print(object modelObj)
        {
            try
            {
                switch (modelObj)
                {
                    case Models.Account:
                        {
                            printObj = (modelObj as Account)?.ToString();
                            break;
                        }
                    case Models.Contract:
                        {
                            printObj = (modelObj as Contract)?.ToString();
                            break;
                        }
                    case Models.Customer:
                        {
                            printObj = (modelObj as Customer)?.AsString();
                            break;
                        }
                    case Employee:
                        {
                            printObj = (modelObj as Employee)?.ToString();
                            break;
                        }
                    case Material:
                        {
                            printObj = (modelObj as Material)?.AsString();
                            break;
                        }
                    case Provider:
                        {
                            printObj = (modelObj as Provider)?.AsString();
                            break;
                        }
                    case Trade:
                        {
                            printObj = (modelObj as Trade)?.ToString();
                            break;
                        }
                    case TTN:
                        {
                            printObj = (modelObj as TTN)?.ToString();
                            break;
                        }
                    case null:
                        {
                            printObj = "error";
                            break;
                        }
                }
                printDocument.PrintPage += PrintPageHandler;


                using (PrintDialog printDialog = new PrintDialog()
                {
                    Document = printDocument
                })
                {
                    if (printDialog.ShowDialog() == DialogResult.OK)
                    {
                        printDialog.Document.Print();
                        return true;
                    }
                }
                return false;
            }
            finally
            {
                printDocument.PrintPage -= PrintPageHandler;
                printObj = string.Empty;
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
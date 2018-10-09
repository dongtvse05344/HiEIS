using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.security;
using SignatureApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BcX509 = Org.BouncyCastle.X509;
using DotNetUtils = Org.BouncyCastle.Security.DotNetUtilities;

namespace SignatureApp
{
    public partial class SignatureDialog : Form
    {
        List<InvoiceModel> invoices;
        //string username;
        private string compGuid;
        LoginDialog parrentForm;
        public SignatureDialog(LoginDialog parrentForm, List<InvoiceModel> list, string compGuid)
        {
            InitializeComponent();
            this.parrentForm = parrentForm;
            this.invoices = list;
            this.compGuid = compGuid;
            LoadTable();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tblInvoice_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                btnSign.Enabled = true;
                DataGridViewRow row = this.tblInvoice.Rows[e.RowIndex];
                txtId.Text = row.Cells[0].Value.ToString();
                string path = Path.Combine("FileClient", "Invoices");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path = Path.Combine(path, row.Cells[7].Value.ToString() + ".pdf");
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                txtPath.Text = path;
                using (var stream = ApiUtils.LoadFileAsync(row.Cells[6].Value.ToString()).GetAwaiter().GetResult())
                {

                    using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                    {
                        stream.Position = 0;
                        stream.CopyTo(fileStream);
                    }
                }
                pdf.Visible = true;
                pdf.LoadFile(path);

            }

        }

        private string SignWithThisCert(X509Certificate2 cert)
        {
            try
            {
                string SourcePdfFileName = txtPath.Text;
                var fileName = Path.GetFileName(SourcePdfFileName);
                string path = Path.Combine("FileClient", "Signed");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                fileName = "Signed_" + fileName;
                string DestPdfFileName = Path.Combine(path, fileName);
                Org.BouncyCastle.X509.X509CertificateParser cp = new Org.BouncyCastle.X509.X509CertificateParser();
                Org.BouncyCastle.X509.X509Certificate[] chain = new Org.BouncyCastle.X509.X509Certificate[] { cp.ReadCertificate(cert.RawData) };
                IExternalSignature externalSignature = new X509Certificate2Signature(cert, "SHA-1");
                PdfReader pdfReader = new PdfReader(SourcePdfFileName);
                using (FileStream signedPdf = new FileStream(DestPdfFileName, FileMode.Create))
                {
                    int numberOfPages = pdfReader.NumberOfPages;
                    PdfStamper pdfStamper = PdfStamper.CreateSignature(pdfReader, signedPdf, '\0');

                    //Edit date
                    AcroFields testForm = pdfStamper.AcroFields;
                    var date = DateTime.Now;
                    testForm.SetField("Day", date.Day.ToString());
                    testForm.SetField("Month", date.Month.ToString());
                    testForm.SetField("Year", date.Date.Year.ToString());
                    BaseColor green = new BaseColor(0, 204, 102);
                    var Font = FontFactory.GetFont(BaseFont.HELVETICA, 10, green);
                    
                    //Add signature
                    PdfSignatureAppearance signatureAppearance = pdfStamper.SignatureAppearance;
                    signatureAppearance.Layer2Font = Font;
                    //here set signatureAppearance at your will

                    string content = GetCertificate(cert.SubjectName.Name);
                    if (content != null)
                    {
                        signatureAppearance.Layer2Text = content;
                    }
                    else
                    {
                        signatureAppearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.DESCRIPTION;
                    }

                    signatureAppearance.SetVisibleSignature(new iTextSharp.text.Rectangle(420, 160, 600, 230), numberOfPages, null); //Hisoft Rectangle(450, 160, 600, 230) (450, 70, 600, 140)
                    MakeSignature.SignDetached(signatureAppearance, externalSignature, chain, null, null, null, 0, CryptoStandard.CMS);
                    
                    PdfContentByte overContent = pdfStamper.GetOverContent(1);
                    pdfStamper.FormFlattening = true;
                    pdfStamper.Close();

                }
                pdfReader.Close();
                return DestPdfFileName;
            }
            catch (Exception)
            {
                return null;
                // throw;
            }

        }

        private string GetCertificate(string subjectName)
        {
            string src = subjectName;
            string content = null;
            string tax = "0314508549";
            if (subjectName.Contains(tax))
            {
                int start = src.IndexOf("MST:") + 4;
                string MST = src.Substring(start, src.Length - start);
                start = src.IndexOf("CN=") + 3;
                int end = src.IndexOf(", OID");
                string name = src.Substring(start, end - start);
                content = "Duoc ky boi: " + RemoveUnicode(name) + " \nMST: " + MST + " - Ngay: " + DateTime.Now.ToShortDateString();

            }

            return content;
        }
        public static string RemoveUnicode(string text)
        {
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
                        "đ",
                        "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
                        "í","ì","ỉ","ĩ","ị",
                        "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
                        "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
                        "ý","ỳ","ỷ","ỹ","ỵ",};
            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
                        "d",
                        "e","e","e","e","e","e","e","e","e","e","e",
                        "i","i","i","i","i",
                        "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
                        "u","u","u","u","u","u","u","u","u","u","u",
                        "y","y","y","y","y",};
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }
            return text;
        }

        private void btnSign_Click(object sender, EventArgs e)
        {
            tblInvoice.ReadOnly = true;
            btnSign.Enabled = false;
            try
            {
                int id = int.Parse(txtId.Text);
                X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadOnly);
                X509Certificate2 cert = null;
                //manually chose the certificate in the store
                store.Open(OpenFlags.ReadOnly);

                X509Certificate2Collection certCollection =
                    store.Certificates.Find(X509FindType.FindBySubjectName,
                    "VN", true);
                cert = certCollection[0];
                BcX509.X509Certificate bcCert = DotNetUtils.FromX509Certificate(cert);
                var chain = new List<BcX509.X509Certificate> { bcCert };
                store.Close();

                var fileName = SignWithThisCert(cert);
                if (fileName != null)
                {
                    var item = invoices.SingleOrDefault(x => x.Id == id);
                    if (item != null)
                    {
                        var param = new KeyValuePair<string, object>[]
                        {
                               new KeyValuePair<string, object>("InvoiceGuid",item.CodeGuid),
                               new KeyValuePair<string, object>("CompGuid",compGuid)
                         };

                        var result = ApiUtils.UploadFile<ResultModel>("Invoices/ReceiveInvoice", fileName, param).GetAwaiter().GetResult();
                        if (result.result)
                        {
                            tblInvoice.DataSource = null;
                            invoices.Remove(item);
                            MessageBox.Show("Đã ký thành công!");
                            LoadTable();
                        } 
                        else
                        {
                            MessageBox.Show("Thất bại! Vui lòng thử lại.");
                        }

                    }
                }
                else
                {
                    MessageBox.Show("Thất bại! Vui lòng thử lại.");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Vui lòng kiểm tra lại chữ ký số!");
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
            parrentForm.Show();
        }


        private void LoadTable()
        {
            btnSign.Enabled = false;
            txtId.Text = "";
            txtPath.Text = "";
            pdf.Visible = false;
            tblInvoice.DataSource = null;
            if (invoices.Count() > 0)
            {
                tblInvoice.AutoGenerateColumns = false;
                tblInvoice.DataSource = invoices;
                tblInvoice.ClearSelection();
                tblInvoice.Refresh();
            }
            else
            {
                tblInvoice.DataSource = null;
                //tblInvoice.Rows.Clear();
                tblInvoice.Refresh();
                MessageBox.Show("Không có hóa đơn cần ký!");
            }

        }


        private void txtId_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPath_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            var result = ApiUtils.PostDataAsync<InvoiceResultModel>(
               "Invoices/GetInvoiceToSign"
               , new
               {
                   compGuid
               }).GetAwaiter().GetResult();
            if (result.result)
            {
                invoices = result.Invoices;

            }
            else
            {
                invoices = null;
            }
            LoadTable();
        }

        private class ResultModel
        {
            public bool result { get; set; }
           

        }

        private class InvoiceResultModel
        {
            public bool result { get; set; }
            public List<InvoiceModel> Invoices { get; set; }

        }
    }
}

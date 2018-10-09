using Microsoft.Win32;
using SignatureApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SignatureApp
{
    public partial class LoginDialog : Form
    {
        SignatureDialog signForm;
        public LoginDialog()
        {
            InitializeComponent();
            txtUsername.Focus();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var username = txtUsername.Text.Trim();
            var password = txtPassword.Text.Trim();
            var result = ApiUtils.PostDataAsync<LoginResultModel>(
                "Account/LoginApp"
                , new
                {
                    username,
                    password
                }).GetAwaiter().GetResult();
            if (result.result)
            {
                //RegistryKey RegistryKey = Registry.CurrentUser.CreateSubKey("ExampleTest"); //path of registry
                //RegistryKey.SetValue("CompanyKey", result.CompanyGuid);
                //RegistryKey.Close();
                signForm = new SignatureDialog(this, result.Invoices, result.CompanyGuid);
                signForm.Show();
                this.Hide();
                txtPassword.Text = "";
                txtUsername.Text = "";
                //MessageBox.Show(result.CompanyGuid);
            }
            else
            {
                MessageBox.Show("Thất bại! Vui lòng thử lại.");
            }
        }

        private class LoginResultModel
        {
            public bool result { get; set; }
            public string CompanyGuid { get; set; }
            public List<InvoiceModel> Invoices { get; set; }
        }

      
    }
}

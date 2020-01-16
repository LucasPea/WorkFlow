using EASendMail;
using Microsoft.SharePoint.Client;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;


namespace NextCowry.AppControllers
{
    [RoutePrefix("cowry")]
    public class EmailDataController : ApiController
    {
        #region Sharepoint Connection
        string webUrl = WebConfigurationManager.AppSettings["webUrl"];
        string userName = WebConfigurationManager.AppSettings["userName"];
        string password = WebConfigurationManager.AppSettings["password"];
        #endregion

       static string name, email;
        DateTime dateofExcel;

        [HttpGet]
        [Route("result/EmailResultToCowry")]
        public bool EmailResultToCowry(string SpId)
        {
            try
            {
                List<ResultData> resultData = new List<ResultData>();
                SecureString sec_pass = new SecureString();
                Array.ForEach(password.ToArray(), sec_pass.AppendChar);
                sec_pass.MakeReadOnly();
                {
                    using (var context = new ClientContext(webUrl))
                    {
                        Web web = context.Web;
                        context.Credentials = new SharePointOnlineCredentials(userName, sec_pass);
                        {
                            List list = context.Web.Lists.GetByTitle("CowryMainResult");
                            context.Load(list);
                            context.ExecuteQuery();

                            CamlQuery camlQuery = new CamlQuery();
                            camlQuery.ViewXml = "<View><Query><Where><Eq><FieldRef Name='SpId'/><Value Type='Text'>" + SpId + "</Value></Eq></Where></Query><RowLimit>100</RowLimit></View>";
                            ListItemCollection collListItem = list.GetItems(camlQuery);
                                                      
                            context.Load(collListItem);
                            context.ExecuteQuery();
                            int count = collListItem.Count;

                            ResultData result = new ResultData();
                            foreach (ListItem oListItem in collListItem)
                            {
                                result.SpId = oListItem["SpId"].ToString();
                                result.Currency = oListItem["CurrencyType"].ToString();

                                result.PremiumUsers = oListItem["PremiumUsers"].ToString();
                                result.EssentialUsers = oListItem["EssentialUsers"].ToString();
                                result.TeamUsers = oListItem["TeamUsers"].ToString();
                                result.TotalUsers = oListItem["TotalUsers"].ToString(); 
                                                              
                                result.StandardSupportPrice = oListItem["StandardSupportPrice"].ToString();
                                result.EnhancedSupportPrice = oListItem["EnhancedSupportPrice"].ToString();
                                result.EnhancedSupportChecked = oListItem["EnhancedSupportChecked"].ToString();

                                result.PaymentOptionMonthlyPrice = oListItem["PaymentOptionTotalMonthlyPrice"].ToString();
                                result.PaymentOptionYearlyPrice = oListItem["PaymentOptionTotalYearlyPrice"].ToString();
                                result.PaymentOptionAns = oListItem["PaymentOptionType"].ToString();

                                result.FirstName = oListItem["FirstName"].ToString();
                                name = oListItem["FirstName"].ToString();
                                                               
                                result.LastName = oListItem["LastName"].ToString();
                                result.JobTitle = oListItem["JobTitle"].ToString();
                                result.ContactNumber = oListItem["ContactNumber"].ToString();
                                result.Email = oListItem["Email"].ToString();
                                email = oListItem["Email"].ToString();

                                result.CompanyName = oListItem["CompanyName"].ToString();
                                result.CompanyNumber = oListItem["CompanyNumber"].ToString();
                                result.CompanyAddress = oListItem["CompanyAddress"].ToString();
                                result.CompanyCountry = oListItem["CompanyCountry"].ToString();
                                result.CompanyState = oListItem["CompanyState"].ToString();
                                result.CompanyCity = oListItem["CompanyCity"].ToString();

                                result.MicrosoftOfficeAccChecked = oListItem["MicrosoftOfficeAccChecked"].ToString();
                                if (oListItem["DomainName"] != null)
                                {
                                    result.DomainName = oListItem["DomainName"].ToString();
                                }
                                result.FindDomain = oListItem["FindDomain"].ToString();
                                result.MicrosoftCloudAgreement = oListItem["MicrosoftCloudAgreement"].ToString();
                                result.SupportContract = oListItem["SupportContract"].ToString();

                                result.CardType = oListItem["PaymentInfoCardType"].ToString();
                                result.NameOnCard = oListItem["PaymentInfoNameOnCard"].ToString();
                                result.CardNumber = oListItem["PaymentInfoCardNumber"].ToString();
                                result.CardCVV = oListItem["PaymentInfoCardCVV"].ToString();
                                result.CardExpiryMonth = oListItem["PaymentInfoCardExpiryMonth"].ToString();
                                result.CardExpiryYear = oListItem["PaymentInfoCardExpiryYear"].ToString();

                                }
                                resultData.Add(result);
                            }
                        }
                    }

                DataTable dt = ConvertToDataTable(resultData);

                ExporttosuccessExcel(dt);

                sendEMailThroughOUTLOOK();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static DataTable ConvertToDataTable(List<ResultData> data)
        {
            PropertyDescriptorCollection properties =
            TypeDescriptor.GetProperties(typeof(ResultData));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (ResultData item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;

            //
        }

        public static void ExporttosuccessExcel(DataTable dt)
        {
            string uname = name + ".xls";
            MemoryStream ms = new MemoryStream();
            using (dt)
            {
                IWorkbook workbook = new HSSFWorkbook();//Create an excel Workbook
                ISheet sheet = workbook.CreateSheet();//Create a work table in the table
                IRow headerRow = sheet.CreateRow(0); //To add a row in the table
                foreach (DataColumn column in dt.Columns)
                    headerRow.CreateCell(column.Ordinal).SetCellValue(column.Caption);
                int rowIndex = 1;
                foreach (DataRow row in dt.Rows)
                {
                    IRow dataRow = sheet.CreateRow(rowIndex);
                    foreach (DataColumn column in dt.Columns)
                    {
                        dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                    }
                    rowIndex++;
                }
                var pathurl = Path.Combine(HttpContext.Current.Server.MapPath("~/ResultData"), uname);                         
                workbook.Write(ms);
                var AssemblyPath = pathurl;               
                FileStream fs = new FileStream(AssemblyPath, FileMode.OpenOrCreate);

                ms.WriteTo(fs);
                ms.Position = 0;
                ms.Close();
                ms.Flush();
                fs.Close();
            }
        }

        public void sendEMailThroughOUTLOOK()
        {
            string uname = name + ".xls";
            try
            {
                SmtpMail oMail = new SmtpMail("TryIt");
                SmtpClient oSmtp = new SmtpClient();

                // Set sender email address, please change it to yours
                oMail.From = "admin@cowrysolutions.com";

                // Set recipient email address, please change it to yours
                oMail.To = "info@cowrysolutions.com";

                // Set email subject
                oMail.Subject = "Cowry Solutions Configurator Result for " + name;

                // Set Html body
                oMail.TextBody = name + " has reached to you for Business Central Purchase options. Please find the details filled by " + name + " in attched sheet.";

                // Your SMTP server address
                SmtpServer oServer = new SmtpServer("smtp.outlook.com");

                // User and password for ESMTP authentication, if your server doesn't require
                // User authentication, please remove the following codes.
                oServer.User = "admin@cowrysolutions.com";
                oServer.Password = "Shivam@2012";

                // use 587 TLS port
                oServer.Port = 587;

                // detect SSL/TLS connection automatically
                oServer.ConnectType = SmtpConnectType.ConnectSSLAuto;

                // If your smtp server requires SSL connection, please add this line
                // oServer.ConnectType = SmtpConnectType.ConnectSSLAuto;

                try
                {
                    var pathurl = Path.Combine(HttpContext.Current.Server.MapPath("~/ResultData"), uname);
                    // Add attachment from local disk
                    oMail.AddAttachment(pathurl);                    

                    Console.WriteLine("start to send email with attachment ...");
                    oSmtp.SendMail(oServer, oMail);
                    Console.WriteLine("email was sent successfully!");
                }
                catch (Exception ep)
                {
                    Console.WriteLine("failed to send email with the following error:");
                    Console.WriteLine(ep.Message);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

    }

    public class ResultData
    {
        //First Data
        public string SpId { get; set; }
        public string Currency { get; set; }

        //Main Data
        public string PremiumUsers { get; set; }
        public string EssentialUsers { get; set; }
        public string TeamUsers { get; set; }
        public string TotalUsers { get; set; }      
        public string StandardSupportPrice { get; set; }
        public string EnhancedSupportPrice { get; set; }
        public string EnhancedSupportChecked { get; set; }

        //Payment Option
        public string PaymentOptionMonthlyPrice { get; set; }
        public string PaymentOptionYearlyPrice { get; set; }
        public string PaymentOptionAns { get; set; }

        //PersonalInformation
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }

        //Company Information
        public string CompanyName { get; set; }
        public string CompanyNumber { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyCountry { get; set; }
        public string CompanyState { get; set; }
        public string CompanyCity { get; set; }
        public string CompanyPostalCode { get; set; }

        //Microsoft Account Information
        public string MicrosoftOfficeAccChecked { get; set; }
        public string DomainName { get; set; }
        public string FindDomain { get; set; }

        //Authorization
        public string MicrosoftCloudAgreement { get; set; }
        public string SupportContract { get; set; }

        //Card Information
        public string CardType { get; set; }
        public string NameOnCard { get; set; }
        public string CardNumber { get; set; }
        public string CardCVV { get; set; }
        public string CardExpiryMonth { get; set; }
        public string CardExpiryYear { get; set; }
    }
}

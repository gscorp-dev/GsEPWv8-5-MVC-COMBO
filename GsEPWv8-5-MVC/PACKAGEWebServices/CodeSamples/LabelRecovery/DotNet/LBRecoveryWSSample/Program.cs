using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LabelRecoveryExample.LabelRecoveryWebReference;


namespace LabelRecoveryExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                LBRecovery lb_recovery = new LBRecovery();
                LabelRecoveryRequest label_recovery_request = new LabelRecoveryRequest();
                UPSSecurity upss = new UPSSecurity();
                UPSSecurityServiceAccessToken upssSvcAccessToken = new UPSSecurityServiceAccessToken();
                upssSvcAccessToken.AccessLicenseNumber = "BD8FC4C98DFA4B75";
                
                upss.ServiceAccessToken = upssSvcAccessToken;
                UPSSecurityUsernameToken upssUsrNameToken = new UPSSecurityUsernameToken();
                
                upssUsrNameToken.Username = "CDCcdc12";
                upssUsrNameToken.Password = "4361Edison";

                upss.UsernameToken = upssUsrNameToken;
                lb_recovery.UPSSecurityValue = upss;
                RequestType request = new RequestType();
                label_recovery_request.Request = request;
                
                label_recovery_request.TrackingNumber = "1Z55E6170309895990";


                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Tls11; //This line will ensure the latest security protocol for consuming the web service call.
                LabelRecoveryResponse label_recovery_response = lb_recovery.ProcessLabelRecovery(label_recovery_request);
               // LabelImageType image=(LabelImageType)label_recovery_response.Items[0];

                LabelImageType image1 = ((LabelRecoveryExample.LabelRecoveryWebReference.LabelResultsType)(label_recovery_response.Items[0])).LabelImage;
                Console.WriteLine("Image Base64:\n " + image1.GraphicImage);


                byte[] labelBuffer = Convert.FromBase64String(image1.GraphicImage);

                string fileName = string.Format("label{0}.gif", label_recovery_request.TrackingNumber.ToString());
                string folder = "C:\\3PL-UPS\\";

                string returnFileName = fileName;

                string phyPath = "TEST";
                string fullPath = String.Format("{0}",  folder);
                if (!System.IO.Directory.Exists(fullPath)) System.IO.Directory.CreateDirectory(fullPath);
                string pathToCheck = String.Format("{0}{1}", fullPath, returnFileName);

                // Should just delete the old file (code copied from another project)  
                if (System.IO.File.Exists(pathToCheck))
                {
                    int counter = 1;
                    while (System.IO.File.Exists(pathToCheck))
                    {
                        returnFileName = counter.ToString() + fileName;
                        pathToCheck = String.Format("{0}{1}", fullPath, returnFileName);
                        counter++;
                    }
                }

                System.IO.FileStream LabelFile = new System.IO.FileStream(pathToCheck, System.IO.FileMode.Create);
                LabelFile.Write(labelBuffer, 0, labelBuffer.Length);
                LabelFile.Close();

               

                Console.ReadKey();
            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                Console.WriteLine("");
                Console.WriteLine("---------LBRecovery Web Service returns error----------------");
                Console.WriteLine("---------\"Hard\" is user error \"Transient\" is system error----------------");
                Console.WriteLine("SoapException Message= " + ex.Message);
                Console.WriteLine("");
                Console.WriteLine("SoapException Category:Code:Message= " + ex.Detail.LastChild.InnerText);
                Console.WriteLine("");
                Console.WriteLine("SoapException XML String for all= " + ex.Detail.LastChild.OuterXml);
                Console.WriteLine("");
                Console.WriteLine("SoapException StackTrace= " + ex.StackTrace);
                Console.WriteLine("-------------------------");
                Console.WriteLine("");
            } catch (Exception ex)
            {
                Console.WriteLine("");
                Console.WriteLine("-------------------------");
                Console.WriteLine(" General Exception= " + ex.Message);
                Console.WriteLine(" General Exception-StackTrace= " + ex.StackTrace);
                Console.WriteLine("-------------------------");

            }
            finally
            {
                Console.ReadKey();
            }

        }
        /// <summary>  
        /// Saves the GIF part of the label as labelTRACKINGNUMBER.gif.  
        /// </summary>  
        /// <returns>The saved file name.</returns>  
        private string SaveShippingLabelGIF(byte[] labelBuffer, string folder, string trackingNumber)
        {
            string fileName = string.Format("label{0}.gif", trackingNumber);
            return SaveShippingLabel(labelBuffer, folder, trackingNumber, fileName);
        }


        private LabelSpecificationType GetUpsLabelSpecification()
        {
            LabelSpecificationType labelSpec = new LabelSpecificationType();

            LabelImageFormatType labelImageFormat = new LabelImageFormatType();
            labelImageFormat.Code = "GIF";
            labelSpec.LabelImageFormat = labelImageFormat;

            return labelSpec;
        }

       
        /// <summary>  
        /// Sets the HTML part of the label as "page_TRACKINGNUMBER.html" and saves the label.  
        /// </summary>  
        /// <returns>The saved file name.</returns>  
        private string SaveShippingLabelHTML(byte[] labelBuffer, string folder, string trackingNumber)
        {
            string fileName = string.Format("page_{0}.html", trackingNumber);
            return SaveShippingLabel(labelBuffer, folder, trackingNumber, fileName);
        }
        private string SaveShippingLabel(byte[] labelBuffer, string folder, string trackingNumber, string fileName)
        {
            string returnFileName = fileName;

            string phyPath = "TEST";
            string fullPath = String.Format("{0}{1}", phyPath, folder);
            if (!System.IO.Directory.Exists(fullPath)) System.IO.Directory.CreateDirectory(fullPath);
            string pathToCheck = String.Format("{0}{1}", fullPath, returnFileName);

            // Should just delete the old file (code copied from another project)  
            if (System.IO.File.Exists(pathToCheck))
            {
                int counter = 1;
                while (System.IO.File.Exists(pathToCheck))
                {
                    returnFileName = counter.ToString() + fileName;
                    pathToCheck = String.Format("{0}{1}", fullPath, returnFileName);
                    counter++;
                }
            }

            System.IO.FileStream LabelFile = new System.IO.FileStream(pathToCheck, System.IO.FileMode.Create);
            LabelFile.Write(labelBuffer, 0, labelBuffer.Length);
            LabelFile.Close();

            return returnFileName;
        }
    }
}

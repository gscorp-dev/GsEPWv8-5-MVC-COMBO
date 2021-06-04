using System;
using System.Collections.Generic;
using System.Text;
using ShipWSSample.ShipWebReference;
using System.ServiceModel;

namespace ShipWSSample
{
    class ShipClient
    {
        static void Main()
        {
            try
            {
                ShipService shpSvc = new ShipService();
                ShipmentRequest shipmentRequest = new ShipmentRequest();
                UPSSecurity upss = new UPSSecurity();
                UPSSecurityServiceAccessToken upssSvcAccessToken = new UPSSecurityServiceAccessToken();
                upssSvcAccessToken.AccessLicenseNumber = "BD8FC4C98DFA4B75";
                upss.ServiceAccessToken = upssSvcAccessToken;
                UPSSecurityUsernameToken upssUsrNameToken = new UPSSecurityUsernameToken();
                upssUsrNameToken.Username = "CDCcdc12";
                upssUsrNameToken.Password = "4361Edison";
                upss.UsernameToken = upssUsrNameToken;
                shpSvc.UPSSecurityValue = upss;
                RequestType request = new RequestType();
                String[] requestOption = { "nonvalidate" };
                request.RequestOption = requestOption;
                shipmentRequest.Request = request;
                ShipmentType shipment = new ShipmentType();
                shipment.Description = "Ship webservice example";
                ShipperType shipper = new ShipperType();
                shipper.ShipperNumber = "3847WY";
                PaymentInfoType paymentInfo = new PaymentInfoType();
                ShipmentChargeType shpmentCharge = new ShipmentChargeType();
                BillShipperType billShipper = new BillShipperType();
                billShipper.AccountNumber = "3847WY";
                shpmentCharge.BillShipper = billShipper;
                shpmentCharge.Type = "01";
                ShipmentChargeType[] shpmentChargeArray = { shpmentCharge };
                paymentInfo.ShipmentCharge = shpmentChargeArray;
                shipment.PaymentInformation = paymentInfo;
                ShipWSSample.ShipWebReference.ShipAddressType shipperAddress = new ShipWSSample.ShipWebReference.ShipAddressType();
                String[] addressLine = { "480 Parkton Plaza" };
                shipperAddress.AddressLine = addressLine;
                shipperAddress.City = "Timonium";
                shipperAddress.PostalCode = "21093";
                shipperAddress.StateProvinceCode = "MD";
                shipperAddress.CountryCode = "US";
                shipperAddress.AddressLine = addressLine;
                shipper.Address = shipperAddress;
                shipper.Name = "ABC Associates";
                shipper.AttentionName = "ABC Associates";
                ShipPhoneType shipperPhone = new ShipPhoneType();
                shipperPhone.Number = "1234567890";
                shipper.Phone = shipperPhone;
                shipment.Shipper = shipper;
                ShipFromType shipFrom = new ShipFromType();
                ShipWSSample.ShipWebReference.ShipAddressType shipFromAddress = new ShipWSSample.ShipWebReference.ShipAddressType();
                String[] shipFromAddressLine = { "Ship From Street" };
                shipFromAddress.AddressLine = addressLine;
                shipFromAddress.City = "Timonium";
                shipFromAddress.PostalCode = "21093";
                shipFromAddress.StateProvinceCode = "MD";
                shipFromAddress.CountryCode = "US";
                shipFrom.Address = shipFromAddress;
                shipFrom.AttentionName = "Mr.ABC";
                shipFrom.Name = "ABC Associates";
                shipment.ShipFrom = shipFrom;
                ShipToType shipTo = new ShipToType();
                ShipToAddressType shipToAddress = new ShipToAddressType();
                String[] addressLine1 = { "GOERLITZER STR.1" };
                shipToAddress.AddressLine = addressLine1;
                shipToAddress.City = "Neuss";
                shipToAddress.PostalCode = "41456";
                shipToAddress.CountryCode = "DE";
                shipTo.Address = shipToAddress;
                shipTo.AttentionName = "DEF";
                shipTo.Name = "DEF Associates";
                ShipPhoneType shipToPhone = new ShipPhoneType();
                shipToPhone.Number = "1234567890";
                shipTo.Phone = shipToPhone;
                shipment.ShipTo = shipTo;
                ServiceType service = new ServiceType();
                service.Code = "08";
                shipment.Service = service;

                ShipmentTypeShipmentServiceOptions shpServiceOptions = new ShipmentTypeShipmentServiceOptions();

                /** **** International Forms ***** */
                InternationalFormType internationalForms = new InternationalFormType();

                /** **** Commercial Invoice ***** */
                String[] formTypeList = { "01" };
                internationalForms.FormType = formTypeList;

                /** **** Contacts and Sold To ***** */
                ContactType contacts = new ContactType();

                SoldToType soldTo = new SoldToType();
                soldTo.Option = "1";
                soldTo.AttentionName = "Sold To Attn Name";
                soldTo.Name = "Sold To Name";
                PhoneType soldToPhone = new PhoneType();
                soldToPhone.Number = "1234567890";
                soldToPhone.Extension = "1234";
                soldTo.Phone = soldToPhone;
                AddressType soldToAddress = new AddressType();
                String[] soldToAddressLine = { "34 Queen St" };
                soldToAddress.AddressLine = soldToAddressLine;
                soldToAddress.City = "Frankfurt";
                soldToAddress.PostalCode = "60547";
                soldToAddress.CountryCode = "DE";
                soldTo.Address = soldToAddress;
                contacts.SoldTo = soldTo;

                internationalForms.Contacts = contacts;

                /** **** Product ***** */
                ProductType product1 = new ProductType();
                String[] description = { "Product 1" };
                product1.Description = description;
                product1.CommodityCode = "111222AA";
                product1.OriginCountryCode = "US";
                UnitType unit = new UnitType();
                unit.Number = "147";
                unit.Value = "478";
                UnitOfMeasurementType uomProduct = new UnitOfMeasurementType();
                uomProduct.Code = "BOX";
                uomProduct.Description = "BOX";
                unit.UnitOfMeasurement = uomProduct;
                product1.Unit = unit;
                ProductWeightType productWeight = new ProductWeightType();
                productWeight.Weight = "10";
                UnitOfMeasurementType uomForWeight = new UnitOfMeasurementType();
                uomForWeight.Code = "LBS";
                uomForWeight.Description = "LBS";
                productWeight.UnitOfMeasurement = uomForWeight;
                product1.ProductWeight = productWeight;
                ProductType[] productList = {product1};
                internationalForms.Product = productList;

                /** **** InvoiceNumber, InvoiceDate, PurchaseOrderNumber, TermsOfShipment, ReasonForExport, Comments and DeclarationStatement  ***** */
                internationalForms.InvoiceNumber = "asdf123";
                internationalForms.InvoiceDate = "20151225";
                internationalForms.PurchaseOrderNumber = "999jjj777";
                internationalForms.TermsOfShipment = "CFR";
                internationalForms.ReasonForExport = "Sale";
                internationalForms.Comments = "Your Comments";
                internationalForms.DeclarationStatement = "Your Declaration Statement";

                /** **** Discount, FreightCharges, InsuranceCharges, OtherCharges and CurrencyCode  ***** */
                IFChargesType discount = new IFChargesType();
                discount.MonetaryValue = "100";
                internationalForms.Discount = discount;
                IFChargesType freight = new IFChargesType();
                freight.MonetaryValue = "50";
                internationalForms.FreightCharges = freight;
                IFChargesType insurance = new IFChargesType();
                insurance.MonetaryValue = "200";
                internationalForms.InsuranceCharges = insurance;
                OtherChargesType otherCharges = new OtherChargesType();
                otherCharges.MonetaryValue = "50";
                otherCharges.Description = "Misc";
                internationalForms.OtherCharges = otherCharges;
                internationalForms.CurrencyCode = "USD";


                shpServiceOptions.InternationalForms = internationalForms;
                shipment.ShipmentServiceOptions = shpServiceOptions;
                
                PackageType package = new PackageType();
                PackageWeightType packageWeight = new PackageWeightType();
                packageWeight.Weight = "10";
                ShipUnitOfMeasurementType uom = new ShipUnitOfMeasurementType();
                uom.Code = "LBS";
                packageWeight.UnitOfMeasurement = uom;
                package.PackageWeight = packageWeight;
                PackagingType packType = new PackagingType();
                packType.Code = "02";
                package.Packaging = packType;
                PackageType[] pkgArray = { package };
                shipment.Package = pkgArray;
                LabelSpecificationType labelSpec = new LabelSpecificationType();
                LabelStockSizeType labelStockSize = new LabelStockSizeType();
                labelStockSize.Height = "6";
                labelStockSize.Width = "4";
                labelSpec.LabelStockSize = labelStockSize;
                LabelImageFormatType labelImageFormat = new LabelImageFormatType();
                labelImageFormat.Code = "GIF";
                labelSpec.LabelImageFormat = labelImageFormat;
                shipmentRequest.LabelSpecification = labelSpec;
                shipmentRequest.Shipment = shipment;
                Console.WriteLine(shipmentRequest);
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Tls11; //This line will ensure the latest security protocol for consuming the web service call.
                ShipmentResponse shipmentResponse = shpSvc.ProcessShipment(shipmentRequest);

                string graphicImage = shipmentResponse.ShipmentResults.PackageResults[0].ShippingLabel.GraphicImage;

      
                byte[] labelBuffer = Convert.FromBase64String(graphicImage);

                string fileName = string.Format("label{0}.gif", shipmentResponse.ShipmentResults.ShipmentIdentificationNumber.ToString());
                string folder = "C:\\3PL-UPS\\";

                string returnFileName = fileName;

                string phyPath = "TEST";
                string fullPath = String.Format("{0}", folder);
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

                //string shippingLabelFileName1 = SaveShippingLabelGIF(byteLabel1, "_labels\\", shipmentResponse.ShipmentResults.ShipmentIdentificationNumber);



                Console.WriteLine("The transaction was a " + shipmentResponse.Response.ResponseStatus.Description);
                Console.WriteLine("The 1Z number of the new shipment is " + shipmentResponse.ShipmentResults.ShipmentIdentificationNumber);
                Console.ReadKey();
            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                Console.WriteLine("");
                Console.WriteLine("---------Ship Web Service returns error----------------");
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
            }
            catch (System.ServiceModel.CommunicationException ex)
            {
                Console.WriteLine("");
                Console.WriteLine("--------------------");
                Console.WriteLine("CommunicationException= " + ex.Message);
                Console.WriteLine("CommunicationException-StackTrace= " + ex.StackTrace);
                Console.WriteLine("-------------------------");
                Console.WriteLine("");

            }
            catch (Exception ex)
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
   
       string phyPath =string.Empty;  
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
